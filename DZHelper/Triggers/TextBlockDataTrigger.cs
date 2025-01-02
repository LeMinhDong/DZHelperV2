using DZHelper.Converters;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;

namespace DZHelper.Triggers
{
    public static class TextBlockDataTrigger
    {
        public static void TextBlockStyle_Status(this TextBlock textBlock, string PropertyBinding="Status", Dictionary<string, Brush> keywordColors=null)
        {
            // Khởi tạo style cho TextBlock
            if (keywordColors == null)
                keywordColors = new Dictionary<string, Brush>
                {
                    { "error", Brushes.Red },
                    { "warning", Brushes.Blue },
                    { "stop", Brushes.Black }
                };
            var style = new Style(typeof(TextBlock));

            foreach (var keywordColor in keywordColors)
            {
                string keyword = keywordColor.Key;
                Brush color = keywordColor.Value;
                // Tạo DataTrigger cho mỗi từ khóa
                var trigger = new DataTrigger
                {
                    Binding = new Binding(PropertyBinding)
                    {
                        Converter = new ContainsKeywordConverter(), // Converter kiểm tra chứa từ khóa
                        ConverterParameter = keyword               // Truyền từ khóa làm tham số
                    },
                    Value = true // Kích hoạt trigger nếu converter trả về true
                };


                // Thiết lập Setter cho Foreground
                trigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, color));

                // Thêm trigger vào style
                style.Triggers.Add(trigger);

            }

            // Tạo Trigger mặc định khi giá trị không khớp từ khóa và không rỗng
            var defaultTrigger = new DataTrigger
            {
                Binding = new Binding(PropertyBinding)
                {
                    Converter = new DefaultKeywordConverter(), // Converter kiểm tra giá trị không null hoặc rỗng
                    ConverterParameter = keywordColors.Keys.ToList() // Truyền danh sách các từ khóa làm tham số
                },
                Value = true // Kích hoạt nếu giá trị không null hoặc rỗng và không thuộc các từ khóa
            };

            // Đặt màu chữ mặc định
            defaultTrigger.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Blue));

            // Thêm Trigger mặc định vào Style
            style.Triggers.Add(defaultTrigger);

            textBlock.Style = style;
        }
    }
}
