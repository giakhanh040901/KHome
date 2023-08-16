using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerBlockadeLiberation;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerBlockadeLiberationServices
    {
        GarnerBlockadeLiberation Add(CreateGarnerBlockadeLiberationDto input);
        List<GarnerBlockadeLiberation> FindAll();
        GarnerBlockadeLiberationDto FindById(int id);
        GarnerBlockadeLiberation Update(UpdateGarnerBlockadeLiberationDto input);
        PagingResult<GarnerOrderMoreInfoDto> FindAllBlockadeLiberation(FilterGarnerOrderDto input, int[] status);
    }
}
