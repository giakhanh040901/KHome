using EPIC.CoreDomain.Interfaces;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using EPIC.Utils.SharedApiService.Dto.SignPdfDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers
{
    /// <summary>
    /// Sale đăng ký tạo investor
    /// </summary>
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/sign-pdf")]
    [ApiController]
    public class SignPdfController : BaseController
    {
        private readonly ISignPdfServices _signPdfServices;
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="signPdfServices"></param>
        public SignPdfController(ISignPdfServices signPdfServices)
        {
            _signPdfServices = signPdfServices;
        }

        /// <summary>
        /// xuất file hợp đồng có chữ đã ký điện tử
        /// </summary>
        /// <param name="dto">id file </param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ExportFileSignature([FromBody] SignPdfDto dto)
        {
            try
            {
                var result = _signPdfServices.SignPdf(dto);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    }
}
