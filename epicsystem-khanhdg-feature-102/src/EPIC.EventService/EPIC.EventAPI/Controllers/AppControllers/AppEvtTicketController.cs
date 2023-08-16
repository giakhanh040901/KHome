using EPIC.EventDomain.Implements;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.EventAPI.Controllers.AppControllers
{
    /// <summary>
    /// Quét mã QR cho vé
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/event/app/ticket")]
    [ApiController]
    public class AppEvtTicketController : BaseController
    {
        private readonly IEvtOrderTicketDetailService _evtOrderTicketDetailServices;

        public AppEvtTicketController(ILogger<AppEvtTicketController> logger,
                        IEvtOrderTicketDetailService evtOrderTicketDetailServices)
        {
            _logger = logger;
            _evtOrderTicketDetailServices = evtOrderTicketDetailServices;
        }

        /// <summary>
        /// Admin quét qr của vé
        /// </summary>
        /// <param name="ticketCode"></param>
        /// <returns></returns>
        [HttpPut("scan/{ticketCode}")]
        [ProducesResponseType(typeof(APIResponse<AppQrScanTicketDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> QRScanAdmin(string ticketCode)
        {
            try
            {
                var result = await _evtOrderTicketDetailServices.QRScanAdmin(ticketCode);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
