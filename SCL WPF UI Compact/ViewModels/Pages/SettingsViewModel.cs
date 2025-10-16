using SCL_WPF_UI_Compact.Helpers;
using SCL_WPF_UI_Compact.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Wpf.Ui.Abstractions.Controls;
using Wpf.Ui.Appearance;

namespace SCL_WPF_UI_Compact.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;
        private readonly AppConfigService _appConfig;
        private readonly UserConfigService _userConfig;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

        [ObservableProperty]
        private ObservableCollection<Theme>? _themes;

        [ObservableProperty]
        private Theme? _selectedTheme;

        [ObservableProperty]
        private ObservableCollection<string> _languages = new();

        [ObservableProperty]
        private string? _selectedLanguage;

        private readonly ILocalizationService _loc;

        public SettingsViewModel(AppConfigService appConfig, UserConfigService userConfig, ILocalizationService loc)
        {
            ArgumentNullException.ThrowIfNull(appConfig);
            ArgumentNullException.ThrowIfNull(userConfig);
            ArgumentNullException.ThrowIfNull(loc);

            _appConfig = appConfig;
            _userConfig = userConfig;
            _loc = loc;

            Themes = new ObservableCollection<Theme>
            {
                new() { Name = "Light" },
                new() { Name = "Dark" },
                new() { Name = "High Contrast" }
            };

            // Initialize Languages collection
            Languages = new ObservableCollection<string>();

            InitializeLocalization();
        }

        private void InitializeLocalization()
        {
            try
            {
                // Populate initial list

                Languages.Clear();
                foreach (var l in _loc.AvailableLanguages)
                {
                    Languages.Add(l);
                }

                // Select current language if available
                SelectedLanguage = _loc.CurrentLanguage;
                WeakReferenceMessenger.Default.Send(new LanguageChangedMessage(SelectedLanguage));


                // Watch for service changes (languages list or current language)
                if (_loc is INotifyPropertyChanged notifyPropertyChanged)
                {
                    notifyPropertyChanged.PropertyChanged += OnLocalizationServicePropertyChanged;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to initialize localization: {ex.Message}");
                // Optionally show error to user via SnackBar or similar
            }
        }

        public Task OnNavigatedToAsync()
        {
            if (!_isInitialized)
                InitializeViewModel();

            return Task.CompletedTask;
        }

        public Task OnNavigatedFromAsync() => Task.CompletedTask;

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            AppVersion = $"{HelperFunctions.AppName} - v{HelperFunctions.GetAssemblyVersion()}";

            switch (CurrentTheme)
            {
                case ApplicationTheme.Light:
                    SelectedTheme = Themes?.FirstOrDefault(t => t.Name == "Light");
                    break;
                case ApplicationTheme.HighContrast:
                    SelectedTheme = Themes?.FirstOrDefault(t => t.Name == "High Contrast");
                    break;
                default:
                    SelectedTheme = Themes?.FirstOrDefault(t => t.Name == "Dark");
                    break;
            }

            _isInitialized = true;
        }

        partial void OnSelectedThemeChanged(Theme? value)
        {
            if (null == value)
                return;

            switch (value.Name)
            {
                case "Light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;
                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;
                    _userConfig.Theme = "Light";
                    break;
                case "High Contrast":
                    if (CurrentTheme == ApplicationTheme.HighContrast)
                        break;
                    ApplicationThemeManager.Apply(ApplicationTheme.HighContrast);
                    CurrentTheme = ApplicationTheme.HighContrast;
                    _userConfig.Theme = "High Contrast";
                    break;
                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;
                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;
                    _userConfig.Theme = "Dark";
                    break;
            }
        }

        private void OnLocalizationServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // When languages change, refresh the collection on UI thread
            if (e.PropertyName == nameof(ILocalizationService.AvailableLanguages))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Languages.Clear();
                    foreach (var l in _loc.AvailableLanguages)
                        Languages.Add(l);
                });
                return;
            }

            // When current language changed externally, update SelectedLanguage
            if (e.PropertyName == nameof(ILocalizationService.CurrentLanguage))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SelectedLanguage = _loc.CurrentLanguage;
                    WeakReferenceMessenger.Default.Send(new LanguageChangedMessage(SelectedLanguage));
                });
            }
        }

        partial void OnSelectedLanguageChanged(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            if (value.Equals(_loc.CurrentLanguage, StringComparison.OrdinalIgnoreCase)) return;

            // attempt to switch language via service
            try
            {
                _loc.SetLanguage(value);
                _userConfig.Language = value;
            }
            catch
            {
                // ignore or show error to user; keep UI responsive
            }
        }

        [RelayCommand]
        private void SaveApplicationSettings()
        {
            AppConfigService.Save(_appConfig, _loc);
        }

        [RelayCommand]
        private void SaveUserSettings()
        {
            UserConfigService.Save(_userConfig, _loc);
        }
    }
}
