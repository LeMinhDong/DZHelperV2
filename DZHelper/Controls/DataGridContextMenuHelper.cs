using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DZHelper.Controls
{
    public static class DataGridContextMenuHelper
    {
        /// <summary>
        /// Thêm hoặc cập nhật ContextMenu cho DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid cần thêm ContextMenu.</param>
        /// <param name="menuItems">Danh sách các mục trong ContextMenu (tên mục và hành động).</param>
        public static void AddContextMenu(this DataGrid dataGrid, params (string Header, Action<DataGridRow> Action)[] menuItems)
        {
            if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));

            var contextMenu = new ContextMenu();

            foreach (var (header, action) in menuItems)
            {
                var menuItem = new MenuItem { Header = header };
                menuItem.Click += (s, e) =>
                {
                    if (dataGrid.SelectedItem is not null && dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.SelectedItem) is DataGridRow row)
                    {
                        action?.Invoke(row);
                    }
                };
                contextMenu.Items.Add(menuItem);
            }

            dataGrid.ContextMenu = contextMenu;
        }

        /// <summary>
        /// Xóa ContextMenu khỏi DataGrid.
        /// </summary>
        /// <param name="dataGrid">DataGrid cần xóa ContextMenu.</param>
        public static void RemoveContextMenu(this DataGrid dataGrid)
        {
            if (dataGrid == null) throw new ArgumentNullException(nameof(dataGrid));
            dataGrid.ContextMenu = null;
        }
    }
}
