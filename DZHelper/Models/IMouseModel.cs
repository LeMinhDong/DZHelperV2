using CommunityToolkit.Mvvm.ComponentModel;

namespace DZHelper.Models
{
    public partial class IMouseModel:BaseModel
    {
        [ObservableProperty]
        private string id;
    }
}
