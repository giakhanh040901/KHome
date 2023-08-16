using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerReceiveContractTemplateServices
    {
        void Add(CreateGarnerReceiveContractTempDto input);
        void ChangeStatus(int id);
        PagingResult<GarnerReceiveContractTemp> FindByDistributionId(FilterGarnerReceiveContractTemplateDto input);
        GarnerReceiveContractTemp FindById(int id);
        void Update(UpdateGarnerReceiveContractTempDto input);
        void Delete(int id);
    }
}
