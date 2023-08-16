using EPIC.RealEstateAPI.Controllers.AppControllers;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstOpenSellFile;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [Route("api/real-estate/order-contract-file")]
    [ApiController]
    public class RstOrderContractFileController : BaseController
    {
        private readonly RstOrderContractFileServices _rstOrderContractFileServices;
        public RstOrderContractFileController(ILogger<RstOrderContractFileController> logger,
            RstOrderContractFileServices rstOrderContractFileServices)
        {
            _logger = logger;
            _rstOrderContractFileServices = rstOrderContractFileServices;
        }

        /// <summary>
        /// get danh sách hợp đồng sau khi đặt lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("get-all-contract-file/{orderId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_HSKHDangKy_DanhSach)]
        public APIResponse GetAllFileTempPdfOrder([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var result = _rstOrderContractFileServices.GetAllFileTempPdfOrder(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return (OkException(ex));
            }
        }
    }
}
