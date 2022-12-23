using AMTestApplication.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Implementations
{
    public class LoggerService : ILoggerService
    {
        private Logger logger = null;
        public LoggerService()
        {
            NLog.LogManager.Setup().LoadConfiguration(builder => {
                builder.ForLogger().FilterMinLevel(NLog.LogLevel.Trace).WriteToFile(fileName: "applog.txt");
            });
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Error(string message)
        {
            if (logger != null)
                logger.Error(message);
        }

        public void Info(string message)
        {
            if (logger != null)
                logger.Info(message);
        }
    }
}
