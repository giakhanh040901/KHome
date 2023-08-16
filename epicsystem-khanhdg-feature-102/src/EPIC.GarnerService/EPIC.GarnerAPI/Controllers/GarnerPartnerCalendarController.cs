using EPIC.Entities.Dto.PartnerCalendar;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.Calendar;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/partner-calendar")]
    [ApiController]
    public class GarnerPartnerCalendarController : BaseController
    {
        private readonly IGarnerPartnerCalendarServices _partnerCalendarServices;
        public GarnerPartnerCalendarController(IGarnerPartnerCalendarServices partnerCalendarServices)
        {
            _partnerCalendarServices = partnerCalendarServices;
        }

        /// <summary>
        /// Tìm theo năm
        /// </summary>
        /// <param name="workingYear"></param>
        /// <returns></returns>
        [Route("find/{workingYear}")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondCaiDat_CHNNL_DanhSach)]
        public APIResponse GetAllCalendars(int workingYear)
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
                var result = _partnerCalendarServices.FindByWorkingDate(workingDate);
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
        //[PermissionFilter(Permissions.BondCaiDat_CHNNL_DanhSach, Permissions.BondCaiDat_CHNNL_DanhSach)]
        public APIResponse AddCalendar([FromBody] CreatePartnerCalendarDto body)
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

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        //[PermissionFilter(Permissions.BondCaiDat_CHNNL_CapNhat)]
        public APIResponse UpdateCalendar([FromBody] UpdateCalendarDto input)
        {
            try
            {
                _partnerCalendarServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
