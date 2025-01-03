using CommunityToolkit.Mvvm.ComponentModel;

namespace DZHelper.Models
{
    public partial class LdModel:BaseModel
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string index;


    }
}
