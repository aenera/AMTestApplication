using AMTestApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Implementations
{
    public class ConfigService : IConfigService
    {
        IDatabaseService databaseService;
        ILoggerService loggerService;
        public ConfigService(IDatabaseService databaseService, ILoggerService loggerService)
        {
            this.databaseService = databaseService;
            this.loggerService = loggerService;
        }
        public async Task<string> GetDBConnectionStringAsync()
        {
            try
            {
                return ConfigurationManager.AppSettings["DbConnectionString"];
            }
            catch (Exception ex)
            {
                loggerService.Error(ex.Message);
            }
            return "";
        }

        public async Task<string> GetDBPasswordAsync()
        {
            return await Task.Run(() => { return ConfigurationManager.AppSettings["DbPassword"]; });

        }
    }
}
