using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Invest
{
    public class SettlementMethod
    {
        public const int TAT_TOAN_KHONG_TAI_TUC = 1;
        public const int NHAN_LOI_NHUAN_VA_TAI_TUC_GOC = 2;
        public const int TAI_TUC_GOC_VA_LOI_NHUAN = 3;

        public static string Name(int settlementMethod)
        {
            return settlementMethod switch
            {
                TAT_TOAN_KHONG_TAI_TUC => "Tất toán không tái tục",
                NHAN_LOI_NHUAN_VA_TAI_TUC_GOC => "Tái tục gốc",
                TAI_TUC_GOC_VA_LOI_NHUAN => "Tái tục gốc và lợi nhuận",
                _ => string.Empty
            };
        }
    }
}
