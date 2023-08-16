using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Đánh giá
    /// </summary>
    [Table("RST_RATING", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(InvestorId), nameof(OrderId), IsUnique = false, Name = "IX_RST_RATING")]
    public class RstRating : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(RstRating)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public int OrderId { get; set; }

        /// <summary>
        /// Số sao đánh giá (từ 1 sao đến 5 sao)
        /// </summary>
        [ColumnSnackCase(nameof(Rate))]
        public int Rate { get; set; }

        /// <summary>
        /// Trải nghiệm sản phẩm (1: Dịch vụ, 2: Tư vấn, 3: Trải nghiệm, 4: Ưu đãi, 5: Hỗ trợ, 6: Dịch vụ hoàn hảo, 7: Tư vấn nhiệt tình, 8: Thân thiện, 9: Hỗ trợ nhanh chóng, 10: Khác)
        /// </summary>
        [ColumnSnackCase(nameof(ProductExperience))]
        public int ProductExperience { get; set; }

        /// <summary>
        /// Góp ý
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Feedback))]
        public string Feedback { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
