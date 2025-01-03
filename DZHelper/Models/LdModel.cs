using CommunityToolkit.Mvvm.ComponentModel;

namespace DZHelper.Models
{
    public partial class LdModel:BaseModel
    {
        public LdModel()
        {
                
        }
        public LdModel(string[] args)
        {
            try
            {

            }
            catch (Exception)
            {
            }
        }


        [ObservableProperty]
        private string index;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string title;
        
        [ObservableProperty]
        private bool isOpen;

        [ObservableProperty]
        private string dataInput;

        public IntPtr TopHandle { get; set; }
        public IntPtr BindHandle { get; set; }
        public int AndroidState { get; set; }
        public int DnplayerPID { get; set; }
        public int VboxPID { get; set; }

    }
}
