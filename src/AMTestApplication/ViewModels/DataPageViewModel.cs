using AMTestApplication.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.ViewModels
{
    public class DataPageViewModel : ObservableObject
    {
        public AsyncRelayCommand LoadFileCommandAsync { get; }

        private DataTable someData;
        public DataTable SomeData { get => someData; set => SetProperty(ref someData, value); }

        private IFileService fileService;
        private ILoggerService loggerService;
        private IBusinessLogic businessLogic;
        private IDatabaseService databaseService;

        public DataPageViewModel(IFileService fileService, ILoggerService loggerService, IBusinessLogic businessLogic, IDatabaseService databaseService)
        {
            this.fileService = fileService;
            this.loggerService = loggerService;
            this.businessLogic = businessLogic;
            this.databaseService = databaseService;

            LoadFileCommandAsync = new AsyncRelayCommand(LoadFileCommandAsyncExecute);
        }

        private async Task LoadFileCommandAsyncExecute()
        {
            loggerService.Info("Loading a file...");
            var fileName = fileService.SelectFile();
            if (String.IsNullOrEmpty(fileName))
            {
                loggerService.Info("Failed");
                return;
            }

            loggerService.Info($"File is {fileName}");

            loggerService.Info("Data loading started...");
            await databaseService.RawDataPostAsync(fileName);

            var processedData = await businessLogic.CalculateDataAsync();
            if (processedData != null)
            {
                loggerService.Info("Data processed");
                SomeData = processedData;
            }
        }


    }
}
