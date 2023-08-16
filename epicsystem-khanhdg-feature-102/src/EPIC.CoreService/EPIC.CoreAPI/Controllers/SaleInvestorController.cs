using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.SaleInvestor;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/core/sale-investor")]
    [ApiController]
    public class SaleInvestorController : BaseController
    {
        private readonly ISaleInvestorServices _saleInvestorServices;
        /// <summary>
        /// Khởi tạo
        /// </summary>
        /// <param name="saleInvestorServices"></param>
        public SaleInvestorController(ISaleInvestorServices saleInvestorServices)
        {
            _saleInvestorServices = saleInvestorServices;
        }

        #region sale đăng ký hộ khách hàng
        /// <summary>
        /// Đăng ký investor
        /// </summary>
        /// <param name="dto"></param>
        [HttpPost]
        //[AuthorizeAdminUserTypeFilter]
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
        //[AuthorizeAdminUserTypeFilter]
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
        //[AuthorizeAdminUserTypeFilter]
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
        //[AuthorizeAdminUserTypeFilter]
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
        [HttpPost]
        //[AuthorizeAdminUserTypeFilter]
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
        //[AuthorizeAdminUserTypeFilter]
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
        //[AuthorizeAdminUserTypeFilter]
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

        #region Xem investor
        /// <summary>
        /// Lấy list investor theo sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("investors")]
        public APIResponse GetListInvestorBySale([FromQuery] GetInvestorBySaleDto dto)
        {
            try
            {
                var data = _saleInvestorServices.GetListInvestorBySale(dto);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lọc investor cho sale
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("investors/filter")]
        public APIResponse FilterListInvestorBySale([FromQuery] SaleFilterInvestorDto dto)
        {
            try
            {
                var data = _saleInvestorServices.FilterInvestor(new FilterManagerInvestorDto
                {
                    Keyword = dto.Keyword,
                    RequireKeyword = true,
                    PageSize = -1
                });
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sale xem thông tin chi tiết của khách hàng
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("investor/{investorId}")]
        public APIResponse GetDetailInvestorById(int investorId)
        {
            try
            {
                var data = _saleInvestorServices.GetDetailInvestorInfo(investorId);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết nhà đầu tư theo sale và doanh số trong đại lý
        /// </summary>
        [Authorize]
        [HttpGet("investor-info/{investorId}")]
        public APIResponse InvestorInfoBySale(int investorId, int tradingProviderId)
        {
            try
            {
                var data = _saleInvestorServices.InvestorInfoBySale(investorId, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, data, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
