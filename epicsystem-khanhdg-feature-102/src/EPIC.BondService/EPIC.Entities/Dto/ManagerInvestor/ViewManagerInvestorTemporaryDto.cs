using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
	/// <summary>
	/// Model để cho các investor đang duyệt (temporary)
	/// </summary>
    public class ViewManagerInvestorTemporaryDto : ViewManagerInvestorBaseDto
    {
		/// <summary>
		/// Trạng thái chỉnh sửa Email
		/// 0 Khởi tạo (chưa có bản ghi CoreApprove) 1: Trình duyệt 2: Đã duyệt
		/// </summary>
		public int? StatusEditEmail { get; set; }
		/// <summary>
		/// Trạng thái chỉnh sửa Phone
		/// </summary>
		public int? StatusEditPhone { get; set; }

		/// <summary>
		/// Trạng thái chỉnh sửa thông tin nhà đầu tư
		/// Nếu xem thông tin Thật: 1: Trình duyệt (trong coreApprove hoặc có chỉnh sửa ở Temp nhưng chưa trình duyệt)
		/// </summary>
		public int? StatusEditInvestor { get; set; }

        public string Status { get; set; }
        public string InvestorStatus { get; set; }
		public List<ViewIdentificationDto> ListIdentification { get; set; }

		public List<InvestorBankAccount> ListBank { get; set; }
		public List<InvestorContactAddress> ListContactAddress { get; set; }
        public List<ViewInvestorStockDto> ListStock { get; set; }
        public DataEntities.CoreApprove Approve { get; set; }

    }

}
