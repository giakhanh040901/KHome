using EPIC.Entities.DataEntities;
using EPIC.EntitiesBase.Interfaces.Payment;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_INTEREST_PAYMENT", Schema = DbSchemas.EPIC)]

    public class InvestInterestPayment : IPaymentAuto
    {
        public static string SEQ = $"SEQ_INV_INTEREST_PAYMENT";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }
        public InvOrder Order { get; set; }

        [ColumnSnackCase(nameof(PeriodIndex))]
        public int? PeriodIndex { get; set; }
        [ColumnSnackCase(nameof(CifCode))]
        public string CifCode { get; set; }
        public CifCodes CifCodes { get; set; }

        /// <summary>
        /// Lợi nhuận chi trả thực
        /// </summary>
        [ColumnSnackCase(nameof(Profit))]
        public decimal Profit { get; set; }

        /// <summary>
        /// First-Save => Lợi nhuận chi trả thực = Profit
        /// Approve => Thì sẽ là số tiền chi thực tế khi duyệt
        /// Duyệt cuối kỳ không tái tục sẽ + thêm TotalValueInvestment
        /// Duyệt cuối kỳ có tái tục không lợi nhuận thì vẫn lợi nhuận thực nhận
        /// Duyệt cuối kỳ có tái tục + lợi nhuận thì = 0
        /// </summary>
        [ColumnSnackCase(nameof(AmountMoney))]
        public decimal AmountMoney { get; set; }

        [ColumnSnackCase(nameof(Tax))]
        public decimal? Tax { get; set; }

        /// <summary>
        /// Tổng giá trị đang đầu tư nếu là cuối kỳ
        /// </summary>
        [ColumnSnackCase(nameof(TotalValueInvestment))]
        public decimal TotalValueInvestment { get; set; }

        /// <summary>
        /// Số tiền đang đầu tư của hợp đồng ở thời điểm chi
        /// </summary>
        [ColumnSnackCase(nameof(TotalValueCurrent))]
        public decimal TotalValueCurrent { get; set; }

        [ColumnSnackCase(nameof(PolicyDetailId))]
        public int? PolicyDetailId { get; set; }
        public PolicyDetail PolicyDetail { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }
        [ColumnSnackCase(nameof(IsLastPeriod))]
        public string IsLastPeriod { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        [ColumnSnackCase(nameof(StatusBank))]
        public int? StatusBank { get; set; }

        [ColumnSnackCase(nameof(PayDate))]
        public DateTime? PayDate { get; set; }
        [ColumnSnackCase(nameof(ApproveDate))]
        public DateTime? ApproveDate { get; set; }
        [ColumnSnackCase(nameof(ApproveBy))]
        public string ApproveBy { get; set; }
        [ColumnSnackCase(nameof(ApproveIp))]
        public string ApproveIp { get; set; }

        /// <summary>
        /// Nội dung duyệt
        /// <see cref="ApproveNoteWithoutPayments"/>
        /// </summary>
        [ColumnSnackCase(nameof(ApproveNote))]
        public int? ApproveNote { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }
        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }
        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }
        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
    }
}
