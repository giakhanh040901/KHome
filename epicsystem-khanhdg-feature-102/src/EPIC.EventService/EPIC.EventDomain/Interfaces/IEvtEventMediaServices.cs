using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.EventEntites.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtEventMediaServices
    {
        List<ViewEvtEventMediaDto> Add(CreateEvtEventMediasDto input);
        void ChangeStatus(int id, string status);
        void Delete(int id);
        IEnumerable<ViewEvtEventMediaDto> Find(int eventId, string location, string status);
        PagingResult<ViewEvtEventMediaDto> FindAll(FilterEvtEventMediaDto input);
        ViewEvtEventMediaDto FindById(int id);
        ViewEvtEventMediaDto Update(UpdateEvtEventMediaDto input);
        void UpdateSortOrder(EvtEventMediaSortDto input);
    }
}
