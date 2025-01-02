using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DZHelper.Extensions
{
    public static class ObjectItemExteion
    {
        public static void ChangeProperty(this object item, string value, string property = "Status")
        {
            try
            {
                var statusProperty = item.GetType().GetProperty(property, BindingFlags.Instance | BindingFlags.Public);
                if (statusProperty != null && statusProperty.CanWrite)
                    statusProperty.SetValue(item, value);
            }
            catch (Exception)
            {
            }
        }
    }
}
