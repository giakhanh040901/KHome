using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Core
{
    /// <summary>
    /// Loại giao dịch
    /// </summary>
    public static class TranClassifies
    {
        // (Invest, Garner: Thanh toán hợp đồng, RealEstase: Thanh toán cọc
        public const int THANH_TOAN = 1;
        public const int CHI_TRA_LOI_TUC = 2;
        public const int RUT_VON = 3;
        public const int TAI_TUC_HOP_DONG = 4;
    }
}
