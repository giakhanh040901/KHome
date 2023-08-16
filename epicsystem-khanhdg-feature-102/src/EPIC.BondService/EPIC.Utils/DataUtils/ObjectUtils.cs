using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHB.Utils.DataUtils
{
    public static class ObjectUtils
    {
        public static bool IsInteger(this decimal number)
        {
            return number == Math.Truncate(number);
        }
    }
}
