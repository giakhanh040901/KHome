using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.DataEntities
{
    public class ExcelCustomerInfoChangeRoot
    {
        /// <summary>
        /// Id của bảng phê duyệt
        /// </summary>
        public int? ApproveId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Id khách hàng cá nhân
        /// </summary>
        public int? InvestorId { get; set; }

        /// <summary>
        /// Id khách hàng doanh nghiệp
        /// </summary>
        public int? BusinessCustomerId { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? EditDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string EditBy { get; set; }

        /// <summary>
        /// Ngày duyệt
        /// </summary>
        public DateTime? ApproveDate { get; set; }
        
        /// <summary>
        /// Ghi chú duyệt
        /// </summary>
        public string ApproveNote { get; set; }

        /// <summary>
        /// Ghi chú hủy duyệt
        /// </summary>
        public string CancelNote { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Tên của người 
        /// </summary>
        public string ApproveUser { get; set; }

        /// <summary>
        /// Loại khách hàng
        /// </summary>
        public string CustomerType { get; set; }
    }
}
