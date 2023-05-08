using Jci.RetailSurveyTool.TechnicianApp.Controls;


namespace Jci.RetailSurveyTool.TechnicianApp.Views.NewAudit
{
    public partial class StoreAreaDetailsPage : TabbedPage
    {
        public StoreAreaDetailsPage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(IssueDetailPage), typeof(IssueDetailPage));
            Routing.RegisterRoute(nameof(DeactivationInventoryDetailsPage), typeof(DeactivationInventoryDetailsPage));
            Routing.RegisterRoute(nameof(PedestalInventoryDetailsPage), typeof(PedestalInventoryDetailsPage));
        }
    }
}
