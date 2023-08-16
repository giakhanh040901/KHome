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

namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_INVESTOR_TO_DO", Schema = DbSchemas.EPIC)]
    [Comment("Nhắc nhở khách hàng cá nhân")]
    public class InvestorToDo
    {
        public static string SEQ { get; } = $"SEQ_{(nameof(InvestorToDo)).ToSnakeUpperCase()}";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int InvestorId { get; set; }

        [ColumnSnackCase(nameof(Type))]
        [Comment("Loại nhắc nhở (1: khoản đầu tư invest đến hạn)")]
        public int Type { get; set; }

        [MaxLength(512)]
        [ColumnSnackCase(nameof(Detail))]
        [Comment("Chi tiết")]
        public string Detail { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("Trạng thái (1: khởi tạo, 2: đã xem)")]
        public int Status { get; set; }
    }
}
