

using DZHelper.Converters;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace DZHelper.Triggers
{
    public static class DataGridDataTrigger
    {
        public static void DataGridRowStyle_StatusContains(this DataGrid dataGrid, string statusColumnName, Dictionary<string, Brush> keywordColors)
        {
            if (keywordColors == null)
                keywordColors = new Dictionary<string, Brush>
{
                { "error", Brushes.Red },
                { "warning", Brushes.Orange },
                { "pause", Brushes.Orange },
                { "stop", Brushes.Black }
            };


            // Tạo Style cho DataGridCell
            var cellStyle = new Style(typeof(DataGridCell));

            // Duyệt qua mỗi từ khóa và màu sắc
            foreach (var keywordColor in keywordColors)
            {
                string keyword = keywordColor.Key;
                Brush color = keywordColor.Value;

                // Tạo DataTrigger cho mỗi từ khóa
                var trigger = new DataTrigger
                {
                    Binding = new Binding(statusColumnName)
                    {
                        Converter = new ContainsKeywordConverter(), // Converter kiểm tra chứa từ khóa
                        ConverterParameter = keyword               // Truyền từ khóa làm tham số
                    },
                    Value = true // Kích hoạt trigger nếu converter trả về true
                };

                // Đặt màu chữ theo từ khóa
                trigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, color));

                // Thêm Trigger vào Style
                cellStyle.Triggers.Add(trigger);
            }

            // Tạo Trigger mặc định khi giá trị không khớp từ khóa và không rỗng
            var defaultTrigger = new DataTrigger
            {
                Binding = new Binding(statusColumnName)
                {
                    Converter = new DefaultKeywordConverter(), // Converter kiểm tra giá trị không null hoặc rỗng
                    ConverterParameter = keywordColors.Keys.ToList() // Truyền danh sách các từ khóa làm tham số
                },
                Value = true // Kích hoạt nếu giá trị không null hoặc rỗng và không thuộc các từ khóa
            };

            // Đặt màu chữ mặc định
            defaultTrigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Blue));

            // Thêm Trigger mặc định vào Style
            cellStyle.Triggers.Add(defaultTrigger);

            // Áp dụng Style cho CellStyle của DataGrid
            dataGrid.CellStyle = cellStyle;
        }
    }
}
