using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Interfaces
{
    public interface ILoggerService
    {
        void Info(string message);
        void Error(string message);
    }
}
