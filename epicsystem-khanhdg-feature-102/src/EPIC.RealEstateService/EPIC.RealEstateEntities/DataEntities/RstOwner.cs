using DocumentFormat.OpenXml.CustomXmlSchemaReferences;
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
    [Table("RST_OWNER", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_OWNER")]
    public class RstOwner : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstOwner).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int BusinessCustomerId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Thông tin tài chính - doanh thu
        /// </summary>
        [ColumnSnackCase(nameof(BusinessTurnover))]
        public decimal? BusinessTurnover { get; set; }

        /// <summary>
        /// Thông tin tài chính - lợi nhuận sau thuế
        /// </summary>
        [ColumnSnackCase(nameof(BusinessProfit))]
        public decimal? BusinessProfit { get; set; }

        /// <summary>
        /// Thông tin tài chính - chỉ số ROA
        /// </summary>
        [ColumnSnackCase(nameof(Roa))]
        public decimal? Roa { get; set; }

        /// <summary>
        /// Thông tin tài chính - chỉ số ROE
        /// </summary>
        [ColumnSnackCase(nameof(Roe))]
        public decimal? Roe { get; set; }

        /// <summary>
        /// Website
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Website))]
        public string Website { get; set; }

        /// <summary>
        /// Đường dây nóng
        /// </summary>
        [MaxLength(50)]
        [ColumnSnackCase(nameof(Hotline))]
        public string Hotline { get; set; }

        /// <summary>
        /// Link page
        /// </summary>
        [MaxLength(1024)]
        [ColumnSnackCase(nameof(Fanpage))]
        public string Fanpage { get; set; }

        /// <summary>
        /// Trạng thái (A: Kích hoạt, D: Không kích hoạt, C: Đóng)
        /// </summary>
        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(DescriptionContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        [ColumnSnackCase(nameof(DescriptionContent), TypeName = "CLOB")]
        public string DescriptionContent { get; set; }

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
