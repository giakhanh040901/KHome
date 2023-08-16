using EPIC.DataAccess.Models;
using EPIC.EventDomain.Interfaces;
using EPIC.EventEntites.Dto.EvtEventMedia;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.EventAPI.Controllers.AppControllers
{
    /// <summary>
    /// Màn chi tiết event
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/event/app/event-media")]
    [ApiController]
    public class AppEvtEventMediaController : BaseController
    {
        private readonly IEvtAppEventMediaService _evtAppEventMediaServices;

        public AppEvtEventMediaController(ILogger<AppEvtEventMediaController> logger,
                        IEvtAppEventMediaService evtAppEventMediaServices)
        {
            _logger = logger;
            _evtAppEventMediaServices = evtAppEventMediaServices;
        }

        /// <summary>
        /// hình ảnh của sự kiện 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("find-event-media")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<AppEventMediaDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindEventMediaByEventId([FromQuery] AppFilterEventMediaDto input)
        {
            try
            {
                var result = _evtAppEventMediaServices.FindEventMediaByEventId(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
