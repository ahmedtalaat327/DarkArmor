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

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {

            await ViewModel.StartCloning();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            await ViewModel.StartSceduler();
        }
    }
}
