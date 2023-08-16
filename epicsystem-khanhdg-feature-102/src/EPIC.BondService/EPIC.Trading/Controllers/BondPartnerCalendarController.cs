using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.PartnerCalendar;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/partner-calendar")]
    [ApiController]
    public class BondPartnerCalendarController : BaseController
    {
        private readonly IBondPartnerCalendarService _partnerCalendarServices;
        public BondPartnerCalendarController(ILogger<BondPartnerCalendarController> logger, IBondPartnerCalendarService partnerCalendarServices)
        {
            _logger = logger;
            _partnerCalendarServices = partnerCalendarServices;
        }

        [Route("find/{workingYear}")]
        [HttpGet]
        [PermissionFilter(Permissions.BondCaiDat_CHNNL_DanhSach)]
        public APIResponse GetAllPartnerCalendars(int workingYear)
        {
            try
            {
                var result = _partnerCalendarServices.FindAll(workingYear);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("{workingDate}")]
        [HttpGet]
        public APIResponse GetByWorkingDate(DateTime workingDate)
        {
            try
            {
                var result = _partnerCalendarServices.FindByWorkingDate(workingDate);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.BondCaiDat_CHNNL_CapNhat, Permissions.BondCaiDat_CHNNL_DanhSach)]
        public APIResponse AddPartnerCalendar([FromBody] CreatePartnerCalendarDto body)
        {
            try
            {
                var result = _partnerCalendarServices.AddByYear(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.BondCaiDat_CHNNL_CapNhat)]
        public APIResponse UpdateCalendar([FromBody] UpdatePartnerCalendarDto body)
        {
            try
            {
                var result = _partnerCalendarServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
