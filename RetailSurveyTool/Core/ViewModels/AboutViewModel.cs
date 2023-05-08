using Jci.RetailSurveyTool.TechnicianApp.Services;
using System.Windows.Input;
using Xamarin.Essentials;
using Browser = Xamarin.Essentials.Browser;

namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }
    }
}