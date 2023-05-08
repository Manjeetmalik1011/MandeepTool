using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using JCI.RetailSurveyTool.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit
{
    public partial class IssueListViewModel : BaseViewModel
    {
        private List<IssueType> dbIssueTypes;
        private List<IssueCategory> dbIssueCategories;
        private List<StoreArea> dbStoreArea;

        /// <summary>
        /// UI control properties for List View
        /// </summary>
        const int RefreshDuration = 2;

        bool _isRefreshing;

        public bool IsRefreshing
        {
            //get { return _isRefreshing; }
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Issue list for the UI List View
        /// </summary>
        private ObservableCollection<Issue> _issues;

        public ObservableCollection<Issue> Issues
        {
            get => _issues;
            set
            {
                _issues = value;
                OnPropertyChanged();
                //OnPropertyChanged("Issues");
            }
        }

        public IssueListViewModel(INavigationService navigationService) : base(navigationService)
        {
            Issues = new ObservableCollection<Issue>();
            dbIssueTypes = new List<IssueType>();
            dbIssueCategories = new List<IssueCategory>();
            dbStoreArea = new List<StoreArea>();



        }

        public override async Task InitializeAsync(object parameter)
        {
        }

        public void InitializeMessenger()
        {
            MessagingCenter.Subscribe<NewAuditViewModel, Audit>(this, MessageNames.StartAuditMessage, (newAuditViewModel, audit) => OnStartAudit(audit));
            MessagingCenter.Subscribe<NewAuditViewModel, Customer>(this, MessageNames.SelectedCustomerMessage, (newAuditViewModel, customer) => OnSelectedCustomer(customer));
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel, StoreArea>(this, MessageNames.SelectedStoreAreaMessage, (auditStoreAreaListViewModel, storeArea) => OnSelectedStoreArea(storeArea));

            // clear issue list
            MessagingCenter.Subscribe<AuditStoreAreaListViewModel>(this, MessageNames.NewIssuesListMessage, (auditStoreAreaListViewModel) => OnResetList());



            // Refresh issue list
            MessagingCenter.Subscribe<IssueDetailViewModel>(this, MessageNames.RefreshIssueListMessage, (issueDetailViewModel) => OnLoadCommand()); //upon exiting the detail screen
            MessagingCenter.Subscribe<StoreAreaDetailsViewModel>(this, MessageNames.RefreshIssueListMessage, (storeAreaDetailsViewModel) => OnLoadCommand()); //tabs

        }




        /// <summary>
        /// Selected Customer is set by the MessagingCenter
        /// </summary>
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private void OnSelectedCustomer(Customer customer)
        {
            SelectedCustomer = customer;
        }


        /// <summary>
        /// Selected Audit is set by the MessagingCenter
        /// </summary>
        private Audit _startAudit;

        public Audit StartAudit
        {
            get { return _startAudit; }
            set
            {
                _startAudit = value;
                OnPropertyChanged();
            }
        }

        private void OnStartAudit(Audit audit)
        {
            StartAudit = audit;
        }

        /// <summary>
        /// Selected StoreArea is set by the MessagingCenter
        /// </summary>
        private StoreArea _selectedStoreArea;

        public StoreArea SelectedStoreArea
        {
            get { return _selectedStoreArea; }
            set
            {
                _selectedStoreArea = value;
                OnPropertyChanged();
            }
        }
        private void OnSelectedStoreArea(StoreArea storeArea)
        {
            SelectedStoreArea = storeArea;
        }



        /// <summary>
        ///  OnResetList method exisit because it's a SingleInstance class defined
        ///  on AppContainer class and these classes havea binding to the UI,
        ///  when loading the form it will call get property for each variable.
        /// </summary>

        private void OnResetList()
        {
            IsRefreshing = false;
            Issues = new ObservableCollection<Issue>();
            OnLoadCommand();
        }




        /////// <summary>
        /////// Adds the new issue set by the MessagingCenter 
        /////// Need to decide, if this should be a refresh command to reload the data from the localdb to update the List View.
        /////// </summary>
        /////// 

        ////private void OnAddIssue(Issue issue)
        ////{
        ////    Issues.Add(issue); // only adding in memory for the grid. appends the item to the end of the list. LoadCommand from DB places the item at the top (Bug, change sort order on LoadCommand)
        ////    //OnPropertyChanged(nameof(Issues));
        ////    //OnLoadCommand(); // reload list from DB
        ////}

        /// <summary>
        /// Loads the data for the UI List View
        /// </summary>
        public ICommand LoadCommand => new Command(OnLoadCommand);

        private void OnLoadCommand()
        {
            try
            {
                // Load lookup data
                dbIssueTypes.Clear();
                var issueTypes = LocalAppDatabase.GetRawConnection().Table<IssueType>().ToListAsync().Result;
                issueTypes.ForEach(x => dbIssueTypes.Add(x));

                dbIssueCategories.Clear();
                var issueCategories = LocalAppDatabase.GetRawConnection().Table<IssueCategory>().ToListAsync().Result;
                issueCategories.ForEach(x => dbIssueCategories.Add(x));

                dbStoreArea.Clear();
                var storeAreas = LocalAppDatabase.GetRawConnection().Table<StoreArea>().ToListAsync().Result;
                storeAreas.ForEach(x => dbStoreArea.Add(x));

                Debug.WriteLine("Hello! IssueListViewModel");

                // requires StartAudit.ID and SelectedStoreArea.ID
                var dbIssues = LocalAppDatabase.GetRawConnection().Table<Issue>().Where
                    (x => (x.AuditID == StartAudit.ID && x.StoreAreaID == SelectedStoreArea.ID)).ToListAsync().Result;

                // creates a new list in memory
                var innerjoinresult_as_issues_list = (from issue in dbIssues
                                                      join issueType in dbIssueTypes on issue.IssueTypeID equals issueType.ID
                                                      join issueCategory in dbIssueCategories on issue.IssueCategoryID equals issueCategory.ID
                                                      join storeArea in dbStoreArea on issue.StoreAreaID equals storeArea.ID

                                                      select new Issue
                                                      {
                                                          ID = issue.ID,
                                                          AuditID = issue.AuditID,
                                                          Audit = StartAudit,
                                                          StoreAreaID = issue.StoreAreaID,
                                                          StoreArea = storeArea,

                                                          IssueTypeID = issue.IssueTypeID,
                                                          IssueType = issueType,
                                                          IssueTypeDescription = issueType.Name,

                                                          IssueCategoryID = issue.IssueCategoryID,
                                                          IssueCategory = issueCategory,
                                                          IssueCategoryDescription = issueCategory.Name,

                                                          IssueStatusName = issue.IssueStatusName,
                                                          IssueAreaAssignedName = issue.IssueAreaAssignedName,

                                                          IssueDescription = issue.IssueDescription,

                                                          Repaired = issue.Repaired
                                                      }
                                                        ).ToList();
                Issues.Clear();

                innerjoinresult_as_issues_list.ForEach(x => Issues.Add(x));

                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }


        /// <summary>
        /// Selected Issue item from the UI List View
        /// </summary>

        public ICommand SelectedCommand => new Command<Issue>(OnSelectedCommand);

        private void OnSelectedCommand(Issue issue)
        {
            //MessagingCenter.Send<IssueListViewModel, Issue>(this, MessageNames.SelectedIssueMessage, issue); // Load selected issue details form
            MessagingCenter.Send<IssueListViewModel>(this, MessageNames.SelectedIssueMessage); // reset issue detail form
            App.NavigationService.NavigateTo("IssueDetailPage", issue);
        }


    }
}
