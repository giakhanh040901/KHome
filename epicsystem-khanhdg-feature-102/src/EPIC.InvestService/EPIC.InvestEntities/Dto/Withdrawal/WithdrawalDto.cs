using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Project;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    public class WithdrawalDto
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public string WithdrawalIndex { get; set; }
        public int? Index { get; set; }
        public string CifCode { get; set; }
        /// <summary>
        /// Mã hợp đồng trong file
        /// </summary>
        public string GenContractCode { get; set; }
        public string Name { get; set; }
        public decimal? AmountMoney { get; set; }
        public decimal ActuallyAmount { get; set; }
        public int? PolicyDetailId { get; set; }
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Lợi tức rút
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        public decimal? DeductibleProfit { get; set; }

        /// <summary>
        /// Thuế lợi nhuận
        /// </summary>
        public decimal? Tax { get; set; }

        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Phí rút
        /// </summary>
        public decimal? WithdrawalFee { get; set; }

        public int? Type { get; set; }
        /// <summary>
        /// Loại tính thuế chính sách 1: NET, 2: GROSS
        /// </summary>
        public decimal? PolicyCalculateType { get; set; }
        public int? Status { get; set; }
        public int? StatusBank { get; set; }
        public int? Source { get; set; }
        public DateTime WithdrawalDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApproveBy { get; set; }
        public string CreatedBy { get; set; }
        public InvOrder Order { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public ProjectDto Project { get; set; }

        /// <summary>
        /// Hình thức chi trả
        /// <see cref="InvestMethodInterests"/>
        /// </summary>
        public int? MethodInterest { get; set; }
    }
}
