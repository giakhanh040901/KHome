using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_POLICY_TEMP", Schema = DbSchemas.EPIC)]
    public class PolicyTemp : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_POLICY_TEMP";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }
        [ColumnSnackCase(nameof(Name))]
		public string Name { get; set; }
        [ColumnSnackCase(nameof(Type))]
        public int? Type { get; set; }
        [ColumnSnackCase(nameof(IncomeTax))]
        public decimal? IncomeTax { get; set; }
        [ColumnSnackCase(nameof(TransferTax))]
        public decimal? TransferTax { get; set; }
        [ColumnSnackCase(nameof(Classify))]
        public decimal? Classify { get; set; }
        [ColumnSnackCase(nameof(MinMoney))]
        public decimal? MinMoney { get; set; }

        /// <summary>
        /// Số tiền đầu tư tối đa
        /// </summary>
        [ColumnSnackCase(nameof(MaxMoney))]
        public decimal? MaxMoney { get; set; }

        [ColumnSnackCase(nameof(IsTransfer))]
        public string IsTransfer { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        /// <summary>
        /// % Phần trăm lợi nhuận cố định: Sử dụng khi InvestCalculateWithdrawTypes = 2
        /// </summary>
        [ColumnSnackCase(nameof(ProfitRateDefault))]
        public decimal? ProfitRateDefault { get; set; }

        /// <summary>
        /// Cách tính lợi tức rút vốn: 1: Kỳ hạn thấp hơn gần nhất, 2: Giá trị cố định
        /// <see cref="InvestCalculateWithdrawTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(CalculateWithdrawType))]
        public int CalculateWithdrawType { get; set; }

        [ColumnSnackCase(nameof(MinWithdraw))]
        public decimal? MinWithdraw { get; set; }
        [ColumnSnackCase(nameof(CalculateType))]
        public decimal? CalculateType { get; set; }
        [ColumnSnackCase(nameof(ExitFee))]
        public decimal? ExitFee { get; set; }
        [ColumnSnackCase(nameof(ExitFeeType))]
        public decimal? ExitFeeType { get; set; }
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }
        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }
        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }
        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }
        [ColumnSnackCase(nameof(PolicyDisplayOrder))]
        public int? PolicyDisplayOrder { get; set; }

        [ColumnSnackCase(nameof(RenewalsType))]
        [Comment("Loại hợp đồng tái tục")]
        public int RenewalsType { get; set; }

        [ColumnSnackCase(nameof(RemindRenewals))]
        [Comment("Nhắc tái tục trước(Ngày)")]
        public int RemindRenewals { get; set; }

        [ColumnSnackCase(nameof(ExpirationRenewals))]
        [Comment("Hạn gửi yêu cầu tái tục trước(Ngày)")]
        public int ExpirationRenewals { get; set; }


        [ColumnSnackCase(nameof(MaxWithDraw))]
        [Comment("Số tiền rút tối đa")]
        public decimal MaxWithDraw { get; set; }

        [ColumnSnackCase(nameof(MinTakeContract))]
        [Comment("Gửi yêu cầu nhận hợp đồng")]
        public decimal MinTakeContract { get; set; }

        [ColumnSnackCase(nameof(MinInvestDay))]
        [Comment("Số ngày đầu tư tối thiểu")]
        public int MinInvestDay { get; set; }

        public List<PolicyDetailTemp> PolicyDetailTemps { get; set; }
    }
}
