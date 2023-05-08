using Jci.RetailSurveyTool.TechnicianApp.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jci.RetailSurveyTool.TechnicianApp.Core.CommonUtility
{
    public class HndlerUtility
    {
        public static void ModifyEntry()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
                if (view is BorderlessEntry)
                {
#if ANDROID
                 handler.PlatformView.Background = null;
                 handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.Layer.BorderWidth = 0;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                            handler.PlatformView.BorderThickness= new Microsoft.UI.Xaml.Thickness(0);
#endif
                }

            });
        }

        public static void ModifyPicker()
        {
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping("MyPickerCustomization", (handler, view) =>
            {
                if (view is BorderlessPicker)
                {
#if ANDROID
                 handler.PlatformView.Background = null;
                 handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
                    handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
                    handler.PlatformView.Layer.BorderWidth = 0;
                    handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                            handler.PlatformView.BorderThickness= new Microsoft.UI.Xaml.Thickness(0);
#endif
                }

            });
        }
    }
}
