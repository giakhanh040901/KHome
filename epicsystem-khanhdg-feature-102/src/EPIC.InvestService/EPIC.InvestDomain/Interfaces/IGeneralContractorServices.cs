using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.GeneralContractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IGeneralContractorServices
    {
        PagingResult<ViewGeneralContractorDto> FindAll(int pageSize, int pageNumber, string keyword, string status);
        ViewGeneralContractorDto FindById(int id);
        GeneralContractor Add(CreateGeneralContractorDto input);
        int Delete(int id);
    }
}
