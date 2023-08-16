using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstContractTemplateTempServices
    {
        RstContractTemplateTempDto Add(CreateRstContractTemplateTempDto input);
        void Delete(int id);
        List<RstContractTemplateTempDto> FindAll();
        RstContractTemplateTempDto FindById(int id);
        RstContractTemplateTempDto Update(UpdateRstContractTemplateTempDto input);
        PagingResult<RstContractTemplateTempDto> FindAllContractTemplateTemp(FilterRstContractTemplateTempDto input);
        PagingResult<RstContractTemplateTempDto> GetAllContractTemplateTemp(FilterRstContractTemplateTempDto input);
        List<RstContractTemplateTempDto> GetAllContractTemplateTemp(int? contractSource = null);
        RstContractTemplateTempDto ChangeStatus(int id);
    }
}
