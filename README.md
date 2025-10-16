# SCL-WPF-UI-Compact
Template for creating WPF Ui project with MVVM pattern, Dependancy Injection, Compact navigation and Mica background.
# Demo animation

![Demo animated gif of SCL-WPF-UI-Compact](https://github.com/Screaminlean/SCL-WPF-UI-Compact/blob/main/Images/SCL-WPF-UI-Compact-Demo.gif)

# Project Status
[![.NET](https://img.shields.io/badge/.NET-9-512BD4)]()
[![License](https://img.shields.io/badge/License-MIT-blue.svg)]()

# Getting Started
## Prerequisites
- Visual Studio 2022
- .NET 9
- Windows 10/11 (for Mica effect support)

## Dependencies
- [WPF-UI](https://github.com/lepoco/wpfui)
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)

## Tasks
Anything marked with * have a known bug!
- [x] Support User Configuration.
- [x] Support Application Configuration.
- [x] Support Localization *.

# Features
Below are the features with a description of how they operate as well as known bugs!

## Configuration
- [x] Automatic handling of configuration schema changes.
- [x] Detection and removal of obsolete settings.
- [x] Automatic addition of new settings with default values.
- [x] Error handling with user feedback through SnackBar notifications.
- [x] Detailed debug logging of configuration changes.

Anytime you want change a setting set it in the App or User config 
- Call `appconfig.somesetting="foo"`.
- To save to file call `AppConfigService.Save(_appConfig, _loc);` .

### App Configuration
Provides functionality for loading and saving application-wide configuration settings. 
- Configuration is stored in JSON format in the common application data folder.
  - `%ProgramData%\AppName\AppConfig.json`
  
### User Configuration
Provides functionality for loading and saving user configuration settings.
- Configuration is stored in JSON format in the user's application data folder.
  - `%AppData%\AppName\UserConfig.json`
  
## Localization
The template includes a robust localization system that supports multiple languages using ISO 639 and 639-1 language codes. Language resources are stored in JSON files and can be loaded from:
- [x] Embedded resources.
- [x] External JSON files in the application directory.
- [x] Custom file paths.
- [x] Supports dynamic language switching at runtime.
- [x] Provides a XAML extension for easy localization in UI elements.
- [x] Includes a service for managing localization in ViewModels.
- [x] Handles missing translations gracefully by falling back to the key or a default language.
- [x] Includes a sample implementation with English.

### Use In ViewModel
```csharp
// Inject the localization service 
public class YourViewModel 
{ 
    private readonly ILocalizationService _loc;

    public YourViewModel(ILocalizationService loc)
    {
        _loc = loc;
        // Get localized string
        string translated = _loc.GetString("YourKey");

        // Switch language
        _loc.SetLanguage("en-US");

        // Check available languages
        var languages = _loc.AvailableLanguages;
    }
}
```

### Use in XAML
Ensure you add the Extensions namespace to your XAML file:
```xml
xmlns:loc="clr-namespace:SCL.WPF.UI.Compact.Extensions"
```
Then you can:\
Use the LocalizeExtension to bind text in XAML:
```xml
<TextBlock Text="{loc:Localize WelcomeMessage}" />
```

### Language Files
Create JSON files named after the language code (e.g., `en-US.json`, `fr-FR.json`) with key-value pairs:
```json
{ 
"WelcomeMessage": "Welcome", 
"Settings": "Settings", 
"Language": "Language" 
}
```
### Bugs
- [ ] Changing language does not update all UI elements automatically. You may need to refresh or rebind certain elements. Specifically the Breadcrumb control does not update without navigating away and back.
- [ ] if a key is missing in the selected language it does not fall back to the default language. It falls back to the key. Implement a user friendly fallback mechanism.

## SnackBar Notifications
The template includes a SnackBar notification system for displaying transient messages to users. The SnackBar is integrated into the MainWindow and can be accessed from any ViewModel through the `ISnackbarService`.
This uses the WeakReference Messenger from CommunityToolkit.Mvvm to avoid memory leaks.

### Usage in ViewModel
To display a SnackBar message from any ViewModel, inject the `ISnackbarService` and call the `Show` method:
```csharp
public class YourViewModel 
{ 
    private readonly ISnackbarService _snackbarService;
    public YourViewModel(ISnackbarService snackbarService)
    {
        _snackbarService = snackbarService;
    }
    public void SomeMethod()
    {
        // Show a SnackBar message
        WeakReferenceMessenger.Default.Send(new SnackBarChangeMessage(
                    new SnackBarMessage
                    {
                        Title = "MessageSuccessTitle",
                        Message = "MessageUserSettingsSaved",
                        Appearance = ControlAppearance.Success,
                        Icon = new SymbolIcon(SymbolRegular.ArrowSync24),
                        TimeOut = TimeSpan.FromSeconds(3)
                    }));
    }
}
```

# Installation
To use this in Visual Studio 2022 you can do one of the following;

## Clone the repository
- `git clone https://github.com/Screaminlean/SCL-WPF-UI-Compact.git`
- Open the solution in Visual Studio 2022.
  - Click **Project**
  - Then **Export as Template** 
  - You need the icon and preview image, I have not included them in the repo as they belong to Lepoco. You can find them in any project created with WPF-UI.
  - Select Auto import.
- Next time you open Visual Studio it will be available.

## Download the zip
- Download the zip archive from the assets on the releases page of this repository. (Link to follow)
- Copy the zip file to `C:\Users\YourUserName\Documents\Visual Studio 2022\Templates\ProjectTemplates\C#`.
- Next time you open Visual Studio it will be available.

# Contribution
Feel free to fork the repository and submit pull requests. For major changes, please open an issue first to discuss what you would like to change.
