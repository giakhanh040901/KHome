using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities.DataEntities;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.DataUtils;
using EPIC.Utils.Attributes;
using System.ComponentModel.DataAnnotations;
using EPIC.Entities;

namespace EPIC.RealEstateEntities.DataEntities
{
    [Table("RST_PROJECT_FAVOURITE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(OpenSellId), nameof(InvestorId), IsUnique = false, Name = "IX_RST_PROJECT_FAVOURITE")]
    public class RstProjectFavourite : ICreatedBy
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectFavourite).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        /// <summary>
        /// ID mở bán
        /// </summary>
        [ColumnSnackCase(nameof(OpenSellId))]
        public int OpenSellId { get; set; }
        /// <summary>
        /// ID nhà đầu tư cá nhân
        /// </summary>
        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        /// <summary>
        /// Ngày yêu thích
        /// </summary>
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get ; set ; }

        /// <summary>
        /// Người yêu thích
        /// </summary>
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get ; set ; }
    }
}
