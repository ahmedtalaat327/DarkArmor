using DarkArmor.Data;
using DarkArmor.Helpers;
using DarkArmor.Models;
using DarkArmor.Models.Skeleton;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Security.Cryptography;


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

        [ObservableProperty]
        public ObservableCollection<ObservableCollection<int>> _processesMimsIds = new ObservableCollection<ObservableCollection<int>>();

        [ObservableProperty]
        public NICController? localNic;

        [ObservableProperty]
        public bool firstLoad = true;

       [RelayCommand]
        private async Task OnCounterIncrement()
        {
           
            Counter =true;
            IndicatorAppear = Visibility.Visible;

            DiscoveredNICControllers.Clear();
            DataShowed.Clear();

            /*
            var local_nicc = new NICController() { Nic_index = 4,
                Address = IPAddress.Parse("192.168.100.45"),
                Gate = IPAddress.Parse("192.168.100.1"),
                Mask = IPAddress.Parse("255.255.255.0")
            };
            */
            if (FirstLoad)
            {
                var local_nicc_asstring = await Task.Run<NICControllerAsString>(
                    () =>
                    {

                        return DesktopAppOnly.LoadFromStreamBlock();
                    });
                LocalNic = new NICController()
                {
                    Nic_Index = Int32.Parse(local_nicc_asstring.Nic_Index),
                    Address = IPAddress.Parse(local_nicc_asstring.Address),
                    Gate = IPAddress.Parse(local_nicc_asstring.Gate),
                    Mask = IPAddress.Parse(local_nicc_asstring.Mask),
                    Broadcast = IPAddress.Parse(local_nicc_asstring.Broadcast),
                    PhysicalAdress = local_nicc_asstring.PhysicalAdress,
                    Manufacture = local_nicc_asstring.Manufacture,
                    Active = bool.Parse(local_nicc_asstring.Active)


                };
                FirstLoad = false;
            }
            // DiscoveredNICControllers.Add(local_nicc);
            // OnPropertyChanged(nameof(DiscoveredNICControllers));

            //check if this network is valid ? or not [2 states not active NIC or Looping
            if ((bool)!LocalNic.Active || LocalNic.Address.ToString().Equals("127.0.0.1"))
            {
                //behold cancel..!!!
                Counter = false;
                IndicatorAppear = Visibility.Collapsed;
                return;
            }
            else
            {
                await new ARPRequest(LocalNic).TrigProcAsync(DesktopAppOnly.PathFinder.GetApplicationRoot());
                /*
                var local2_nicc = new NICController()
                {
                    Nic_index = 4,
                    Address = IPAddress.Parse("192.168.79.34"),
                    Gate = IPAddress.Parse("192.168.79.243"),
                    Mask = IPAddress.Parse("255.255.255.0"),
                    PhysicalAdress = "02:23:a1:11:e8"
                };
                */
                //  DiscoveredNICControllers.Add(local2_nicc);


                Counter = false;
                IndicatorAppear = Visibility.Collapsed;

            }
        }
        [RelayCommand]
        private async Task OnCounterReset()
        {
            Counter = false;
            IndicatorAppear = Visibility.Collapsed;

            //you need a way to capture the process and stop it / end it
            await ARPRequest.StopAllProcess();
        }
        [RelayCommand]
        public async Task OnToggleUnCheck(int keyin) {

            //becomes unactive 
            //DataShowed[keyin].Active = false;


            var local_nicc_asstring = await Task.Run<NICControllerAsString>(
              () => {

                  return DesktopAppOnly.LoadFromStreamBlock();
              });
            NICController local_nicc = new NICController()
            {
                Nic_Index = Int32.Parse(local_nicc_asstring.Nic_Index),
                Address = IPAddress.Parse(local_nicc_asstring.Address),
                Gate = IPAddress.Parse(local_nicc_asstring.Gate),
                Mask = IPAddress.Parse(local_nicc_asstring.Mask),
                PhysicalAdress = local_nicc_asstring.PhysicalAdress
            };

            await new ManARP(DesktopAppOnly.PathFinder.GetApplicationRoot(), local_nicc, 1,DataShowed[keyin],keyin).TrigAsyncProc();

        }
        [RelayCommand]
        public async Task OnToggleCheck(int keyin)
        {

            //becomes active 
            //DataShowed[keyin].Active = true;
            /*
            var local_nicc_asstring = await Task.Run<NICControllerAsString>(
              () => {

                  return DesktopAppOnly.LoadFromStreamBlock();
              });
            NICController local_nicc = new NICController()
            {
                Nic_index = Int32.Parse(local_nicc_asstring.Nic_index),
                Address = IPAddress.Parse(local_nicc_asstring.Address),
                Gate = IPAddress.Parse(local_nicc_asstring.Gate),
                Mask = IPAddress.Parse(local_nicc_asstring.Mask),
                PhysicalAdress = local_nicc_asstring.PhysicalAdress
            };

            await new ManARP(DesktopAppOnly.PathFinder.GetApplicationRoot(), local_nicc, 0, DataShowed[keyin]).TrigAsyncProc();
            */
            for(int x = 0; x < ProcessesMimsIds.Count; x++)
            {
                for(int y = 0; y < ProcessesMimsIds[x].Count; y++) {
                
                   if(keyin == ProcessesMimsIds[x][y])
                    {
                        var processId = ProcessesMimsIds[x][y+1];
                        KillProcessAndChildren(processId);
                        DataShowed[keyin].Active = true;
                        break;
                    }
                }
            }
        }

        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
    }
   
}
