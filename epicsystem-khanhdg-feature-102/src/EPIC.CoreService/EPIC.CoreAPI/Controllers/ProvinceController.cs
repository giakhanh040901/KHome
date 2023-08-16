using EPIC.CoreDomain.Interfaces;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    //[AuthorizeAdminUserTypeFilter]
    [Route("api/core")]
    [ApiController]
    public class ProvinceController : BaseController
    {
        private readonly IProvinceServices _provinceService;
        public ProvinceController(IProvinceServices provinceServices)
        {
            _provinceService = provinceServices;
        }

        /// <summary>
        /// Danh sách tỉnh thành
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("province")]
        public APIResponse GetListProvince(string keyword)
        {
            try
            {
                var result = _provinceService.GetListProvince(keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách quận huyện
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("district")]
        public APIResponse GetListDistrict(string keyword, string provinceCode)
        {
            try
            {
                var result = _provinceService.GetListDistrict(keyword, provinceCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách xã phường
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="districtCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ward")]
        public APIResponse GetListWard(string keyword, string districtCode)
        {
            try
            {
                var result = _provinceService.GetListWard(keyword, districtCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
