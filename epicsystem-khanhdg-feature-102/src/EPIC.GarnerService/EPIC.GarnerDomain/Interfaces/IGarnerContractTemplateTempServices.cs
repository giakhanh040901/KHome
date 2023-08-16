using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using System.Collections.Generic;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerContractTemplateTempServices
    {
        GarnerContractTemplateTemp Add(CreateGarnerContractTemplateTempDto input);
        void Delete(int id);
        List<GarnerContractTemplateTempDto> FindAll(int policyTempId);
        GarnerContractTemplateTempDto FindById(int id);
        GarnerContractTemplateTemp Update(UpdateGarnerContractTemplateTempDto input);
        PagingResult<GarnerContractTemplateTemp> FindAllContractTemplateTemp(FilterGarnerContractTemplateTempDto input);
        List<GarnerContractTemplateTempDto> GetAllContractTemplateTemp(int? contractSource = null);
        GarnerContractTemplateTemp ChangeStatus(int id);
    }
}
