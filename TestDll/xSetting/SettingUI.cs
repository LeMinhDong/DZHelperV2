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

        [ObservableProperty]
        private Dictionary<string, bool> dic_Expanded;

        [ObservableProperty]
        private int numThreads = 2;

        #region Setting UI
        [ObservableProperty]
        private int tabcontrolIndex;
        #endregion

        #region Setting ADB
        [ObservableProperty]
        private string backupFilePath;

        [ObservableProperty]
        private string apkFolder;
        #endregion

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
