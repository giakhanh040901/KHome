using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtEventDescriptionMediaServices
    {
        IEnumerable<EvtEventDescriptionMediaDto> Add(CreateEvtEventDescriptionDto input);
        void Delete(int id);
        IEnumerable<EvtEventDescriptionMediaDto> FindByEventId(int eventId);
        void Update(UpdateEvtEventDescriptionMediaDto input);
    }
}
