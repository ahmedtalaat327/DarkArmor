using DarkArmor.ViewModels.Messagaes;
using System.Windows.Controls;


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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            await ViewModel.StartUnpacking();
        }
    }
}
