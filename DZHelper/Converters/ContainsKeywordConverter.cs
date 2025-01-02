using System.Globalization;
using System.Windows.Data;

namespace DZHelper.Converters
{
    public class ContainsKeywordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Lấy từ khóa từ parameter
            string keyword = parameter as string;
            if (string.IsNullOrEmpty(keyword) || value == null)
                return false;

            // Kiểm tra nếu giá trị chứa từ khóa
            string cellValue = value.ToString();
            return cellValue.Contains(keyword, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DefaultKeywordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Lấy danh sách từ khóa từ parameter
            var keywords = parameter as List<string>;
            if (keywords == null || value == null)
                return false;

            // Kiểm tra nếu giá trị không rỗng và không thuộc danh sách từ khóa
            string cellValue = value.ToString();
            return !string.IsNullOrEmpty(cellValue) && (!keywords.Any(keyword => cellValue.Contains(keyword, StringComparison.OrdinalIgnoreCase) || cellValue.StartsWith(".")));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
