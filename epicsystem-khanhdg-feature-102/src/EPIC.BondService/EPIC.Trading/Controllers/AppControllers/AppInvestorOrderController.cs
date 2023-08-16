using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondOrder;
using EPIC.BondEntities.Dto.RenewalsRequest;
using EPIC.BondEntities.Dto.SaleInvestor;
using EPIC.Entities.Dto.Bond;
using EPIC.Entities.Dto.Order;
using EPIC.FileEntities.Settings;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
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
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers.InvestorControllers
{
    /// <summary>
    /// Nhà đầu tư cá nhân đặt lệnh
    /// </summary>
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/bond/investor-order")]
    [ApiController]
    public class AppInvestorOrderController : BaseController
    {
        private readonly IBondOrderService _bondOrderServices;
        private readonly IBondSharedService _bondSharedService;
        private readonly IBondContractTemplateService _contractTemplateServices;
        private readonly IBondContractDataService _contractDataServices;
        private readonly IConfiguration _configuration;
        private readonly IBondRenewalsRequestService _renewalsRequestServices;

        public AppInvestorOrderController(
            ILogger<AppInvestorOrderController> logger,
            IBondOrderService bondOrderServices,
            IBondSharedService bondSharedService,
            IBondContractTemplateService contractTemplateServices,
            IBondContractDataService contractDataServices,
            IBondRenewalsRequestService renewalsRequestServices,
            IConfiguration configuration)
        {
            _logger = logger;
            _bondOrderServices = bondOrderServices;
            _bondSharedService = bondSharedService;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _configuration = configuration;
            _renewalsRequestServices = renewalsRequestServices;
        }

        #region lưu lệnh trên app, dòng tiền
        /// <summary>
        /// Kiểm tra lệnh
        /// </summary>
        /// <returns></returns>
        [Route("order/check")]
        [HttpPost]
        public APIResponse CheckOrder([FromBody] CheckOrderAppDto input)
        {
            try
            {
                _bondOrderServices.CheckOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get mô tả dòng tiền
        /// </summary>
        [HttpGet]
        [Route("cash-flow")]
        public APIResponse GetCashFlow([Range(1, double.MaxValue)] decimal totalValue, [Range(1, int.MaxValue)] int policyDetailId)
        {
            try
            {
                var result = _bondSharedService.GetCashFlow(totalValue, policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách hợp đồng mẫu
        /// </summary>
        [HttpGet]
        [Route("contract/find")]
        public APIResponse GetContract([Range(1, int.MaxValue)]  int policyDetailId)
        {
            try
            {
                var contracts = _contractTemplateServices.FindAllForApp(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách tài khoản thụ hưởng
        /// </summary>
        [HttpGet("bank-account/find")]
        public APIResponse GetBankAccount()
        {
            try
            {
                var result = _bondOrderServices.FindAllListBank();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get ngân hàng hỗ trợ
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-support-bank")]
        public APIResponse GetSupportBank(string keyword)
        {
            try
            {
                var result = _bondOrderServices.GetListBankSupport(keyword);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check mã OTP, lưu order, đồng thời sinh file đã ký nếu không có file hoặc file không tồn tại vẫn cho lưu lệnh thành công như bình thường
        /// </summary>
        /// <returns></returns>
        [HttpPost("order/add")]
        public APIResponse OrderAdd([FromBody] CreateOrderAppDto input)
        {
            try
            {
                var result = _bondOrderServices.InvestorAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sale đặt lệnh hộ investor
        /// </summary>
        /// <returns></returns>
        [HttpPost("order/sale-add")]
        public APIResponse SaleOrderAdd([FromBody] SaleInvestorAddOrderDto input)
        {
            try
            {
                var result = _bondOrderServices.SaleAddInvestorOrder(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check sale trước khi đặt lệnh. Nếu thỏa mãn các điều kiện thì trả về thông tin của sale
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="secondaryId"></param>
        /// <returns></returns>
        [HttpGet("order/add/check/sale/{referralCode}/secondary/{secondaryId}")]
        public APIResponse CheckSalerBeforeAddOrder(string referralCode, int secondaryId)
        {
            try
            {
                _logger.LogInformation("MGT => ", referralCode);
                _logger.LogInformation("MSP => ", secondaryId);
                var result = _bondOrderServices.CheckSaleBeforeAddOrder(referralCode, secondaryId);
                return new APIResponse(result == null ? Utils.StatusCode.Error : Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sale theo mã giới thiệu đặt lệnh
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpGet("order/find-referral-code")]
        public APIResponse AppSaleOrderFindReferralCode(string referralCode, [Range(1, int.MaxValue)] int policyDetailId)
        {
            try
            {
                var result = _bondOrderServices.AppSaleOrderFindReferralCode(referralCode?.Trim(), policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        #region hợp đồng
        /// <summary>
        /// export contract tạm
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-contract")]
        public async Task<IActionResult> ExportContract([Range(1, double.MaxValue)] decimal totalValue, [Range(1, int.MaxValue)] int policyDetailId, [Range(1, int.MaxValue)] int BankAccId, [Range(1, int.MaxValue)] int identificationId, [Range(1, int.MaxValue)] int contractTemplateId, [Range(1, int.MaxValue)] int? investorId = null)
        {
            try
            {
                var result = await _contractDataServices.ExportContractApp(totalValue, policyDetailId, BankAccId, identificationId, contractTemplateId, investorId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// xuất file hợp đồng có chữ đã ký
        /// </summary>
        /// <param name="secondaryContractFileId">id file trong bảng EP_BOND_-SECONDARY_CONTARCT</param>
        /// <returns></returns>
        [HttpGet("export-contract-signature")]
        public IActionResult ExportFileSignature([Range(1, int.MaxValue)] int secondaryContractFileId)
        {
            try
            {
                var result = _contractDataServices.ExportContractSignatureApp(secondaryContractFileId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
        #endregion

        /// <summary>
        /// Màn sổ lệnh của khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/get-order")]
        public APIResponse AppOrderGetAll()
        {
            try
            {
                var result = _bondOrderServices.AppOrderGetAll(AppOrderGroupStatus.SO_LENH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Màn đang đầu tư của khách hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet("order/get-investing")]
        public APIResponse AppOrderInvetingGetAll()
        {
            try
            {
                var result = _bondOrderServices.AppOrderGetAll(AppOrderGroupStatus.DANG_DAU_TU);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("order/{orderId}")]
        public APIResponse AppOrderInvestorDetail([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var result = _bondOrderServices.AppOrderInvestorDetail(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy lệnh cho App đang trong trạng thái khởi tạo hoặc chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("order/cancel/{orderId}")]
        public APIResponse AppCancelOrder([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var result = _bondOrderServices.AppCancelOrder(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi phương thức tất toán cuối kỳ
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("order/settlement-method/{orderId}")]
        public APIResponse AppUpdateSettlementMethod([Range(1, int.MaxValue)] int orderId, SettlementMethodDto input)
        {
            try
            {
                var result = _bondOrderServices.AppUpdateSettlementMethod(orderId, input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /*/// <summary>
        /// export contract receive
        /// </summary>
        /// <returns></returns>
        [HttpGet("export-contract-receive")]
        public async Task<IActionResult> ExportContractReceive([Range(1, int.MaxValue)] int orderId, [Range(1, int.MaxValue)] int bondSecondaryId, [Range(1, int.MaxValue)] int tradingProviderId)
        {
            try
            {
                var result = await _contractDataServices.ExportContractReceive(orderId, bondSecondaryId, tradingProviderId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }*/

        /// <summary>
        /// Đổi trạng thái đang giao sang đã nhận cho app
        /// </summary>
        /// <param name="deliveryCode"></param>
        /// <returns></returns>
        [Route("order/delivery-status-recevired/{deliveryCode}")]
        [HttpPut]
        public APIResponse ChangeDeliveryStatusRecevired(string deliveryCode)
        {
            try
            {
                var result = _bondOrderServices.ChangeDeliveryStatusRecevired(deliveryCode);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách file hợp đồng đã ký
        /// </summary>
        [HttpGet]
        [Route("contract/find-file-signature")]
        public APIResponse FindAllFileSignatureForApp([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var contracts = _contractTemplateServices.FindAllFileSignatureForApp(orderId);
                return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region Thay đổi phương thức tất toán
        /// <summary>
        /// Yêu cầu thay đổi phương thức tất toán cuối kỳ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("order/renewals-request")]
        public APIResponse AppRenewalsRequest([FromBody] AppCreateRenewalsRequestDto input)
        {
            try
            {
                var result = _renewalsRequestServices.AppRenewalsRequest(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
