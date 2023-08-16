using EPIC.CoreDomain.Interfaces;
using EPIC.Entities.Dto.Partner;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
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
    [Route("api/core")]
    [ApiController]
    public class PartnerController : BaseController
    {
        private readonly IPartnerServices _partnerServices;
        public PartnerController(ILogger<PartnerController> logger, IPartnerServices partnerServices)
        {
            _logger = logger;
            _partnerServices = partnerServices;
        }

        /// <summary>
        /// Lấy danh sách đối tác
        /// </summary>
        [Route("partner/find")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreDoiTac_DanhSach)]
        public APIResponse GetAllPartner(int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _partnerServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        // Api get danh sách partner sử dụng cho dropdown 
        [Route("partner/find-all")]
        [HttpGet]
        public APIResponse GetAll(int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _partnerServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin đối tác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("partner/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreDoiTac_ThongTinChiTiet, Permissions.CoreThongTinDoanhNghiep)]
        public APIResponse GetPartner(int id)
        {
            try
            {
                var result = _partnerServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo thông tin đối tác
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("partner/add")]
        [HttpPost]
        [PermissionFilter(Permissions.CoreDoiTac_Account_ThemMoi)]
        public APIResponse AddPartner([FromBody] CreatePartnerDto body)
        {
            try
            {
                var result = _partnerServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin đối tác
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("partner/update")]
        [HttpPut]
        public APIResponse UpdatePartner(int id, [FromBody] UpdatePartnerDto body)
        {
            try
            {
                var result = _partnerServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá thông tin đối tác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("partner/delete/{id}")]
        [HttpDelete]
        [PermissionFilter(Permissions.CoreDoiTac_Xoa)]
        public APIResponse DeletePartner(int id)
        {
            try
            {
                var result = _partnerServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ĐLSC theo đối tác đang đăng nhập
        /// </summary>
        [Route("partner/find-trading-provider")]
        [HttpGet]
        public APIResponse FindTradingProviderByPartner()
        {
            try
            {
                var result = _partnerServices.FindTradingProviderByPartner();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
