using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectUtilityExtendServices
    {
        List<RstProjectUtilityExtendDto> Add(CreateRstProjectUtilityExtendDto input);
        void Update(UpdateRstProjectUtilityExtendDto input);
        void Delete(int id);
        void ChangeStatus(int id);
        List<RstProjectUtilityExtendDto> GetAll(int projectId);
        RstProjectUtilityExtendDto GetById(int id);
    }  
}
