using DocumentFormat.OpenXml.Wordprocessing;
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

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Ngân hàng đảm bảo
    /// </summary>
    [Table("RST_PROJECT_GUARANTEE_BANK", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(ProjectId), IsUnique = false, Name = "IX_RST_PROJECT_GUARANTEE_BANK")]
    public class RstProjectGuaranteeBank
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectGuaranteeBank).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Ngân hàng bảo đảm, lấy từ bảng CORE_BANK
        /// </summary>
        [ColumnSnackCase(nameof(BankId))]
        public int BankId { get; set; }
    }
}
