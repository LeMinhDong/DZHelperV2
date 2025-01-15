using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DZHelper.Commands;
using DZHelper.Extensions;
using DZHelper.HelperCsharf;
using DZHelper.Helpers;
using DZHelper.Helpers.AttributeHelper;
using DZHelper.Models;
using DZHelper.ProjectInitialize;
using DZHelper.Temp;
using DZHelper.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media.Media3D;
using TestDll.Models;
using TestDll.Temp;
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
            xSettingData.Devices.Add(new IMouseDevice() { Id = "id"});
        }

        private async Task SetPathLd()
        {
            LdplayerHelper.SetPath("C:\\LDPlayer\\LDPlayer64\\ldconsole.exe");
        }


        #region Load Commands
        private async Task LoadCommandsFuns()
        {
            //load all commands mong muốn.
            List<CommandInfo> commandInfos = new List<CommandInfo>();
            
            var listHelper = ReflectionScanner.ScanLdCommands(typeof(AutoImouseHelper));
            listHelper = listHelper.Where(commandInfo => commandInfo.MethodInfo.GetCustomAttribute<MethodCategoryAttribute>().IsLoadFun).ToList();
            commandInfos.AddRange(listHelper);
            commandInfos.AddRange(await LoadCommandMain());
            
            foreach (var commandInfo in commandInfos)
            {
                var parameters = commandInfo.MethodInfo.GetParameters();
                var returnType = commandInfo.MethodInfo.ReturnType;
                commandInfo.Command = new AsyncRelayCommand(async () =>
                {
                    await MainAction2(commandInfo, commandInfo.ForeachDevices);
                });
            }

            // phân ra commandsMain và commandsFunction
            XSettingData.CommandsInfo.AddItemsNotExits(commandInfos);
            var commandsMain = XSettingData.CommandsInfo.Where(item => item.Group == "Main").ToList();
            commandsMain.ForEach(item => item.Group = "");
            XSettingData.CommandsMain.AddItemsNotExits(commandsMain);
            XSettingData.CommandsInfo.RemoveItemsExits(commandsMain);
            commandsMain.Clear();


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
            List<MethodInfo> methodinfors = new List<MethodInfo>();
            methodinfors.Add(ReflectionScanner.GetMethodByName(typeof(MainActionController), "PauseAction"));
            methodinfors.Add(ReflectionScanner.GetMethodByName(typeof(MainActionController), "StopAction"));
            foreach (var methodinfor in methodinfors)
            {
                if (methodinfor == null)
                    continue;
                commands.Add(new CommandInfo()
                {
                    Group = "Main", // Tên Group
                    Name = methodinfor.Name.ConvertToSpacedStringManual(), // Tên phương thức
                    MethodInfo = methodinfor,
                    ForeachDevices = true
                });
            }
            return commands;
        }
        #endregion

        #region CommandInfo
        private async Task MainAction2(CommandInfo commandInfo, bool IsForeach)
        {

            if (XSettingData.Devices.Count == 0) { XSettingData.Devices.Add(new IMouseDevice() { Id = "id 1" }); }
            if (IsForeach)
            {
                XSettingData.Status = commandInfo.Name;
                // Chạy cho mỗi device
                foreach (var device in XSettingData.Devices.Where(item => item.Select))
                {
                    StartTask(async () =>
                    {
                        try
                        {
                            device.Status = commandInfo.Name;
                            var arg = await GetParameter(device,commandInfo.MethodInfo);
                            var result = await LdplayerController.InvokeLdCommandAsync(commandInfo, arg);
                            UpdateActionResultItem(device, result);
                            device.Status = "."+ device.Status;
                        }
                        catch (Exception eee)
                        {
                            device.Status = eee.Message;
                        }
                    });
                    await Task.Delay(commandInfo.MethodInfo.GetCustomAttribute<MethodCategoryAttribute>().ThreadDelay);
                }
                XSettingData.Status = "." + commandInfo.Name;
            }
            else
            {
                // Chạy 1 lần
                // Ở đây param tùy bạn: device hay SettingData.Status, ...
                XSettingData.Status = commandInfo.Name;
                try
                {
                    var arg = await GetParameter2(commandInfo.MethodInfo);
                    var result = await LdplayerController.InvokeLdCommandAsync(commandInfo, arg);
                    UpdateActionResultStatus(result);
                    XSettingData.Status = "." + commandInfo.Name;
                }
                catch (Exception eee)
                {
                    XSettingData.Status = eee.Message;
                }
                
            }
            
        }
        private async Task UpdateActionResultItem(IMouseDevice model, object result)
        {
            if (result == null)
                return;

            if (result is Task<string> reString)
                model.DataResult = await reString;

            if (result is Task<bool> reBool)
                model.Status = (await reBool).ToString();

            
        }
        private async Task UpdateActionResultStatus(object result)
        {
            if (result == null)
                return;

            if (result is Task<string> text)
                XSettingData.Status = await text;

            if (result is Task<List<LdModel>> devices)
            {
                List<LdDevice> list = new List<LdDevice>();
                foreach (var re in await devices)
                {
                    list.Add(new LdDevice() { Name = re.Name, Index = re.Index });
                }

                //XSettingData.Devices.AddItemsNotExits(list);
                //XSettingData.Devices.RemoveItemsNotExits(list);
                XSettingData.Datagrid.InitLoadingRow();
            }
        }

        #endregion

        #region Parameter of Command
        

        private async Task<object[]> GetParameter(IMouseDevice device, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();

            // Tự động điều chỉnh tham số dựa trên kiểu dữ liệu
            var args = new object[parameters.Length == 0 ? 1 : parameters.Length];
            args[0] = device;

            for (int i = 1; i < parameters.Length; i++)
            {
                string paramName = parameters[i].Name;
                // Tìm thuộc tính trong SettingUI có tên tương ứng
                var propertyInfo = XSettingUI.GetType().GetProperty(paramName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                    args[i] = propertyInfo.GetValue(XSettingUI);
                
                else if (i == 2)
                    args[i] = device.TextInput1;
                else if(i == 3)
                    args[i] = device.TextInput2;
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                    throw new Exception($"error: parameter {i + 1} null");
                
            }
            return args;
        }

        private async Task<object[]> GetParameter2(MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                string paramName = parameters[i].Name;
                var propertyInfo = XSettingUI.GetType().GetProperty(paramName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                    args[i] = propertyInfo.GetValue(XSettingUI);
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                    throw new Exception($"error: parameter {i + 1} null");
                
            }
            return args;
        }
        // Hàm tạo tham số cho mỗi device

        #endregion

    }
}
