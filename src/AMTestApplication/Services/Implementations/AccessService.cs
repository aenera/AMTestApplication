using AMTestApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AMTestApplication.Services.Implementations
{
    public class AccessService : IAccessService
    {
        public bool IsAccessEnabled { get; set; }
        public string DbPassword { get; set; }

        private IDatabaseService databaseService;
        private IConfigService configService;

        public AccessService(IDatabaseService databaseService, IConfigService configService)
        {
            this.databaseService = databaseService;
            this.configService = configService;
        }

        /// <summary>
        /// Check application password
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<bool> CheckAppPasswordAsync(string pwd)
        {
            if (String.IsNullOrEmpty(pwd))
                return false;

            var result = await databaseService.PasswordGetAsync();

            return result.ToUpper() == CreateMD5(pwd).ToUpper();
        }

        /// <summary>
        /// Check Database password
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<bool> CheckDbPasswordAsync(string pwd)
        {
            if (String.IsNullOrEmpty(pwd))
                return false;

            var result = await configService.GetDBPasswordAsync();
            
            if (result.ToUpper() == CreateMD5(pwd).ToUpper())
            {
                DbPassword = pwd;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get MD5 hash
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }

    }
}
