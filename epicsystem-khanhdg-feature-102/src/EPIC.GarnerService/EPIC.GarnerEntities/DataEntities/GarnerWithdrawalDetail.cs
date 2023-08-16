using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.GarnerEntities.DataEntities
{
    [Table("GAN_WITHDRAWAL_DETAIL", Schema = DbSchemas.EPIC_GARNER)]
    public class GarnerWithdrawalDetail
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(GarnerWithdrawalDetail)).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(WithdrawalId))]
        public long WithdrawalId { get; set; }

        [ColumnSnackCase(nameof(OrderId))]
        public long OrderId { get; set; }

        [ColumnSnackCase(nameof(AmountMoney))]
        [Comment("Số tiền rút")]
        public decimal AmountMoney { get; set; }

        [Comment("Lợi tức rút")]
        [ColumnSnackCase(nameof(Profit))]
        public decimal Profit { get; set; }

        [Comment("Phần trăm Lợi tức rút")]
        [ColumnSnackCase(nameof(ProfitRate))]
        public decimal ProfitRate { get; set; }

        [Comment("Lợi tức khấu trừ")]
        [ColumnSnackCase(nameof(DeductibleProfit))]
        public decimal DeductibleProfit { get; set; }

        [Comment("Thuế lợi nhuận")]
        [ColumnSnackCase(nameof(Tax))]
        public decimal Tax { get; set; }

        [Comment("Lợi tức thực nhận")]
        [ColumnSnackCase(nameof(ActuallyProfit))]
        public decimal ActuallyProfit { get; set; }

        [Comment("Phí rút")]
        [ColumnSnackCase(nameof(WithdrawalFee))]
        public decimal WithdrawalFee { get; set; }

        [Comment("Số tiền thực nhận")]
        [ColumnSnackCase(nameof(AmountReceived))]
        public decimal AmountReceived { get; set; }
    }
}
