using EPIC.CoreDomain.Interfaces;
using EPIC.Entities.Dto.CoreProductNews;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.CoreAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/core/core-produtch-news")]
    [ApiController]
    public class CoreProductNewsController : BaseController
    {
        private readonly ICoreProductNewsServices _coreProductNewsServices;
        public CoreProductNewsController(ILogger<CoreProductNewsController> logger, ICoreProductNewsServices coreProdutcNewsServices)
        {
            _logger = logger;
            _coreProductNewsServices = coreProdutcNewsServices;
        }

        /// <summary>
        /// tìm tất cả tin tức
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse FindAll(int pageNumber, int? pageSize, string status, int location)
        {
            try
            {
                var result = _coreProductNewsServices.FindAll(pageSize ?? 100, pageNumber, status, location);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tìm kiếm tin tức theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _coreProductNewsServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thêm mới tin tức
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse Add([FromBody] CreateCoreProductNewsDto body)
        {
            try
            {
                var result = _coreProductNewsServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhật lại tin tức
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromBody] UpdateCoreProductNewsDto body)
        {
            try
            {
                var result = _coreProductNewsServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa tin tức theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _coreProductNewsServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thay đổi trạng thái status ẩn, hiện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status")]
        public APIResponse ChangeStatus(int id)
        {
            try
            {
                var result = _coreProductNewsServices.ChangeStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thay đổi trạng thái tin tức có nổi bật hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-feature")]
        public APIResponse ChangeFeature(int id)
        {
            try
            {
                var result = _coreProductNewsServices.ChangeFeature(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
