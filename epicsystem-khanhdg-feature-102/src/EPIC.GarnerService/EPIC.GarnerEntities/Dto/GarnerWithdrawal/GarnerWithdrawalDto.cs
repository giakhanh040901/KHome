using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class GarnerWithdrawalDto
    {
        public long Id { get; set; }
        public string CifCode { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
        public int PolicyId { get; set; }
        public decimal AmountMoney { get; set; }
        public int Source { get; set; }
        public DateTime WithdrawalDate { get; set; }
        public int Status { get; set; }
        public int StatusBank { get; set; }
        public string ApproveBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string CancelBy { get; set; }
        public DateTime? CancelDate { get; set; }
        public int BankAccountId { get; set; }
        public BankAccountInfoDto BankAccountInfo { get; set; }
        public GarnerPolicyDto Policy { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public GarnerProductDto Product { get; set; }

        /// <summary>
        /// Tổng lợi tức thực nhận
        /// </summary>
        public decimal ActuallyProfit { get; set; }

        /// <summary>
        /// Tổng tiền thực nhận
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// Chi tiết rút tiền
        /// </summary>
        public List<GarnerWithdrawalOrderDetailDto> WithdrawalDetail { get; set; }
    }
}
