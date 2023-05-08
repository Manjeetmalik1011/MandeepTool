

namespace Jci.RetailSurveyTool.TechnicianApp.Helpers
{
    public static class TheTheme
    {
        public static void SetTheme()
        {
            switch (Settings.Theme)
            {
                //default
                case 0:
                    App.Current.UserAppTheme = AppTheme.Unspecified;
                    break;
                //light
                case 1:
                    App.Current.UserAppTheme = AppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = AppTheme.Dark;
                    break;
            }

            var nav = App.Current.MainPage as NavigationPage;

            //var e = DependencyService.Get<IEnvironment>();
            //if (App.Current.RequestedTheme == AppTheme.Dark)
            //{
            //    e?.SetStatusBarColor(Color.Black, false);
            //    if (nav != null)
            //    {
            //        nav.BarBackgroundColor = Color.Black;
            //        nav.BarTextColor = Color.FromHex("#f7f7f7");
            //    }
            //}
            //else
            //{
            //    e?.SetStatusBarColor(Color.FromHex("#f7f7f7"), true);
            //    if (nav != null)
            //    {
            //        nav.BarBackgroundColor = Color.White;
            //        nav.BarTextColor = Color.Black;
            //    }
            //}


        }
    }
}
