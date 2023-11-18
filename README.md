
[![nuget][nuget-badge1]][nuget-url1]

 [nuget-badge1]: https://img.shields.io/badge/nuget-v2.0.0-blue.svg
 [nuget-url1]: https://www.nuget.org/packages/ApplicationOptions
 [source-url]: https://github.com/skint007/ApplicationOptions
 # ApplicationOptions
 A very simple library for creating easy to use to use application settings that use json serialization.

## What it does?

This library allows you to quickly and easily create application settings classes. 
I generally use this in my WPF apps.

## Prerequisites

 - .Net Standard 2+,.Net 6+

## Installation and sources

<pre>
  nuget install ApplicationOptions
</pre>

 - [NuGet package][nuget-url1]
 - [Source code][source-url]

 ## Example app settings class
 
 ```csharp
public class AppSettings : BaseOptions<AppSettings>
{
	// Override base folder
	protected override string BaseFolder => @"settings\";

	// Override file name (Default is $"options.{typeof(T).Name}.json")
	protected override string FileName => "appsettings.json";

	public DateTime LastLaunch { get; set; }
	public string ApiKey { get; set; }
	public string ApiEndpoint { get; set; }
	//Sets a default value when initializing new settings
	public Rectangle WindowPosition { get; set; } = new Rectangle(100, 100, 850, 900);

	
}
```

## Example subscribing to events
```csharp
BaseOptionSettings.WatchForOptionChanges = true;
AppSettings.Instance.Loaded += () => Console.WriteLine("AppSettings Loaded");
AppSettings.Instance.Saved += () => Console.WriteLine("AppSettings Saved");
 ```
 
 ## Example usage
 ```csharp
public class MyWindow : Window
{
	private Settings => AppSettings.Instance;
	
	public MyWindow()
	{
		Top = Settings.WindowPosition.Y;
		Left = Settings.WindowPosition.X;
		Width = Settings.WindowPosition.Width;
		Height = Settings.WindowPosition.Height;
	}
	
	private void OnClosing()
	{
		BaseOptionSettings.LoadedOptions.ForEach(o => o.Save());
	}
}
 ```

 ## Global settings usage
 The `BaseOptionSettings` class lets you configure how the BaseOptions class.
 ```csharp
 // Watch for changes in the settings files
 BaseOptionSettings.WatchForOptionChanges = true;

 // Save all loaded options
 BaseOptionSettings.LoadedOptions.ForEach(o => o.Save());

 // Indent the json files or not
 BaseOptionSettings.IndentJson = false;
 ```
## Quick contributing guide

 - Fork and clone locally
 - Create a topic specific branch. Add some nice feature.
 - Send a Pull Request to spread the fun!

