using Microsoft.Maui.ApplicationModel;
using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Jci.RetailSurveyTool.TechnicianApp.Converters
{
    internal class StoreListBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IssueInventoryLogged = (bool)value;
            var currentTheme = Application.Current.RequestedTheme;

            if (currentTheme == AppTheme.Light && IssueInventoryLogged)
            {
                return Color.FromHex("#DECAC2");

            }
            else if (currentTheme == AppTheme.Dark && IssueInventoryLogged)
            {
                return Color.FromHex("#3C2A21");
            }
            else if (currentTheme == AppTheme.Dark)
            {
                return Color.FromHex("#242526");
            }
            return Color.FromHex("#f7f7f7");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private bool ContainsNumber(string text)
        {
            return Regex.IsMatch(text, @"\d");
        }
    }
}
