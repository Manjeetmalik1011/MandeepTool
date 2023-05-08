using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Jci.RetailSurveyTool.TechnicianApp.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CommandTaskAttribute : Attribute
    {
        public CommandTaskAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; }

        public static void InitCommands(object obj)
        {
            foreach (var prop in obj.GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(CommandTaskAttribute))))
            {
                var thisAttr = (CommandTaskAttribute)GetCustomAttribute(prop, typeof(CommandTaskAttribute));
                var _methodInfo = obj.GetType().GetMethod(thisAttr.MethodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                var isAwaitable = _methodInfo.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null;

                if (isAwaitable)
                {
                    prop.SetValue(obj, new Command(async () => await (dynamic)_methodInfo.Invoke(obj, null)));
                }
                else
                {

                    prop.SetValue(obj, new Command(() => _methodInfo.Invoke(obj, null)));
                }


            }
        }
    }
}
