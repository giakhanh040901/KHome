using EPIC.Utils.ConstantVariables.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    /// <summary>
    /// Loại dữ liệu
    /// </summary>
    public static class XvtTokenDataTypes
    {
        public const string EP_INVESTOR = "EP_INVESTOR";
        public const string EP_CORE_SALE = "EP_CORE_SALE";

        public static string[] DATA_TYPES = new string[]
        {
            EP_INVESTOR, EP_CORE_SALE
        };
    }
}
