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
        private string textInput;

        public override bool Equals(object obj)
        {
            // Kiểm tra null và kiểu của obj
            if (obj == null || GetType() != obj.GetType())
                return false;

            // So sánh chỉ dựa trên Name
            var other = (LdModel)obj;
            return Name == other.Name;
        }

        // Ghi đè GetHashCode
        public override int GetHashCode()
        {
            // Chỉ sử dụng Name để tạo hash code
            return Name?.GetHashCode() ?? 0;
        }

    }
}
