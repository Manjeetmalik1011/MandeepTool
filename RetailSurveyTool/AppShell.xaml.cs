using Jci.RetailSurveyTool.TechnicianApp.Views;
using System;
using Xamarin.Essentials;

namespace Jci.RetailSurveyTool.TechnicianApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
           try
            {
                InitializeComponent();
                Routing.RegisterRoute(nameof(SyncPage), typeof(SyncPage));
                //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
                if (App.objUserModel != null && !string.IsNullOrEmpty(App.objUserModel.DisplayName))
                {
                    userNameID.Text = App.objUserModel.DisplayName;
                }
                //LbLAppVersion.Text = AppInfo.VersionString;
            }
            catch(Exception ex)
            {

            }
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ForceSyncPage();
        }
    }
}
