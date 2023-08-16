using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Garner
{
    /// <summary>
    ///  Các trạng thái khác của App xem lịch sử
    /// </summary>
    public static class GarnerOrderOtherStatus
    {
        public const int CHO_DUYET_RUT_VON = 1;
        public const int CHO_CHUYEN_DOI = 2;
        public const int RUT_VON_THANH_CONG = 3;
        public const int CHUYEN_DOI_THANH_CONG = 4;
        public const int HUY_DUYET_RUT_VON = 5;
    }
    public static class SettlementTypes
    {
        public const int NHAN_GOC_VA_LOI_TUC = 1;
        public const int TAI_TUC_GOC = 2;
        public const int TAI_TUC_GOC_VA_LOI_NHUAN = 3;
    }
}
