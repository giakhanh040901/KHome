using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class AppInvestorContactListDto
    {
        /// <summary>
        /// InvestorId
        /// </summary>
        public int? InvestorId { get; set; }
        /// <summary>
        /// TradingProviderId
        /// </summary>
        public int? TradingProviderId { get; set; }
        /// <summary>
        /// SĐT
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Avatar
        /// </summary>
        public string AvatarImageUrl { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Tên viết tắt
        /// </summary>
        public string ShortName { get; set; }
    }
}
