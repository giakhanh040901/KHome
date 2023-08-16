using EPIC.BondEntities.Dto.BondSecondary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondInfoOverviewService
    {
        void UpdateOverviewContent(UpdateBondInfoOverviewContentDto input);
        BondSecondaryOverviewDto FindOverViewById(int bondSecondaryid);
    }
}
