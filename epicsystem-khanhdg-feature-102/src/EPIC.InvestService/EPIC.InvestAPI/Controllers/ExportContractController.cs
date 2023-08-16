using EPIC.FileEntities.Settings;
using EPIC.InvestDomain.Interfaces;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EPIC.InvestAPI.Controllers
{
    /// <summary>
    /// Xuất file hợp đồng cho cms
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/export-contract")]
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
                var result = _contractDataServices.ExportFileScanContract(orderContractFileId);
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
                var result = _contractDataServices.ExportContractTemp(orderContractFileId);
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
        [AllowAnonymous]
        public IActionResult ExportFileTempPdf(int orderContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportContractTempPdf(orderContractFileId);
                return File(result.fileData, MimeTypeNames.ApplicationPdf, result.fileDownloadName);
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
        public IActionResult ExportFileSignature(int orderContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportContractSignature(orderContractFileId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// export contract receive
        /// </summary>
        /// <returns></returns>
        [HttpGet("file-receive")]
        public async Task<IActionResult> ExportContractReceive([Range(1, int.MaxValue)] int orderId, [Range(1, int.MaxValue)] int distributionId, [Range(1, int.MaxValue)] int tradingProviderId, [Range(1, 2)] int source)
        {
            try
            {
                var result = await _contractDataServices.ExportContractReceive(orderId, distributionId, tradingProviderId, source);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// export contract temp  word test
        /// </summary>
        /// <param name="contractTemplateId"></param>
        /// <param name="type">Hợp đồng dành cho cá nhân hay doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet("file-template-word")]
        public IActionResult ExportContractWordTest([Range(1, int.MaxValue)] int contractTemplateId, string type)
        {
            try
            {
                var result = _contractDataServices.ExportContractWordTest(contractTemplateId, type);
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
        /// <param name="contractTemplateId"></param>
        /// <param name="type">Hợp đồng dành cho cá nhân hay doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet("file-template-pdf")]
        public async Task<IActionResult> ExportContractPdfTest([Range(1, int.MaxValue)] int contractTemplateId, string type)
        {
            try
            {
                var result = await _contractDataServices.ExportContractPdfTest(contractTemplateId, type);
                return File(result.fileData, MimeTypeNames.ApplicationPdf, result.fileDownloadName);
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
        [HttpGet("file-receive-template-pdf")]
        public async Task<IActionResult> ExportContractReceivePdfTest([Range(1, int.MaxValue)] int tradingProviderId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = await _contractDataServices.ExportContractReceivePdfTest(tradingProviderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Fill data vào hợp đồng word (Mẫu hợp đồng cài đặt)
        /// </summary>
        /// <param name="contractTemplateTempId"></param>
        /// <param name="type">Hợp đồng dành cho cá nhân hay doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet("file-template-temp-word")]
        public IActionResult ExportContractTempWordTest([Range(1, int.MaxValue)] int contractTemplateTempId, string type)
        {
            try
            {
                var result = _contractDataServices.ExportContractTempWordTest(contractTemplateTempId, type);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Fill data vào hợp đồng pdf (Mẫu hợp đồng cài đặt)
        /// </summary>
        /// <param name="contractTemplateTempId">Id mẫu hợp đồng mẫu</param>
        /// <param name="type">Hợp đồng dành cho cá nhân hay doanh nghiệp</param>
        /// <returns></returns>
        [HttpGet("file-template-temp-pdf")]
        public async Task<IActionResult> ExportContractTempPdfTest([Range(1, int.MaxValue)] int contractTemplateTempId, string type)
        {
            try
            {
                var result = await _contractDataServices.ExportContractTempPdfTest(contractTemplateTempId, type);
                return File(result.fileData, MimeTypeNames.ApplicationPdf, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
