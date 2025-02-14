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

## Cách sử dụng Ldplayer
### Class Helper >> list module
```Functions in class Helper
public static async Task Test(object item)
{
    
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
```
- object item: **item là model**. nhưng code này là trong dll.
- còn model để trong project application thì vẫn binding Property Status để update lên views được.

### Class Helper >> return commands
```Functions in class Helper
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
```
trong viewmodel hãy binging list<CommandInfo> lên ItemControl lên views

# Demo 1.0.1
![image](https://github.com/user-attachments/assets/8e3c5a5a-1277-4ce6-9e8e-096b6c0391c5)

khi click vào button **Load Devices** thì function Test sẽ hoạt động



