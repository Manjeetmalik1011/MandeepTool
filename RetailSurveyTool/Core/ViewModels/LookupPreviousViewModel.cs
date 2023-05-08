using Jci.RetailSurveyTool.TechnicianApp.Attributes;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using static Jci.RetailSurveyTool.TechnicianApp.Utility.ErrorHandler;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels
{
    public class LookupPreviousViewModel : BaseViewModel
    {
        public LookupPreviousViewModel(INavigationService navigationService) : base(navigationService)
        {
            CommandTaskAttribute.InitCommands(this);

            LoadCustomersCommand.Execute(null);
        }

        [CommandTask(nameof(LoadCustomersTask))]
        public Command LoadCustomersCommand { private set; get; }
        public async Task LoadCustomersTask()
        {
            Customers.Clear();
            foreach (var cust in (await LocalAppDatabase.GetCustomerAsync()).OrderBy(x => x.Name))
            {

                Customers.Add(cust);

            }
        }

        public Customer SelectedCustomer { set; get; }
        public string SearchString { set; get; }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }


        [CommandTask(nameof(DoSearchTask))]
        public Command DoSearchCommand { private set; get; }

        public ObservableCollection<Customer> Customers { get; } = new ObservableCollection<Customer>();

        public ObservableCollection<Audit> Audits { get; } = new ObservableCollection<Audit>();

        public async Task DoSearchTask()
        {

            if (SelectedCustomer == null)
            {
                await Shell.Current.DisplayAlert("Info", "Please Select A Customer First", "OK");
                return;
            }
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                await Shell.Current.DisplayAlert("Info", "Please Enter A Store # First", "OK");
                return;
            }


            try
            {
                IsRefreshing = true;

                var apiResults = await LocalAppDatabase.restService.SearchForAudits(SelectedCustomer.ID, SearchString);
                Audits.Clear();

                foreach (var audit in apiResults)
                {
                    Audits.Add(audit);
                }
            }
            catch (Exception e)
            {
                DisplayError("Server error, failed to load the audits!");
                SendExceptionReport(e);
            }
            finally
            {
                IsRefreshing = false;
            }


           

           
        }



        /*public Audit SelectedAudit
        {
            get => selectedAudit; set
            {
                SetProperty(ref selectedAudit, value);
                if (selectedAudit != null)
                {
                    Shell.Current.GoToAsync($"ExistingAuditStoreAreaList?audit={value.ID}");
                }
            }
        }*/

        //private SVMX_WO _woselectedItem;
        private Audit selectedAudit;
        public Audit SelectedAudit
        {
            get => selectedAudit;
            set
            {
                if (value != null)
                {
                    selectedAudit = value;
                    OnPropertyChanged("SelectedAudit");
                    App.NavigationService.NavigateTo("PreviousAuditDetailsPage", value);
                    selectedAudit = null;
                }

            }
        }
    }
}
