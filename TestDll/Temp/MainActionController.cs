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
        [MethodCategory( "Main", 1,  true)]
        public static async Task StopAction(LdDevice device)
        {
            device.IsStop = true;
        }

        [MethodCategory("Main", 1,  true)]
        public static async Task PauseAction(LdDevice device)
        {
            device.IsStop = true;
        }
    }
}
