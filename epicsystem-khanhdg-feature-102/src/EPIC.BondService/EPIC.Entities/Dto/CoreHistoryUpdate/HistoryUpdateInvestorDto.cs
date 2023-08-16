using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreHistoryUpdate
{
    public class HistoryUpdateInvestorDto
    {
		public string Phone { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string TaxCode { get; set; }
		public string ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string Status { get; set; }
		public int TradingProviderId { get; set; }
		public string FaceImageUrl { get; set; }
		public string AvatarImageUrl { get; set; }
		public string ReferralCode { get; set; }
		public string ReferralCodeSelf { get; set; }
        public DateTime? ReferralDate { get; set; }
	}
}
