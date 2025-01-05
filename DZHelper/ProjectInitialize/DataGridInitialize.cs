using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using DZHelper.Controls;
using DZHelper.ViewModels;
using DZHelper.Triggers;
using System;

namespace DZHelper.ProjectInitialize
{
    public static class DataGridInitialize
    {
        public static void DataGrid_InitializeShortcuts_V1(this DataGrid dataGrid)
        {
            dataGrid.InputKeyBinding(); //keyinput binding
            dataGrid.InitLoadingRow(); //loading row
            dataGrid.EnableHeaderCheckboxHandling(isChecked => { KeyBinding_ControlA(dataGrid,isChecked); });
            
            dataGrid.DataGridRowStyle_StatusContains("Status", null); //style textblock
        }
        
        public static void InputKeyBinding(this DataGrid dataGrid)
        {
            if (dataGrid == null) return;

            dataGrid.AddKeyBinding(new KeyGesture(Key.Enter), () =>
            {
                KeyBinding_Enter(dataGrid);
            });

            dataGrid.AddKeyBinding(new KeyGesture(Key.Enter, ModifierKeys.Shift), () =>
            {
                KeyBinding_ShiftEnter(dataGrid);
            });

            dataGrid.AddKeyBinding(new KeyGesture(Key.Enter, ModifierKeys.Control), () =>
            {
                KeyBinding_ControlEnter(dataGrid);
            });

            dataGrid.AddKeyBinding(new KeyGesture(Key.Enter, ModifierKeys.Alt), () =>
            {
                KeyBinding_AltEnter(dataGrid);
            });

            dataGrid.AddKeyBinding(new KeyGesture(Key.A, ModifierKeys.Control), () =>
            {
                KeyBinding_ControlA(dataGrid);
            });

            dataGrid.AddKeyBinding(new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift), () =>
            {
                KeyBinding_ControlShiftA(dataGrid);
            });

            dataGrid.AddKeyBinding(new KeyGesture(Key.V, ModifierKeys.Control), () =>
            {
                dataGrid.PasteDataFromClipboard();
            });

            dataGrid.ContextMenu = new ContextMenu();
            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Enter",
                Command = new RelayCommand(_ => KeyBinding_Enter(dataGrid))
            });

            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Shift + Enter",
                Command = new RelayCommand(_ => KeyBinding_ShiftEnter(dataGrid))
            });

            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Control + Enter",
                Command = new RelayCommand(_ => KeyBinding_ControlEnter(dataGrid))
            });
            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Alt + Enter",
                Command = new RelayCommand(_ => KeyBinding_AltEnter(dataGrid))
            });
            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Control + A",
                Command = new RelayCommand(_ => KeyBinding_ControlA(dataGrid))
            });
            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Control + Shift + A",
                Command = new RelayCommand(_ => KeyBinding_ControlShiftA(dataGrid))
            });
            dataGrid.ContextMenu.Items.Add(new MenuItem
            {
                Header = "Paste | Control + V",
                Command = new RelayCommand(_ => dataGrid.PasteDataFromClipboard())
            });
        }

        public static void InitLoadingRow(this DataGrid dataGrid)
        {
            if (dataGrid == null) return;

            // Đăng ký sự kiện LoadingRow
            dataGrid.LoadingRow += (sender, e) =>
            {
                e.Row.Header = (e.Row.GetIndex() + 1).ToString();
            };
        }

        private static void KeyBinding_Enter(DataGrid dataGrid)
        {
            // Lấy danh sách items từ DataGrid
            dataGrid.ToggleSelect(true, false);
            // Refresh DataGrid để cập nhật UI
            dataGrid.Items.Refresh();
        }

        private static void KeyBinding_ControlEnter(DataGrid dataGrid)
        {
            // Lấy danh sách items từ DataGrid
            dataGrid.ToggleSelectSelectedOnly(false);
            // Refresh DataGrid để cập nhật UI
            dataGrid.Items.Refresh();
        }

        private static void KeyBinding_ShiftEnter(DataGrid dataGrid)
        {
            // Lấy danh sách items từ DataGrid
            dataGrid.ToggleSelectSelectedOnly(true);
            // Refresh DataGrid để cập nhật UI
            dataGrid.Items.Refresh();
        }

        private static void KeyBinding_AltEnter(DataGrid dataGrid)
        {
            // Lấy danh sách items từ DataGrid
            dataGrid.ToggleSelect(false, true);
            // Refresh DataGrid để cập nhật UI
            dataGrid.Items.Refresh();
        }

        public static void KeyBinding_ControlA(DataGrid dataGrid,bool isSelect = true)
        {
            // Lấy danh sách items từ DataGrid
            dataGrid.SetSelectAll(isSelect);
            //dataGrid.Items.Refresh();
            // Refresh DataGrid để cập nhật UI
        }

        private static void KeyBinding_ControlShiftA(DataGrid dataGrid)
        {
            // Lấy danh sách items từ DataGrid
            dataGrid.SetSelectAll(false);
            //dataGrid.Items.Refresh();
            // Refresh DataGrid để cập nhật UI
        }

        public static void ToggleSelect(this DataGrid dataGrid, bool selectedRowSelect, bool notSelectedRowSelect)
        {
            var itemsSource = dataGrid.ItemsSource as IEnumerable<object>;
            if (itemsSource == null) return;
            foreach (var item in itemsSource)
            {
                bool hasSelectedCell = dataGrid.SelectedCells.Any(cell => cell.Item == item);
                SetSelectProperty(item, hasSelectedCell ? selectedRowSelect : notSelectedRowSelect);
            }
        }

        public static void ToggleSelectSelectedOnly(this DataGrid dataGrid, bool selectedRowSelect)
        {
            var itemsSource = dataGrid.ItemsSource as IEnumerable<object>;
            if (itemsSource == null) return;
            foreach (var item in itemsSource)
            {
                bool isSelected = dataGrid.SelectedCells.Any(cell => cell.Item == item);
                if (isSelected)
                {
                    SetSelectProperty(item, selectedRowSelect);
                }
            }
        }

        public static void SetSelectAll(this DataGrid dataGrid, bool selectValue)
        {
            var itemsSource = dataGrid.ItemsSource as IEnumerable<object>;
            if (itemsSource == null) return;
            foreach (var item in itemsSource)
            {
                SetSelectProperty(item, selectValue);
            }
        }

        private static void SetSelectProperty(object item, bool selectValue)
        {
            var property = item.GetType().GetProperty("Select");
            if (property != null && property.PropertyType == typeof(bool) && property.CanWrite)
            {
                property.SetValue(item, selectValue);
            }
        }

        public static void PasteDataFromClipboard(this DataGrid dataGrid)
        {
            // Xác định cột đích từ ô đang chọn
            string targetColumn = GetTargetColumnFromSelectedCell(dataGrid);
            if (string.IsNullOrEmpty(targetColumn))
                return;

            // Lấy dữ liệu từ Clipboard
            List<object> datas = GetDataFromClipboard();
            if (datas == null || datas.Count == 0)
                return;

            // Paste dữ liệu vào các hàng được chọn
            dataGrid.PasteDataToSelectedRows(datas, targetColumn);
        }

        private static string GetTargetColumnFromSelectedCell(DataGrid dataGrid)
        {
            if (dataGrid.SelectedCells.Count > 0)
            {
                // Lấy cột đầu tiên từ các ô đang chọn
                var cell = dataGrid.SelectedCells[0];
                var column = cell.Column as DataGridBoundColumn;

                // Nếu cột là DataGridBoundColumn, trả về tên Property gắn với cột đó
                if (column != null && column.Binding is System.Windows.Data.Binding binding)
                {
                    return binding.Path.Path; // Tên property của cột
                }
            }
            return null;
        }

        private static List<object> GetDataFromClipboard()
        {
            string clipboardText = Clipboard.GetText().Trim();
            if (string.IsNullOrEmpty(clipboardText))
                return null;

            // Tách dữ liệu thành từng dòng, mỗi dòng là một phần tử trong danh sách
            var lines = clipboardText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            return lines.Cast<object>().ToList();
        }

        public static void PasteDataToSelectedRows(this DataGrid dataGrid, List<object> datas, string targetColumn)
        {
            if (datas == null || datas.Count == 0)
                return;

            // Lấy các hàng được chọn (selected rows) trong DataGrid
            var selectedCells = dataGrid.SelectedCells;
            int dataCount = datas.Count;
            int rowCount = selectedCells.Count;

            for (int i = 0; i < selectedCells.Count; i++)
            {
                // Xác định index của dữ liệu cần paste vào ô
                int dataIndex = i % dataCount;

                // Lấy dữ liệu cần paste từ danh sách datas
                var data = datas[dataIndex];

                // Lấy thông tin Item và Column từ ô được chọn
                var cellInfo = selectedCells[i];
                var rowItem = cellInfo.Item; // Object của hàng
                var column = cellInfo.Column; // Cột được chọn

                // Kiểm tra nếu cột hiện tại trùng với cột mục tiêu
                if (column != null && column.SortMemberPath == targetColumn)
                {
                    // Gán giá trị vào cột đích của hàng
                    var property = rowItem.GetType().GetProperty(targetColumn);
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(rowItem, data);
                    }
                }
            }
        }
    }
}
