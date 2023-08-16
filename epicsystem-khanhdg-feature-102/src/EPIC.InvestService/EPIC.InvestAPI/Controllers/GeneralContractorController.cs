using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/general-contractor")]
    [ApiController]
    public class GeneralContractorController : BaseController
    {
        private IGeneralContractorServices _generalContractorServices;
        public GeneralContractorController(ILogger<GeneralContractorController> logger, IGeneralContractorServices generalContractorServices)
        {
            _logger = logger;
            _generalContractorServices = generalContractorServices;
        }

        /// <summary>
        /// Lấy danh sách tổng thầu theo đối tác
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.InvestTongThau_DanhSach)]
        public APIResponse GetAll(int pageNumber, int? pageSize, string keyword, string status)
        {
            try
            {
                var result = _generalContractorServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin chi tiết tổng thầu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [PermissionFilter(Permissions.InvestTongThau_ChiTiet)]
        public APIResponse GetById(int id)
        {
            try
            {
                var result = _generalContractorServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm tổng thầu
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestTongThau_ThemMoi)]
        public APIResponse Add([FromBody] CreateGeneralContractorDto body)
        {
            try
            {
                var result = _generalContractorServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa tổng thầu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [PermissionFilter(Permissions.InvestTongThau_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _generalContractorServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
