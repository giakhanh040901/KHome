using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Shared
{
    public class InterestPaymentStatus
    {
        /// <summary>
        /// Đã lập chưa chi trả
        /// </summary>
        public const int DA_LAP_CHUA_CHI_TRA = 1;

        /// <summary>
        /// Đã duyệt chi trả tiền từ ngân hàng
        /// </summary>
        public const int DA_DUYET_CHI_TIEN = 2;

        /// <summary>
        /// Đã hủy chi trả
        /// </summary>
        public const int HUY_DUYET = 3;

        /// <summary>
        /// Đã duyệt không chi trả tiền (duyệt thủ công)
        /// </summary>
        public const int DA_DUYET_KHONG_CHI_TIEN = 4;

        public const int CHO_PHAN_HOI = 5;

        public static readonly int[] NHOM_CHUA_CHI_TRA = new int[]
        {
            DA_LAP_CHUA_CHI_TRA, CHO_PHAN_HOI
        };

        public static readonly int[] NHOM_DA_CHI_TRA = new int[]
        {
            DA_DUYET_CHI_TIEN, DA_DUYET_KHONG_CHI_TIEN
        };
    }
}
