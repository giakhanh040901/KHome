using EPIC.GarnerDomain.Interfaces;
using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateEntities.Dto.RstOrderContractFile;
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

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/export-contract")]
    [ApiController]
    public class RstExportContractController : BaseController
    {
        private readonly RstOrderContractFileServices _rstOrderContractFileServices;

        public RstExportContractController(ILogger<RstExportContractController> logger,
            RstOrderContractFileServices rstOrderContractFileServices)
        {
            _logger = logger;
            _rstOrderContractFileServices = rstOrderContractFileServices;
        }   

        /// <summary>
        /// Tải file lưu trữ
        /// </summary>
        /// <param name="orderContractFileId"></param>
        /// <returns></returns>
        [HttpGet("file-scan/{orderContractFileId}")]
        public IActionResult ExportFileScan(int orderContractFileId)
        {
            try
            {
                var result = _rstOrderContractFileServices.ExportContract(new RstExportOrderContractFileDto() { Id = orderContractFileId, ContractType = ContractFileTypes.SCAN });
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
        [HttpGet("file-temp/{orderContractFileId}")]
        //[PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSM)]
        public IActionResult ExportFileTemp(int orderContractFileId)
        {
            try
            {
                var result = _rstOrderContractFileServices.ExportContract(new RstExportOrderContractFileDto() { Id = orderContractFileId, ContractType = ContractFileTypes.TEMP });
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
        [HttpGet("file-temp-pdf/{orderContractFileId}")]
        public IActionResult ExportFileTempPdf(int orderContractFileId)
        {
            try
            {
                var result = _rstOrderContractFileServices.ExportContract(new RstExportOrderContractFileDto() { Id = orderContractFileId, ContractType = ContractFileTypes.PDF });
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
        [HttpGet("file-signature/{orderContractFileId}")]
        //[PermissionFilter(Permissions.GarnerHDPP_SoLenh_TTCT_HSKHDangKy_TaiHSCKDT)]
        public IActionResult ExportFileSignature(int orderContractFileId)
        {
            try
            {
                var result = _rstOrderContractFileServices.ExportContract(new RstExportOrderContractFileDto() { Id = orderContractFileId, ContractType = ContractFileTypes.SIGNATURE });
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
