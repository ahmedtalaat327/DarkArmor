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
using Wpf.Ui.Controls;

namespace DarkArmor.Views.Messages
{
    /// <summary>
    /// Interaction logic for ProfileSettingsMessage.xaml
    /// </summary>
    public partial class ProfileSettingsMessage : UserControl
    {

        public static ProfileSettingsMessageViewModel ViewModel { get; set; } = new ProfileSettingsMessageViewModel();

        public ProfileSettingsMessage() {
           
            DataContext = ViewModel;

            InitializeComponent();
        }

      

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SubwindVisibility = false;
        }
    }
}
