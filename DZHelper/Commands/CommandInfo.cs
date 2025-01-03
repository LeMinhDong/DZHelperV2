using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DZHelper.Models;
using System.Collections.ObjectModel;

namespace DZHelper.Commands
{
    public partial class GroupedCommand : ObservableObject
    {

        [ObservableProperty] private string groupName;
        [ObservableProperty] private bool isExpanded;
        public ObservableCollection<CommandInfo> Commands { get; set; }

    }

    public class CommandInfo
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public bool IsEnabled { get; set; } = true;
        public Func<BaseModel, Task> Action { get; set; }
        public AsyncRelayCommand Command { get; set; }

        public string Description { get; set; }
        public string IconPath { get; set; }
    }
}
