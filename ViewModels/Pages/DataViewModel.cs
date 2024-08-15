using DarkArmor.Data;
using DarkArmor.Helpers;
using DarkArmor.Models;
using DarkArmor.Models.Skeleton;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace DarkArmor.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;


        [ObservableProperty]
        private bool _counter = false;
        [ObservableProperty]
        private Visibility _indicatorAppear = Visibility.Collapsed;

        [ObservableProperty]
        private ObservableCollection<string> _cardsnics = new ObservableCollection<string>();

        [ObservableProperty]
        private int _selectednicindex = 0;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private async Task InitializeViewModel()
        {
            Counter = true;
            IndicatorAppear = Visibility.Visible;


            var res  = await new CollectNICS().TrigExpProcAsync(DesktopAppOnly.PathFinder.GetApplicationRoot());

            Cardsnics.Clear();

            foreach(var nic in res)
            {
                Cardsnics.Add(nic.Manufacture);
            }
            //usually this come from presaved settings 
            Selectednicindex = 0;

            Counter = false;
            IndicatorAppear = Visibility.Collapsed;

            _isInitialized = true;
        }
    }
}
