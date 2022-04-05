using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Interfaces;
using TVScapper.Models;
using TVScapper.Services;
using Xunit;

namespace TVScrapNet
{
    public class UnitTest1
    {
        [Fact]
        public async Task HasMoreIsCorrect()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {Constant.AppSettings.MaximumPageSize, "5"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            Mock<IBaseRepository> tvMaze = new Mock<IBaseRepository>(MockBehavior.Strict);

            tvMaze.Setup(x => x.QueryAsync<TVShowDBModel>("GetTVShowsWithCast", It.IsAny<object>(), CommandType.StoredProcedure))
                .ReturnsAsync((string spName, object spParam, CommandType? commandType) =>
                {
                    return new List<TVShowDBModel>()
                    {
                        new TVShowDBModel()
                        {
                            ID = 3
                        },
                         new TVShowDBModel()
                        {
                            ID = 4
                        }, 
                        new TVShowDBModel()
                        {
                            ID = 5
                        }
                    };
                });

            var tvMazeService = new TVMazeService(tvMaze.Object, configuration);
            var pagedTV = await tvMazeService.GetTVShowsWithCastAsync(2, 2);

            pagedTV.Items = null;
            var example = new TVShowPage()
            {
                HasMore = true,
                PageNumber = 2,
                PageSize = 2,
                Items = null
            };


            Assert.Equal(example.HasMore, pagedTV.HasMore);
            Assert.Equal(example.PageNumber, pagedTV.PageNumber);
            Assert.Equal(example.PageSize, pagedTV.PageSize);
        }
    }
}
