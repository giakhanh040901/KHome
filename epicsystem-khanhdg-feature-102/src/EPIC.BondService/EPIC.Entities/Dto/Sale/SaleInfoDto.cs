using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class SaleInfoDto
    {
        public int SaleId { get; set; }
        public string ReferralCode { get; set; }
        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? SaleType { get; set; }
        public string Status { get; set; }
        public int TradingProviderId { get; set; }
        public string TradingProviderName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentAddress { get; set; }
        public string Area { get; set; }
        public int? SaleParentId { get; set; }

        /// <summary>
        /// Id sale quản lý phòng ban
        /// </summary>
        public int? ManagerId { get; set; }
        public string AvatarImageUrl { get; set; }
    }
}
