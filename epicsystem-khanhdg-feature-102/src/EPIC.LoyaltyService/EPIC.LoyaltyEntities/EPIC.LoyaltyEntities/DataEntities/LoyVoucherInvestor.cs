using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml.Wordprocessing;

namespace EPIC.LoyaltyEntities.DataEntities
{
    [Table("LOY_VOUCHER_INVESTOR", Schema = DbSchemas.EPIC_LOYALTY)]
    //[Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_OWNER")]
    public class LoyVoucherInvestor
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(LoyVoucherInvestor).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(VoucherId))]
        public int? VoucherId { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int? InvestorId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }

        /// <summary>
        /// Id yêu cầu tích điểm / đổi điểm
        /// </summary>
        [ColumnSnackCase(nameof(HisAccumulatePointId))]
        public int? HisAccumulatePointId { get; set; }

        /// <summary>
        /// Trạng thái (0: Khởi tạo; 1: Kích hoạt; 2: Hủy kích hoạt; 3: Đã xóa)
        /// <see cref="LoyVoucherInvestorStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Ngày giao
        /// </summary>

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
    }
}
