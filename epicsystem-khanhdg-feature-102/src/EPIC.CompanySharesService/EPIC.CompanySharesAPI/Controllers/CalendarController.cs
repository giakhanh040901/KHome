using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.Calendar;
using EPIC.Entities.Dto.Issuer;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
    [Route("api/company-shares/calendar")]
    [ApiController]
    public class CalendarController : BaseController
    {
        private readonly ICalendarServices _calendarServices;
        public CalendarController(ILogger<CalendarController> logger, ICalendarServices calendarServices)
        {
            _logger = logger;
            _calendarServices = calendarServices;
        }

        [Route("find/{workingYear}")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondCaiDat_CHNNL_DanhSach)]
        public APIResponse GetAllCalendars(int workingYear)
        {
            try
            {
                var result = _calendarServices.FindAll(workingYear);
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
                var result = _calendarServices.FindByWorkingDate(workingDate);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("add")]
        [HttpPost]
        //[PermissionFilter(Permissions.BondCaiDat_CHNNL_DanhSach, Permissions.BondCaiDat_CHNNL_DanhSach)]
        public APIResponse AddCalendar([FromBody] CreateCalendarDto body)
        {
            try
            {
                var result = _calendarServices.AddByYear(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("update")]
        [HttpPut]
        //[PermissionFilter(Permissions.BondCaiDat_CHNNL_CapNhat)]
        public APIResponse UpdateCalendar([FromBody] UpdateCalendarDto body)
        {
            try
            {
                var result = _calendarServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
