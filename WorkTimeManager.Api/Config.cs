using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeManager.Api
{
    public static class Config
    {
        #region Database
        private static readonly string _dbUser = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string _dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
        private static readonly string _dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        private static readonly string _dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        private static readonly string _dbName = Environment.GetEnvironmentVariable("DB_NAME");

        //PostgreSQL DB Connection String
        public static readonly string DbConnectionString = $"User ID={_dbUser};" +
                                                           $"Password={_dbPassword};" +
                                                           $"Host={_dbHost};" +
                                                           $"Port={_dbPort};" +
                                                           $"Database={_dbName};" +
                                                           $"SSL Mode=Require;" +
                                                           $"Trust Server Certificate=true";
        #endregion
    }
}
