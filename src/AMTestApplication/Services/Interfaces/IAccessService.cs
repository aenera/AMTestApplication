using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.Services.Interfaces
{
    public interface IAccessService
    {
        bool IsAccessEnabled { get; set; }
        string DbPassword { get; set; }
        Task<bool> CheckDbPasswordAsync(string pwd);
        Task<bool> CheckAppPasswordAsync(string pwd);
    }
}
