using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DZHelper.Commands;
using DZHelper.HelperCsharf;
using DZHelper.Helpers;
using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using DZHelper.ViewModels;
using System.Collections.ObjectModel;
using System.Reflection;

namespace TestDll.ViewModels
{
    public partial class MainViewModel:BaseViewModel
    {
        [ObservableProperty]
        private List<CommandInfo> commands;

        public ObservableCollection<GroupedCommand> GroupedCommands { get; set; }

        [ObservableProperty]
        public ObservableCollection<LdModel> devices = new ObservableCollection<LdModel>();

        public MainViewModel()
        {
            LdplayerHelper.SetPath("C:\\LDPlayer\\LDPlayer64\\ldconsole.exe");
            Commands = CommandHelper.GenerateCommands();
            foreach (var Command in Commands)
            {
                if (Command.IsForeach)
                {
                    Command.Command = new AsyncRelayCommand(() => MainAction(Command.Action));
                }
            }

            GroupedCommands = new ObservableCollection<GroupedCommand>(
            Commands
                .GroupBy(cmd => cmd.Group) // Nhóm các lệnh theo thuộc tính Group
                .Select(g => new GroupedCommand
                {
                    GroupName = g.Key, // Tên nhóm
                    Commands = new ObservableCollection<CommandInfo>(g.ToList()) // Các lệnh trong nhóm
                })
            );
        }
        

        private async Task MainAction(Func<LdModel, Task> action)
        {

            if (Devices.Count == 0)
                Devices.Add(new LdModel() { Name = "name", Index = "0" });


            foreach (var device in Devices)
            {
                try
                {
                    await action(device);
                }
                catch (Exception ex)
                {
                }
            }
        }


        
        
    }
}
