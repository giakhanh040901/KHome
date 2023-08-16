using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestEntities.Dto.Policy;
using System;
using System.Collections.Generic;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectOverviewOrg;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class ViewDistributionDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int TradingProviderId { get; set; }
        public int? BusinessCustomerBankAccId { get; set; }
        public int Status { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
        public string IsClose { get; set; }
        public string IsShowApp { get; set; }
        public string IsCheck { get; set; }
        public string InvName { get; set; }
        public decimal? HanMucToiDa { get; set; }
        public string Image { get; set; }
        /// <summary>
        /// Số tiền đã đầu tư
        /// </summary>
        public decimal? IsInvested { get; set; }
        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn (1: có chi tiền, 2: không chi tiền)
        /// </summary>
        public int MethodInterest { get; set; }
        
        /// <summary>
        /// có phải bán hộ hay không
        /// </summary>
        public bool IsSalePartnership { get; set; }
        public List<ViewPolicyDto> Policies { get; set; }
        public ProjectDto Project { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
        public BusinessCustomerBankDto BusinessCustomerBank { get; set; }
        //Danh sách ngân hàng của trading 
        public List<DistributionTradingBankAccount> TradingBankAccouts { get; set; }
        public List<int> TradingBankAcc { get; set; }
        public List<int> TradingBankAccPays { get; set; }
        public List<BusinessCustomerBankDto> ListBusinessCustomerBanks { get; set; }
        public List<ViewProjectOverViewFileDto> ProjectOverviewFiles { get; set; }
        public List<ViewProjectOverviewOrgDto> ProjectOverviewOrgs { get; set; }
    }
}
