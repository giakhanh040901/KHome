using EPIC.Entities;
using EPIC.EntitiesBase.Interfaces.Audit;
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

namespace EPIC.GarnerEntities.DataEntities
{
    /// <summary>
    /// Chi trả lợi tức chi tiết
    /// </summary>
    [Table("GAN_INTEREST_PAYMENT_DETAIL", Schema = DbSchemas.EPIC_GARNER)]
    [Comment("Chi tra loi tuc chi tiet")]
    public class GarnerInterestPaymentDetail
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerInterestPaymentDetail)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(InterestPaymentId))]
        public long InterestPaymentId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(AmountReceived))]
        [Comment("Số tiền thực nhận")]
        public decimal AmountReceived { get; set; }

        [ColumnSnackCase(nameof(Tax))]
        [Comment("Thue phai chiu")]
        public decimal Tax { get; set; }

        [ColumnSnackCase(nameof(ProfitRate))]
        [Comment("phan tram loi nhuan nhan lai")]
        public decimal ProfitRate { get; set; }

        [ColumnSnackCase(nameof(Profit))]
        [Comment("loi nhuan chi tra")]
        public decimal Profit { get; set; }

        /// <summary>
        /// Số tiền đầu tư của hợp đồng ở thời điểm chi
        /// </summary>
        [ColumnSnackCase(nameof(TotalValueCurrent))]
        public decimal TotalValueCurrent { get; set; }
    }
}
