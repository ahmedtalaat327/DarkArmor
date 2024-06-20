using DarkArmor.Data;
using DarkArmor.Helpers;
using DarkArmor.Models;
using DarkArmor.Models.Skeleton;
using System.Collections.ObjectModel;
using System.Net;


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

        [ObservableProperty]
        public ObservableCollection<NICController> _discoveredNICControllers = new ObservableCollection<NICController> ();

       [RelayCommand]
        private async Task OnCounterIncrement()
        {
           

            Counter =true;
            IndicatorAppear = Visibility.Visible;

            /*
             DataShowed.Add(new NetworkDevice() {
                 DeviceIndex = 0, DomainName = "John Doe", Type = Models.Skeleton.DeviceType.UDevice  , 
                 //Status = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED1C24")) ,
                 Active = true ,
                 Nic = new NICController() { Nic_index = 0, Address = IPAddress.Parse("192.168.0.0"), Manufacture = "some trading manf", PhysicalAdress = "00:00:00:00" }
             });
            */
            var local_nicc = new NICController() { Nic_index = 4, Address = IPAddress.Parse("192.168.79.34")
                                                  ,Gate = IPAddress.Parse("192.168.79.243"), Mask = IPAddress.Parse("255.255.255.0")
            };

             await new ARPRequest(local_nicc, DiscoveredNICControllers).TrigProcAsync(DesktopAppOnly.PathFinder.GetApplicationRoot());
            
            /*
            for(int index = 0; index < DiscoveredNICControllers.Count; index++)
            {
                DataShowed.Add(new NetworkDevice()
                {
                    DeviceIndex = 0,
                    DomainName = "John Doe",
                    Type = Models.Skeleton.DeviceType.UDevice,
                    //Status = new SolidColorBrush((Color)ColorConverter.ConvertFromS tring("#ED1C24")) ,
                    Active = true,
                    Nic = DiscoveredNICControllers[index]
                });
            }
            */

        }
        [RelayCommand]
        private void OnCounterReset()
        {
            Counter = false;
            IndicatorAppear = Visibility.Collapsed;
        }
        [RelayCommand]
        public void OnToggleUnCheck(int keyin) {

            //becomes unactive 
            DataShowed[keyin].Active = false;

        }
        [RelayCommand]
        public void OnToggleCheck(int keyin)
        {

            //becomes active 
            DataShowed[keyin].Active = true;

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
