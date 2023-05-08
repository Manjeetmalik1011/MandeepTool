

namespace Jci.RetailSurveyTool.TechnicianApp.Views.ExistingAudit
{
    //[QueryProperty(nameof(AuditId), "audit")]
    public partial class AuditStoreAreaList : ContentPage
    {
        //private string auditId;

        //public string AuditId
        //{
        //    get => auditId; set
        //    {
        //        auditId = value;
        //        ((NewAuditViewModel)this.BindingContext).LoadExistingData(int.Parse(value));
        //    }
        //}

        public AuditStoreAreaList()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(StoreAreaDetailsPage), typeof(StoreAreaDetailsPage));
        }
    }
}
