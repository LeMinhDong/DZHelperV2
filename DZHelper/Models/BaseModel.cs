using CommunityToolkit.Mvvm.ComponentModel;
using DZHelper.ViewModels;

namespace DZHelper.Models
{
    public partial class BaseModel:BaseViewModel
    {
        [ObservableProperty]
        private bool isStop;

        [ObservableProperty]
        private bool isStopNow;

        [ObservableProperty]
        private bool select =true;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private string step;

        [ObservableProperty]
        private string dataResult;

        [ObservableProperty]
        private string textInput1;

        [ObservableProperty]
        private string textInput2;

    }
}
