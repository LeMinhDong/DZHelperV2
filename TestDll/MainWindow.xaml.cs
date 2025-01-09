using DZHelper.Commands;
using DZHelper.Settings;
using DZHelper.Triggers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestDll.ViewModels;

namespace TestDll
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tb_Status.TextBlockStyle_Status("XSettingData.Status");

            //Test3
            //test4
            //test5
            //List<string> list = new List<string>();
            //list.Add("test1");
            //list.Add("test2");
            //list.Add("test3");
            //list = list.Take(0).ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var MainVM = this.DataContext as MainViewModel;
            SettingUIHandler.OpenSetting(MainVM.XSettingUI);

            //cập nhập lại binding Expanded của GroupedCommands
            foreach (var item in MainVM.XSettingData.GroupedCommands)
            {
                var dic = MainVM.XSettingUI.Dic_Expanded.Keys.Any(Key => Key == item.GroupName);
                item.IsExpanded = dic ? MainVM.XSettingUI.Dic_Expanded[item.GroupName] : true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var MainVM = this.DataContext as MainViewModel;
            MainVM.XSettingUI.Dic_Expanded = MainVM.XSettingData.GroupedCommands.ToDictionary(item => item.GroupName, item => item.IsExpanded);
            SettingUIHandler.SaveSetting(MainVM.XSettingUI);
        }
    }
}