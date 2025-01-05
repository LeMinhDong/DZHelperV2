using CommunityToolkit.Mvvm.ComponentModel;
using DZHelper.Commands;
using System.Collections.ObjectModel;
using TestDll.Models;

namespace TestDll.xSetting
{
    public partial class SettingData:ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<LdDevice> devices = new ObservableCollection<LdDevice>();

        [ObservableProperty]
        private ObservableCollection<CommandInfo> commandsInfo = new ObservableCollection<CommandInfo>();

        [ObservableProperty]
        private ObservableCollection<CommandInfo> commandsMain = new ObservableCollection<CommandInfo>();

        [ObservableProperty]
        public ObservableCollection<GroupedCommand> groupedCommands;

        [ObservableProperty]
        private string status = "Stop";
    }
}
