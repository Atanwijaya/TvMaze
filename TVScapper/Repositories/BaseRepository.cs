using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using TVScapper.Interfaces;
using TVScapper.Models;

namespace TVScapper.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly IConfiguration _config;
        public BaseRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string commandString, object parameters, CommandType? commandType)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString(Constant.AppSettings.MSSQLConnectionString)))
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                try
                {
                   return await dbConnection.QueryAsync<T>(commandString, parameters, commandType: commandType);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                }
            }
        }

        public async Task QueryAsync(string commandString, object parameters, CommandType? commandType)
        {
            using (IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString(Constant.AppSettings.MSSQLConnectionString)))
            {
                if (dbConnection.State != ConnectionState.Open)
                    dbConnection.Open();

                try
                {
                    var scripts = commandString.Split("GO");
                    foreach (string script in scripts)
                    {
                        if(!string.IsNullOrEmpty(script))
                        {
                            await dbConnection.QueryAsync(script, parameters, commandType: commandType);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (dbConnection.State == ConnectionState.Open)
                        dbConnection.Close();
                }
            }
        }
    }
}
