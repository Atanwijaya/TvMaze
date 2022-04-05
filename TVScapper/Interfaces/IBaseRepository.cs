using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Models;

namespace TVScapper.Interfaces
{
    public interface IBaseRepository
    {
        Task<List<T>> QueryAsync<T>(string commandString, object parameters, CommandType? commandType);
        Task QueryAsync(string commandString, object parameters, CommandType? commandType);
    }
}
