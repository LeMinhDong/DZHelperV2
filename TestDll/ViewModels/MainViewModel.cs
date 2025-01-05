using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DZHelper.Commands;
using DZHelper.Extensions;
using DZHelper.HelperCsharf;
using DZHelper.Helpers;
using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using DZHelper.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media.Media3D;
using TestDll.Models;
using TestDll.xSetting;

namespace TestDll.ViewModels
{
    public partial class MainViewModel:BaseViewModel
    {
        


        [ObservableProperty]
        private SettingData xSettingData = new SettingData();

        [ObservableProperty]
        private SettingUI xSettingUI = new SettingUI();
        public MainViewModel()
        {
            SetPathLd();
            LoadCommandsFuns();
            
        }

        private async Task SetPathLd()
        {
            LdplayerHelper.SetPath("C:\\LDPlayer\\LDPlayer64\\ldconsole.exe");
        }
        private async Task LoadCommandsFuns()
        {
            LoadCommandMain();
            
            //lấy danh sách CommandsInfo từ ldplayerHelper
            XSettingData.CommandsInfo.AddItemsNotExits(CommandHelper.GetCommandsInfo_LdPlayer());

            // Tạo các commands từ danh sách CommandsInfo
            foreach (var commandInfo in XSettingData.CommandsInfo)
            {
                var parameters = commandInfo.MethodInfo.GetParameters();
                var returnType = commandInfo.MethodInfo.ReturnType;

                // Gán Command dựa trên số lượng tham số
                commandInfo.Command = parameters.Length == 0
                    ? new AsyncRelayCommand(() => ExecuteWithoutParameters(commandInfo.Action, commandInfo.MethodInfo))
                    : new AsyncRelayCommand(() => ExecuteWithParameters(commandInfo.ActionWithParameters, commandInfo.MethodInfo));
            }
            var commandsMain = XSettingData.CommandsInfo.Where(item => item.Group == "Main").ToList();
            XSettingData.CommandsMain.AddItemsNotExits(commandsMain);

            foreach (var item in XSettingData.CommandsMain)
                item.Group = "";

            XSettingData.CommandsInfo.RemoveItemsExits(commandsMain);


            // Tạo các nhóm lệnh từ danh sách CommandsInfo. Binding lên view theo Group
            XSettingData.GroupedCommands = new ObservableCollection<GroupedCommand>(
            XSettingData.CommandsInfo
                .GroupBy(cmd => cmd.Group) // Nhóm các lệnh theo thuộc tính Group
                .Select(g => new GroupedCommand
                {
                    GroupName = g.Key, // Tên nhóm
                    Commands = new ObservableCollection<CommandInfo>(g.ToList()) // Các lệnh trong nhóm
                })
            );
        }
        private async Task<List<CommandInfo>> LoadCommandMain()
        {
            List<CommandInfo> commands = new List<CommandInfo>();
            commands.Add(new CommandInfo()
            {
                Group = "Main", // Tên Group
                Name = "Pause", // Tên phương thức
                Command = new AsyncRelayCommand(() =>
                            MainActionButton(PauseAction, "Pause"))
            });
            commands.Add(new CommandInfo()
            {
                Group = "Main", // Tên Group
                Name = "Stop", // Tên phương thức
                Command = new AsyncRelayCommand(() =>
                            MainActionButton(StopAction, "Stop"))
            });

            XSettingData.CommandsMain.AddItemsNotExits(commands);
            return commands;
        }

        
        

        

        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private async Task<object[]> GetParameter(LdDevice device, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            // Tự động điều chỉnh tham số dựa trên kiểu dữ liệu
            var args = new object[parameters.Length];
            if (parameters.Length > 0)
                args[0] = device;

            for (int i = 1; i < parameters.Length; i++)
            {
                string paramName = parameters[i].Name;
                // Tìm thuộc tính trong SettingUI có tên tương ứng
                var propertyInfo = XSettingUI.GetType().GetProperty(paramName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                    args[i] = propertyInfo.GetValue(XSettingUI);
                else
                {
                    // Xử lý khi không tìm thấy thuộc tính tương ứng
                    // Bạn có thể gán giá trị mặc định, ném ngoại lệ, hoặc bỏ qua
                    args[i] = GetDefaultValue(parameters[i].ParameterType);
                }

                switch (parameters[i].Name)
                {
                    case "command":
                    case "tapValue":
                    case "remoteFilePath":
                    case "localFilePath":
                    case "apkFilePath":
                    case "backupFilePath":
                    case "restoreFilePath":
                        // Các case chưa gán giá trị, giữ nguyên
                        break;

                    case "inputText":
                        // Kiểm tra thêm inputText dạng gì
                        args[i] = device.TextInput;
                        break;

                    case "copyDevicename":
                    case "renameDeviceName":
                    case "addDeviceName":
                    case "packageName":
                        // Các case có chung logic gán giá trị
                        args[i] = device.Step;
                        break;
                }
            }
            return args;
        }

        private async Task MainActionButton(Func<LdDevice, Task> action,string method)
        {
            XSettingData.Status = method;
            foreach (var device in XSettingData.Devices.Where(item => item.Select)) // Giả sử Devices là danh sách các LdDevice
            {
                try
                {
                    device.StartTask(async() =>
                    {
                        await action(device);
                    });
                    // Thực thi hàm được truyền vào
                    
                }
                catch (Exception ex)
                {
                    // Xử lý lỗi
                    XSettingData.Status = $"Error in MainAction: {ex.Message}";
                }
            }
            XSettingData.Status = "." + XSettingData.Status;

        }

        private async Task StopAction(LdDevice device)
        {
            device.IsStop = true;
        }

        private async Task PauseAction(LdDevice device)
        {
            device.IsStop = true;
        }

        private async Task LoadDeviceAction(Func<Task<List<LdModel>>> action, MethodInfo methodInfo)
        {
            XSettingData.Status = methodInfo.Name.ConvertToSpacedStringManual();
            var devices = await action();
            if (devices != null && devices.Any())
            {
                var xDevices = devices.Select(item => new LdDevice
                {
                    Index = item.Index,
                    Name = item.Name
                });
                //SettingData.Devices.Clear();
                XSettingData.Devices.AddItemsNotExits(xDevices);
                XSettingData.Devices.RemoveItemsExits(xDevices);
            }
            XSettingData.Status = "." + XSettingData.Status;
        }

        private async Task ExecuteActionCommand(Delegate action, MethodInfo methodInfo,  object[] parameters = null)
        {
            try
            {

                object result = null;

                // Xử lý kết quả trả về
                if (parameters == null && action is Func<Task<object>> taskAction)
                {
                    result = await taskAction(); // Không có tham số
                }
                else if (parameters != null && action is Func<object[], Task<object>> taskWithParamsAction)
                {
                    result = await taskWithParamsAction(parameters); // Có tham số
                }
                else
                {
                    throw new InvalidOperationException($"Action for {methodInfo.Name} is not a valid delegate type.");
                }

                // Xử lý kết quả trả về
                if (methodInfo.ReturnType == typeof(Task<string>))
                {
                    ProcessStringResult(result as string);
                }
                else if (methodInfo.ReturnType == typeof(Task<bool>))
                {
                    ProcessBoolResult(result as bool?);
                }
                else if (methodInfo.ReturnType == typeof(Task<List<LdModel>>))
                {
                    ProcessListResult(result as List<LdModel>);
                }

                // Kết thúc thực thi
            }
            catch (Exception ex)
            {
                XSettingData.Status = $"Error in {methodInfo.Name}: {ex.Message}";
            }
        }

        private async Task ExecuteWithoutParameters(Func<Task<object>> action, MethodInfo methodInfo)
        {
            XSettingData.Status = methodInfo.Name;
            await ExecuteActionCommand(action, methodInfo);

            XSettingData.Status = "." + methodInfo.Name;
        }
        private async Task ExecuteWithParameters(Func<object[], Task<object>> actionWithParameters, MethodInfo methodInfo)
        {
            XSettingData.Status = methodInfo.Name;
            foreach (var device in XSettingData.Devices)
            {
                try
                {
                    var parameters = GenerateParametersForDevice(device, methodInfo);
                    await ExecuteActionCommand(() => actionWithParameters(parameters), methodInfo);
                }
                catch (InvalidOperationException ee)
                {
                    device.Status ="error:"+ ee.Message;
                }
                catch (Exception)
                {

                }
                
            }
            XSettingData.Status = "." + methodInfo.Name;
        }


        // Hàm xử lý giá trị trả về dựa trên kiểu trả về
        private void ProcessReturnValue(object result, Type returnType)
        {
            if (returnType == typeof(Task<List<LdModel>>))
            {
                var listResult = result as List<LdModel>;
                ProcessListResult(listResult);
            }
            else if (returnType == typeof(Task<string>))
            {
                var stringResult = result as string;
                ProcessStringResult(stringResult);
            }
            else if (returnType == typeof(Task<bool>))
            {
                var boolResult = result as bool?;
                ProcessBoolResult(boolResult);
            }
            // Có thể thêm các kiểu trả về khác nếu cần
        }

        // Hàm tạo tham số cho mỗi device
        private object[] GenerateParametersForDevice(LdDevice device, MethodInfo methodInfo)
        {
            var methodParams = methodInfo.GetParameters();
            var args = new object[methodParams.Length];

            for (int i = 0; i < methodParams.Length; i++)
            {
                switch (methodParams[i].Name)
                {
                    case "device":
                        args[i] = device;
                        break;
                    case "command":
                        args[i] = "SampleCommand"; // Giá trị mẫu
                        break;
                    default:
                        args[i] = GetDefaultValue(methodParams[i].ParameterType);
                        break;
                }
            }

            return args;
        }

        // Trả về giá trị mặc định cho tham số
       

        // Xử lý kết quả trả về là danh sách
        private void ProcessListResult(List<LdModel> listResult)
        {
            if (listResult == null || !listResult.Any())
                return;
            var list = listResult.Select(item => new LdDevice
            {
                Index = item.Index,
                Name = item.Name
            });
            //XSettingData.Devices.Clear();
            XSettingData.Devices.RemoveItemsNotExits(list);
            XSettingData.Devices.AddItemsNotExits(list);
        }

        // Xử lý kết quả trả về là chuỗi
        private void ProcessStringResult(string stringResult)
        {
            if (!string.IsNullOrWhiteSpace(stringResult))
            {
                Console.WriteLine($"String result: {stringResult}");
            }
        }

        // Xử lý kết quả trả về là boolean
        private void ProcessBoolResult(bool? boolResult)
        {
            if (boolResult.HasValue)
            {
                Console.WriteLine($"Boolean result: {boolResult.Value}");
            }
        }
    }
}
