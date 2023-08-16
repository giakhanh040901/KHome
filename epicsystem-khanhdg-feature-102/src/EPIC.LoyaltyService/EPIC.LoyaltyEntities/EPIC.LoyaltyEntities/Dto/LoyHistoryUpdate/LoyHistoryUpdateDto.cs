using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.LoyaltyEntities.Dto.LoyHistoryUpdate
{
    public class LoyHistoryUpdateDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Update bảng nào<br/>
        /// <see cref="LoyHistoryUpdateTables"/>
        /// </summary>
        public int UpdateTable { get; set; }

        /// <summary>
        /// Id Bảng thật
        /// </summary>
        public int RealTableId { get; set; }

        /// <summary>
        /// Giá trị cũ
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Giá trị mới
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Tên trường
        /// <see cref="LoyFieldName"/>
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Hành động (1: thêm mới, 2: cập nhật, 3: xoá)
        /// <see cref="ActionTypes"/>
        /// </summary>
        public int Action { get; set; }


        /// <summary>
        /// Nội dung tổng quan là làm cái gì (vd: khởi tạo lệnh, tạo thanh toán, cập nhật(cập nhật trạng thái))
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Ngay thuc hien
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người thực hiện
        /// </summary>
        public string CreatedBy { get; set; }
    }
}
