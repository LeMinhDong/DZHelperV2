using DZHelper.Commands;
using DZHelper.Extensions;
using DZHelper.HelperCsharf;
using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
        [MethodCategory(true,"Ld-Auto", 1, true,200)]
        public static async Task ATest(object item,string command)
        {
            var model = CastModel(item);
            model.Status = "ATest";

        }

        [MethodCategory(true, "Ld-Auto", 4, true, 200)]
        public static async Task Tap(object item,string tapValue)
        {
            var model = CastModel(item);
            model.Status = "quit";
            //await ExecuteCommand($"quit --index {model.Index}");
        }

        [MethodCategory(true, "Ld-Auto",  20, true, 200)]
        public static async Task Pull(object item, string remoteFilePath, string localFilePath)
        {
            var model = CastModel(item);
            model.Status = "Pull";
            await ExecuteCommand($@"pull --index {model.Index} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }

        [MethodCategory(true, "Ld-Auto",  20, true, 200)]
        public static async Task Push(object item, string remoteFilePath, string localFilePath)
        {
            var model = CastModel(item);
            model.Status = "Push";
            await ExecuteCommand($@"push --index {model.Index} --remote ""{remoteFilePath}"" --local ""{localFilePath}""");
        }

        [MethodCategory(true, "Ld-Auto",  20, true, 200)]
        public static async Task InputText(object item)
        {
            var model = CastModel(item);
            model.Status = "input text";
            if (string.IsNullOrWhiteSpace(model.TextInput1))
            {
                model.Status = "Input1 value null";
                return;
            }
            await ExecuteCommand($"action --index {model.Index} --key call.input --value '{model.TextInput1}'");
        }


        [MethodCategory(true, "Ld-Auto", 20, true, 200)]
        public static async Task DumpXml(object item)
        {
            var model = CastModel(item);
            model.Status = "Dump Xml";
            await ExecuteCommand($"adb --index {model.Index} --command 'shell uiautomator dump'");
            model.DataResult = await ExecuteCommandForResult($"adb --index {model.Index} --command 'shell cat /sdcard/window_dump.xml'");
            await ExecuteCommand($"adb --index {model.Index} --command 'shell rm /sdcard/window_dump.xml'");
        }

        [MethodCategory(true, "Ld-Auto", 20, true, 200)]
        public static async Task Activity(object item)
        {
            var model = CastModel(item);
            model.Status = "Dump Activity";
            model.DataResult = await ExecuteCommandForResult($"adb --index {model.Index} --command 'shell dumpsys window windows | grep mCurrentFocus'");
        }

        #endregion

        #region Ld-Adb
        [MethodCategory(true, "Ld-Adb", 4, false, 200)]
        public static async Task RestartAdb()
        {
            await ExecuteCMD($"adb kill-server");
            await ExecuteCMD($"adb start-server");
        }

        [MethodCategory(true, "Ld-Adb", 5, false, 200)]
        public static async Task ReconnectOffline()
        {
            await ExecuteCMD($"adb reconnect offline");
        }

        [MethodCategory(true, "Ld-Adb", 5, false, 200)]
        public static async Task AdbDevices()
        {
            await ExecuteCMD($"adb devices");
        }




        #endregion

        #region Ld-Device
        

        [MethodCategory(true, "Main", 1, false, 200)]
        public static async Task<List<LdModel>> LoadRunnings()
        {
            var runningstring = await ExecuteCommandForResult("runninglist");
            var list2string = await ExecuteCommandForResult("list2");

            var list = list2string.ParseLines().Where(itemAll => runningstring.ParseLines().Any(running => itemAll.Contains(running.Trim()))).ToList();
            return list.Select(item => new LdModel() { Index = item.Split(',')[0], Name = item.Split(',')[1] }).ToList();
        }

        [MethodCategory(true, "Main", 2, false, 200)]
        public static async Task<List<LdModel>> LoadAll()
        {
            var list2 = await ExecuteCommandForResult("list2");
            return list2.ParseLines().Select(item => new LdModel() { Index = item.Split(',')[0], Name = item.Split(',')[1] }).ToList();
        }


        [MethodCategory(true, "Ld-Device",  3, true, 2000)]
        public static async Task Open(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "launch";
            await ExecuteCommand($"launch --index {model.Index}");
        }

        [MethodCategory(true, "Ld-Device", 3, false, 200)]
        public static async Task SortWnd()
        {
            await ExecuteCommand("sortWnd");
        }

        [MethodCategory(true, "Ld-Device",  4, true, 200)]
        public static async Task Close(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "quit";
            await ExecuteCommand($"quit --index {model.Index}");
        }

        [MethodCategory(true, "Ld-Device", 5, false, 200)]
        public static async Task CloseAll()
        {
            await ExecuteCommand("quitall");
        }

        [MethodCategory(true, "Ld-Device",  6, true, 4000)]
        public static async Task CopyDevice(object item)//
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = $"copy {model.Step}";
            await ExecuteCommand($"copy --name {model.Step} --from {model.Index}");
            await ModifyRandom(item);
        }

        [MethodCategory(true, "Ld-Device",  7, true, 200)]
        public static async Task ModifyRandom(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "modify Random";
            await ExecuteCommand($"modify  --index {model.Index} --imei auto --androidid auto --mac auto");
        }

        [MethodCategory(true, "Ld-Device",  8, true, 200)]
        public static async Task RenameDevice(object item)//
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = $"rename {model.Index}";
            if (string.IsNullOrWhiteSpace(model.TextInput1))
            {
                model.Status = $"rename {model.TextInput1}";
                return;
            }
            await ExecuteCommand($"rename --index {model.Index} --title {model.TextInput1}");
        }

        [MethodCategory(true, "Ld-Device",  9, false, 200)]
        public static async Task AddDevice(int NumThreads)//
        {
            for (int i = 1; i <= NumThreads; i++)
            {
                await ExecuteCommand($"add --name AddNew{i}");
            }
        }

        [MethodCategory(true, "Ld-Device",  10, true, 200)]
        public static async Task RemoveDevice(object item)//
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = $"remove {model.Index}";
            await ExecuteCommand($"remove --index {model.Index}");
        }

        [MethodCategory(true, "Ld-Device",  11, true, 200)]
        public static async Task InstallFromFile(object item, string ApkFolder)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "install apk";
            ApkFolder = ApkFolder + "/" + model.TextInput1 + ".apk";
            if (!File.Exists(ApkFolder))
            {
                model.Status = $"error: Not exits {ApkFolder}";
                return;
            }
            await ExecuteCommand($"installapp --index {model.Index} --filename \"{ApkFolder}\"");
        }

        [MethodCategory(true, "Ld-Device",  12, true, 200)]
        public static async Task InstallFromPackage(object item, string packageName)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = "install package";
            await ExecuteCommand($"installapp --index {model.Index} --packagename {packageName}");
        }

        [MethodCategory(true, "Ld-Device",  13, true, 200)]
        public static async Task UninstallApp(object item, string ApkFolder)
        {
            var model = CastModel(item);
            if (model == null)
                return;


            model.Status = $"uninstallapp {model.TextInput1}";
            await ExecuteCommand($"uninstallapp --index {model.Index} --packagename {model.TextInput1}");
        }

        [MethodCategory(true, "Ld-Device",  21, true, 200)]
        public static async Task BackupDevice(object item, string backupFilePath)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = $"backup {backupFilePath}";
            if (!Directory.Exists(backupFilePath))
            {
                model.Status = $"error: Not exits {backupFilePath}";
                return;
            }

            backupFilePath = backupFilePath + "/" + model.Name + ".ldbk";
            
            await ExecuteCommand($"backup --index {model.Index} --file \"{backupFilePath}\"");
        }

        [MethodCategory(true, "Ld-Device",  22, true, 200)]
        public static async Task RestoreDevice(object item, string backupFilePath)
        {
            var model = CastModel(item);
            if (model == null)
                return;
            model.Status = $"restore {backupFilePath}";
            backupFilePath = backupFilePath + "/" + model.Name + ".ldbk";
            if (!File.Exists(backupFilePath))
            {
                model.Status = $"error: Not exits {backupFilePath}";
                return;
            }

            await ExecuteCommand($"restore --index {model.Index} --file \"{backupFilePath}\"");
        }

        [MethodCategory(true, "Ld-Device",  20, true, 200)]
        public static async Task<bool> IsRunning(object item)
        {
            var model = CastModel(item);
            if (model == null)
                return false;
            model.Status = "Check running";
            var result = await ExecuteCommandForResult($"isrunning --index {model.Index}");
            if (result.Trim().Equals("running", StringComparison.OrdinalIgnoreCase))
            {
                model.Status = ".Device running";
                return true;
            }
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
