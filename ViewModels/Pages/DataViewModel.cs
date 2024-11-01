using DarkArmor.Data;
using DarkArmor.Helpers;
using DarkArmor.Models;
using DarkArmor.Models.Skeleton;
using System.Collections.ObjectModel;
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
        private ObservableCollection<NICController> _cardsNics = new ObservableCollection<NICController>();

        [ObservableProperty]//not used
        private int _selectednicIndex = 0;

        [ObservableProperty]
        private string _printedCode = "null";

        [ObservableProperty]    
        private bool _nICComboBoxEnabled = true;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private async Task InitializeViewModel()
        {

            _isInitialized = true;


            Counter = true;
            IndicatorAppear = Visibility.Visible;


            var res  = await new CollectNICS().TrigExpProcAsync(DesktopAppOnly.PathFinder.GetApplicationRoot());

            CardsNics.Clear();

            foreach(var nic in res)
            {
                CardsNics.Add(nic);
            }
            //usually this come from presaved settings 
            SelectednicIndex = 0;

            Counter = false;
            IndicatorAppear = Visibility.Collapsed;

           
        }
        [RelayCommand]
        public async Task OnNicChose(NICController nicc)
        {
            //print result
            PrintedCode = $"<?xml version = \"1.0\"?>" +
                $"\r\n" +
                $"<NICControllerAsString>" +
                $"\r\n   " +
                $"<Nic_Index>{nicc.Nic_Index}</Nic_Index>" +
                $"\r\n   " +
                $"<FriendlyName>{nicc.FriendlyName}</FriendlyName>" +
                $"\r\n   " +
                $"<Address>{nicc.Address}</Address>" +
                $"\r\n   " +
                $"<Mask>{nicc.Mask}</Mask>" +
                $"\r\n   " +
                $"<Gate>{nicc.Gate}</Gate>" +
                $"\r\n   " +
                $"<PhysicalAdress>{nicc.PhysicalAdress}</PhysicalAdress>" +
                $"\r\n   " +
                $"<Manufacture>{nicc.Manufacture}</Manufacture>" +
                $"\r\n   " +
                $"<Broadcast>{nicc.Broadcast}</Broadcast>" +
                $"\r\n   " +
                $"<Active>{nicc.Active}</Active>" +
                $"\r\n " +
                $"</NICControllerAsString>" +
                $"\r\n\r\n\r\n";
            //chnge the current or defult nic controller used in scan 
            App.GetService<DashboardViewModel>().LocalNic = nicc;
            //break the role [loading as first time from the file]
            App.GetService<DashboardViewModel>().FirstLoad = false ;
            //save this to the scheme.nic file by writing it 
            NICComboBoxEnabled =  await DesktopAppOnly.PutToStreamBlock(nicc);
        }
    }
}
