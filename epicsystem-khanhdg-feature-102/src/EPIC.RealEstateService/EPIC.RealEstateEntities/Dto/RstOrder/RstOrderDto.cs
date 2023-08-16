using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrder
{
    public class RstOrderDto
    {
        public int Id { get; set; }
        public string CifCode { get; set; }
        public int TradingProviderId { get; set; }
        public int ProductItemId { get; set; }
        public int? CartId { get; set; }
        public string ContractCode { get; set; }
        /// <summary>
        /// Hình thức thanh toán 1: Thanh toán thường, 2: Thanh toán sớm, 3: Trả trước ngân hàng
        /// </summary>
        public int? PaymentType { get; set; }
        public int? InvestorIdenId { get; set; }
        public int? ContractAddressId { get; set; }
        public int DistributionPolicyId { get; set; }
        public decimal DepositMoney { get; set; }
        public string SaleReferralCode { get; set; }
        public int? SaleOrderId { get; set; }
        public int Source { get; set; }
        public DateTime? ExpTimeDeposit { get; set; }
        public int Status { get; set; }
        public string IpAddressCreated { get; set; }
        public int? OpenSellDetailId { get; set; }
        public string GenContractCode { get; set; }
    }
}
