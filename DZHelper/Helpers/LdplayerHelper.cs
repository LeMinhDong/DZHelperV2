using DZHelper.Commands;
using DZHelper.Extensions;
using DZHelper.HelperCsharf;
using DZHelper.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace DZHelper.Helpers
{
    public static class LdplayerHelper
    {
        private static string PathLD = "C:\\LDPlayer\\LDPlayer64\\ldconsole.exe";

        public static void SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            PathLD = path;
        }
        

        public static void LaunchInstance(LdModel item)
        {
            item.ChangeProperty($"launch");
            ExecuteCommand($"launch --index {item.Index}");
        }

        public static void QuitInstance(LdModel item)
        {
            item.ChangeProperty("quit");
            ExecuteCommand($"quit --index {item.Index}");
        }

        public static void QuitAllInstances()
        {
            ExecuteCommand("quitall");
        }

        public static void AddInstance(LdModel item, string name)//
        {
            item.ChangeProperty($"add {name}");
            ExecuteCommand($"add --name {item.TextInput}");
        }

        public static void CopyInstance(LdModel item)//
        {
            item.ChangeProperty($"copy {item.TextInput}");
            ExecuteCommand($"copy --name {item.TextInput} --from {item.Index}");
        }

        public static void RemoveInstance(LdModel item)//
        {
            item.ChangeProperty($"remove {item.Index}");
            ExecuteCommand($"remove --index {item.Index}");
        }

        public static void RenameInstance(LdModel item, string newTitle)//
        {
            item.ChangeProperty($"rename {item.Index}");
            ExecuteCommand($"rename --index {item.Index} --title {newTitle}");
        }

        public static void InstallAppFromFile(LdModel item, string apkFilePath)
        {
            item.ChangeProperty("install apk");
            ExecuteCommand($"installapp --index {item.Index} --filename \"{apkFilePath}\"");
        }

        public static void InstallAppFromPackage(LdModel item, string packageName)
        {
            item.ChangeProperty("install package");
            ExecuteCommand($"installapp --index {item.Index} --packagename {packageName}");
        }

        public static void UninstallApp(LdModel item, string packageName)
        {
            item.ChangeProperty("uninstallapp package");
            ExecuteCommand($"uninstallapp --index {item.Index} --packagename {packageName}");
        }

        public static void BackupInstance(LdModel item, string filePath)
        {
            item.ChangeProperty($"backup {filePath}");
            ExecuteCommand($"backup --index {item.Index} --file \"{filePath}\"");
        }

        public static void RestoreInstance(LdModel item, string filePath)
        {
            item.ChangeProperty($"restore {filePath}");
            ExecuteCommand($"restore --index {item.Index} --file \"{filePath}\"");
        }

        public static void modifyRandom(LdModel item)
        {
            item.ChangeProperty($"modify Random");
            ExecuteCommand($"modify  --index {item.Index} --imei auto --androidid auto --mac auto");
        }

       
        public static void Pull(LdModel item, string remoteFilePath, string localFilePath)
        {
            item.ChangeProperty($"Pull");
            ExecuteCommand($@"pull --index {item.Index} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }
        
        

        public static void Push(LdModel item, string remoteFilePath, string localFilePath)
        {
            item.ChangeProperty($"Push");
            ExecuteCommand($@"push --index {item.Index} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }
        public static void InputText(LdModel item)
        {
            item.ChangeProperty($"input text");
            string text = item.TextInput.EscapeString();
            ExecuteCommand($"adb --index {item.Index} --command \"shell input text '{text}'\"");
        }
        public static void RestartAdb(LdModel item)
        {
            item.ChangeProperty($"Restart Adb");
            ExecuteCMD($"adb kill-server");
            ExecuteCMD($"adb start-server");
        }
        public static void ReconnectOffline(LdModel item)
        {
            item.ChangeProperty($"Reconnect Offline");
            ExecuteCMD($"adb reconnect offline");
        }
        public static List<string> GetRunningInstances(LdModel item)
        {
            item.ChangeProperty("runninglist");
            var result = ExecuteCommandForResult("runninglist");
            return ParseList(result);
        }

        public static List<string> GetAllInstances(LdModel item)
        {
            item.ChangeProperty("list");
            var result = ExecuteCommandForResult("list");
            return ParseList(result);
        }

        public static bool IsInstanceRunning(LdModel item)
        {
            var result = ExecuteCommandForResult($"isrunning --index {item.Index}");
            if (result.Trim().Equals("running", StringComparison.OrdinalIgnoreCase))
            {
                item.ChangeProperty("Device Running");
                return true;
            }
            item.ChangeProperty("Device Offine");
            return false;
        }

        private static void ExecuteCommand(string command)
        {
            Process cmdProcess;
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.WorkingDirectory = PathLD;
            cmdProcess.StartInfo.Verb = "runas";
            cmdProcess.StartInfo.Arguments = "/C " + command;


            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit(6000);
        }

        private static string ExecuteCommandForResult(string command)
        {
            Process cmdProcess;
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.WorkingDirectory = PathLD;
            cmdProcess.StartInfo.Verb = "runas";
            cmdProcess.StartInfo.Arguments = "/C " + command;


            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit(6000);
            if (output == null) return "";
            return output;
        }

        public static void ExecuteCMD(string command)
        {
            Process cmdProcess;
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(PathLD);
            cmdProcess.StartInfo.Verb = "runas";
            cmdProcess.StartInfo.Arguments = "/C " + command;


            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit(6000);
        }

        public static string ExecuteCMD_ForResult(string command)
        {
            Process cmdProcess;
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(PathLD);
            cmdProcess.StartInfo.Verb = "runas";
            cmdProcess.StartInfo.Arguments = "/C " + command;


            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.Start();
            string output = cmdProcess.StandardOutput.ReadToEnd();
            cmdProcess.WaitForExit(6000);
            if (output == null) return "";
            return output;
        }

        private static List<string> ParseList(string output)
        {
            if (string.IsNullOrWhiteSpace(output))
                return new List<string>();
            return new List<string>(output.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
