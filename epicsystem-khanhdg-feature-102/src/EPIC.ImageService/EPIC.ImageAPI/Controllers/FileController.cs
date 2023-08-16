using EPIC.FileDomain.Services;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.Utils.Net.MimeTypes;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EPIC.ImageAPI.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : BaseController
    {
        private readonly IFileServices _imageServices;
        public FileController(IFileServices imageServices)
        {
            _imageServices = imageServices;
        }

        /// <summary>
        /// Get File
        /// </summary>
        /// <returns></returns>
        [Route("get")]
        [HttpGet]
        public IActionResult GetFile([FromQuery] string folder, [FromQuery] string file, [FromQuery] bool download)
        {
            try
            {
                var result = _imageServices.GetFile(folder, file);

                if (download)
                {
                    return File(result, MimeTypeNames.ApplicationOctetStream, file);
                }
                return FileByFormat(result, file);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <returns></returns>
        [Route("upload")]
        [DisableRequestSizeLimit]
        [HttpPost]
        [Authorize]
        public APIResponse UploadFile(IFormFile file, [FromQuery] string folder)
        {
            try
            {
                var result = _imageServices.UploadFile(new Models.UploadFileModel
                {
                    File = file,
                    Folder = folder,
                });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
