using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils
{
    /// <summary>
    /// Trạng thái sale
    /// </summary>
    public static class SaleStatus
    {
        public const string ACTIVE = "A";
        public const string DEACTIVE = "D";
        public const string WAIT_TO_SIGN = "W";
    }

    public static class IsSign
    {
        public const int YES = 1;
        public const int No = 0;
    }
}
