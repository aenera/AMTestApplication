using AMTestApplication.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.Messaging;
using AMTestApplication.Models;

namespace AMTestApplication.ViewModels
{
    public class LoginPageViewModel : ObservableObject
    {
        private string databasePassword;
        public string DatabasePassword { get => databasePassword; set => SetProperty(ref databasePassword, value); }

        private string applicationPassword;
        public string ApplicationPassword { get => applicationPassword; set => SetProperty(ref applicationPassword, value); }

        private bool isIncorrectLogin;
        public bool IsIncorrectLogin { get => isIncorrectLogin; set 
        {
            SetProperty(ref isIncorrectLogin, value);
            if (isIncorrectLogin)
            {
                    Task.Run(() => { Thread.Sleep(3000); IsIncorrectLogin = false; });
            }
        } }

        public AsyncRelayCommand LoginCommandAsync { get; }

        private readonly IAccessService accessInterface;
        private readonly ILoggerService loggerService;

        public LoginPageViewModel(IAccessService accessInterface, ILoggerService loggerService)
        {
            this.accessInterface = accessInterface;
            this.loggerService = loggerService;
            LoginCommandAsync = new AsyncRelayCommand(LoginCommandAsyncExecute);
        }

        /// <summary>
        /// Check passwords
        /// </summary>
        /// <returns></returns>
        private async Task LoginCommandAsyncExecute()
        {
            loggerService.Info("Signing in...");
            var result = await accessInterface.CheckDbPasswordAsync(DatabasePassword);
            if (!result)
            {
                loggerService.Info("Database password is incorrect");
                IsIncorrectLogin = true;
                DatabasePassword = "";
                ApplicationPassword = "";
                return;
            }
            var anotherResult = await accessInterface.CheckAppPasswordAsync(ApplicationPassword);
            if (!anotherResult)
            {
                loggerService.Info("Application password is incorrect");
                IsIncorrectLogin = true;
                DatabasePassword = "";
                ApplicationPassword = "";
                return;
            }

            loggerService.Info("Signed in successfully");
            accessInterface.IsAccessEnabled = true;
            // Send a message from some other module
            WeakReferenceMessenger.Default.Send(new AccessChangedMessage(accessInterface.IsAccessEnabled));
        }
    }
}
