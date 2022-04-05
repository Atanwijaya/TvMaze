using Dapper;
using DapperParameters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TVScapper.Interfaces;
using TVScapper.Models;
using TVScapper.Models.TVMaze_API;
using TVScapper.Repositories;

namespace TVScapper.Services
{
    public class TVMazeSetupService : ITVMazeSetupService
    {
        private readonly IBaseRepository _baseRepo;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUtilityService _retryService;
        public TVMazeSetupService(IBaseRepository baseRepo, IHttpClientFactory httpClientFactory, IConfiguration configuration,
            IEnumerable<IUtilityService> utilityServices)
        {
            _baseRepo = baseRepo;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

            foreach (IUtilityService service in utilityServices)
            {
                if (service is RetryService)
                {
                    _retryService = service;
                    break;
                }
            }
        }

        public async Task SetupTVMazeArchitectureAsync()
        {
            var isRunSetups = _configuration.GetValue<bool>(Constant.AppSettings.SetupTableTypes);
            var isRunCallService = _configuration.GetValue<bool>(Constant.AppSettings.SetupTableData);

            if (isRunSetups)
            {
                string script = string.Empty;
                using (var sr = new StreamReader(Constant.SetupTableFilePath))
                {
                    script = await sr.ReadToEndAsync();
                }

                await _baseRepo.QueryAsync(script, null, CommandType.Text);
            }

            if (isRunCallService)
            {
                int pages = _configuration.GetValue<int>(Constant.AppSettings.ScrapPages);
                var client = _httpClientFactory.CreateClient(Constant.TVMazeAPIClientName);

                List<Show> shows = new List<Show>();

                for (int i = 0; i < pages; i++)
                {
                    var showResult = await _retryService.DoActionAsync<List<Show>>(async () => await GetTVMazeAsync(client, i));
                    shows.AddRange(showResult);
                }

                List<TVShow> tvShows = new List<TVShow>();
                List<TVCharacter> characters = new List<TVCharacter>();
                List<Cast> casts = new List<Cast>();
                List<TVShowCast> tvShowCasts = new List<TVShowCast>();
                List<TVShowGenres> tvShowGenres = new List<TVShowGenres>();

               // for (int i = 0; i < shows.Count; i++)
               for (int i = 0; i < 100; i++)
                {
                    var currentShow = shows[i];
                    var showPersonCharacters = await _retryService.DoActionAsync<List<ShowPersonCharacter>>(async () => await GetShowCastsAsync(client, currentShow.ID));
                    var currentTVShow = new TVShow()
                    {
                        ID = currentShow.ID,
                        Type = currentShow.Type,
                        URL=currentShow.Url,
                        AverageRuntime = currentShow.AverageRuntime,
                        Ended = currentShow.Ended,
                        Image = currentShow.Image.Medium,
                        Language = currentShow.Language,
                        Name = currentShow.Name,
                        Summary = currentShow.Summary,
                        ScheduleTime = currentShow.Schedule.Time,
                        OfficialSite = currentShow.OfficialSite,
                        Premiered = currentShow.Premiered,
                        Rating = currentShow.Rating.Average,
                        Runtime = currentShow.Runtime,
                        Status = currentShow.Status
                    };

                    foreach(string day in currentShow.Schedule.Days)
                    {
                        if(day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Monday))
                        {
                            currentTVShow.ScheduleMon = true;
                        }
                        else if (day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Tuesday))
                         {
                             currentTVShow.ScheduleTue = true;
                         }
                        else if (day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Wednesday))
                        {
                            currentTVShow.ScheduleWed = true;
                        }
                        else if (day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Thursday))
                        {
                            currentTVShow.ScheduleThu = true;
                        }
                        else if (day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Friday))
                        {
                            currentTVShow.ScheduleFri = true;
                        }
                        else if (day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Saturday))
                        {
                            currentTVShow.ScheduleSat = true;
                        }
                        else if (day == Enum.GetName(typeof(DayOfWeek), DayOfWeek.Sunday))
                        {
                            currentTVShow.ScheduleSun = true;
                        }
                    }

                    foreach(ShowPersonCharacter spc in showPersonCharacters)
                    {
                        if(!characters.Any(x => x.ID == spc.Character.ID))
                        {

                            characters.Add(new TVCharacter()
                            {
                                ID = spc.Character.ID,
                                Image = spc.Character.Image?.Medium,
                                Name = spc.Character.Name,
                                URL = spc.Character?.URL
                            });
                        }

                        if (!casts.Any(x=>x.ID == spc.Person.ID))
                        {
                            casts.Add(new Cast()
                            {
                                ID = spc.Person.ID,
                                Birthday = spc.Person.Birthday,
                                Deathday = spc.Person.Deathday,
                                Gender = spc.Person.Gender?.ToUpper()[0],
                                Image = spc.Person.Image?.Medium,
                                Name = spc.Person.Name,
                                URL = spc.Person.URL,
                                CountryName = spc.Person.Country?.Name,
                                CountryCode = spc.Person.Country?.Code,
                                CountryTZ = spc.Person.Country?.Timezone
                            });
                        }

                        tvShowCasts.Add(new TVShowCast()
                        {
                            IDCast = spc.Person.ID,
                            IDCharacter = spc.Character.ID,
                            IDTV = currentShow.ID
                        });
                       
                    }

                    foreach (string genre in currentShow.Genres)
                    {
                        tvShowGenres.Add(new TVShowGenres()
                        {
                            IDTV = currentShow.ID,
                            Name = genre
                        });
                    }

                    tvShows.Add(currentTVShow);
                }

                var parameters = new DynamicParameters();
                parameters.AddTable("@TVShow", "TVShow", tvShows);
                parameters.AddTable("@Character", "TVCharacter", characters);
                parameters.AddTable("@Cast", "Cast", casts);
                parameters.AddTable("@TVShow_Cast", "TVShow_Cast", tvShowCasts);
                parameters.AddTable("@TVShow_Genres", "TVShow_Genres", tvShowGenres);

                var errMsg = await _baseRepo.QueryAsync<string>("InsertTVShowsWithCast", parameters, commandType: CommandType.StoredProcedure);
            }


        }

        private async Task<List<Show>> GetTVMazeAsync(HttpClient client, int page)
        {
            List<Show> shows = new List<Show>();
            using (var responseMessage = await client.GetAsync("/shows?page=" + page))
            {
                responseMessage.EnsureSuccessStatusCode();

                var respondStr = await responseMessage.Content.ReadAsStringAsync();
                var respond = JsonConvert.DeserializeObject<List<Show>>(respondStr);

                shows.AddRange(respond);
            }

            return shows;
        }

        private async Task<List<ShowPersonCharacter>> GetShowCastsAsync(HttpClient client, int showID)
        {
            bool retry429 = true;
            List<ShowPersonCharacter> showPeople = new List<ShowPersonCharacter>();
            int delaySeconds = _configuration.GetValue<int>(Constant.AppSettings.SleepCallMS);

            while(retry429)
            {
                using (var responseMessage = await client.GetAsync("/shows/" + showID + "/cast"))
                {
                    if (responseMessage.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay(delaySeconds);
                    }
                    else
                    {
                        responseMessage.EnsureSuccessStatusCode();

                        var respondStr = await responseMessage.Content.ReadAsStringAsync();
                        showPeople = JsonConvert.DeserializeObject<List<ShowPersonCharacter>>(respondStr);
                        retry429 = false;
                    }
                }
            }
           

            return showPeople;
        }
    }
}
