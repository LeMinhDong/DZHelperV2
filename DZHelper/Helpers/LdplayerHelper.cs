using DZHelper.Commands;
using DZHelper.Extensions;
using DZHelper.HelperCsharf;
using DZHelper.Helpers.AttributeHelper;
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

        
        private static LdModel CastModel(object item)
        {
            if (item == null)
                throw new InvalidCastException("Cannot cast null object to LdModel");
            return item as LdModel;
        }

        #region Ld-Auto
        [MethodCategory("LdPlayer", "Ld-Auto", 1)]
        public static async Task ATest(object item,string command)
        {
            var model = CastModel(item);
            model.Status = "ATest";

            model.Status = "." + model.Status;
        }
        [MethodCategory("LdPlayer", "Ld-Auto", 4)]
        public static async Task Tap(object item,string tapValue)
        {
            var model = CastModel(item);
            model.Status = "quit";
            //await ExecuteCommand($"quit --index {model.Index}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Auto",  20)]
        public static async Task Pull(object item, string remoteFilePath, string localFilePath)
        {
            var model = CastModel(item);
            model.Status = "Pull";
            await ExecuteCommand($@"pull --index {model.Index} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Auto",  20)]
        public static async Task Push(object item, string remoteFilePath, string localFilePath)
        {
            var model = CastModel(item);
            model.Status = "Push";
            await ExecuteCommand($@"push --index {model.Index} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Auto",  20)]
        public static async Task InputText(object item,string inputText)
        {
            var model = CastModel(item);
            model.Status = "input text";
            string text = inputText.EscapeString();
            await ExecuteCommand($"action --index {model.Index} --key call.input --value '{text}'");
            model.Status = "." + model.Status;
        }


        [MethodCategory("LdPlayer", "Ld-Auto", 20)]
        public static async Task DumpXml(object item)
        {
            var model = CastModel(item);
            model.Status = "Dump Xml";
            await ExecuteCommand($"adb --index {model.Index} --command 'shell uiautomator dump'");
            var content = ExecuteCommandForResult($"adb --index {model.Index} --command 'shell cat /sdcard/window_dump.xml'").Result;
            await ExecuteCommand($"adb --index {model.Index} --command 'shell rm /sdcard/window_dump.xml'");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Auto", 20)]
        public static async Task Activity(object item)
        {
            var model = CastModel(item);
            model.Status = "Dump Activity";
            var content = ExecuteCommandForResult($"adb --index {model.Index} --command 'shell dumpsys window windows | grep mCurrentFocus'").Result;
            model.Status = "." + model.Status;
        }

        #endregion

        #region Ld-Adb
        [MethodCategory("LdPlayer", "Ld-Adb", 4)]
        public static async Task RestartAdb()
        {
            await ExecuteCMD($"adb kill-server");
            await ExecuteCMD($"adb start-server");
        }

        [MethodCategory("LdPlayer", "Ld-Adb", 5)]
        public static async Task ReconnectOffline()
        {
            await ExecuteCMD($"adb reconnect offline");
        }

        [MethodCategory("LdPlayer", "Ld-Adb", 5)]
        public static async Task AdbDevices()
        {
            await ExecuteCMD($"adb devices");
        }




        #endregion

        #region Ld-Device
        

        [MethodCategory("LdPlayer", "Main", 1, false)]
        public static async Task<List<LdModel>> LoadRunnings()
        {
            var runningstring = await ExecuteCommandForResult("runninglist");
            var list2string = await ExecuteCommandForResult("list2");

            var list = list2string.ParseLines().Where(itemAll => runningstring.ParseLines().Any(running => itemAll.Contains(running.Trim()))).ToList();
            return list.Select(item => new LdModel() { Index = item.Split(',')[0], Name = item.Split(',')[1] }).ToList();
        }

        [MethodCategory("LdPlayer", "Main", 2,false)]
        public static async Task<List<LdModel>> LoadAll()
        {
            var list2 = await ExecuteCommandForResult("list2");
            return list2.ParseLines().Select(item => new LdModel() { Index = item.Split(',')[0], Name = item.Split(',')[1] }).ToList();
        }


        [MethodCategory("LdPlayer", "Ld-Device",  3, 2500)]
        public static async Task Open(object item)
        {
            var model = CastModel(item);
            model.Status = "launch";
            await ExecuteCommand($"launch --index {model.Index}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device", 3)]
        public static async Task SortWnd()
        {
            await ExecuteCommand("sortWnd");
        }

        [MethodCategory("LdPlayer", "Ld-Device",  4)]
        public static async Task Close(object item)
        {
            var model = CastModel(item);
            model.Status = "quit";
            await ExecuteCommand($"quit --index {model.Index}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device", 5)]
        public static async Task CloseAll()
        {
            await ExecuteCommand("quitall");
        }

        [MethodCategory("LdPlayer", "Ld-Device",  6)]
        public static async Task CopyDevice(object item,string copyDevicename)//
        {
            var model = CastModel(item);
            model.Status = $"copy {model.TextInput}";
            await ExecuteCommand($"copy --name {copyDevicename} --from {model.Index}");
            await ModifyRandom(item);
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  7)]
        public static async Task ModifyRandom(object item)
        {
            var model = CastModel(item);
            model.Status = "modify Random";
            await ExecuteCommand($"modify  --index {model.Index} --imei auto --androidid auto --mac auto");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  8)]
        public static async Task RenameDevice(object item,string renameDeviceName)//
        {
            var model = CastModel(item);
            model.Status = $"rename {model.Index}";
            await ExecuteCommand($"rename --index {model.Index} --title {renameDeviceName}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  9)]
        public static async Task AddDevice(object item,string addDeviceName)//
        {
            var model = CastModel(item);
            model.Status = $"add {model.TextInput}";
            await ExecuteCommand($"add --name {addDeviceName}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  10)]
        public static async Task RemoveDevice(object item)//
        {
            var model = CastModel(item);
            model.Status = $"remove {model.Index}";
            await ExecuteCommand($"remove --index {model.Index}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  11)]
        public static async Task InstallFromFile(object item, string apkFilePath)
        {
            var model = CastModel(item);
            model.Status = "install apk";
            await ExecuteCommand($"installapp --index {model.Index} --filename \"{apkFilePath}\"");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  12)]
        public static async Task InstallFromPackage(object item, string packageName)
        {
            var model = CastModel(item);
            model.Status = "install package";
            await ExecuteCommand($"installapp --index {model.Index} --packagename {packageName}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  13)]
        public static async Task UninstallApp(object item, string packageName)
        {
            var model = CastModel(item);
            model.Status = "uninstallapp package";
            await ExecuteCommand($"uninstallapp --index {model.Index} --packagename {packageName}");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  21)]
        public static async Task BackupDevice(object item, string backupFilePath)
        {
            var model = CastModel(item);
            model.Status = $"backup {backupFilePath}";
            await ExecuteCommand($"backup --index {model.Index} --file \"{backupFilePath}\"");
            model.Status = "." + model.Status;
        }

        [MethodCategory("LdPlayer", "Ld-Device",  22)]
        public static async Task RestoreDevice(object item, string restoreFilePath)
        {
            var model = CastModel(item);
            model.Status = $"restore {restoreFilePath}";
            await ExecuteCommand($"restore --index {model.Index} --file \"{restoreFilePath}\"");
            model.Status = "." + model.Status;
        }


        

        [MethodCategory("LdPlayer", "Ld-Device",  20)]
        public static async Task<bool> IsRunning(object item)
        {
            var model = CastModel(item);
            model.Status = "Check running";
            var result = await ExecuteCommandForResult($"isrunning --index {model.Index}");
            if (result.Trim().Equals("running", StringComparison.OrdinalIgnoreCase))
            {
                model.Status = ".Device running";
                return true;
            }
            model.Status = ".Device Offine";
            return false;
        }
        #endregion

        #region CMD

        public static async Task ExecuteCommand(string command)
        {
            try
            {
                using (var cmdProcess = new Process())
                {
                    cmdProcess.StartInfo.FileName = "powershell.exe";
                    cmdProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(PathLD);
                    cmdProcess.StartInfo.Verb = "runas";
                    cmdProcess.StartInfo.Arguments = "ldconsole "+command;
                    cmdProcess.StartInfo.RedirectStandardOutput = true;
                    cmdProcess.StartInfo.UseShellExecute = false;
                    cmdProcess.StartInfo.CreateNoWindow = true;

                    cmdProcess.Start();

                    string output = await cmdProcess.StandardOutput.ReadToEndAsync(); // Đọc kết quả bất đồng bộ
                    await cmdProcess.WaitForExitAsync(); // Chờ tiến trình hoàn tất một cách bất đồng bộ
                }
            }
            catch (Exception)
            {
            }
        }
        public static async Task<string> ExecuteCommandForResult(string command)
        {
            try
            {
                using (var cmdProcess = new Process())
                {
                    cmdProcess.StartInfo.FileName = "powershell.exe";
                    cmdProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(PathLD);
                    cmdProcess.StartInfo.Verb = "runas";
                    cmdProcess.StartInfo.Arguments = "ldconsole " + command;
                    cmdProcess.StartInfo.RedirectStandardOutput = true;
                    cmdProcess.StartInfo.UseShellExecute = false;
                    cmdProcess.StartInfo.CreateNoWindow = true;

                    cmdProcess.Start();

                    string output = await cmdProcess.StandardOutput.ReadToEndAsync(); // Đọc kết quả bất đồng bộ
                    await cmdProcess.WaitForExitAsync(); // Chờ tiến trình hoàn tất một cách bất đồng bộ
                    return output ?? string.Empty;
                }
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }


        public static async Task ExecuteCMD(string command)
        {
            try
            {
                using (var cmdProcess = new Process())
                {
                    cmdProcess.StartInfo.FileName = "cmd.exe";
                    cmdProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(PathLD);
                    cmdProcess.StartInfo.Verb = "runas";
                    cmdProcess.StartInfo.Arguments = command;
                    cmdProcess.StartInfo.RedirectStandardOutput = true;
                    cmdProcess.StartInfo.UseShellExecute = false;
                    cmdProcess.StartInfo.CreateNoWindow = true;

                    cmdProcess.Start();

                    string output = await cmdProcess.StandardOutput.ReadToEndAsync(); // Đọc kết quả bất đồng bộ
                    await cmdProcess.WaitForExitAsync(); // Chờ tiến trình hoàn tất một cách bất đồng bộ
                }
            }
            catch (Exception)
            {
            }
        }
        public static async Task<string> ExecuteCMD_ForResult(string command)
        {
            try
            {
                using (var cmdProcess = new Process())
                {
                    cmdProcess.StartInfo.FileName = "cmd.exe";
                    cmdProcess.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(PathLD);
                    cmdProcess.StartInfo.Verb = "runas";
                    cmdProcess.StartInfo.Arguments = command;
                    cmdProcess.StartInfo.RedirectStandardOutput = true;
                    cmdProcess.StartInfo.UseShellExecute = false;
                    cmdProcess.StartInfo.CreateNoWindow = true;

                    cmdProcess.Start();

                    string output = await cmdProcess.StandardOutput.ReadToEndAsync(); // Đọc kết quả bất đồng bộ
                    await cmdProcess.WaitForExitAsync(); // Chờ tiến trình hoàn tất một cách bất đồng bộ
                    return output ?? string.Empty;
                }
            }
            catch (Exception)
            {
            }
            return string.Empty;
        }

        #endregion
        
    }
}
