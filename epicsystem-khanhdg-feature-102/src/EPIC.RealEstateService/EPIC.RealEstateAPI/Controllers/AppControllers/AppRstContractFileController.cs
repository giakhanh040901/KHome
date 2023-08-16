using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using EPIC.GarnerDomain.Interfaces;
using EPIC.RealEstateDomain.Implements;
using EPIC.GarnerDomain.Implements;
using Microsoft.Extensions.Logging;
using EPIC.RealEstateRepositories;
using EPIC.Utils.Net.MimeTypes;
using System.Threading.Tasks;
using EPIC.RealEstateEntities.Dto.RstOrderContractFile;
using EPIC.Utils.ConstantVariables.RealEstate;

namespace EPIC.RealEstateAPI.Controllers.AppControllers
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/real-estate/contract-file")]
    [ApiController]
    public class AppRstContractFileController : BaseController
    {
        private readonly RstOrderContractFileServices _rstOrderContractFileServices;
        public AppRstContractFileController(ILogger<AppRstContractFileController> logger,
            RstOrderContractFileServices rstOrderContractFileServices)
        {
            _logger = logger;
            _rstOrderContractFileServices = rstOrderContractFileServices;
        }
        /// <summary>
        /// Lấy danh sách mẫu hợp đồng đặt cọc
        /// </summary>
        /// <param name="openSellDetailId">Id chính sách</param>
        /// <param name="contractType">Loại hợp đồng</param>
        /// <returns></returns>
        [HttpGet("find-contract-template")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerContractTemplateAppDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllContractTemplate([Range(1, int.MaxValue)] int openSellDetailId, [Range(1, int.MaxValue)] int? contractType = null)
        {
            try
            {
                var result = _rstOrderContractFileServices.FindAllForApp(openSellDetailId, contractType);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem tạm hợp đồng đặt cọc
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("export-contract")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ExportContract([FromQuery]RstExportContracDto input)
        {
            try
            {
                var result = await _rstOrderContractFileServices.ExportFileContract(input);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// xuất file hợp đồng có chữ đã ký
        /// </summary>
        /// <param name="orderContractFileId">id file trong bảng RST_ORDER_CONTRACT_FILE</param>
        /// <returns></returns>
        [HttpGet("export-contract-temp-pdf/{orderContractFileId}")]
        public IActionResult ExportFileSignature([Range(1, int.MaxValue)] int orderContractFileId)
        {
            try
            {
                var result = _rstOrderContractFileServices.ExportContractTempPdf(orderContractFileId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// get danh sách hợp đồng sau khi đặt lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("get-all-contract-temp/{orderId}")]
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
