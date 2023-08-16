using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    /// <summary>
    /// Loại voucher
    /// </summary>
    public class LoyVoucherUseTypes
    {
        public const string TIEU_DUNG = "TD";
        public const string MUA_SAM = "MS";
        public const string AM_THUC = "AT";
        public const string DICH_VU = "DV";

        public static string VoucherUseTypeName(string voucherType)
        {
            return voucherType switch
            {
                TIEU_DUNG => "Tiêu dùng",
                MUA_SAM => "Mua sắm",
                AM_THUC => "Ẩm thực",
                DICH_VU => "Dịch vụ",
                _ => "",
            };
        }
    }
}
