using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDetail;
using EPIC.EventEntites.Dto.EvtEventMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtAppEventMediaService
    {
        #region app
        PagingResult<AppEventMediaDto> FindEventMediaByEventId(AppFilterEventMediaDto input);
        #endregion
    }
}
