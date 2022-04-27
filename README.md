
Main [![nuget][nuget-badge1]][nuget-url1] Extension [![nuget][nuget-badge2]][nuget-url2]

 [nuget-badge1]: https://img.shields.io/badge/nuget-v1.3.2-blue.svg
 [nuget-url1]: https://www.nuget.org/packages/ApplicationOptions
 [nuget-badge2]: https://img.shields.io/badge/nuget-v1.3.2-blue.svg
 [nuget-url2]: https://www.nuget.org/packages/ApplicationOptions.Notify
 [source-url]: https://github.com/skint007/ApplicationOptions
 # ApplicationOptions
 A very simple libary for creating easy to use to use application settings that use json serialization.

## What it does?

This library allows you to quickly and easily create application settings classes. You can extend it with ApplicationOptions.Notify to allow for built in property changed events.
I generally use this in my WPF apps.

## Prerequisites

 - .Net Standard 2+,.Net 6+

## Installation and sources

<pre>
  nuget install ApplicationOptions
  nuget install ApplicationOptions.Notify
</pre>

 - [NuGet package][nuget-url1] (ApplicationOptions)
 - [NuGet package][nuget-url2] (ApplicationOptions.Notify)
 - [Source code][source-url]

 ## Example app settings class
 
 ```csharp
public class AppSettings : BaseOptions<AppSettings>
{
	public DateTime LastLaunch { get; set; }
	public string ApiKey { get; set; }
	public string ApiEndpoint { get; set; }
	//Sets a default value when initializing new settings
	public Rectangle WindowPosition { get; set; } = new Rectangle(100, 100, 850, 900);
}
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
		BaseOptions.LoadedOptions.ForEach(o => o.Save());
	}
}
 ```

## Quick contributing guide

 - Fork and clone locally
 - Create a topic specific branch. Add some nice feature.
 - Send a Pull Request to spread the fun!

