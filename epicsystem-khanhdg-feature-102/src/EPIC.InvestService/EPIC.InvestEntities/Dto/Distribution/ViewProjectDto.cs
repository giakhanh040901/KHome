using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using System;
using System.Collections.Generic;

namespace EPIC.InvestEntities.Dto.Project
{
    public class ViewProjectDto
    {
        public int? Id { get; set; }
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
        public double? Area { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LocationDescription { get; set; }
        public decimal? TotalInvestment { get; set; }
        public int? ProjectType { get; set; }
        public string ProjectProgress { get; set; }
        public string GuaranteeOrganization { get; set; }
        public string IsCheck { get; set; }
        public int? Status { get; set; }
        public List<ProjectJuridicalFileDto> JuridicalFiles { get; set; }
        public ViewOwnerDto Owner { get; set; }
        public ViewGeneralContractorDto GeneralContractor { get; set; }
    }
}
