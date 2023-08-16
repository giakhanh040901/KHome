using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectPolicyServices
    {
        RstProjectPolicy Add(CreateRstProjectPolicyDto input);
        RstProjectPolicy ChangeStatus(int id);
        void Delete(int id);
        PagingResult<RstProjectPolicy> FindAll(FilterRstProjectPolicyDto input);
        RstProjectPolicy FindById(int id);
        RstProjectPolicy Update(UpdateRstProjectPolicyDto input);
    }
}
