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
        //Phân theo nhóm trên xaml
        public string Group { get; }
        public bool ForeachDevices { get; set; } = true;


        //sắp xếp trên xaml
        public int Index { get; }

        public int ThreadDelay { get; set; } = 500;
        public bool IsLoadFun { get; } = true;


        public MethodCategoryAttribute(bool isLoadFun, string group, int index, bool foreachDevices,int threadDelay)
        {
            IsLoadFun = isLoadFun;
            Group = group;
            Index = index;
            ForeachDevices = foreachDevices;
            ThreadDelay = threadDelay;
        }
    }
}
