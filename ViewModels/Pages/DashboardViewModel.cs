using DarkArmor.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;

namespace DarkArmor.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _counter = false;
        [ObservableProperty]
        private Visibility _indicatorAppear = Visibility.Collapsed;

        [ObservableProperty]
        private ObservableCollection<NetworkDevice> _dataShowed = new ObservableCollection<NetworkDevice>();

       
        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter=true;
            IndicatorAppear = Visibility.Visible;

           
            DataShowed.Add(new NetworkDevice() { DeviceIndex = 0, DomainName = "John Doe", Type = Models.Skeleton.DeviceType.UDevice  , 
                Status = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED1C24")) ,
                Active = false
            });
           



        }
        [RelayCommand]
        private void OnCounterReset()
        {
            Counter = false;
            IndicatorAppear = Visibility.Collapsed;
        }
    }
    /*
    public partial class MyDeiceTest : ObservableObject
    {
        [ObservableProperty]
        private int _index = 0;

        [ObservableProperty]
        private string _name = "None";

        [ObservableProperty]
        [Browsable(false)]
        private string _description  = "Device";

        [ObservableProperty]
        private bool _active  = true;

        [ObservableProperty]
        [Browsable(false)]
        private SolidColorBrush _status = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#40f4cd"));

    }*/
}
