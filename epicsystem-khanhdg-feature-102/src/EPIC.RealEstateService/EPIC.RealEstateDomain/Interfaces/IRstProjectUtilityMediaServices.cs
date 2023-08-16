using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityExtend;
using EPIC.RealEstateEntities.Dto.RstProjectUtilityMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectUtilityMediaServices
    {
        RstProjectUtilityMediaDto Add(CreateRstProjectUtilityMediaDto input);
        RstProjectUtilityMediaDto Update(UpdateRstProjectUtilityMediaDto input);
        void Delete(int id);
        void ChangeStatus(int id);
        List<RstProjectUtilityMediaDto> GetAll(int projectId);
        RstProjectUtilityMediaDto GetById(int id);
    }
}
