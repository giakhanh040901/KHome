using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectJuridicalFile;
using EPIC.InvestEntities.Dto.ProjectType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Project
{
    /// <summary>
    /// Thông tin Dự án của INVEST không chứa Id
    /// </summary>
    public class SCInvestProjectDto
    {
        public string InvCode { get; set; }
        public string InvName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string IsPaymentGuarantee { get; set; }
        public string Area { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string LocationDescription { get; set; }
        public decimal? TotalInvestment { get; set; }
        public decimal? TotalInvestmentDisplay { get; set; }
        public string ProjectProgress { get; set; }
        public string GuaranteeOrganization { get; set; }
    }
}
