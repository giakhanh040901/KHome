using EPIC.EventEntites.Dto.EvtTicket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtOrderTicketDetailService
    {
        Task<AppQrScanTicketDto> QRScanAdmin(string ticketCode);
    }
}
