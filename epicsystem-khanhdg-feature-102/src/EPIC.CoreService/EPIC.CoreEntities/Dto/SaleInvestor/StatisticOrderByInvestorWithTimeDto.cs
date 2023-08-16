using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.SaleInvestor
{
    /// <summary>
    /// Thống kê hợp đồng và khách hàng của Sale theo thời gian
    /// </summary>
    public class StatisticOrderByInvestorWithTimeDto
    {
        /// <summary>
        /// Ngày
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Loại biểu đồ: 1. Theo ngày, 2: Theo Tuần, 3: Theo Tháng
        /// </summary>
        public int StatisticType { get; set; }

        /// <summary>
        /// Tổng số nhân viên
        /// </summary>
        public int TotalInvestor { get; set; }

        /// <summary>
        /// Tổng hợp đồng 
        /// </summary>
        public int TotalContract { get; set; }

        /// <summary>
        /// Ngày bắt đầu của tuần hoặc tháng
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc của tuần hoặc tháng
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Nếu là loại biểu đồ Tuần, Tuần thứ bao nhiêu trong năm
        /// Nếu là loại biểu đồ Ngày, Ngày trong tháng
        /// Nếu là tháng thì là tháng bao nhiêu
        /// </summary>
        public int? Time { get; set; }
    }
}
