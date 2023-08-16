using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Dashboard
{
    public class CustomerListDto
    {
        /// <summary>
        /// Số thứ tự
        /// </summary>
        public int STT { get; set; }
        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }  
        /// <summary>
        /// Thế hệ
        /// </summary>
        public int Generation { get; set; }
        /// <summary>
        /// Số sản phẩm đã sử dụng
        /// </summary>
        public int NumberProducts { get; set; }
        /// <summary>
        /// Đang đầu tư
        /// </summary>
        public decimal TotalActiveAmount { get; set; }
        /// <summary>
        /// Đã đầu tư
        /// </summary>
        public decimal TotalActivedAmount { get; set; }
        /// <summary>
        /// Tổng
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
