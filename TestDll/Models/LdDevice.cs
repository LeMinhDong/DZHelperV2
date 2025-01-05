using CommunityToolkit.Mvvm.ComponentModel;
using DZHelper.Models;

namespace TestDll.Models
{
    public partial class LdDevice:LdModel
    {
        [ObservableProperty]
        private int tonkho;

        [ObservableProperty]
        private string account;

        [ObservableProperty]
        private string goiXa;

        [ObservableProperty]
        private int currency;

        [ObservableProperty]
        private int loopXa;
       
    }
}
