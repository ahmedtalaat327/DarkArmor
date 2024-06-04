using System.Collections.ObjectModel;

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

            DataShowed.Add(new MyDeiceTest() { Index = 0, Name = "Ahmed", Description = "PC" , Active = true });
            DataShowed.Add(new MyDeiceTest() { Index = 0, Name = "Ahmed", Description = "PC" });
            DataShowed.Add(new MyDeiceTest() { Index = 0, Name = "Ahmed", Description = "PC" });
            DataShowed.Add(new MyDeiceTest() { Index = 0, Name = "Ahmed", Description = "PC" });
            DataShowed.Add(new MyDeiceTest() { Index = 0, Name = "Ahmed", Description = "PC" });


        }
        [RelayCommand]
        private void OnCounterReset()
        {
            Counter = false;
            IndicatorAppear = Visibility.Collapsed;
        }
    }
    public class MyDeiceTest
    {
        public int Index { get; set; } = 0;
        public string Name { get; set; } = "None";
        public string Description { get; set; } = "Device";

        public bool Active { get; set; } = false;

    }
}
