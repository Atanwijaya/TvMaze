using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TVScapper.Exceptions;
using TVScapper.Interfaces;
using TVScapper.Models;
using TVScapper.Services;
using Xunit;

namespace TVScrapNet
{
    public class CorrectException
    {
        [Fact]
        public async Task Test1()
        {
            Mock<IBaseRepository> tvMaze = new Mock<IBaseRepository>(MockBehavior.Strict);
            tvMaze.Setup(x => x.QueryAsync<TVShowDBModel>("GetTVShowsWithCastByIDAsync", It.IsAny<object>(), CommandType.StoredProcedure))
                .ReturnsAsync(() => { return new List<TVShowDBModel>(); });

            TVMazeService tVMazeService = new TVMazeService(tvMaze.Object, null);
            Assert.ThrowsAsync<NotFoundException>(async () => await tVMazeService.GetTVShowsWithCastByIDAsync(3123123));
        }
    }
}
