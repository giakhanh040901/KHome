using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_APPROVE", Schema = DbSchemas.EPIC)]
    public class InvestApprove : EntityTypeMapper
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserRequestId))]
        public int? UserRequestId { get; set; }

        [ColumnSnackCase(nameof(UserApproveId))]
        public int? UserApproveId { get; set; }

        [ColumnSnackCase(nameof(RequestDate))]
        public DateTime? RequestDate { get; set; }

        [ColumnSnackCase(nameof(ApproveDate))]
        public DateTime? ApproveDate { get; set; }

        [ColumnSnackCase(nameof(CancelDate))]
        public DateTime? CancelDate { get; set; }

        [ColumnSnackCase(nameof(RequestNote))]
        public string RequestNote { get; set; }

        [ColumnSnackCase(nameof(ApproveNote))]
        public string ApproveNote { get; set; }

        [ColumnSnackCase(nameof(CancelNote))]
        public string CancelNote { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(DataType))]
        public int? DataType { get; set; }

        [ColumnSnackCase(nameof(DataStatus))]
        public int? DataStatus { get; set; }

        [ColumnSnackCase(nameof(DataStatusStr))]
        public string DataStatusStr { get; set; }

        [ColumnSnackCase(nameof(ActionType))]
        public int? ActionType { get; set; }

        [ColumnSnackCase(nameof(ReferIdTemp))]
        public int? ReferIdTemp { get; set; }

        [ColumnSnackCase(nameof(ReferId))]
        public int? ReferId { get; set; }

        [ColumnSnackCase(nameof(IsCheck))]
        public string IsCheck { get; set; }

        [ColumnSnackCase(nameof(UserCheckId))]
        public int? UserCheckId { get; set; }

        [ColumnSnackCase(nameof(Summary))]
        public string Summary { get; set; }
    }
}
