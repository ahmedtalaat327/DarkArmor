using DarkArmor.ViewModels.Messagaes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DarkArmor.Views.Messages
{
    /// <summary>
    /// Interaction logic for SpeediSetup.xaml
    /// </summary>
    public partial class SpeediSetupMessage : UserControl
    {
        public SpeediSetupMessageViewModel SpeediSetupMessageViewModel { get; set; } = new SpeediSetupMessageViewModel();
        public SpeediSetupMessage()
        {
            DataContext = SpeediSetupMessageViewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SpeediSetupMessageViewModel.UnpackProcessStatus = "Finished";
        }
    }
}
