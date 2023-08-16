using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class InvestorTodoDto
    {
        /// <summary>
        /// Id cá nhân
        /// </summary>
        public int InvestorId { get; set; }
        /// <summary>
        /// Loại nhắc nhở
        /// </summary>
        public int Type { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
        /// <summary>
        /// Id lệnh để điều hướng
        /// </summary>
        public long OrderId { get; set; }
    }
}
