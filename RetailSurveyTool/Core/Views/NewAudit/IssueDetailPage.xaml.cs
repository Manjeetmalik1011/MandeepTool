using System;


namespace Jci.RetailSurveyTool.TechnicianApp.Views.NewAudit
{
    public partial class IssueDetailPage : ContentPage
    {
        public IssueDetailPage()
        {
            try
            {
                NavigationPage.SetHasBackButton(this, false);
                InitializeComponent();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
