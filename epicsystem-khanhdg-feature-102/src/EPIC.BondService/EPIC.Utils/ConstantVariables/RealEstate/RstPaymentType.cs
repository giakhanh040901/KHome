using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstPaymentType
    {
        public static string PaymentType(int paymentType)
        {
            return paymentType switch
            {
                PaymentTypes.TIEN_MAT => RstPaymentTypeText.TienMat,
                PaymentTypes.CHUYEN_KHOAN => RstPaymentTypeText.ChuyenKhoan,
                _ => string.Empty
            };
        }

        public class RstPaymentTypeText
        {
            public const string TienMat = "Tiền mặt";
            public const string ChuyenKhoan = "Chuyển khoản";
            public static readonly List<string> All = new List<string>()
            {
                 TienMat, ChuyenKhoan
            };
        }
    }
}
