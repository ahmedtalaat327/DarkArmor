
using DarkArmor.ViewModels.Pages;
using Helpers.DarkArmor;
using System.Windows.Controls;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace DarkArmor.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            this.tabledata.AutoGeneratingColumn += Tabledata_AutoGeneratingColumn;
        }

        private void Tabledata_AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if ((string)e.Column.Header == nameof(MyDeiceTest.Status))
            {
                e.Cancel = true;
            }
            if ((string)e.Column.Header == nameof(MyDeiceTest.Active))
            {
                e.Cancel = true;
            }

        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((Wpf.Ui.Controls.DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;
            else
            {
                this.messageControl.Visibility = Visibility.Visible;
            }
        }

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {

            ToggleSwitch checkBox = (ToggleSwitch)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (index == -1) return;


            ViewModel.DataShowed[index].Status = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#40f4cd"));

        }

        private void PART_Editor_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch checkBox = (ToggleSwitch)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (index == -1) return;

            
            ViewModel.DataShowed[index].Status = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#ED1C24"));
           
           


        }
    }
}
