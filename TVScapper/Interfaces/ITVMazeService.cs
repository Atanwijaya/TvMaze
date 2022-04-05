using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Models;

namespace TVScapper.Interfaces
{
    public interface ITVMazeService
    {
        Task<TVShowVM> GetTVShowsWithCastByIDAsync(int ID);
        Task<TVShowPage> GetTVShowsWithCastAsync(int page, int? limit);
    }
}
