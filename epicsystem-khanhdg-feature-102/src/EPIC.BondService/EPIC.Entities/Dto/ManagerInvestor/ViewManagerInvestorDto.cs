using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
	/// <summary>
	/// Model dùng cho Investor thật
	/// </summary>
    public class ViewManagerInvestorDto : ViewManagerInvestorBaseDto
    {
		public string Status { get; set; }
		public List<ViewIdentificationDto> ListIdentification { get; set; }
		public List<InvestorBankAccount> ListBank { get; set; }
		public List<InvestorContactAddress> ListContactAddress { get; set; }
        public List<ViewInvestorStockDto> ListStock { get; set; }
        public DataEntities.CoreApprove Approve { get; set; }


    }
}
