using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    /// <summary>
    /// Chia sẻ dự án
    /// </summary>
    [Table("EP_INV_PROJECT_INFORMATION_SHARE", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), nameof(ProjectId), nameof(PartnerId), IsUnique = false, Name = "IX_INVEST_PROJECT_INFORMATION_SHARE")]
    public class InvestProjectInformationShare : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(InvestProjectInformationShare).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Tiêu đề
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Title))]
        [MaxLength(512)]
        public string Title { get; set; }

        /// <summary> 
        /// trạng thái A:Active, D:Deactive
        /// </summary>
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string Status { get; set; }

        #region Mô tả thông tin dự án
        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(ContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string ContentType { get; set; }

        /// <summary>
        /// Nội dung tổng quan
        /// </summary>
        [ColumnSnackCase(nameof(OverviewContent), TypeName = "CLOB")]
        public string OverviewContent { get; set; }
        #endregion

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

        public Project Project { get; set; }
        public List<InvestProjectInformationShareDetail> ProjectInformationShareDetails { get; } = new();
    }
}
