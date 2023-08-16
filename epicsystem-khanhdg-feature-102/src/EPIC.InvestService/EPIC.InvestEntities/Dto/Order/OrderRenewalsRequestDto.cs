using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.Investor;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.Project;
using EPIC.Utils.ConstantVariables.Invest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class OrderRenewalsRequestDto
    {
        public int Id { get; set; }
        public long OrderId { get; set; }
        public int PolicyId { get; set; }
        public string CustomerName { get; set; }

        /// <summary>
        /// Id của hợp đồng mới sau khi tái tục thành công
        /// </summary>
        public long OrderNewId { get; set; }
        public int SettlementMethod { get; set; }
        public int RenewalsPolicyDetailId { get; set; }
        public string ContractCode { get; set; }

        public string ContractCodeOriginal { get; set; }

        /// <summary>
        /// Số tiền tái tục
        /// </summary>
        public decimal? RenewalMoney { get; set; }

        /// <summary>
        /// Người duyệt tái tục
        /// </summary>
        public string ApproveRenewalBy { get; set; }

        /// <summary>
        /// Ngày duyệt tái tục
        /// </summary>
        public DateTime? ApproveRenewalDate { get; set; }
        /// <summary>
        /// Mã hợp đồng trong file
        /// </summary>
        public string GenContractCode { get; set; }
        /// <summary>
        /// Loại hợp đồng tái tục của chính sách: 1: Tạo hợp đồng mới, 2 giữ hợp đồng cũ
        /// </summary>
        public int RenewalsType { get; set; }
        public ProjectDto Project { get; set; }
        public InvestorDto Investor { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public DateTime? RequestDate { get; set; }
        public PolicyDetailDto PolicyDetail { get; set; }
        public PolicyDto Policy { get; set; }
        public int Status { get; set; }
        public int? OrderSource { get; set; }
        public int? Source { get; set; }
    }
}
