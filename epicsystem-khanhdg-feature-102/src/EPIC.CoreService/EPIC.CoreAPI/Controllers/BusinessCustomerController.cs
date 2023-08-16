using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DigitalSign;
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
    [Route("api/core/business-customer")]
    [ApiController]
    public class BusinessCustomerController : BaseController
    {
        private IBusinessCustomerServices _businessCustomerServices;

        public BusinessCustomerController(ILogger<BusinessCustomerController> logger, IBusinessCustomerServices businessCustomerServices)
        {
            _logger = logger;
            _businessCustomerServices = businessCustomerServices;
        }

        [Route("findTaxCode/{taxCode}")]
        [HttpGet]
        public APIResponse FindBC(string taxCode)
        {
            try
            {
                var result = _businessCustomerServices.FindBusinessCustomerByTaxCode(taxCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách duyệt thông tin doanh nghiệp
        /// </summary>
        [Route("approve/find")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreMenuDuyetKHDN, Permissions.CoreThongTinDoanhNghiep)]
        public APIResponse GetAllApprove([FromQuery]FilterBusinessCustomerTempDto input)
        {
            try
            {
                var result = _businessCustomerServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin duyệt doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("approve/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreDuyetKHDN_ThongTinKhachHang, Permissions.CoreQLPD_KHDN_ThongTinChiTiet)]
        public APIResponse GetApprove(int id)
        {
            try
            {
                var result = _businessCustomerServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo thông tin duyệt doanh nghiệp
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("approve/add")]
        [HttpPost]
        [PermissionFilter(Permissions.CoreDuyetKHDN_ThemMoi)]
        public APIResponse AddApprove([FromBody] CreateBusinessCustomerTempDto body)
        {
            try
            {
                var result = _businessCustomerServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin duyệt doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("approve/update/{id}")]
        [HttpPut]
        [PermissionFilter(Permissions.CoreDuyetKHDN_CapNhat, Permissions.CoreTTDN_CapNhat)]
        public APIResponse UpdateApprove(int id, [FromBody] UpdateBusinessCustomerTempDto body)
        {
            try
            {
                var result = _businessCustomerServices.Update(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật duyệt doanh nghiệp ở bảng chính
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        [PermissionFilter(Permissions.CoreKHDN_CapNhat)]
        public APIResponse Update([FromBody] CreateBusinessCustomerTempDto body)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerUpdate(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("request")]
        [PermissionFilter(Permissions.CoreDuyetKHDN_TrinhDuyet)]
        public APIResponse AddBusinessCustomerRequest([FromBody] RequestBusinessCustomerDto input)
        {
            try
            {
                _businessCustomerServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt doanh nghiệp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve")]
        [PermissionFilter(Permissions.CoreQLPD_KHDN_PheDuyetOrHuy)]
        public APIResponse Approve(ApproveBusinessCustomerDto input)
        {
            try
            {
                var result = _businessCustomerServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Epic xác minh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("check")]
        [PermissionFilter(Permissions.CoreKHDN_XacMinh)]
        public APIResponse Check([FromBody] CheckBusinessCustomerDto input)
        {
            try
            {
                _businessCustomerServices.Check(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt trong trạng thái trình duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel")]
        [PermissionFilter(Permissions.CoreQLPD_KHDN_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] CancelBusinessCustomerDto input)
        {
            try
            {
                _businessCustomerServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách thông tin doanh nghiệp, keyword tìm theo mã số thuế
        /// </summary>
        [Route("find")]
        [HttpGet]
        public APIResponse GetAllByTaxCode(int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _businessCustomerServices.FindAllBusinessCustomerByTaxCode(pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find-all")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreKHDN_DanhSach)]
        public APIResponse GetAll([FromQuery]FilterBusinessCustomerDto input)
        {
            try
            {
                var result = _businessCustomerServices.FindAllBusinessCustomer(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreKHDN_ThongTinKhachHang, Permissions.CoreThongTinDoanhNghiep)]
        public APIResponse Get(int id)
        {
            try
            {
                var result = _businessCustomerServices.FindBusinessCustomerById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("find-bond/{id}")]
        [HttpGet]
        public APIResponse GetBond(int id)
        {
            try
            {
                var result = _businessCustomerServices.FindBusinessCustomerById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách duyệt thông tin doanh nghiệp
        /// </summary>
        [Route("bank/find")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreKHDN_TKNH, Permissions.CoreTTDN_TKNganHang, Permissions.CoreTTDN_TKNganHang)]
        public APIResponse GetAllBusinessCustomerBank(int businessCustomerId, int pageNumber, int? pageSize, string keyword)
        {
            try
            {
                var result = _businessCustomerServices.FindAllBusinessCusBank(businessCustomerId, pageSize ?? 100, pageNumber, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("bank/{id}")]
        [HttpGet]
        public APIResponse GetBusinessCustomerBank(int id)
        {
            try
            {
                var result = _businessCustomerServices.FindBusinessCusBankById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo thông tin ngân hàng
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("bank/add")]
        [HttpPost]
        [PermissionFilter(Permissions.CoreDuyetKHDN_TKNH_ThemMoi, Permissions.CoreKHDN_TKNH_ThemMoi)]
        public APIResponse AddBusinessCustomerBank([FromBody] CreateBusinessCustomerBankTempDto body)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerBankAdd(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update thông tin ngân hàng ở bảng chính
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("bank/update/{id}")]
        [HttpPut]
        public APIResponse UpdateBusinessCustomerBank(int id, [FromBody] UpdateBusinessCustomerBankTempDto body)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerBankUpdate(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [Route("bank-temp/update/{id}")]
        [HttpPut]
        public APIResponse UpdateBusinessCustomerBankTemp(int id, [FromBody] UpdateBusinessCustomerBankTempDto body)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerBankTempUpdate(id, body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("bank/active/{id}")]
        public APIResponse ActiveBusinessCustomerBank([FromRoute] int id, bool isActive)
        {
            try
            {
                var result = _businessCustomerServices.ActiveBusinessCustomerBank(id, isActive);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        [Route("delete/{id}")]
        [HttpDelete]
        public APIResponse BusinessCustomerDelete(int id)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerDelete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá ngân hàng của doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("bank/delete/{id}")]
        [HttpDelete]
        public APIResponse BusinessCustomerBankDelete(int id)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerBankDelete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi ngân hàng mặc định
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("bank/is-default")]
        [HttpPut]
        [PermissionFilter(Permissions.CoreDuyetKHDN_TKNH, Permissions.CoreKHDN_TKNH_SetDefault, Permissions.CoreTTDN_TKNH_SetDefault)]
        public APIResponse SetBankDefault([FromBody] BusinessCustomerBankDefault body)
        {
            try
            {
                var result = _businessCustomerServices.BankSetDefault(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi ngân hàng mặc định
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("bank-temp/is-default")]
        [HttpPut]
        public APIResponse SetBankTempDefault([FromBody] BusinessCustomerBankTempDefault body)
        {
            try
            {
                var result = _businessCustomerServices.BankTempSetDefault(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin ngân hàng doanh nghiệp bảng tạm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("bank-temp/{id}")]
        [HttpGet]
        public APIResponse GetBusinessCustomerBankTemp(int id)
        {
            try
            {
                var result = _businessCustomerServices.FindBusinessCustomerBankTempById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ngân hàng doanh nghiệp bảng tạm bằng id doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("bank-temp/find/{id}")]
        [HttpGet]
        [PermissionFilter(Permissions.CoreDuyetKHDN_TKNH)]
        public APIResponse FindBankTempByBusinessCustomer(int id)
        {
            try
            {
                var result = _businessCustomerServices.FindBankTempByBusinessCustomer(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// So sánh sự thay đổi của khách hàng doanh nghiệp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("check-update/{id}")]
        public APIResponse BusinessCustomerCheckUpdate(int id)
        {
            try
            {
                var result = _businessCustomerServices.BusinessCustomerCheckUpdate(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Cập nhập thông tin chữ ký số ở thông tin khách hàng doanh nghiệp tạm
        /// </summary>
        /// <param name="businessCustomerTempId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-digital-sign-temp/{businessCustomerTempId}")]
        public APIResponse DigitalSignTempUpdate(int? businessCustomerTempId, DigitalSignDto input)
        {
            try
            {
                var result = _businessCustomerServices.UpdateDigitalSignTemp(businessCustomerTempId, input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Lấy thông tin chữ ký số của khách hàng doanh nghiệp bảng tạm
        /// </summary>
        /// <param name="businessCustomerTempId"></param>
        /// <returns></returns>
        [HttpGet("get-digital-sign-temp/{businessCustomerTempId}")]
        public APIResponse GetDigitalSignTemp(int? businessCustomerTempId)
        {
            try
            {
                var result = _businessCustomerServices.GetDigitalSignTemp(businessCustomerTempId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chữ ký số của khách hàng doanh nghiệp bảng chính
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <returns></returns>
        [HttpGet("get-digital-sign/{businessCustomerId}")]
        public APIResponse GetDigitalSign(int? businessCustomerId)
        {
            try
            {
                var result = _businessCustomerServices.GetDigitalSign(businessCustomerId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");

            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhập thông tin chữ ký số của khách hàng doanh nghiệp bảng chính
        /// </summary>
        /// <param name="businessCustomerId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-digital-sign/{businessCustomerId}")]
        public APIResponse UpdateDigitalSign(int? businessCustomerId, DigitalSignDto input)
        {
            try
            {
                var result = _businessCustomerServices.UpdateDigitalSign(businessCustomerId, input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

    }
}
