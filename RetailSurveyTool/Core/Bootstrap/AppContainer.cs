using Autofac;
using Jci.RetailSurveyTool.TechnicianApp.Contracts.Repository;
using Jci.RetailSurveyTool.TechnicianApp.Repository;
using Jci.RetailSurveyTool.TechnicianApp.Services;
using Jci.RetailSurveyTool.TechnicianApp.ViewModels;
using Jci.RetailSurveyTool.TechnicianApp.ViewModels.ExistingAudit;
using Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit;
using System;

namespace Jci.RetailSurveyTool.TechnicianApp.Bootstrap
{
    public class AppContainer
    {
        private static Autofac.IContainer _container;

        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //ViewModels
            builder.RegisterType<SyncViewModel>();
            builder.RegisterType<LoginViewModel>();

            builder.RegisterType<JobListViewModel>();
            builder.RegisterType<NewAuditViewModel>().SingleInstance();
            builder.RegisterType<LookupPreviousViewModel>();
            builder.RegisterType<CustomerSelectionViewModel>();
            builder.RegisterType<AuditStoreAreaListViewModel>().SingleInstance();
            builder.RegisterType<StoreAreaDetailsViewModel>().SingleInstance(); //list
            builder.RegisterType<InventoryListViewModel>().SingleInstance();
            builder.RegisterType<IssueListViewModel>().SingleInstance();
            builder.RegisterType<PedestalInventoryDetailsViewModel>().SingleInstance();
            builder.RegisterType<DeactivationInventoryDetailsViewModel>().SingleInstance();
            builder.RegisterType<IssueDetailViewModel>().SingleInstance();
            builder.RegisterType<ViewPicturesViewModel>();
            builder.RegisterType<PreviousAuditDetailsViewModel>().SingleInstance();
            builder.RegisterType<SelectInventoryViewModel>().SingleInstance();
            builder.RegisterType<AuditConfirmationViewModel>().SingleInstance();
            builder.RegisterType<ForceSyncViewModel>();
            //services - data
            //builder.RegisterType<CatalogDataService>().As<ICatalogDataService>();
            //builder.RegisterType<ShoppingCartDataService>().As<IShoppingCartDataService>();
            //builder.RegisterType<ContactDataService>().As<IContactDataService>();
            //builder.RegisterType<OrderDataService>().As<IOrderDataService>();

            //services - general
            //builder.RegisterType<ConnectionService>().As<IConnectionService>();
            builder.RegisterType<NavigationService>().As<INavigationService>();
            //builder.RegisterType<AuthenticationService>().As<IAuthenticationService>();
            //builder.RegisterType<DialogService>().As<IDialogService>();
            //builder.RegisterType<PhoneService>().As<IPhoneService>();
            //builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();

            //General
            builder.RegisterType<GenericRepository>().As<IGenericRepository>();

            _container = builder.Build();
        }

        public static object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
