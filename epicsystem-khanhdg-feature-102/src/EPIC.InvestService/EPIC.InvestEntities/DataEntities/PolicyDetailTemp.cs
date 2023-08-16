using EPIC.Entities;
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
    [Table("EP_INV_POLICY_DETAIL_TEMP", Schema = DbSchemas.EPIC)]
    public class PolicyDetailTemp : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_POLICY_DETAIL_TEMP";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PolicyTempId))]
		public int PolicyTempId { get; set; }
        public PolicyTemp PolicyTemp { get; set; }
        public int? STT { get; set; }

        [ColumnSnackCase(nameof(ShortName))]
        public string ShortName { get; set; }
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }
        [ColumnSnackCase(nameof(PeriodType))]
        public string PeriodType { get; set; }
        [ColumnSnackCase(nameof(PeriodQuantity))]
        public int? PeriodQuantity { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }
        [ColumnSnackCase(nameof(Profit))]
        public decimal? Profit { get; set; }
        [ColumnSnackCase(nameof(InterestDays))]
        public int? InterestDays { get; set; }
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

        /// <summary>
        /// Kiểu trả lợi tức: 1 Định kỳ, 2 Cuối kỳ, 3 Ngày cố định hàng tháng, 4 Ngày đầu tháng, 5 Ngày cuối tháng
        /// </summary>
        [ColumnSnackCase(nameof(InterestType))]
        public int? InterestType { get; set; }
        
        [ColumnSnackCase(nameof(InterestPeriodQuantity))]
        public int? InterestPeriodQuantity { get; set; }
        [ColumnSnackCase(nameof(InterestPeriodType))]
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// Ngày trả cố định khi kiểu trả lợi tức Ngày cố định
        /// </summary>
        [ColumnSnackCase(nameof(FixedPaymentDate))]
        public int? FixedPaymentDate { get; set; }
    }
}
