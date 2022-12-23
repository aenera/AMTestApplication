using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace AMTestApplication.Services.Interfaces
{
    public interface IDatabaseService
    {
        Task<string> PasswordGetAsync();

        Task<DataTable> DataGetAsync();
        Task RawDataPostAsync(string fileName);

        Task ExecuteNonQueryAsync(string query);
    }
}
