using EPIC.GarnerDomain.Interfaces;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using EPIC.Shared.Filter;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.WebAPIBase.FIlters;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/calendar")]
    [ApiController]
    public class CalendarController : BaseController
    {
        private readonly IGarnerCalendarServices _calendarServices;
        public CalendarController(IGarnerCalendarServices calendarServices)
        {
            _calendarServices = calendarServices;
        }

        /// <summary>
        /// Tìm theo năm
        /// </summary>
        /// <param name="workingYear"></param>
        /// <returns></returns>
        [Route("find/{workingYear}")]
        [HttpGet]
        [PermissionFilter(Permissions.GarnerCauHinhNNL_DanhSach)]
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

        /// <summary>
        /// Tìm theo ngày
        /// </summary>
        /// <param name="workingDate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        [PermissionFilter(Permissions.GarnerCauHinhNNL_CapNhat, Permissions.GarnerCauHinhNNL_DanhSach)]
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

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.GarnerCauHinhNNL_CapNhat)]
        public APIResponse UpdateCalendar([FromBody] UpdateCalendarDto body)
        {
            try
            {
                 _calendarServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
