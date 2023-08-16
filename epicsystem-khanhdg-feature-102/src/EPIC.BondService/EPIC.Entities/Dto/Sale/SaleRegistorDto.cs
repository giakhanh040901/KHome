using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class SaleRegisterDto
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }
        public int InvestorBankAccId { get; set; }
        public int Status { get; set; }
        public string IpAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? DirectionDate { get; set; }
        public DateTime? CancelDate { get; set; }
    }

    public class SaleRegisterWithTradingDto
    {
        public int SaleRegisterId { get; set; }
        public string Fullname { get; set; }
        public int Status { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string AvatarImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? RegisterDate { get; set; }
        public DateTime? DirectionDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public string ReferralCode { get; set; }
        /// <summary>
        /// Lịch sử điều hướng
        /// </summary>
        public List<SaleRegisterDirectionToTradingProviderDto> SaleTradingProviders { get; set; }
        /// <summary>
        /// Danh sách đại lý mà sale được điều hướng đang tham gia
        /// </summary>
        public List<SaleRegisterDirectionToTradingProviderDto> TradingProviders { get; set; }
    }

    public class AppListSaleRegisterDto : SaleRegisterWithTradingDto
    {
        public int InvestorId { get; set; }
    }
}
