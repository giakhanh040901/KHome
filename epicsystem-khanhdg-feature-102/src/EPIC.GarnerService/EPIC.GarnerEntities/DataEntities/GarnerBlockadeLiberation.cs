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

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_BLOCKADE_LIBERATION", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Phong toa giai toa")]
    public class GarnerBlockadeLiberation
    {
        public static string SEQ = $"SEQ_{nameof(GarnerBlockadeLiberation).ToSnakeUpperCase()}";
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Comment("Id phong toa giai toa")]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(Type))]
        [Comment("Loai phong toa (1: Khac, 2: Cam co khoan vay, 3: Ung von)")]
        public int Type { get; set; }

        [ColumnSnackCase(nameof(BlockadeDescription))]
        [MaxLength(1024)]
        [Comment("Ghi chu phong toa")]
        public string BlockadeDescription { get; set; }

        [ColumnSnackCase(nameof(BlockadeDate), TypeName = "DATE")]
        [Comment("Ngay phong toa")]
        public DateTime? BlockadeDate { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        [Required]
        [Comment("Id so lenh")]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(Blockader))]
        [MaxLength(256)]
        [Comment("Nguoi phong toa")]
        public string Blockader { get; set; }

        [ColumnSnackCase(nameof(BlockadeTime), TypeName = "DATE")]
        [Comment("Thoi gian phong toa")]
        public DateTime? BlockadeTime { get; set; }

        [ColumnSnackCase(nameof(LiberationDescription))]
        [MaxLength(1024)]
        [Comment("Ghi chu giai toa")]
        public string LiberationDescription { get; set; }

        [ColumnSnackCase(nameof(LiberationDate), TypeName = "DATE")]
        [Comment("Ngay giai toa")]
        public DateTime? LiberationDate { get; set; }

        [ColumnSnackCase(nameof(Liberator))]
        [MaxLength(256)]
        [Comment("Nguoi giai toa")]
        public string Liberator { get; set; }

        [ColumnSnackCase(nameof(LiberationTime), TypeName = "DATE")]
        [Comment("Thoi gian giai toa")]
        public DateTime? LiberationTime { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [MaxLength(1)]
        [Required]
        [Comment("Trang thai  (1: Phong toa; 2: Giai toa)")]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        [Comment("Id dai ly so cap")]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }
    }
}
