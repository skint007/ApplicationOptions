using ApplicationOptions;
using ApplicationOptions.Test.Settings;
var appSettings = AppSettings.Instance;
var moduleSettings = ModuleSettings.Instance;
var userSettings = UserSettings.Instance;

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
Console.WriteLine($"    Count: {BaseOptions.LoadedOptions.Count}");

BaseOptions.LoadedOptions.ForEach(o => o.Save());