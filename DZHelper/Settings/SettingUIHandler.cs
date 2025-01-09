using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;

namespace DZHelper.Settings
{
    public static class SettingUIHandler
    {
        /// <summary>
        /// Lưu các thuộc tính của object vào file JSON.
        /// </summary>
        /// <param name="settingsObject">Object chứa các thuộc tính cần lưu.</param>
        /// <param name="filePath">Đường dẫn đến file JSON.</param>

        public static void SaveSetting(object settingsObject, string filePath = "Config.json")
        {
            if (settingsObject == null)
                return;

            var jsonObject = new JObject();
            foreach (var property in settingsObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanRead)
                {
                    try
                    {
                        var value = property.GetValue(settingsObject);
                        jsonObject[property.Name] = value != null ? JToken.FromObject(value) : JValue.CreateNull();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            File.WriteAllText(filePath, jsonObject.ToString());
        }

        /// <summary>
        /// Nạp các thuộc tính từ file JSON vào object.
        /// </summary>
        /// <param name="settingsObject">Object mà bạn muốn gán các thuộc tính đã nạp từ file JSON.</param>
        /// <param name="filePath">Đường dẫn đến file JSON.</param>

        public static void OpenSetting(object settingsObject, string filePath = "Config.json")
        {
            if (settingsObject == null)
                return;

            if (!File.Exists(filePath))
                return;

            try
            {
                var jsonObject = JObject.Parse(File.ReadAllText(filePath));

                foreach (var property in settingsObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (property.CanWrite && jsonObject.TryGetValue(property.Name, out JToken value))
                    {
                        var convertedValue = value.ToObject(property.PropertyType);
                        property.SetValue(settingsObject, convertedValue);
                    }
                }
            }
            catch (Exception)
            {

            }
            
        }
    }
}
