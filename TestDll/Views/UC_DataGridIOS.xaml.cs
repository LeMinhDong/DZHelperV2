using DZHelper.ProjectInitialize;
using System.Windows.Controls;
using TestDll.ViewModels;

namespace TestDll.Views
{
    /// <summary>
    /// Interaction logic for UC_DataGridIOS.xaml
    /// </summary>
    public partial class UC_DataGridIOS : UserControl
    {
        public UC_DataGridIOS()
        {
            InitializeComponent();
            _grid.DataGrid_InitializeShortcuts_V1();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var VM = DataContext as MainViewModel;
            VM.XSettingData.Datagrid = _grid;
        }
    }
}
