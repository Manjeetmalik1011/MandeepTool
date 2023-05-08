using Jci.RetailSurveyTool.TechnicianApp.Services;
using JCI.RetailSurveyTool.DataBase.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.ExistingAudit
{
    public class PreviousAuditDetailsViewModel : BaseViewModel
    {

        private Audit _audit;

        public Audit Audit
        {
            get => _audit;
            set
            {
                _audit = value;
                OnPropertyChanged();
            }
        }

        private StoreType _storeType;

        public StoreType StoreType
        {
            get => _storeType;
            set
            {
                _storeType = value;
                OnPropertyChanged();
            }
        }

        private int _inventoriesNumber;

        public int InventoriesNumber
        {
            get => _inventoriesNumber;
            set
            {
                _inventoriesNumber = value;
                OnPropertyChanged();
            }

        }

        private int _issuesNumber;
        public int IssuesNumber
        {
            get => _issuesNumber;
            set
            {
                _issuesNumber = value;
                OnPropertyChanged();
            }
        }

        public PreviousAuditDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }



        public override async Task InitializeAsync(object parameter)
        {
            Audit = (Audit)parameter;
            StoreType = ((await LocalAppDatabase.GetRawConnection().Table<StoreType>().Where(x => x.ID == Audit.StoreTypeID).FirstOrDefaultAsync()));

            IssuesNumber = Audit.Issues.Count;
            InventoriesNumber = Audit.Inventories.Sum(x => x.TotalQty);

        }
    }
}
