using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.RealEstate
{
    public class RstOrderPaymentStatus
    {
        public static string PaymentStatus(int status)
        {
            return status switch
            {
                OrderPaymentStatus.NHAP => RstOrderPaymentStatusText.Nhap,
                OrderPaymentStatus.DA_THANH_TOAN => RstOrderPaymentStatusText.DaThanhToan,
                OrderPaymentStatus.HUY_THANH_TOAN => RstOrderPaymentStatusText.HuyThanhToan,
                _ => string.Empty
            };
        }

        public class RstOrderPaymentStatusText
        {
            public const string Nhap = "Trình duyệt";
            public const string DaThanhToan = "Đã thanh toán";
            public const string HuyThanhToan = "Hủy thanh toán";
            public static readonly List<string> All = new List<string>()
            {
                 Nhap, DaThanhToan, HuyThanhToan
            };
        }
    }
}
