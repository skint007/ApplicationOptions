using ApplicationOptions;
using ApplicationOptions.Test.Settings;

namespace ApplicationOptions.Test
{
    internal class Program
    {
        private static AppSettings appSettings => AppSettings.Instance;
        private static ModuleSettings moduleSettings => ModuleSettings.Instance;
        private static UserSettings userSettings => UserSettings.Instance;
        static void Main(string[] args)
        {
            BaseOptionSettings.WatchForOptionChanges = true;
            appSettings.Loaded += () => Console.WriteLine("AppSettings Loaded");
            appSettings.Saved += () => Console.WriteLine("AppSettings Saved");

            moduleSettings.Loaded += () => Console.WriteLine("ModuleSettings Loaded");
            moduleSettings.Saved += () => Console.WriteLine("ModuleSettings Saved");

            userSettings.Loaded += () => Console.WriteLine("UserSettings Loaded");
            userSettings.Saved += () => Console.WriteLine("UserSettings Saved");

            if (string.IsNullOrEmpty(appSettings.ApiEndpoint))
            {
                Console.WriteLine("Applying some settings to the app settings");
                appSettings.ApiEndpoint = "https://someurl.com/api";
                appSettings.ApiKey = "e03a4f379449f25307113e322df803bc8a1d44049c1a7";
                appSettings.LastLaunch = DateTime.Now;

                moduleSettings.BaseModulePath = "Modules/";
                moduleSettings.Modules.AddRange(new[] { "Module1", "Module2", "Module3", "Module4" });

                userSettings.FirstName = "John";
                userSettings.LastName = "Smith";
                userSettings.UserName = "jsmith";

                moduleSettings.BaseModulePath = "this/IS/the/clone";
                moduleSettings.Modules.Add("System");
                moduleSettings.Modules.Add("System.Windows");
            }

            if (BaseOptionSettings.WatchForOptionChanges)
            {
                Console.WriteLine("Saving instances...");
                Console.WriteLine("");
                BaseOptionSettings.LoadedOptions.ForEach(o => o.Save());
                Console.WriteLine("Make a change to the config files to auto reload them.");
                Console.WriteLine("");
                Console.ReadLine();
            }

            Console.WriteLine("AppSettings:");
            Console.WriteLine($"    Last:{appSettings.LastLaunch}");
            Console.WriteLine($"    Endp:{appSettings.ApiEndpoint}");
            Console.WriteLine($"    ApiK:{appSettings.ApiKey}");

            Console.WriteLine("ModuleSettings:");
            Console.WriteLine($"    Mods: {string.Join(", ", moduleSettings.Modules)}");
            Console.WriteLine($"    Base: {moduleSettings.BaseModulePath}");

            Console.WriteLine("UserSettings:");
            Console.WriteLine($"    Full: {userSettings.FullName}");

            Console.WriteLine("LoadedOptions:");
            Console.WriteLine($"    Count: {BaseOptionSettings.LoadedOptions.Count}");

            Console.WriteLine("");
            BaseOptionSettings.LoadedOptions.ForEach(o => o.Save());
        }
    }
}