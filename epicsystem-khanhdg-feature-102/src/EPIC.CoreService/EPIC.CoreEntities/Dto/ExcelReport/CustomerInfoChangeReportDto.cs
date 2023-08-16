using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ExcelReport
{
    public class CustomerInfoChangeReportDto
    {
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// ĐKKD/CCCD/CMT/HC
        /// </summary>
        public string IdNo { get; set; }

        /// <summary>
        /// Loại thay đổi
        /// </summary>
        public string ChangeType { get; set; }

        /// <summary>
        /// Thông tin trước thay đổi
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Thông tin sau thay đổi
        /// </summary>
        public string NewValue { get; set; }

        /// <summary>
        /// Thời gian sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Người duyệt
        /// </summary>
        public string ApproveBy { get; set; }

        /// <summary>
        /// Thời gian duyệt
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Description { get; set; }
    }
}
