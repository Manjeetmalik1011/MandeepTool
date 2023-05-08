using System;



namespace Jci.RetailSurveyTool.TechnicianApp.Views.NewAudit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAuditPage : ContentPage
    {
        public NewAuditPage()
        {
            try
            {
                InitializeComponent();
                Routing.RegisterRoute(nameof(CustomerSelectionPage), typeof(CustomerSelectionPage));
                Routing.RegisterRoute(nameof(AuditStoreAreaList), typeof(AuditStoreAreaList));
            }
            catch (Exception ex)
            {

            }
        }

    }
}