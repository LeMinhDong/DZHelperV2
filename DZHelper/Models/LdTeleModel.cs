
using CommunityToolkit.Mvvm.ComponentModel;

namespace DZHelper.Models
{
    public partial class LdTeleModel:LdModel
    {
        [ObservableProperty]
        private int tonkho;

        [ObservableProperty]
        private int loopXa;

        [ObservableProperty]
        private string goiXa;

        [ObservableProperty]
        private int currency;

    }
}
