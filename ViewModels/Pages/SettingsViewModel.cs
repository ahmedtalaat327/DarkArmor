using System.Windows.Media.Imaging;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace DarkArmor.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = String.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Dark;

        [ObservableProperty]
        private Uri? _appIcon = null;

        [ObservableProperty]
        private bool _released = false;
        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            switch (Released)
            {
                case (true):
                    AppVersion = $"Released - {GetAssemblyVersion()}";
                    AppIcon = new Uri("/Assets/wpfui-icon-256.png", UriKind.RelativeOrAbsolute);
                    break;
                case (false):
                    AppVersion = $"BETA - {GetAssemblyVersion()}";
                    AppIcon = new Uri("/Assets/beta-wpfui-icon-256.png", UriKind.RelativeOrAbsolute);
                    break;
                default:
                    AppVersion = $"BETA - {GetAssemblyVersion()}";
                    AppIcon = new Uri("/Assets/beta-wpfui-icon-256.png", UriKind.RelativeOrAbsolute);
            }
           
            _isInitialized = true;
        }

        private string GetAssemblyVersion()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? String.Empty;
        }

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;

                    break;

                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;

                    break;
            }
        }
    }
}
