using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public partial class CustomerSelectionViewModel : BaseViewModel
    {
        public CustomerSelectionViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        public override async Task InitializeAsync(object parameter)
        {
            Customers = new ObservableCollection<Customer>(App.CustomerDataService.GetAllCustomers());
        }


        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        const int RefreshDuration = 2;

        bool isRefreshing;

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged("Customers");
            }
        }


        public ICommand LoadCommand => new Command(OnLoadCommand);
        public ICommand SelectedCommand => new Command<Customer>(OnSelectedCommand);


        private void OnLoadCommand()
        {
            IsRefreshing = true;
            Customers = new ObservableCollection<Customer>(App.CustomerDataService.GetAllCustomers());
            IsRefreshing = false;
        }


        private void OnSelectedCommand(Customer customer)
        {
            SelectedCustomer = customer;
            MessagingCenter.Send<CustomerSelectionViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, SelectedCustomer);
            ////Works MessagingCenter.Send(this, MessageNames.SelectedCustomerMessage, SelectedCustomer);
            App.NavigationService.GoBack();
        }


    }
}
