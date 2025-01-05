using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using DZHelper.HelperCsharf;

namespace DZHelper.Controls
{
    public static class DataGridHeader
    {
        public static void EnableHeaderCheckboxHandling(this DataGrid dataGrid, Action<bool> onCheckboxClick)
        {
            if (dataGrid == null || onCheckboxClick == null)
                return;

            // Lặp qua tất cả các cột trong DataGrid
            foreach (var column in dataGrid.Columns)
            {
                if (column is DataGridCheckBoxColumn checkBoxColumn)
                {
                    // Tạo CheckBox cho Header
                    var headerCheckBox = new CheckBox
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    // Gán sự kiện Click cho Header CheckBox
                    headerCheckBox.Click += (s, e) =>
                    {
                        if (s is CheckBox headerCheckBox)
                        {
                            bool isChecked = headerCheckBox.IsChecked == true;
                            onCheckboxClick(isChecked); // Gọi delegate xử lý logic Selected Items
                        }
                    };

                    // Gán CheckBox vào Header
                    checkBoxColumn.Header = headerCheckBox;
                }
            }

            
        }

    }
}
