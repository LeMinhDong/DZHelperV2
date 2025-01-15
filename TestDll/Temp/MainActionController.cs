using DZHelper.Helpers.AttributeHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDll.Models;

namespace TestDll.Temp
{
    public static class MainActionController
    {
        [MethodCategory(true, "Main", 1,  true,200)]
        public static async Task StopAction(LdDevice device)
        {
            device.IsStop = true;
        }

        [MethodCategory(true, "Main", 1,  true, 200)]
        public static async Task PauseAction(LdDevice device)
        {
            device.IsStop = true;
        }
    }
}
