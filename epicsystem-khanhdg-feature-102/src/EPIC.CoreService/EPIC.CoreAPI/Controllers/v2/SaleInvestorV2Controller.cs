using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.Entities.Dto.SaleInvestor;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.WebAPIBase.FIlters;
using Humanizer;

namespace EPIC.CoreAPI.Controllers.v2
{
    /// <summary>
    /// Sale đăng ký tạo investor v2
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/core/sale-investor-v2")]
    [ApiController]
    public class SaleInvestorV2Controller : BaseController
    {
        private readonly ISaleInvestorV2Services _saleInvestorServices;

        public SaleInvestorV2Controller(ISaleInvestorV2Services saleInvestorServices)
        {
            _saleInvestorServices = saleInvestorServices;
        }

        #region sale đăng ký hộ khách hàng
        /// <summary>
        /// Đăng ký investor
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost]
        [Route("register")]
        public APIResponse Register([FromBody] SaleRegisterInvestorDto dto)
        {
            try
            {
                var result = _saleInvestorServices.RegisterInvestor(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Ekyc
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ekyc")]
        public async Task<APIResponse> Ekyc([FromForm] EkycSaleInvestorDto dto)
        {
            try
            {
                var result = await _saleInvestorServices.UpdateEkycIdentification(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xác nhận thông tin ekyc và cập nhật lại
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("confirm")]
        public APIResponse ConfirmAndUpdateEkyc([FromBody] SaleInvestorConfirmUpdateDto dto)
        {
            try
            {
                _saleInvestorServices.ConfirmAndUpdateEkyc(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật ảnh đại diện
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("avatar")]
        public APIResponse UploadAvatar([FromForm] SaleInvestorUploadAvatarDto dto)
        {
            try
            {
                var result = _saleInvestorServices.UploadAvatar(dto);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm địa chỉ liên hệ
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("contact-address")]
        public APIResponse AddContactAddress([FromBody] CreateContactAddressDto dto)
        {
            try
            {
                _saleInvestorServices.AddContactAddress(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo bank
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bank")]
        public async Task<APIResponse> AddBank([FromBody] CreateBankDto dto)
        {
            try
            {
                await _saleInvestorServices.AddBank(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Nhà đầu tư chuyên nghiệp
        /// <summary>
        /// Upload file nhà đầu tư chuyên nghiệp
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("upload/prof")]
        public APIResponse UploadProfFile([FromForm] SaleInvestorUploadProfFileDto dto)
        {
            try
            {
                _saleInvestorServices.UploadProfFile(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        /// <summary>
        /// Lấy danh sách tất cả khách hàng của sale
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("investor/get-all")]
        public APIResponse GetAllListInvestor()
        {
            try
            {
                return new(_saleInvestorServices.GetAllListInvestor());
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
