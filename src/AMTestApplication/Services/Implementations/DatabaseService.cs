using AMTestApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows.Markup;
using System.IO;
using System.Collections;
using AMTestApplication.Pages;

namespace AMTestApplication.Services.Implementations
{
    public class DatabaseService : IDatabaseService
    {
        private string databaseConnectionString;
        private ILoggerService loggerService;
        public DatabaseService(ILoggerService loggerService)
        {
            this.loggerService = loggerService;
        }

        /// <summary>
        /// Configure connaction using a password from a Login Page
        /// </summary>
        /// <returns></returns>
        private async Task ConfigureConnectionAsync()
        {
            if (!String.IsNullOrEmpty(databaseConnectionString))
                return;

            var configService = Ioc.Default.GetRequiredService<IConfigService>();
            var accessService = Ioc.Default.GetRequiredService<IAccessService>();

            var tempString = await configService.GetDBConnectionStringAsync();
            var sqlConnBuilder = new SqlConnectionStringBuilder(tempString);
            sqlConnBuilder.Password = accessService.DbPassword;
            
            var appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            sqlConnBuilder.AttachDBFilename = Path.Combine(Path.GetDirectoryName(appPath),"Database1.mdf");

            databaseConnectionString = sqlConnBuilder.ToString();
        }

        /// <summary>
        /// Get the whole table [dbo].[Data]
        /// </summary>
        /// <returns></returns>
        public async Task<DataTable> DataGetAsync()
        {
            loggerService.Info("DataGetAsync");
            await ConfigureConnectionAsync();

            DataTable dataTable = new DataTable();
            using (var sqlConnection = new SqlConnection(databaseConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "Select * from [data]";

                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    da.Fill(dataTable);
                    loggerService.Info("ok");
                }
                catch (Exception ex)
                {
                    loggerService.Error(ex.Message);
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Execute a query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task ExecuteNonQueryAsync(string query)
        {
            loggerService.Info("ExecuteNonQueryAsync");
            await ConfigureConnectionAsync();

            using (var sqlConnection = new SqlConnection(databaseConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = query;

                    await sqlCommand.ExecuteNonQueryAsync();
                    loggerService.Info("ok");
                }
                catch (Exception ex)
                {
                    loggerService.Error(ex.Message);
                }
            }
        }

        /// <summary>
        /// Get application password from db
        /// </summary>
        /// <returns></returns>
        public async Task<string> PasswordGetAsync()
        {
            loggerService.Info("PasswordGetAsync");
            await ConfigureConnectionAsync();

            using (var sqlConnection = new SqlConnection(databaseConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    var sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = "Select top 1 password from [password]";

                    var o = await sqlCommand.ExecuteScalarAsync();
                    loggerService.Info("ok");
                    return (string)o;
                }
                catch (Exception ex)
                {
                    loggerService.Error(ex.Message);
                }
            }
            return "";
        }

        /// <summary>
        /// Write data from a file to the RawData table
        /// </summary>
        /// <param name="fileName"></param>
        public async Task RawDataPostAsync(string fileName)
        {
            loggerService.Info("RawDataPostAsync");
            await ConfigureConnectionAsync();

            await ExecuteNonQueryAsync("truncate table [RawData]");
            await ExecuteNonQueryAsync("truncate table [Data]");

            var lines = await System.IO.File.ReadAllLinesAsync(fileName);
            if (lines.Count() == 0) 
                return;
            var columns = lines[0].Split(',');
            var table = new DataTable();
            foreach (var c in columns)
                table.Columns.Add(c);

            for (int i = 1; i < lines.Count() - 1; i++)
                table.Rows.Add(lines[i].Split(','));

            var sqlBulk = new SqlBulkCopy(databaseConnectionString);
            sqlBulk.DestinationTableName = "[RawData]";

            try
            {
                await sqlBulk.WriteToServerAsync(table);
                loggerService.Info("ok");
            }
            catch (Exception ex)
            {
                loggerService.Error(ex.Message);
            }
        }

        
    }
}
