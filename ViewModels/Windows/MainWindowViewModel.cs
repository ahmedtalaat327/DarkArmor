using DarkArmor.ViewModels.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace DarkArmor.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "inDark Sneaky";

        [ObservableProperty]
        private Uri _appIcon = null;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Dashboard",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage),
                
            },
            new NavigationViewItem()
            {
                Content = "Preferences",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Book24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                 
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        /*
        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Notification", Tag = "tray_bell" }
        };
        */
        [RelayCommand]
        public void OnWindowLoad()
        {
            var x = App.GetService<SettingsViewModel>().Released;
            if (x)
            {
                AppIcon = new Uri("/Assets/wpfui-icon-256.png", UriKind.RelativeOrAbsolute);
            }
            else
            {
                AppIcon = new Uri("/Assets/beta-wpfui-icon-256.png", UriKind.RelativeOrAbsolute);
            }
        }
    }
}
