using EPIC.InvestDomain;
using EPIC.InvestEntities.Dto.Calendar;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/calendar")]
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
        [PermissionFilter(Permissions.InvestCauHinhNNL_DanhSach)]
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
        [PermissionFilter(Permissions.InvestCauHinhNNL_DanhSach)]
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
        [PermissionFilter(Permissions.InvestCauHinhNNL_CapNhat, Permissions.InvestCauHinhNNL_DanhSach)]
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
        [PermissionFilter(Permissions.InvestCauHinhNNL_CapNhat)]
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
