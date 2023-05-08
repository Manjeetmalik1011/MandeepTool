using Jci.RetailSurveyTool.TechnicianApp.Services;
using System.Windows.Input;



namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public class SelectInventoryViewModel : BaseViewModel
    {
        public SelectInventoryViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public ICommand GoToPedestalInventory => new Command(OnGoToPedestalInventory);
        public ICommand GoToDeactivateInventory => new Command(OnGoToDeactivateInventory);

        private void OnGoToPedestalInventory()
        {

            App.NavigationService.NavigateTo("PedestalInventoryDetailsPage");
        }

        private void OnGoToDeactivateInventory()
        {

            App.NavigationService.NavigateTo("DeactivationInventoryDetailsPage");
        }
    }



}
