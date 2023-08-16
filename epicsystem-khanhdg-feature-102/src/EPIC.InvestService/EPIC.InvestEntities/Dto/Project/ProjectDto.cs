using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using EPIC.InvestEntities.Dto.ProjectType;
using System;
using System.Collections.Generic;

namespace EPIC.InvestEntities.Dto.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public int? PartnerId { get; set; }
        public int? OwnerId { get; set; }
        public int? GeneralContractorId { get; set; }
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public string Content { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Image { get; set; }
        public string IsPaymentGuarantee { get; set; }
        public string Area { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LocationDescription { get; set; }
        public decimal? TotalInvestment { get; set; }
        public decimal? TotalInvestmentDisplay { get; set; }
        public int? ProjectType { get; set; }
        public string ProjectProgress { get; set; }
        public string GuaranteeOrganization { get; set; }
        public string IsCheck { get; set; }
        public int? TradingProviderId { get; set; }
        public int? Status { get; set; }
        public string HasTotalInvestmentSub { get; set; }

        /// <summary>
        /// Khi đại lý được yêu cầu có tổng mức đầu tư, khi đại lý lấy danh sách
        /// </summary>
        public decimal? TotalInvestmentSub { get; set; }
        public List<ProjectJuridicalFileDto> JuridicalFiles { get; set; }
        public List<ProjectImageDto> ProjectImages { get; set; }
        public List<int?> ProjectTypes { get; set; }
        public ViewOwnerDto Owner { get; set; }
        public ViewGeneralContractorDto GeneralContractor { get; set; }
    }
}
