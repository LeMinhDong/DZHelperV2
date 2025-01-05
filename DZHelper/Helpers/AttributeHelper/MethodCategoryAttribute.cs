using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZHelper.Helpers.AttributeHelper
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodCategoryAttribute : Attribute
    {
        //thường sẽ là ldplayer
        public string Category { get; }


        //Phân theo nhóm trên xaml
        public string Group { get; }


        //sắp xếp trên xaml
        public int Index { get; }

        public int ThreadDelay { get; } = 100;
        public bool IsLoadFun { get; } = true;

        public MethodCategoryAttribute(string category,string group, int index, bool isLoadFun = true)
        {
            Category = category;
            Group = group;
            Index = index;
            IsLoadFun = isLoadFun;
        }
        public MethodCategoryAttribute(string category, string group, int index, int threadDelay)
        {
            Category = category;
            Group = group;
            Index = index;
            ThreadDelay = threadDelay;
            
        }
    }
}
