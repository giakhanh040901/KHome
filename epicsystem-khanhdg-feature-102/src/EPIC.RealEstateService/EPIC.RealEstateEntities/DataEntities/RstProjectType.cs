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
using EPIC.Utils.Attributes;
using EPIC.Utils.DataUtils;
using System.ComponentModel.DataAnnotations;
using EPIC.Entities;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Loại hình trong dự án
    /// </summary>
    [Table("RST_PROJECT_TYPE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(ProjectId), IsUnique = false, Name = "IX_RST_PROJECT_TYPE")]
    public class RstProjectType
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectType).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Loại hình: 1: Sản phẩm dự án, 2: Loại hình phân phối
        /// </summary>
        [ColumnSnackCase(nameof(ProjectType))]
        public int ProjectType { get; set; }

        /// <summary>
        /// Chi tiết loại hình 
        /// ProjectType = 1 -> 1: Chung cư, 2 Biệt thự, 3: Liền kề, 4 Khách sạn...
        /// ProjectType = 2 -> 1: Chủ Đầu tư phân phối, 2: Đại lý phân phối
        /// </summary>
        [ColumnSnackCase(nameof(Type))]
        public int Type { get; set; }
    }
}
