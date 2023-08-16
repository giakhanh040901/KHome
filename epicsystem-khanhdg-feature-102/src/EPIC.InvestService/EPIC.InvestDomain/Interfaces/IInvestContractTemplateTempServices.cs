using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ContractTemplateTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestContractTemplateTempServices
    {
        InvestContractTemplateTempDto Add(CreateInvestContractTemplateTempDto input);
        void Delete(int id);
        InvestContractTemplateTempDto FindById(int id);
        InvestContractTemplateTemp Update(UpdateInvestContractTemplateTempDto input);
        PagingResult<InvestContractTemplateTemp> FindAllContractTemplateTemp(FilterInvestContractTemplateTempDto input);
        List<InvestContractTemplateTempDto> GetAllContractTemplateTemp(int? contractSource);
        InvestContractTemplateTemp ChangeStatus(int id);
    }
}
