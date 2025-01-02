using DZHelper.Extensions;
using DZHelper.HelperCsharf;
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

        public static Task ExecuteActionAsync(object item, Funs actionId, string[] args)
        {
            //args[0] = index
            //args[1] = name
            //args[2] = input/status2/step
            //args[3] = 
            //args[4] = 
            if (actionId == Funs.Launch)
                LaunchInstance(item,args[0]);
            if (actionId == Funs.Quit)
                QuitInstance(item, args[0]);
            if (actionId == Funs.QuitAll)
                QuitAllInstances();
            if (actionId == Funs.Add)
                AddInstance(item, args[2]);//
            if (actionId == Funs.Copy)
                CopyInstance(item, args[1], args[2]);//
            if (actionId == Funs.Remove)
                RemoveInstance(item, args[0]);//
            if (actionId == Funs.Rename)
                RenameInstance(item, args[0], args[2]);
            if (actionId == Funs.InstallApk)
                InstallAppFromFile(item, args[0],"path apk");
            if (actionId == Funs.InstallPackage)
                InstallAppFromPackage(item, args[0],"package name");
            if (actionId == Funs.UnInstallPackage)
                UninstallApp(item, args[0],"pagekage name");
            if (actionId == Funs.Backup)
                BackupInstance(item, args[0],"filename");
            if (actionId == Funs.Restore)
                RestoreInstance(item, args[0], "filename");
            if (actionId == Funs.Modify)
                modifyRandom(item, args[0]);
            if (actionId == Funs.Pull)
                Pull(item, args[0], "", "");
            if (actionId == Funs.Push)
                Push(item, args[0], "", "");
            if (actionId == Funs.InputText)
                InputText(item, args[0], args[2]);
            if (actionId == Funs.RestartAdb)
                RestartAdb(item);
            if (actionId == Funs.AdbReconnect)
                ReconnectOffline(item);


            return Task.CompletedTask;
        }

        public enum Funs
        {
            [Description("launch")]
            Launch = 0,
            [Description("Quit")]
            Quit = 1,
            [Description("QuitAll")]
            QuitAll = 2,
            [Description("Add")]
            Add = 3,
            [Description("Copy")]
            Copy = 4,
            [Description("Remove")]
            Remove = 5,
            [Description("rename")]
            Rename = 6,
            [Description("install Apk")]
            InstallApk = 7,
            [Description("install Package")]
            InstallPackage = 8,
            [Description("uninstall Package")]
            UnInstallPackage = 9,
            [Description("backup")]
            Backup = 10,
            [Description("restore")]
            Restore = 11,
            [Description("modify")]
            Modify = 12,
            [Description("restore")]
            Pull = 13,
            [Description("restore")]
            Push = 14,
            [Description("restore")]
            InputText = 15,
            [Description("adb Recconect Offline")]
            AdbReconnect = 16,
            [Description("restart Adb")]
            RestartAdb = 17
        }
        public static void LaunchInstance(object item, string nameOrIndex)
        {
            item.ChangeProperty($"launch");
            ExecuteCommand($"launch --index {nameOrIndex}");
        }

        public static void QuitInstance(object item, string nameOrIndex)
        {
            item.ChangeProperty("quit");
            ExecuteCommand($"quit --index {nameOrIndex}");
        }

        public static void QuitAllInstances()
        {
            ExecuteCommand("quitall");
        }

        public static void AddInstance(object item, string name)//
        {
            item.ChangeProperty($"add {name}");
            ExecuteCommand($"add --name {name}");
        }

        public static void CopyInstance(object item, string newName, string sourceNameOrIndex)//
        {
            item.ChangeProperty($"copy {newName}");
            ExecuteCommand($"copy --name {newName} --from {sourceNameOrIndex}");
        }

        public static void RemoveInstance(object item, string nameOrIndex)//
        {
            item.ChangeProperty($"remove {nameOrIndex}");
            ExecuteCommand($"remove --index {nameOrIndex}");
        }

        public static void RenameInstance(object item, string nameOrIndex, string newTitle)//
        {
            item.ChangeProperty($"rename {nameOrIndex}");
            ExecuteCommand($"rename --index {nameOrIndex} --title {newTitle}");
        }

        public static void InstallAppFromFile(object item, string nameOrIndex, string apkFilePath)
        {
            item.ChangeProperty("install apk");
            ExecuteCommand($"installapp --index {nameOrIndex} --filename \"{apkFilePath}\"");
        }

        public static void InstallAppFromPackage(object item, string nameOrIndex, string packageName)
        {
            item.ChangeProperty("install package");
            ExecuteCommand($"installapp --index {nameOrIndex} --packagename {packageName}");
        }

        public static void UninstallApp(object item, string nameOrIndex, string packageName)
        {
            item.ChangeProperty("uninstallapp package");
            ExecuteCommand($"uninstallapp --index {nameOrIndex} --packagename {packageName}");
        }

        public static void BackupInstance(object item, string nameOrIndex, string filePath)
        {
            item.ChangeProperty($"backup {filePath}");
            ExecuteCommand($"backup --index {nameOrIndex} --file \"{filePath}\"");
        }

        public static void RestoreInstance(object item, string nameOrIndex, string filePath)
        {
            item.ChangeProperty($"restore {filePath}");
            ExecuteCommand($"restore --index {nameOrIndex} --file \"{filePath}\"");
        }

        public static void modifyRandom(object item, string nameOrIndex)
        {
            item.ChangeProperty($"modify Random");
            ExecuteCommand($"modify  --index {nameOrIndex} --imei auto --androidid auto --mac auto");
        }

       
        public static void Pull(object item, string nameOrIndex, string remoteFilePath, string localFilePath)
        {
            item.ChangeProperty($"Pull");
            ExecuteCommand($@"pull --index {nameOrIndex} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }
        
        

        public static void Push(object item, string nameOrIndex, string remoteFilePath, string localFilePath)
        {
            item.ChangeProperty($"Push");
            ExecuteCommand($@"push --index {nameOrIndex} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }
        public static void InputText(object item, string nameOrIndex, string text)
        {
            item.ChangeProperty($"input text");
            text = text.EscapeString();
            ExecuteCommand($"adb --index {nameOrIndex} --command \"shell input text '{text}'\"");
        }
        public static void RestartAdb(object item)
        {
            item.ChangeProperty($"Restart Adb");
            ExecuteCMD($"adb kill-server");
            ExecuteCMD($"adb start-server");
        }
        public static void ReconnectOffline(object item)
        {
            item.ChangeProperty($"Reconnect Offline");
            ExecuteCMD($"adb reconnect offline");
        }
        public static List<string> GetRunningInstances(object item)
        {
            item.ChangeProperty("runninglist");
            var result = ExecuteCommandForResult("runninglist");
            return ParseList(result);
        }

        public static List<string> GetAllInstances(object item)
        {
            item.ChangeProperty("list");
            var result = ExecuteCommandForResult("list");
            return ParseList(result);
        }

        public static bool IsInstanceRunning(object item, string nameOrIndex)
        {
            var result = ExecuteCommandForResult($"isrunning --name {nameOrIndex}");
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
