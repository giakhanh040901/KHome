using EPIC.GarnerDomain.Interfaces;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/export-contract")]
    [ApiController]
    public class ExportContractController : BaseController
    {
        private readonly IGarnerContractDataServices _garnerContractDataServices;

        public ExportContractController(ILogger<ExportContractController> logger,
            IGarnerContractDataServices garnerContractDataServices)
        {
            _logger = logger;
            _garnerContractDataServices = garnerContractDataServices;
        }

        /// <summary>
        /// Tải file lưu trữ
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-scan")]
        public IActionResult ExportFileScan(int orderContractFileId)
        {
            try
            {
                var result = _garnerContractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.SCAN);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file mẫu
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-temp")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM)]
        public IActionResult ExportFileTemp(int orderContractFileId)
        {
            try
            {
                var result = _garnerContractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.TEMP);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file mẫu
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-temp-pdf")]
        public IActionResult ExportFileTempPdf(int orderContractFileId)
        {
            try
            {
                var result = _garnerContractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.PDF);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file đã ký
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-signature")]
        [PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT)]
        public IActionResult ExportFileSignature(int orderContractFileId)
        {
            try
            {
                var result = _garnerContractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.SIGNATURE);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file hợp đồng giao nhận
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("file-receive")]
        public async Task<IActionResult> ExportFileReceive(long orderId)
        {
            try
            {
                var result = await _garnerContractDataServices.ExportContractReceive(orderId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Test file hợp đồng giao nhận
        /// </summary>
        /// <returns></returns>
        [HttpGet("file-receive-template-pdf")]
        public async Task<IActionResult> ExportContractReceivePdfTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = await _garnerContractDataServices.ExportContractReceivePdfTest(tradingProviderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

    }
}
