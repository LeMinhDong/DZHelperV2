using CommunityToolkit.Mvvm.ComponentModel;

namespace DZHelper.Models
{
    public partial class BaseModel:ObservableObject
    {
        [ObservableProperty]
        private bool isStop;

        [ObservableProperty]
        private bool isStopNow;

        [ObservableProperty]
        private bool select;

        [ObservableProperty]
        private string status;

        [ObservableProperty]
        private string step;
    }
}
