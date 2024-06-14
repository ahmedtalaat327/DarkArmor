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
        private ObservableCollection<MyDeiceTest> _dataShowed = new ObservableCollection<MyDeiceTest>();

       
        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter=true;
            IndicatorAppear = Visibility.Visible;

            DataShowed.Add(new MyDeiceTest() { Index = 0, Name = "John Doe", Description = "PC" });
            DataShowed.Add(new MyDeiceTest() { Index = 1, Name = "Kendrik Paul", Description = "PC" , 
                Status = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ED1C24")) ,
                Active = false
            });
            DataShowed.Add(new MyDeiceTest() { Index = 2, Name = "Boland Upon", Description = "PC" });
            DataShowed.Add(new MyDeiceTest() { Index = 3, Name = "Sado Firo", Description = "PC" });
            DataShowed.Add(new MyDeiceTest() { Index = 4, Name = "Dimitry Alon", Description = "PC" });



        }
        [RelayCommand]
        private void OnCounterReset()
        {
            Counter = false;
            IndicatorAppear = Visibility.Collapsed;
        }
    }
    public partial class MyDeiceTest : ObservableObject
    {
        [ObservableProperty]
        private int _index = 0;

        [ObservableProperty]
        private string _name = "None";
        [ObservableProperty]
        private string _description  = "Device";
        [ObservableProperty]
        private bool _active  = true;

        [ObservableProperty]
        [Browsable(false)]
        private SolidColorBrush _status = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#40f4cd"));

    }
}
