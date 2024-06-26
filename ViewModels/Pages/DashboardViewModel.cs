﻿using DarkArmor.Data;
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

            DiscoveredNICControllers.Clear();
            DataShowed.Clear();

            /*
            var local_nicc = new NICController() { Nic_index = 4,
                Address = IPAddress.Parse("192.168.100.45"),
                Gate = IPAddress.Parse("192.168.100.1"),
                Mask = IPAddress.Parse("255.255.255.0")
            };
            */
            var local_nicc_asstring = DesktopAppOnly.LoadFromStreamBlock();
            NICController local_nicc = new NICController()
            {
                Nic_index = Int32.Parse(local_nicc_asstring.Nic_index),
                Address = IPAddress.Parse(local_nicc_asstring.Address),
                Gate = IPAddress.Parse(local_nicc_asstring.Gate),
                Mask = IPAddress.Parse(local_nicc_asstring.Mask),
                PhysicalAdress = local_nicc_asstring.PhysicalAdress
            };

            // DiscoveredNICControllers.Add(local_nicc);
            // OnPropertyChanged(nameof(DiscoveredNICControllers));

            await new ARPRequest(local_nicc).TrigProcAsync(DesktopAppOnly.PathFinder.GetApplicationRoot());

            var local2_nicc = new NICController()
            {
                Nic_index = 4,
                Address = IPAddress.Parse("192.168.79.34"),
                Gate = IPAddress.Parse("192.168.79.243"),
                Mask = IPAddress.Parse("255.255.255.0"),
                PhysicalAdress = "02:23:a1:11:e8"
            };

          //  DiscoveredNICControllers.Add(local2_nicc);


            Counter = false;
            IndicatorAppear = Visibility.Collapsed;


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
   
}
