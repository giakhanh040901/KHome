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
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.DataEntities
{
    [Table("EP_MSB_NOTIFICATION", Schema = DbSchemas.EPIC)]
    [Comment("Thong tin thanh toan Msb")]
    public class MsbNotification
    {
        public static string SEQ = $"SEQ_{nameof(MsbNotification).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [ColumnSnackCase(nameof(TranSeq))]
        public string TranSeq { get; set; }

        [ColumnSnackCase(nameof(VaCode))]
        public string VaCode { get; set; }

        [ColumnSnackCase(nameof(VaNumber))]
        public string VaNumber { get; set; }

        [ColumnSnackCase(nameof(FromAccountName))]
        public string FromAccountName { get; set; }

        [ColumnSnackCase(nameof(FromAccountNumber))]
        public string FromAccountNumber { get; set; }

        [ColumnSnackCase(nameof(ToAccountName))]
        public string ToAccountName { get; set; }

        [ColumnSnackCase(nameof(ToAccountNumber))]
        public string ToAccountNumber { get; set; }

        [ColumnSnackCase(nameof(TranAmount))]
        public string TranAmount { get; set; }

        [ColumnSnackCase(nameof(TranRemark))]
        public string TranRemark { get; set; }

        [ColumnSnackCase(nameof(TranDate))]
        public string TranDate { get; set; }

        [ColumnSnackCase(nameof(Signature))]
        public string Signature { get; set; }

        [ColumnSnackCase(nameof(Exception))]
        public string Exception { get; set; }

        [ColumnSnackCase(nameof(Status))]
        [Comment("Status")]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(Ip), TypeName = "VARCHAR2")]
        public string Ip { get; set; }

        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime CreatedDate { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        /// <summary>
        /// Loại sản phẩm: EI, EG, EH tương ứng với loại sp
        /// <see cref="ContractCodes"/>
        /// </summary>
        [ColumnSnackCase(nameof(ProjectType))]
        public string ProjectType { get; set; }

        /// <summary>
        /// Id của hợp đồng theo loại sản phẩm
        /// </summary>
        [ColumnSnackCase(nameof(ReferId))]
        public long? ReferId { get; set; }

        /// <summary>
        /// Ngày yêu cầu được translated từ TranDate
        /// </summary>
        [ColumnSnackCase(nameof(TransferDate))]
        public DateTime? TransferDate { get; set; }
    }
}
