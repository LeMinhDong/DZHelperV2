using DZHelper.Controls;
using DZHelper.ProjectInitialize;
using System.Windows.Controls;

namespace TestDll.Views
{
    /// <summary>
    /// Interaction logic for UC_DataGrid.xaml
    /// </summary>
    public partial class UC_DataGrid : UserControl
    {
        public UC_DataGrid()
        {
            InitializeComponent();
            _grid.DataGrid_InitializeShortcuts_V1();
           
        }
        
    }
}
