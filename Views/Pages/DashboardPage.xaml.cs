
using DarkArmor.Models;
using DarkArmor.ViewModels.Pages;
using Helpers.DarkArmor;
using System.Windows.Controls;
using Wpf.Ui.Controls;
using System.Windows.Threading;
using DarkArmor.Models.Skeleton;
using System.Windows.Navigation;
using System;
using System.Reflection;

namespace DarkArmor.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        //private DispatcherTimer checkMyDataShowedCollection = new DispatcherTimer();

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();

            
            this.tabledata.AutoGeneratingColumn += Tabledata_AutoGeneratingColumn;


           // checkMyDataShowedCollection.Tick += CheckMyDataShowedCollection_Tick; ;
           // checkMyDataShowedCollection.Interval = new TimeSpan(0, 0, 1);

            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            
        }
        int xcount = 0;
        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ViewModel.DiscoveredNICControllers)))
            {
               
                    App.Current.Dispatcher.Invoke(() =>
                    {

                    try
                    {
                        ///////////////TO-MAKE-ChAHNGES-ACCROSS-MULTIPLE-THREADS//////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////
                        ViewModel.DataShowed.Add(new NetworkDevice()
                        {
                            DeviceIndex = xcount,
                            DomainName = "John Doe",
                            Type = Models.Skeleton.DeviceType.UDevice,
                            Active = true,
                            Nic = ViewModel.DiscoveredNICControllers[xcount]
                        });
                            //////////////////////////////////////////////////////////////////////////////////
                            //////////////////////////////////////////////////////////////////////////////////
                    }
                    catch (Exception notchangedcorrectly)
                    {

                    }
                    });
                    xcount++;
               
            }
        }

        /*
        private void CheckMyDataShowedCollection_Tick(object? sender, EventArgs e)
        {
            
            if (ViewModel.DiscoveredNICControllers.Count > ViewModel.DataShowed.Count)
            {
             //   int k = ViewModel.DiscoveredNICControllers.Count - ViewModel.DataShowed.Count - 1;

            for (int index = 0; index < ViewModel.DiscoveredNICControllers.Count; index++)
            {
                    if (ViewModel.DataShowed.Count > 0)
                    {
                        foreach (var x in ViewModel.DataShowed.ToArray())
                        {
                            if (x.Nic != ViewModel.DiscoveredNICControllers[index])
                            {
                                ViewModel.DataShowed.Add(new NetworkDevice()
                                {
                                    DeviceIndex = index,
                                    DomainName = "John Doe",
                                    Type = Models.Skeleton.DeviceType.UDevice,
                                    //Status = new SolidColorBrush((Color)ColorConverter.ConvertFromS tring("#ED1C24")) ,
                                    Active = true,
                                    Nic = ViewModel.DiscoveredNICControllers[index]
                                });
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        ViewModel.DataShowed.Add(new NetworkDevice()
                        {
                            DeviceIndex = index,
                            DomainName = "John Doe",
                            Type = Models.Skeleton.DeviceType.UDevice,
                            //Status = new SolidColorBrush((Color)ColorConverter.ConvertFromS tring("#ED1C24")) ,
                            Active = true,
                            Nic = ViewModel.DiscoveredNICControllers[index]
                        });
                    }
                
                    
             
            }
            }
            else
            {
                return;
            }
            

          
        }
        */

        private void Tabledata_AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if ((string)e.Column.Header == nameof(NetworkDevice.OSName))
            {
                e.Cancel = true;
            }
            if ((string)e.Column.Header == nameof(NetworkDevice.Active))
            {
                e.Cancel = true;
            }
            if ((string)e.Column.Header == nameof(NetworkDevice.Type))
            {
                e.Cancel = true;
            }
            if ((string)e.Column.Header == nameof(NetworkDevice.Nic))
            {
                e.Cancel = true;
            }
            else
            {
                e.Column.Width = new DataGridLength(1,DataGridLengthUnitType.Auto);
            }

            e.Column.CanUserSort = false;

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

        private void PART_Editor_UnChecked(object sender, RoutedEventArgs e)
        {

            ToggleSwitch checkBox = (ToggleSwitch)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (index == -1) return;

            //command here
            ViewModel.OnToggleUnCheck(index);
        }

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            ToggleSwitch checkBox = (ToggleSwitch)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (index == -1) return;

            
            ViewModel.OnToggleCheck(index);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            xcount = 0;
            //checkMyDataShowedCollection.Start();
        }
    }
}
