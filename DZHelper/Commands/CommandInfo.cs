using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DZHelper.Models;
using System.Collections.ObjectModel;
using System.Reflection;

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
        public int Index { get; set; }
        public bool IsEnabled { get; set; } = true;
        public Func<Task<object>> Action { get; set; } // Delegate cho phương thức không có tham số
        public bool ForeachDevices { get; set; }
        public AsyncRelayCommand Command { get; set; }
        public MethodInfo MethodInfo { get; set; } // Thêm thông tin MethodInfo

        public override bool Equals(object obj)
        {
            // Kiểm tra null và kiểu của obj
            if (obj == null || GetType() != obj.GetType())
                return false;

            // So sánh chỉ dựa trên Name
            var other = (CommandInfo)obj;
            return Name == other.Name;
        }

        // Ghi đè GetHashCode
        public override int GetHashCode()
        {
            // Chỉ sử dụng Name để tạo hash code
            return Name?.GetHashCode() ?? 0;
        }
    }
}
