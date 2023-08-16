using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class InvestorApprove : IFullAudited
    {

		[Column(Name = "ID")]
		public int Id { get; set; }
		[Column(Name = "INVESTOR_ID")]
		public int InvestorId { get; set; }
		[Column(Name = "STATUS")]
		public int Status { get; set; }
		[Column(Name = "APPROVED_BY")]
		public string ApprovedBy { get; set; }
		[Column(Name = "APPROVED_DATE")]
		public DateTime ApprovedDate { get; set; }
		[Column(Name = "CREATED_BY")]
		public string CreatedBy { get; set; }
		[Column(Name = "CREATED_DATE")]
		public DateTime? CreatedDate { get; set; }
		[Column(Name = "MODIFIED_BY")]
		public string ModifiedBy { get; set; }
		[Column(Name = "MODIFIED_DATE")]
		public DateTime? ModifiedDate { get; set; }
		[Column(Name = "DELETED")]
		public string Deleted { get; set; }
	}
}
