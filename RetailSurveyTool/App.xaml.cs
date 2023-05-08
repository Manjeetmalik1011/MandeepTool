using Autofac;
using Jci.RetailSurveyTool.TechnicianApp.Bootstrap;
using Jci.RetailSurveyTool.TechnicianApp.Data;
using Jci.RetailSurveyTool.TechnicianApp.Helpers;
using Jci.RetailSurveyTool.TechnicianApp.Models;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.Utility;
using Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit;
using Jci.RetailSurveyTool.TechnicianApp.Views;
using Jci.RetailSurveyTool.TechnicianApp.Views.NewAudit;
using Microsoft.Identity.Client;
using System;
using System.IO;
using Xamarin.Essentials;
using System.Collections.Generic;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter;
using Microsoft.Maui.Controls.Internals;

namespace Jci.RetailSurveyTool.TechnicianApp
{
    public partial class App : Application
    {
        public static object ParentWindow { get; set; }
        public static UserModel objUserModel { get; internal set; }
        public static string[] Scopes = { "User.Read" };
        public static IPublicClientApplication PCA = null;
        private const string TenantId = "a1f1e214-7ded-45b6-81a1-9e8ae3459641";
        private const string ClientId = "3bf71188-13e7-4563-b683-efd9ed4df3f4";
        private const string RedirectURI = "msal3bf71188-13e7-4563-b683-efd9ed4df3f4://auth";
        public static NavigationService NavigationService { get; } = new NavigationService();

        public static CustomerDataService CustomerDataService { get; } = new CustomerDataService();

        static Autofac.IContainer container;
        static readonly ContainerBuilder builder = new ContainerBuilder();
        bool built = false;
        public App()
        {
            InitializeComponent();
            ErrorHandler.StartAppTracking();
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            SetUpServices();
            InitializeApp();
            Init();
            MainPage = new LoginPage();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            AppCenter.SetUserId(objUserModel.Mail);
            Crashes.TrackError(ex);
            //ErrorHandler.SendExceptionReport(e.ExceptionObject as Exception);
        }
        protected override void OnStart()
        {
            OnResume();
        }

        protected override void OnSleep()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            TheTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }
        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(() =>
            {
                TheTheme.SetTheme();
            });
        }
        public void Init()
        {
            // Configure App Views
            NavigationService.Configure(ViewNames.AuditConfirmationPage, typeof(AuditConfirmationPage));
            NavigationService.Configure(ViewNames.PreviousAuditDetailsPage, typeof(Jci.RetailSurveyTool.TechnicianApp.Views.ExistingAudit.PreviousAuditDetailsPage));
            NavigationService.Configure(ViewNames.SelectInventoryPage, typeof(SelectInventoryPage));
            NavigationService.Configure(ViewNames.JobListPage, typeof(JobListPage));
            NavigationService.Configure(ViewNames.NewAuditPage, typeof(NewAuditPage));
            NavigationService.Configure(ViewNames.CustomerSelectionPage, typeof(CustomerSelectionPage));
            NavigationService.Configure(ViewNames.AuditStoreAreaList, typeof(AuditStoreAreaList));

            NavigationService.Configure(ViewNames.StoreAreaDetailsPage, typeof(StoreAreaDetailsPage));
            NavigationService.Configure(ViewNames.InventoryListPage, typeof(InventoryListPage));
            NavigationService.Configure(ViewNames.PedestalInventoryDetailsPage, typeof(PedestalInventoryDetailsPage));
            NavigationService.Configure(ViewNames.DeactivationInventoryDetailsPage, typeof(DeactivationInventoryDetailsPage));
            NavigationService.Configure(ViewNames.IssueListPage, typeof(IssueListPage));
            NavigationService.Configure(ViewNames.IssueDetailPage, typeof(IssueDetailPage));
            NavigationService.Configure(ViewNames.ViewPicturesPage, typeof(ViewPicturesPage));
            NavigationService.Configure(ViewNames.ForceSyncPage, typeof(ForceSyncPage));

            //DependencyService.Register<RestService>();
            //DependencyService.Register<LocalAppDatabase>();
        }
        private void InitializeApp()
        {
            AppContainer.RegisterDependencies();

            var newAuditViewModel = AppContainer.Resolve<NewAuditViewModel>();
            newAuditViewModel.InitializeMessenger();

            var auditStoreAreaListViewModel = AppContainer.Resolve<AuditStoreAreaListViewModel>();
            auditStoreAreaListViewModel.InitializeMessenger();

            var storeAreaDetailsViewModel = AppContainer.Resolve<StoreAreaDetailsViewModel>(); //list store areas
            storeAreaDetailsViewModel.InitializeMessenger();

            var issueListViewModel = AppContainer.Resolve<IssueListViewModel>();
            issueListViewModel.InitializeMessenger();

            var issueDetailViewModel = AppContainer.Resolve<IssueDetailViewModel>();
            issueDetailViewModel.InitializeMessenger();

            var inventoryListViewModel = AppContainer.Resolve<InventoryListViewModel>();
            inventoryListViewModel.InitializeMessenger();

            var deactivationInventoryDetailsViewModel = AppContainer.Resolve<DeactivationInventoryDetailsViewModel>();
            deactivationInventoryDetailsViewModel.InitializeMessenger();

            var pedestalInventoryDetailsViewModel = AppContainer.Resolve<PedestalInventoryDetailsViewModel>();
            pedestalInventoryDetailsViewModel.InitializeMessenger();

            var auditConfirmationViewModel = AppContainer.Resolve<AuditConfirmationViewModel>();
            auditConfirmationViewModel.InitializeMessenger();
        }
        private void SetUpServices()
        {

            PCA = PublicClientApplicationBuilder.Create(ClientId)
                .WithRedirectUri(RedirectURI)
                .WithTenantId(TenantId)
                .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                .Build();

            DependencyResolver.ResolveUsing(type => container.IsRegistered(type) ? container.Resolve(type) : null);

            if (!built)
            {
                builder.RegisterType<RestService>().WithParameters(new[] { new NamedParameter("baseURL", "https://jciretailsurveytoolwebportaltest.azurewebsites.net/api/") });
                builder.RegisterType<LocalAppDatabase>().WithParameters(new[] { new NamedParameter("dbPath", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LocalDb.db3")) });
                try
                {
                    container = builder.Build();
                }
                catch (Exception e)
                {

                }
                built = true;
            }
        }
    }
}
