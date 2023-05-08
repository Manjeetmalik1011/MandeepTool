using Android.App;
using Android.Runtime;

namespace RetailSurveyTool;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
        Jci.RetailSurveyTool.TechnicianApp.App.ParentWindow = this;
    }

	//protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
	protected override MauiApp CreateMauiApp()
	{
        //Jci.RetailSurveyTool.TechnicianApp.App.ParentWindow = this;
        return MauiProgram.CreateMauiApp();
    }
}
