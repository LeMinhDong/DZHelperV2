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

        public int ThreadDelay { get; set; } = 100;
        public bool IsLoadFun { get; } = true;

        public MethodCategoryAttribute( string group, int index, bool foreachDevices)
        {
            Group = group;
            Index = index;
            ForeachDevices = foreachDevices;
        }
    }
}
