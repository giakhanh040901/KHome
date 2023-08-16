using DocumentFormat.OpenXml.Vml.Spreadsheet;
using EPIC.Utils.RolePermissions.Constant;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.ConstantVariables.Loyalty
{
    public static class LoyHisAccumulatePointReasons
    {
        public const int KHAC = 1;

        #region tích điểm
        /// <summary>
        /// Quy đổi đầu tư tài chính
        /// </summary>
        public const int QUY_DOI_DAU_TU_TAI_CHINH = 2;
        /// <summary>
        /// Tặng tham gia sự kiện
        /// </summary>
        public const int TANG_THAM_GIA_SU_KIEN = 3;
        /// <summary>
        /// Tặng thưởng khách hàng
        /// </summary>
        public const int TANG_THUONG_KHACH_HANG = 4;
        #endregion

        #region tiêu điểm
        /// <summary>
        /// Đổi voucher
        /// </summary>
        public const int DOI_VOUCHER = 5;
        /// <summary>
        /// Giảm điểm khách hàng
        /// </summary>
        public const int GIAM_DIEM_KHACH_HANG = 6;
        #endregion

        /// <summary>
        /// Định nghĩa lý do 
        /// </summary>
        public static List<ViewListReasonsDto> ConfigReasons = new List<ViewListReasonsDto>
        {
            new ViewListReasonsDto{ Label = "Khác", Value = KHAC, Type = null },
            new ViewListReasonsDto{ Label = "Quy đổi đầu tư", Value = QUY_DOI_DAU_TU_TAI_CHINH, Type = LoyPointTypes.TICH_DIEM },
            new ViewListReasonsDto{ Label = "Tặng tham gia sự kiện", Value = TANG_THAM_GIA_SU_KIEN, Type = LoyPointTypes.TICH_DIEM },
            new ViewListReasonsDto{ Label = "Tặng thưởng khách hàng", Value = TANG_THUONG_KHACH_HANG, Type = LoyPointTypes.TICH_DIEM },
            new ViewListReasonsDto{ Label = "Đổi voucher", Value = DOI_VOUCHER, Type = LoyPointTypes.TIEU_DIEM },
            new ViewListReasonsDto{ Label = "Giảm điểm khách hàng", Value = GIAM_DIEM_KHACH_HANG, Type = LoyPointTypes.TIEU_DIEM },
        };
    }

    public class ViewListReasonsDto
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public int? Type { get; set; }
    }


}
