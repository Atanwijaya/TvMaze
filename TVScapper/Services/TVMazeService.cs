using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TVScapper.Exceptions;
using TVScapper.Interfaces;
using TVScapper.Models;
using TVScapper.Repositories;

namespace TVScapper.Services
{
    public class TVMazeService : ITVMazeService
    {
        private readonly IBaseRepository _baseRepo;
        private readonly IConfiguration _configuration;
        public TVMazeService(IBaseRepository baseRepo, IConfiguration configuration)
        {
            _baseRepo = baseRepo;
            _configuration = configuration;
        }

        public async Task<TVShowPage> GetTVShowsWithCastAsync(int page, int? limit)
        {
            var maxPageSize = _configuration.GetValue<int>(Constant.AppSettings.MaximumPageSize);
            int limitAsInt = 0;
            if (limit == null || limit > maxPageSize)
            {
                limitAsInt = maxPageSize;
            }
            else
                limitAsInt = (int)limit;

            int offSet = page * limitAsInt;
            limitAsInt++;
            var tvShowsDB = (await _baseRepo.QueryAsync<TVShowDBModel>("GetTVShowsWithCast", new { Offset = offSet, Limit = limitAsInt }, CommandType.StoredProcedure)).ToList();
          
            return ConstructViewModel(tvShowsDB, page, limitAsInt);
        }

        public async Task<TVShowVM> GetTVShowsWithCastByIDAsync(int ID)
        {
            var tvShowsDB = (await _baseRepo.QueryAsync<TVShowDBModel>("GetTVShowsWithCastByID", new { ID = ID}, CommandType.StoredProcedure)).ToList();

            if (!tvShowsDB.Any())
                throw new NotFoundException(Constant.ExceptionMessage.IDNotFoundException);

            var tvShow = ConstructViewModel(tvShowsDB, 0, 2);

            return tvShow.Items.First();

        }

        private TVShowPage ConstructViewModel(List<TVShowDBModel> tvShowsDB, int page, int limitAsInt)
        {

            int totalItem = tvShowsDB.Count;
            bool hasMore = false;
            var tvShowPage = new TVShowPage()
            {
                PageNumber = page,
                PageSize = limitAsInt - 1,
                HasMore = hasMore,
                Items = new List<TVShowVM>()
            };

            for (int i = 0; i < totalItem; i++)
            {
                var currentDBItem = tvShowsDB.ElementAt(i);

                int index = tvShowPage.Items.FindIndex(x => x.ID == currentDBItem.ID);
                if (index == -1 && limitAsInt - tvShowPage.Items.Count == 1)
                {
                    tvShowPage.HasMore = true;
                    break;
                }

                Dictionary<int, Cast> castsDB = new Dictionary<int, Cast>();
                Dictionary<int, TVShow> tvShowDB = new Dictionary<int, TVShow>();
                Dictionary<int, TVCharacter> charDB = new Dictionary<int, TVCharacter>();

                var cast = new Cast()
                {
                    ID = currentDBItem.CastID,
                    Birthday = currentDBItem.CastBirthday,
                    CountryCode = currentDBItem.CastCountryCode,
                    CountryName = currentDBItem.CastCountryName,
                    CountryTZ = currentDBItem.CastCountryTZ,
                    Deathday = currentDBItem.CastDeathday,
                    Gender = currentDBItem.CastGender,
                    Image = currentDBItem.CastImage,
                    Name = currentDBItem.CastName,
                    URL = currentDBItem.CastURL
                };

                var character = new TVCharacter()
                {
                    ID = currentDBItem.CharacterID,
                    Image = currentDBItem.CharacterIMG,
                    URL = currentDBItem.CharacterURL,
                    Name = currentDBItem.CharacterName
                };

                var tvShow = new TVShowVM()
                {
                    ID = currentDBItem.ID,
                    Image = currentDBItem.Image,
                    URL = currentDBItem.URL,
                    Name = currentDBItem.Name,
                    AverageRuntime = currentDBItem.AverageRuntime,
                    Ended = currentDBItem.Ended?.ToString("yyyy-MM-dd"),
                    Language = currentDBItem.Language,
                    OfficialSite = currentDBItem.OfficialSite,
                    Premiered = currentDBItem.Premiered?.ToString("yyyy-MM-dd"),
                    Rating = currentDBItem.Rating,
                    Runtime = currentDBItem.Runtime,
                    Status = currentDBItem.Status,
                    Summary = currentDBItem.Summary,
                    Type = currentDBItem.Type
                };




                if (!castsDB.TryGetValue(cast.ID, out Cast castExisting))
                {
                    castsDB.Add(cast.ID, cast);
                }
                else
                    cast = castExisting;


                if (!charDB.TryGetValue(character.ID, out TVCharacter characterExisting))
                {
                    charDB.Add(character.ID, character);
                }
                else
                    character = characterExisting;

                if (index == -1)
                {
                    List<string> schedules = new List<string>();
                    if (currentDBItem.ScheduleMon)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Monday));

                    if (currentDBItem.ScheduleTue)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Tuesday));

                    if (currentDBItem.ScheduleWed)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Wednesday));

                    if (currentDBItem.ScheduleThu)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Thursday));

                    if (currentDBItem.ScheduleFri)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Friday));

                    if (currentDBItem.ScheduleSat)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Saturday));

                    if (currentDBItem.ScheduleSun)
                        schedules.Add(Enum.GetName(typeof(DayOfWeek), DayOfWeek.Sunday));

                    tvShow.Genres = new List<string>() { currentDBItem.Genre };
                    tvShow.Schedules = new TVShowSchedules() { Days = schedules, time = currentDBItem.ScheduleTime };
                    tvShow.Characters = new List<PersonCharacter>()
                        {
                            new PersonCharacter()
                            {
                                Character = character,
                                Person = cast
                            }
                        };

                    tvShowPage.Items.Add(tvShow);
                }
                else
                {
                    var elementRef = tvShowPage.Items.ElementAt(index);
                    if (!elementRef.Genres.Any(x => x == currentDBItem.Genre))
                    {
                        elementRef.Genres.Add(currentDBItem.Genre);
                    }

                    if (!elementRef.Characters.Any(x => x.Character.ID == character.ID && x.Person.ID == cast.ID))
                    {
                        elementRef.Characters.Add(new PersonCharacter()
                        {
                            Character = character,
                            Person = cast
                        });
                    }

                }

            }

            return tvShowPage;
            
        }
    }
}
