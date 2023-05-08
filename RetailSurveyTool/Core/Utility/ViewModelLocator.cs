using Jci.RetailSurveyTool.TechnicianApp.Bootstrap;
using System;
using System.Globalization;
using System.Reflection;
//using Jci.RetailSurveyTool.TechnicianApp.ViewModels;
//using Jci.RetailSurveyTool.TechnicianApp.ViewModels.NewAudit;


namespace Jci.RetailSurveyTool.TechnicianApp.Utility
{
    public static class ViewModelLocator
    {
        //public static JobListViewModel JobListViewModel { get; set; } = new JobListViewModel();
        //public static NewAuditViewModel NewAuditViewModel { get; set; } = new NewAuditViewModel();
        //public static CustomerSelectionViewModel CustomerSelectionViewModel { get; set; } = new CustomerSelectionViewModel();

        public static readonly BindableProperty AutoWireViewModelProperty =
    BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool),
        typeof(ViewModelLocator), default(bool),
        propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            if (viewType.FullName != null)
            {
                var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.").Replace("Page", "View"); //patch to resolve view names without View in the filename. e.g. NewAuditPage

                //patch to resolve view names with view in the filename. e.g. AuditStoreAreaList
                string[] subs = viewName.Split('.');
                viewName = (subs[subs.GetUpperBound(0)].Contains("View")) ? viewName : viewName + "View";

                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

                var viewModelType = Type.GetType(viewModelName);
                if (viewModelType == null)
                {
                    return;
                }
                var viewModel = AppContainer.Resolve(viewModelType);
                view.BindingContext = viewModel;
            }
        }
    }
}
