using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Jci.RetailSurveyTool.TechnicianApp.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using SQLite;


namespace Jci.RetailSurveyTool.TechnicianApp.Utility
{
    public static class ErrorHandler
    {
        private static string AndroidSecret = "fb2c5dab-d76f-450d-80ed-e7935d221eb8;";
        private static string IosSecret = "ad787ee6-86e0-4fb0-bc59-86e8ade34c4c;";
        

        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Add custom properties to the exception
           
        }

        public static void StartAppTracking()
        {
            AppCenter.Start($"android={AndroidSecret}" +
                  $"ios={IosSecret}", typeof(Crashes));
        }


        public static async void DisplayError(string errorMsg)
        {
            await App.Current.MainPage.DisplayAlert("Error", errorMsg, "Report Now");
        }

        public static async Task<string> APICustomExceptionMsg(HttpResponseMessage httpResponse)
        {
            string errorResponse = await httpResponse.Content.ReadAsStringAsync();
            return $"Request failed with status code {httpResponse.StatusCode} Response message: {errorResponse}";
        }


        public static void SendExceptionReport(Exception e,params KeyValuePair<string, object>[] keyValuePairs)
        {
            AppCenter.SetUserId(App.objUserModel?.Mail);
            var properties = new Dictionary<string, string> {};

            foreach (var keyValue in keyValuePairs)
            {
                properties.Add(keyValue.Key, keyValue.Value.ToString());
            }

            Crashes.TrackError(e, properties);

    }

       
    }
}
