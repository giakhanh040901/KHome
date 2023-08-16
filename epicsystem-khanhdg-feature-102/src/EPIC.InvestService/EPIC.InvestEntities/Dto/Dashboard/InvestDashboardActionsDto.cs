using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Dashboard
{
    public class InvestDashboardActionsDto
    {
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// Hành động 
        /// 1: Đặt lệnh mới; 2: Rút tiền; 3: Yêu cầu nhận hợp đồng, 4: Tạo yêu cầu tái tục
        /// </summary>
        public int Action { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long OrderId { get; set; }
        public long WithdrawalId { get; set; }
    }
}
