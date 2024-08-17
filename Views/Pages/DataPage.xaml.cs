using DarkArmor.Models.Skeleton;
using DarkArmor.ViewModels.Pages;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace DarkArmor.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            NICController selectedNIC =(NICController)((ComboBox)sender).SelectedItem;
            ViewModel.OnNicChose(selectedNIC);
        }
    }
}
