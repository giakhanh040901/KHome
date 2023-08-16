using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    /// <summary>
    /// Hình thức thanh toán mua nhà: 1 Thanh toán thường, 2 Thanh toán Sớm 3: Trả góp ngân hàng
    /// </summary>
    public class RstOrderPaymentypes
    {
        public const int THANH_TOAN_THUONG = 1;
        public const int THANH_TOAN_SOM = 2;
        public const int TRA_GOP_NGAN_HANG = 3;

        public static string PaymentTypeName(int? paymentType)
        {
            return paymentType switch
            {
                THANH_TOAN_THUONG => "Thanh toán thường",
                THANH_TOAN_SOM => "Thanh toán sớm",
                TRA_GOP_NGAN_HANG => "Trả góp ngân hàng",
                _ => string.Empty
            };
        }
    }
}
