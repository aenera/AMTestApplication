using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Interfaces
{
    public interface IConfigService
    {
        Task<string> GetDBConnectionStringAsync();
        Task<string> GetDBPasswordAsync();
    }
}
