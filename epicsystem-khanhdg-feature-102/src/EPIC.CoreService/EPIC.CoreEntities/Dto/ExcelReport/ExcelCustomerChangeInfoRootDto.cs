using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class ExcelCustomerChangeInfoRootDto
    {
        /// <summary>
        /// Ngày duyệt
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? EditDate { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Lần chỉnh sửa thứ bao nhiêu
        /// </summary>
        public int EditTimes { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string EditBy { get; set; }

        /// <summary>
        /// Người duyệt
        /// </summary>
        public string ApproveBy{ get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Ghi chú chỉnh sửa = ghi chú duyệt
        /// </summary>
        public string ApproveNote { get; set; }

        /// <summary>
        /// Ghi chú hủy duyệt
        /// </summary>
        public string CancelNote { get; set; }

        /// <summary>
        /// Loại khách hàng
        /// </summary>
        public string CustomerType { get; set; }
    }
}
