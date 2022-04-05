using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
    public static class Constant
    {
        public const string TVMazeAPIClientName = "TVMazeClient";
        public const string SetupTableFilePath = "DBScripts/Create table TV.sql";

        public static class AppSettings
        {
            public const string MSSQLConnectionString = "MSSQL";
            public const string DefaultPageSize = "DefaultPageSize";
            public const string ScrapPages = "ScrapPages";
            public const string RetryCount = "RetryCount";
            public const string MaximumPageSize = "MaximumPageSize";
            public const string SetupTableData = "SetupTableData";
            public const string SleepCallMS = "SleepCallMS";
            public const string SetupTableTypes = "SetupTableTypes";
            
        }

        public static class ExceptionMessage
        {
            public const string RetryAggregateException = "Failed to execute";
            public const string IDNotFoundException = "Show ID not found!";
        }

    }
}
