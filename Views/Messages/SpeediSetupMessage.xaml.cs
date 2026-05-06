using DarkArmor.ViewModels.Messagaes;
using System.Windows.Controls;

////*this viewmodel property change works in old fashion*///
namespace DarkArmor.Views.Messages
{
    /// <summary>
    /// Interaction logic for SpeediSetup.xaml
    /// </summary>
    public partial class SpeediSetupMessage : UserControl
    {
      
        /// <summary>
        /// view model 
        /// </summary>
        public static SpeediSetupMessageViewModel ViewModel { get; set; } = new SpeediSetupMessageViewModel();

        /// <summary>
        /// ctr
        /// </summary>
        public SpeediSetupMessage()
        {
            DataContext = ViewModel;

            InitializeComponent();
            

            ViewModel.PropertyChanged += SpeediSetupMessageViewModel_PropertyChanged; ;

        }

        private void SpeediSetupMessageViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewModel.UnpackProcessStatus)))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if(!ViewModel.RunUnpackingProcessButtonfloag)
                    ViewModel.RunUnpackingProcessButtonfloag = true;
                   
                });
            }
            if (e.PropertyName.Equals(nameof(ViewModel.CloneOneProcessStatus)))
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                  
                    if (!ViewModel.RunCloneOneProcessButtonfloag)
                        ViewModel.RunCloneOneProcessButtonfloag = true;
                });
            }
            if (e.PropertyName.Equals(nameof(ViewModel.TaskExecuterStatus)))
            {
                App.Current.Dispatcher.Invoke(() =>
                {

                    if (!ViewModel.RunTaskExecuterProcessButtonfloag)
                        ViewModel.RunTaskExecuterProcessButtonfloag = true;
                });
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            await ViewModel.StartUnpacking();
        }
        //step no 1 is for cloning the program files, step no 1 is for cloning the mod files
        //to system32 AND drivers folders [Packet.dll - drivers\npf.sys]
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            await ViewModel.StartCloning(1);
        }


        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartSceduler();
        }
        //step no 2 is for cloning the game files, step no 2 is for cloning the mod files
        //to system32 only [wpcap.dll - pthreadVC.dll]
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartCloning(2);
        }
        //step no 3 is for cloning the mod files to the game folder
        //to sysWow64 folder [Packet.dll]
        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartCloning(3);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {

        }
    }
}
