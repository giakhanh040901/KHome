using EPIC.CoreEntities.Dto.CollabContractTemp;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CollabContractTemp;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ICollabContractServices
    {
        CollabContractTemp Add(CreateCollabContractTempDto body);
        int Delete(int id);
        int Update(UpdateCollabContractTempDto body);
        PagingResult<ViewCollabContractTempDto> FindAll(FilterCollabContractTempDto input);
        ViewCollabContractTempDto FindById(int id);
        PagingResult<ViewCollabContractTempSaleDto> FindAllBySale(int pageSize, int pageNumber, int saleId, int? tradingProvider);
    }
}
