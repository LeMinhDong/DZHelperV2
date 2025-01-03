
using DZHelper.Helpers;

namespace DZHelper.Commands
{
    public static  class CommandHelper
    {
        public static List<CommandInfo> LoadCommand_LdPlayer()
        {
            List<CommandInfo> commands = new List<CommandInfo>();
            return commands;
        }

        public static List<CommandInfo> LoadCommand_Adb()
        {
            List<CommandInfo> commands = new List<CommandInfo>();
            return commands;
        }

        public static List<CommandInfo> LoadCommand_Selenium()
        {
            List<CommandInfo> commands = new List<CommandInfo>();
            return commands;
        }

        public static List<CommandInfo> CommandsMain(int index)
        {
            List<CommandInfo> commands = new List<CommandInfo>();

            commands.Add(new CommandInfo()
            {
                Name = "Load Devices",
                Action = async item => LdplayerHelper.LaunchInstance(item)
            });
            foreach (var command in commands)
            {
                command.Group = "LdPlayer-Main";
            }
            return commands;
        }
    }
}
