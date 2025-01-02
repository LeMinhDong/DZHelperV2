using System.IO;
using System.Text;
using System.Windows.Shapes;

namespace DZHelper.HelperCsharf
{
    public static class DZFileHeper
    {
        private static readonly object _lock = new object();
        public static void WriteLine(string filePath,string content)
        {
            CreateFileIfNotExits(filePath);
            using (FileStream fileStream = new FileStream(filePath,FileMode.OpenOrCreate,FileAccess.Write,FileShare.ReadWrite))          // Cho phép các process khác đọc/ghi
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine(content);
                    writer.Flush(); // Đảm bảo dữ liệu được ghi ngay lập tức
                }
            }
        }

        public static void CreateFileIfNotExits(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                // Chuyển chuỗi thành byte[]
                byte[] data = Encoding.UTF8.GetBytes("");

                // Ghi dữ liệu vào file
                fileStream.Write(data, 0, data.Length);
            }
        }
        public static void ClearFile(string filePath)
        {
            if (!File.Exists(filePath)) return;
            lock (_lock)
            {
                try
                {
                    File.WriteAllText(filePath, string.Empty, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error clearing file: {ex.Message}");
                }
            }
        }
        public static string ReadAllText(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) // Cho phép các process khác đọc/ghi
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }

        public static void RemoveLinesContaining(string filePath, string text)
        {
            lock (_lock)
            {
                try
                {
                    var lines = ReadAllText(filePath).ParseLines();
                    lines = lines.Where(item => !item.Contains(text,StringComparison.OrdinalIgnoreCase)).ToList();
                    if (lines != null)
                        File.WriteAllLines(filePath, lines);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing lines: {ex.Message}");
                }
            }
        }


    }
}
