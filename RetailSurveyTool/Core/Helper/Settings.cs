using Xamarin.Essentials;
using Preferences = Microsoft.Maui.Storage.Preferences;

namespace Jci.RetailSurveyTool.TechnicianApp.Helpers
{
    public static class Settings
    {
        // 0 = default, 1 = light, 2 = dark
        const int theme = 0;
        public static int Theme
        {
            get => Preferences.Get(nameof(Theme), theme);
            set => Preferences.Set(nameof(Theme), value);
        }
    }
}
