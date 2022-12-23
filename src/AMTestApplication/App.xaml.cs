using AMTestApplication.Services.Implementations;
using AMTestApplication.Services.Interfaces;
using AMTestApplication.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AMTestApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Register services
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddTransient<MainWindowViewModel>()
                .AddTransient<LoginPageViewModel>()
                .AddTransient<DataPageViewModel>()
                .AddTransient<IBusinessLogic, BusinessLogic>()
                .AddTransient<IFileService, FileService>()
                .AddTransient<IConfigService, ConfigService>()
                .AddSingleton<IAccessService, AccessService>()
                .AddSingleton<IDatabaseService, DatabaseService>()
                .AddSingleton<ILoggerService, LoggerService>()
                .BuildServiceProvider());
        }
    }
}
