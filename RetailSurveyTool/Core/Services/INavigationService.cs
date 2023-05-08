using System;


namespace Jci.RetailSurveyTool.TechnicianApp.Services
{
    public interface INavigationService
    {
        Page MainPage { get; }

        void Configure(string key, Type pageType);
        void GoBack();
        void NavigateTo(string pageKey, object parameter = null);
    }
}
