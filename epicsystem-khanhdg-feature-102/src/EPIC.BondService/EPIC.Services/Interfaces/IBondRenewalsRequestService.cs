using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.RenewalsRequest;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.CoreApprove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondRenewalsRequestService
    {
        PagingResult<ViewRenewalsRequestDto> Find(FilterRenewalsRequestDto dto);
        BondRenewalsRequest AddRequest(CreateRenewalsRequestDto input);
        BondRenewalsRequest AppRenewalsRequest(AppCreateRenewalsRequestDto input);
        void ApproveRequest(ApproveStatusDto input);
        void CancelRequest(CancelStatusDto input);
    }
}
