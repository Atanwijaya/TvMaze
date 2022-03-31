using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Models;

namespace TVScapper.Interfaces
{
    public interface ITVMazeRepository
    {
        Task InsertTVShowsWithCast(List<TVShow> tvShows, List<Character> characters, List<Cast> casts);
        Task GetTVShowsWithCast(int offset, int limit);
    }
}
