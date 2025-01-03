# DzHelperV2.dll
Dll dự kiến có các helper liên quan đến:
- Adb
- Ldplayer
- Selnium

Có thể sử dụng để tương tác với các ứng dụng android, giả lập android, web browser.

## Các Dll được tích hợp:
- CommunityToolkit.Mvvm


## Ngôn ngữ
- Dll: wpf class library
- Application: wpf application >>c#,wpf

## Installation
https://www.nuget.org/packages/DZ.WPF.DZHelperV2/

## Đặc điểm dll.
- Tạo các module function trong class dll.
- Tạo commands từ trong class dll.
- Binding commands lên view. để khỏi tạo lại nhiều xaml.

# Ldplayer
``` ## Functions in class Helper
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

---

``` ## Get Command in class helper
public static List<CommandInfo> CommandsMain(int index)
{
    List<CommandInfo> commands = new List<CommandInfo>();

    commands.Add(new CommandInfo()
    {
        Name = "Load Devices",
        Action = async item => Test(item)
    });
    foreach (var command in commands)
    {
        command.Group = "LdPlayer-Main";
    }
    return commands;
}

# Test


