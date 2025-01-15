using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DZHelper.Helpers
{
    public class AutoImouseApi
    {
        private static HttpClient _httpClient;
        private static string _baseUrl;

        public void SetPath(string baseUrl)
        {
            _baseUrl = "http://127.0.0.1:9912/api";
            //_baseUrl = baseUrl;
            _httpClient = new HttpClient();
        }

       

        private static async Task<string> PostAsync(string endpoint, string payload)
        {
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/{endpoint}", content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task TapAsync(string deviceId, int x, int y)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"x\":{x},\"y\":{y}}}";
            await PostAsync("tap", payload);
        }

        public async Task ClickAsync(string deviceId, int x, int y, string button = "left")
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"x\":{x},\"y\":{y},\"button\":\"{button}\"}}";
            await PostAsync("click", payload);
        }


        public static async Task SwipeAsync(string deviceId, int startX, int startY, int endX, int endY)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"sx\":{startX},\"sy\":{startY},\"ex\":{endX},\"ey\":{endY}}}";
            await PostAsync("swipe", payload);
        }

        public static async Task LongPressAsync(string deviceId, int x, int y, int duration)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"x\":{x},\"y\":{y},\"duration\":{duration}}}";
            await PostAsync("long_press", payload);
        }

        public static async Task ScreenshotAsync(string deviceId)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\"}}";
            var response = await PostAsync("screenshot", payload);
        }

        public static async Task<string> LoadDevicesAsync()
        {
            return await PostAsync("get_device_list", "{}");
        }

        public async Task MouseWheelAsync(string deviceId, string direction, int length, int number)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"direction\":\"{direction}\",\"length\":{length},\"number\":{number}}}";
            await PostAsync("mouse_wheel", payload);
            Console.WriteLine("Mouse wheel action executed.");
        }

        public async Task FindImageAsync(string deviceId, string imageBase64, int[] rect = null, bool original = false, float similarity = 0.8f)
        {
            var rectString = rect != null ? $"[{string.Join(",", rect)}]" : "null";
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"img\":\"{imageBase64}\",\"rect\":{rectString},\"original\":{original.ToString().ToLower()},\"similarity\":{similarity}}}";
            var response = await PostAsync("find_image", payload);
            Console.WriteLine($"Find image response: {response}");
        }

        public async Task OcrAsync(string deviceId, int[] rect, bool original = false)
        {
            var rectString = $"[{string.Join(",", rect)}]";
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"rect\":{rectString},\"original\":{original.ToString().ToLower()}}}";
            var response = await PostAsync("ocr", payload);
            Console.WriteLine($"OCR response: {response}");
        }

        public async Task OcrExAsync(string deviceId, int[] rect, bool original = false)
        {
            var rectString = $"[{string.Join(",", rect)}]";
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"rect\":{rectString},\"original\":{original.ToString().ToLower()}}}";
            var response = await PostAsync("ocr_ex", payload);
            Console.WriteLine($"OCR Ex response: {response}");
        }

        public async Task ShortcutGetClipboardAsync(string deviceId)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\"}}";
            var response = await PostAsync("shortcut_get_clipboard", payload);
            Console.WriteLine($"Clipboard content: {response}");
        }

        public async Task ShortcutOpenUrlAsync(string deviceId, string url)
        {
            var payload = $"{{\"deviceid\":\"{deviceId}\",\"url\":\"{url}\"}}";
            await PostAsync("shortcut_open_url", payload);
            Console.WriteLine($"URL '{url}' opened successfully on device '{deviceId}'.");
        }
    }
}
