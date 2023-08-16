using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtility;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectUtilityServices
    {
        List<RstProjectUtilityDto> UpdateProjectUtility(CreateRstProjectUtilityDto input);
        void DeleteProjectUtility(int id);
        PagingResult<RstProjectUtilityDto> FindAll(FilterRstProjectUtilityDto input, int projectId);
        List<RstProjectUtilityData> GetAllUtility();
        List<GroupRstProjectUtility> GetAllGroupUtility();
    }
}
