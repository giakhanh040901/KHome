using EPIC.BondDomain.Interfaces;
using EPIC.FileEntities.Settings;
using EPIC.Shared.Filter;
using EPIC.Utils;
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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers
{
    /// <summary>
    /// Xuất file hợp đồng cho cms
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/export-contract")]
    [ApiController]
    public class BondExportContractController : BaseController
    {
        private readonly IBondContractDataService _contractDataServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;

        public BondExportContractController(
            ILogger<BondExportContractController> logger,
            IBondContractDataService contractDataServices, 
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig)
        {
            _configuration = configuration;
            _logger = logger;
            _contractDataServices = contractDataServices;
            _fileConfig=fileConfig;
        }


        /*[HttpGet("invest-bond")]
        public IActionResult ExportDataInvestBond(int orderId, int contractTemplateId)
        {
            try
            {
                var result = _contractDataServices.exportInvestBondContract(orderId, contractTemplateId, _fileConfig.Value.Path);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }*/

        /// <summary>
        /// Tải file mẫu (bỏ)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <returns></returns>
        [HttpGet("invest-bond")]
        public IActionResult ExportContract(int orderId, int contractTemplateId)
        {
            try
            {
                var result = _contractDataServices.ExportContract(orderId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Tải file lưu trữ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="secondaryContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-scan")]
        public IActionResult ExportFileScan(int orderId, int contractTemplateId, int secondaryContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportFileScanContract(orderId, contractTemplateId, secondaryContractFileId);
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
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="secondaryContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-temp")]
        public IActionResult ExportFileTemp(int orderId, int contractTemplateId, int secondaryContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportContractTemp(orderId, contractTemplateId, secondaryContractFileId);
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
        /// <param name="orderId"></param>
        /// <param name="contractTemplateId"></param>
        /// <param name="secondaryContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-signature")]
        public IActionResult ExportFileSignature(int orderId, int contractTemplateId, int secondaryContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportContractSignature(orderId, contractTemplateId, secondaryContractFileId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        #region Receive File
        /// <summary>
        /// export contract receive
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-contract-receive")]
        [PermissionFilter(Permissions.BondHDPP_GiaoNhanHopDong_XuatHopDong)]
        public async Task<IActionResult> ExportContractReceive([Range(1, int.MaxValue)] int orderId, [Range(1, int.MaxValue)] int bondSecondaryId, [Range(1, int.MaxValue)] int tradingProviderId, [Range(1, 2)] int source)
        {
            try
            {
                var result = await _contractDataServices.ExportContractReceive(orderId, bondSecondaryId, tradingProviderId, source);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
        #endregion

        /// <summary>
        /// export contract temp word test
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

        /// <summary>
        /// export contract receive temp test
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
    }
}
