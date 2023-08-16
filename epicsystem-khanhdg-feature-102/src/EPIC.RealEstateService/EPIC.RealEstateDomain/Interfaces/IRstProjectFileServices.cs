using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectFileServices
    {
        List<RstProjectFile> Add(CreateRstProjectFilesDto input);
        RstProjectFile ChangeStatus(int id, string status);
        void Delete(int id);
        PagingResult<RstProjectFile> FindAll(FilterRstProjectFileDto input);
        RstProjectFile FindById(int id);
        RstProjectFile Update(UpdateRstProjectFileDto input);
    }
}
