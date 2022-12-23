using AMTestApplication.Models;
using AMTestApplication.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMTestApplication.ViewModels
{
    public class MainWindowViewModel : ObservableObject//, IRecipient<AccessChangedMessage>
    {
        private ILoggerService loggerService;


        private ObservableCollection<Uri> navigationSources;
        
        private Uri navigationSource;
        public Uri NavigationSource { get => navigationSource; set => SetProperty(ref navigationSource, value); }

        public MainWindowViewModel()
        {
            loggerService?.Info("Started");

            navigationSources = new ObservableCollection<Uri>() { new Uri("/Pages/LoginPage.xaml", UriKind.Relative), new Uri("/Pages/DataPage.xaml", UriKind.Relative) };
            NavigationSource = navigationSources.First();

            WeakReferenceMessenger.Default.Register<AccessChangedMessage>(this, (r, m) => { NavigationSource = navigationSources.Last(); });

        }

        ///// <summary>
        ///// Received a message
        ///// </summary>
        ///// <param name="message"></param>
        public void Receive(bool value)
        {
            NavigationSource = navigationSources.Last();
        }
    }
}
