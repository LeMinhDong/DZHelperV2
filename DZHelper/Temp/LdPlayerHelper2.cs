using DZHelper.HelperCsharf;
using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZHelper.Temp
{
    public static class LdPlayerHelper2
    {

        private static string PathLD = "C:\\LDPlayer\\LDPlayer64\\ldconsole.exe";

        public static void SetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;

            PathLD = path;
        }

        [MethodCategory("Ld-Auto", 1, true)]
        public static async Task Tap(object item, string tapValue)
        {
            // Giả lập thực thi logic TAP
            await Task.Delay(100);
            // Cập nhật status, ...
        }

        [MethodCategory("Ld-Auto", 2,  true)]
        public static async Task<string> InputText(object device, string text)
        {
            await Task.Delay(50);
            return $"Đã input: {text}";
        }

        [MethodCategory("Ld-Auto", 3,  false)]
        public static async Task<bool> CheckIsRunning(object device)
        {
            await Task.Delay(80);
            return true;
        }

        [MethodCategory("Ld-Auto", 4,  true)]
        public static async Task<List<LdModel>> GetAllModes(object device)
        {
            await Task.Delay(60);
            return new List<LdModel>
            {
                new LdModel { Name="Mode1",Index = "0" },
                new LdModel { Name="Mode2",Index = "1" }
            };
        }

        [MethodCategory("Main", 1, false)]
        public static async Task<List<LdModel>> LoadRunnings()
        {
            var runningstring = await ExecuteCommandForResult("runninglist");
            var list2string = await ExecuteCommandForResult("list2");

            var list = list2string.ParseLines().Where(itemAll => runningstring.ParseLines().Any(running => itemAll.Contains(running.Trim()))).ToList();
            var devices = list.Select(item => new LdModel() { Index = item.Split(',')[0], Name = item.Split(',')[1] }).ToList();
            return devices;
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
    }
}
