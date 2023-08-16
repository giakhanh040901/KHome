using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEventDetail;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Dto.EvtEventMediaDetail;
using EPIC.EventEntites.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtEventMediaDetailServices
    {
        IEnumerable<EvtEventMediaDetailDto> Add(CreateEvtEventMediaDetailsDto input);
        IEnumerable<EvtEventMediaDetailDto> AddListMediaDetail(AddEvtEventMediaDetailsDto input);
        void ChangeStatus(int id, string status);
        void Delete(int id);
        IEnumerable<EvtEventMediaDto> Find(int eventId, string status);
        EvtEventMediaDetailDto FindById(int id);
        EvtEventMediaDetailDto Update(UpdateEvtEventMediaDetailDto input);
        void UpdateSortOrder(EvtEventMediaDetailSortDto input);
    }
}
