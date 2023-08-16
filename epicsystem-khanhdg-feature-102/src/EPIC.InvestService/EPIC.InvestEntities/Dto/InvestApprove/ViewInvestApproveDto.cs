using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.User;
using EPIC.InvestEntities.Dto.Distribution;
using System;

namespace EPIC.InvestEntities.Dto.InvestApprove
{
	public class ViewInvestApproveDto
	{
		public decimal ApproveID { get; set; }
		public int? UserRequestId { get; set; }
		public int? UserApproveId { get; set; }
		public DateTime? RequestDate { get; set; }
		public DateTime? ApproveDate { get; set; }
		public DateTime? CancelDate { get; set; }
		public string RequestNote { get; set; }
		public string ApproveNote { get; set; }
		public string CancelNote { get; set; }
		public int? Status { get; set; }
		public int? DataType { get; set; }
		public int? DataStatus { get; set; }
		public string DataStatusStr { get; set; }
		public int? ActionType { get; set; }
		public int? ReferId { get; set; }
		public int? ReferIdTemp { get; set; }
		public string IsCheck { get; set; }
		public int? UserCheckId { get; set; }
		public UserDto UserRequest { get; set; }
		public UserDto UserApprove { get; set; }
		public string Summary { get; set; }
		public string InvestmentProduct { get; set; }
    }
}
