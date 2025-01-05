using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZHelper.ExceptionHelper
{
    public class StopException : Exception
    {
        public StopException(string messmage) : base(messmage)
        {

        }
    }

    public class LdException : Exception
    {
        public LdException(string messmage) : base(messmage)
        {

        }
    }
    public class AdbException : Exception
    {
        public AdbException(string messmage) : base(messmage)
        {

        }
    }
}
