//using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_APPROVE", Schema = DbSchemas.EPIC)]
    public class CoreApprove
    {
        public static string SEQ { get; } = $"SEQ_CORE_APPROVE";

        [Key]
		[Column("APPROVE_ID")]
		public int ApproveID { get; set; }

		[Column("USER_REQUEST_ID")]
		public int? UserRequestId { get; set; }

		[Column("USER_APPROVE_ID")]
		public int? UserApproveId { get; set; }

		[Column("REQUEST_DATE")]
		public DateTime? RequestDate { get; set; }

		[Column("APPROVE_DATE")]
		public DateTime? ApproveDate { get; set; }

		[Column("CANCEL_DATE")]
		public DateTime? CancelDate { get; set; }

		[Column("REQUEST_NOTE")]
		public string RequestNote { get; set; }

		[Column("APPROVE_NOTE")]
		public string ApproveNote { get; set; }

		[Column("CANCEL_NOTE")]
		public string CancelNote { get; set; }

		[Column("STATUS")]
		public int? Status { get; set; }

		[Column("DATA_TYPE")]
		public int? DataType { get; set; }

		[Column("DATA_STATUS")]
		public int? DataStatus { get; set; }

		[Column("DATA_STATUS_STR")]
		public string DataStatusStr { get; set; }

		[Column("ACTION_TYPE")]
		public int? ActionType { get; set; }

		[Column("REFER_ID_TEMP")]
		public int? ReferIdTemp { get; set; }

		[Column("REFER_ID")]
		public int? ReferId { get; set; }

		[Column("IS_CHECK")]
		public string IsCheck { get; set; }

		[Column("USER_CHECK_ID")]
		public int? UserCheckId { get; set; }

        [Column("SUMMARY")]
        public string Summary { get; set; }

		[ColumnSnackCase(nameof(TradingProviderId))]
		public int? TradingProviderId { get; set; }

		[ColumnSnackCase(nameof(PartnerId))]
		public int? PartnerId { get; set; }

        /// <summary>
        /// Lưu file upload khi tạo yêu cầu trình duyệt
        /// </summary>
        [ColumnSnackCase(nameof(ApproveRequestFileUrl))]
        public string ApproveRequestFileUrl { get; set; }
    }
}
