using Jci.RetailSurveyTool.TechnicianApp.Views.ExistingAudit;




namespace Jci.RetailSurveyTool.TechnicianApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookupPreviousPage : ContentPage
    {
        public LookupPreviousPage()
        {
            InitializeComponent();
            Routing.RegisterRoute("ExistingAuditStoreAreaList", typeof(AuditStoreAreaList));
        }
    }
}