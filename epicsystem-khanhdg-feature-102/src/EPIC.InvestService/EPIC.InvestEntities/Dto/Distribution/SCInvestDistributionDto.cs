using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectOverviewOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    /// <summary>
    /// Bán phân phối của Invest không chứa Id
    /// </summary>
    public class SCInvestDistributionDto
    {
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
    }
}
