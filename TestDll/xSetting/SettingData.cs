using CommunityToolkit.Mvvm.ComponentModel;
using DZHelper.Commands;
using DZHelper.Models;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using TestDll.Models;

namespace TestDll.xSetting
{
    public partial class SettingData:ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<IMouseDevice> devices = new ObservableCollection<IMouseDevice>();

        [ObservableProperty]
        private ObservableCollection<CommandInfo> commandsInfo = new ObservableCollection<CommandInfo>();

        [ObservableProperty]
        private ObservableCollection<CommandInfo> commandsMain = new ObservableCollection<CommandInfo>();

        [ObservableProperty]
        public ObservableCollection<GroupedCommand> groupedCommands;

        [ObservableProperty]
        private string status = "Stop";

        [ObservableProperty]
        private DataGrid datagrid;
    }
}
