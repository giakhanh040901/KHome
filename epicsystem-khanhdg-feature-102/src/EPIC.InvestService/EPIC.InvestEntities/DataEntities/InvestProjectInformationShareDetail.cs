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
using EPIC.Utils.ConstantVariables.RealEstate;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_PROJECT_INFORMATION_SHARE_DETAIL", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), nameof(ProjectShareId), IsUnique = false, Name = "IX_INVEST_PROJECT_INFO_SHARE_DETAIL")]
    public class InvestProjectInformationShareDetail : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(InvestProjectInformationShareDetail).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectShareId))]
        public int ProjectShareId { get; set; }

        /// <summary>
        /// Loại nội dụng: 1: File, 2: Hình ảnh<br/>
        /// <see cref="ProjectInformationShareFileTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(Type))]
        public int Type { get; set; }

        /// <summary>
        /// Tên đường dẫn file
        /// </summary>
        [ColumnSnackCase(nameof(Name))]
        [MaxLength(512)]
        public string Name { get; set; }

        /// <summary>
        /// Đường dẫn file
        /// </summary>
        [ColumnSnackCase(nameof(FileUrl))]
        public string FileUrl { get; set; }

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

        public InvestProjectInformationShare ProjectInformationShare { get; set; }
    }
}
