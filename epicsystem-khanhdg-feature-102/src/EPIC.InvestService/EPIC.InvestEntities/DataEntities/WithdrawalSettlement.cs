using EPIC.Entities.DataEntities;
using EPIC.EntitiesBase.Interfaces.Payment;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_WITHDRAWAL", Schema = DbSchemas.EPIC)]
    public class Withdrawal : IPaymentAuto
    {
        public static string SEQ = $"SEQ_INV_WITHDRAWAL";
        [Key]
        [Column("ID")]
        public long Id { get; set; }

        [Column("ORDER_ID")]
        public long? OrderId { get; set; }
        public InvOrder Order { get; set; }

        [Column("CIF_CODE")]
        public string CifCode { get; set; }
        public CifCodes CifCodes { get; set; }

        /// <summary>
        /// Số tiền rút
        /// </summary>
        [Column("AMOUNT_MONEY")]
        public decimal? AmountMoney { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        [Column("ACTUALLY_AMOUNT")]
        public decimal? ActuallyAmount { get; set; }

        /// <summary>
        /// Lợi tức rút
        /// </summary>
        [Column("PROFIT")]
        public decimal? Profit { get; set; }

        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        [Column("DEDUCTIBLE_PROFIT")]
        public decimal? DeductibleProfit { get; set; }

        /// <summary>
        /// Thuế lợi nhuận
        /// </summary>
        [Column("TAX")]
        public decimal? Tax { get; set; }

        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        [Column("ACTUALLY_PROFIT")]
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Phí rút
        /// </summary>
        [Column("WITHDRAWAL_FEE")]
        public decimal? WithdrawalFee { get; set; }

        [Column("POLICY_DETAIL_ID")]
        public int? PolicyDetailId { get; set; }
        public PolicyDetail PolicyDetail { get; set; }
        [Column("TRADING_PROVIDER_ID")]
        public int? TradingProviderId { get; set; }
        [Column("TYPE")]
        public int? Type { get; set; }
        [Column("SOURCE")]
        public int? Source { get; set; }
        [Column("STATUS")]
        public int? Status { get; set; }
        [Column("STATUS_BANK")]
        public int? StatusBank { get; set; }
        [Column("WITHDRAWAL_DATE")]
        public DateTime WithdrawalDate { get; set; }
        [Column("REQUEST_IP")]
        public string RequestIp { get; set; }
        [Column("APPROVE_DATE")]
        public DateTime? ApproveDate { get; set; }
        [Column("APPROVE_BY")]
        public string ApproveBy { get; set; }
        [Column("APPROVE_IP")]
        public string ApproveIp { get; set; }

        /// <summary>
        /// Nội dung duyệt
        /// <see cref="ApproveNoteWithoutPayments"/>
        /// </summary>
        [ColumnSnackCase(nameof(ApproveNote))]
        public int? ApproveNote { get; set; }

        [Column("CREATED_BY")]
        public string CreatedBy { get; set; }
        [Column("CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }
        [Column("MODIFIED_BY")]
        public string ModifiedBy { get; set; }
        [Column("MODIFIED_DATE")]
        public DateTime? ModifiedDate { get; set; }
        [Column("DELETED")]
        public string Deleted { get; set; }
    }
}
