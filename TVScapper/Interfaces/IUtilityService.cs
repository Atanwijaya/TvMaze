using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Models;

namespace TVScapper.Interfaces
{
    public interface IUtilityService
    {
        Task<TResult> DoActionAsync<TResult>(Func<Task<TResult>> action);
    }
}
