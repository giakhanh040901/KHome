using EPIC.CompanySharesDomain.Interfaces;
using EPIC.FileEntities.Settings;
using EPIC.Utils.ConstantVariables.CompanyShares;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EPIC.CompanySharesAPI.Controllers
{
    /// <summary>
    /// Xuất file hợp đồng cho cms
    /// </summary>
    [Authorize]
    [Route("api/company-shares/export-contract")]
    [ApiController]
    public class ExportContractController : BaseController
    {
        private readonly IContractDataServices _contractDataServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;

        public ExportContractController(
            ILogger<ExportContractController> logger,
            IContractDataServices contractDataServices,
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig)
        {
            _configuration = configuration;
            _logger = logger;
            _contractDataServices = contractDataServices;
            _fileConfig = fileConfig;
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
                var result = _contractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.SCAN);
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

        public IActionResult ExportFileTemp(int orderContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.TEMP);
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
                var result = _contractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.PDF);
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
        //[PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT)]

        public IActionResult ExportFileSignature(int orderContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.SIGNATURE);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        ///// <summary>
        ///// export contract receive
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("file-receive")]
        ////[PermissionFilter(Permissions.InvestHDPP_GiaoNhanHopDong_XuatHopDong)]

        //public async Task<IActionResult> ExportContractReceive([Range(1, int.MaxValue)] int orderId, [Range(1, int.MaxValue)] int distributionId, [Range(1, int.MaxValue)] int tradingProviderId, [Range(1, 2)] int source)
        //{
        //    try
        //    {
        //        var result = await _contractDataServices.ExportContractReceive(orderId, distributionId, tradingProviderId, source);
        //        return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(OkException(ex));
        //    }
        //}

        /// <summary>
        /// export contract temp  word test
        /// </summary>
        /// <returns></returns>
        [HttpGet("file-template-word")]
        public IActionResult ExportContractWordTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = _contractDataServices.ExportContractWordTest(tradingProviderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// export contract temp pdf test
        /// </summary>
        /// <returns></returns>
        [HttpGet("file-template-pdf")]
        public async Task<IActionResult> ExportContractPdfTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = await _contractDataServices.ExportContractPdfTest(tradingProviderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        ///// <summary>
        ///// export contract temp pdf test
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("file-receive-template-pdf")]
        //public async Task<IActionResult> ExportContractReceivePdfTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        //{
        //    try
        //    {
        //        var result = await _contractDataServices.ExportContractReceivePdfTest(tradingProviderId, contractTemplateId);
        //        return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(OkException(ex));
        //    }
        //}
    }
}
