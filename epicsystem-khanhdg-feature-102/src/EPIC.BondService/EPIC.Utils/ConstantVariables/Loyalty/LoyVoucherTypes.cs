using EPIC.Utils.ConstantVariables.Garner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    /// <summary>
    /// Loại hình thẻ
    /// </summary>
    public static class LoyVoucherTypes
    {
        /// <summary>
        /// Cứng
        /// </summary>
        public const string CUNG = "C";

        /// <summary>
        /// Điện tử
        /// </summary>
        public const string DIEN_TU = "DT";

        public static string VoucherTypeName(string voucherType)
        {
            return voucherType switch
            {
                CUNG => "Cứng",
                DIEN_TU => "Điện tử",
                _ => "",
            };
        }
    }
}
