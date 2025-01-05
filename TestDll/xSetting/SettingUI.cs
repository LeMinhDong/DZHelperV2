using CommunityToolkit.Mvvm.ComponentModel;
using DZHelper.HelperCsharf;

namespace TestDll.xSetting
{
    public partial class SettingUI:ObservableObject
    {
        [ObservableProperty]
        private string command;

        [ObservableProperty]
        private string tapValue;

        [ObservableProperty]
        private string remoteFilePath;

        [ObservableProperty]
        private string localFilePath;

        [ObservableProperty]
        private string inputTypeValue;

        [ObservableProperty]
        private string textInput = "Text input";

        public int GetStringInput()
        {
            int index = 0;

            return index;
        }
        public string IndexInputData(int index)
        {
            if(string.IsNullOrWhiteSpace(TextInput))
                return string.Empty;
            var lines = TextInput.ParseLines(StringSplitOptions.None);
            if ((lines.Count > index))
                return lines[index];
            return string.Empty;
        }

    }
}
