using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TVScapper.Interfaces;
using TVScapper.Models;

namespace TVScapper.Services
{
    public class RetryService : IUtilityService
    {
        private readonly IConfiguration _configuration;
        public RetryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TResult> DoActionAsync<TResult>(Func<Task<TResult>> action)
        {
            int retryCount = _configuration.GetValue<int>(Constant.AppSettings.RetryCount);
            List<Exception> exceptions = new List<Exception>();

            TResult result = default(TResult);
            bool isSuccess = true;

            for (int i = 1; i <= retryCount; i++)
            {
                try
                {
                    result = await action();
                    break;
                }
                catch(Exception ex)
                {
                    exceptions.Add(ex); 

                    if (i == retryCount)
                        isSuccess = false;
                }
            }

            if (isSuccess)
                return result;
            else
                throw new AggregateException(Constant.ExceptionMessage.RetryAggregateException, exceptions);
        }
    }
}
