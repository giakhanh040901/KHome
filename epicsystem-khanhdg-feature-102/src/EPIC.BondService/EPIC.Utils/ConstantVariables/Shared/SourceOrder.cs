using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    /// <summary>
    /// Nguồn đặt lệnh
    /// </summary>
    public static class SourceOrder
    {
        public const int ONLINE = 1;
        public const int OFFLINE = 2;
        public const int ALL = 3;
    }

    public static class SourceOrderText
    {
        public const string ONLINE = "Online";
        public const string OFFLINE = "Offline";
        public const string ALL = "Tất cả";
    }
}
