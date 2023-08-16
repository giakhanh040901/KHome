using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.Utils.Controllers;
using EPIC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EPIC.Utils.Net.MimeTypes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.GarnerDomain.Implements;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using Microsoft.AspNetCore.Authorization;
using EPIC.Entities.Dto.ContractData;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateApp;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.GarnerEntities.Dto.GarnerRating;

namespace EPIC.GarnerAPI.Controllers.AppController
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/garner/investor-order")]
    [ApiController]
    public class AppInvestorOrderController : BaseController
    {
        private readonly IGarnerOrderServices _garnerOrderServices;
        private readonly IGarnerContractDataServices _garnerContractDataServices;
        private readonly IGarnerContractTemplateServices _garnerContractTemplateServices;
        private readonly IGarnerWithdrawalServices _garnerWithdrawalServices;
        private readonly IGarnerRatingServices _garnerRatingServices;

        public AppInvestorOrderController(ILogger<AppInvestorOrderController> logger,
            IGarnerOrderServices garnerOrderServices,
            IGarnerContractDataServices garnerContractDataServices,
            IGarnerContractTemplateServices garnerContractTemplateServices,
            IGarnerWithdrawalServices garnerWithdrawalServices,
            IGarnerRatingServices garnerRatingServices)
        {
            _logger = logger;
            _garnerOrderServices = garnerOrderServices;
            _garnerContractDataServices = garnerContractDataServices;
            _garnerContractTemplateServices = garnerContractTemplateServices;
            _garnerWithdrawalServices = garnerWithdrawalServices;
            _garnerRatingServices = garnerRatingServices;
        }

        /// <summary>
        /// Check thêm mới hợp đồng
        /// </summary>
        [HttpPost("check")]
        [ProducesResponseType(typeof(APIResponse<AppGarnerOrderDto>), (int)HttpStatusCode.OK)]
        public APIResponse CheckOrder([FromBody] AppCheckGarnerOrderDto input)
        {
            try
            {
                _garnerOrderServices.CheckOrder(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đặt lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<AppGarnerOrderDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> InvestorOrderAdd([FromBody] AppCreateGarnerOrderDto input)
        {
            try
            {
                var result = await _garnerOrderServices.InvestorOrderAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Sale Đặt lệnh hộ investor
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("sale/add")]
        [ProducesResponseType(typeof(APIResponse<AppGarnerOrderDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> SaleInvestorOrderAdd([FromBody] AppSaleCreateGarnerOrderDto input)
        {
            try
            {
                var result = await _garnerOrderServices.SaleInvestorOrderAdd(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sale theo mã gioi thieu cua sale và chính sách đang chọn
        /// </summary>
        /// <param name="referralCode"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpGet("order/find-referral-code")]
        public APIResponse AppSaleOrderFindReferralCode(string referralCode, [Range(1, int.MaxValue)] int policyId)
        {
            try
            {
                var result = _garnerOrderServices.AppSaleOrderFindReferralCode(referralCode?.Trim(), policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử đang đầu tư của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-investing")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerOrderByPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorOrderInvesting()
        {
            try
            {
                var result = _garnerOrderServices.AppInvestorGetListOrder(AppOrderGroupStatus.DANG_DAU_TU);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử sổ lệnh chưa thanh toán của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-order")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerOrderByPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorOrder()
        {
            try
            {
                var result = _garnerOrderServices.AppInvestorGetListOrder(AppOrderGroupStatus.SO_LENH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy lịch sử sổ lệnh đã tất toán của nhà đầu tư cá nhân
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-history")]
        [ProducesResponseType(typeof(APIResponse<List<AppGarnerOrderByPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse AppInvestorOrderHistory()
        {
            try
            {
                var result = _garnerOrderServices.AppInvestorGetListOrderHistory(AppOrderGroupStatus.DA_TAT_TOAN);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// App xem chi tiết thông tin lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [ProducesResponseType(typeof(APIResponse<AppGarnerOrderDetailDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AppOrderDetail(long orderId)
        {
            try
            {
                var result = await _garnerOrderServices.AppOrderDetail(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết rút vốn
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <returns></returns>
        [HttpGet("withdrawal-by-id")]
        [ProducesResponseType(typeof(APIResponse<GarnerWithdrawalByPolicyDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetOrderWithdrawalById(long withdrawalId)
        {
            try
            {
                var result = _garnerWithdrawalServices.GetOrderWithdrawalById(withdrawalId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thay đổi trước khi yêu cầu rút vốn
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        [HttpGet("view-change-withdrawal-request")]
        [ProducesResponseType(typeof(APIResponse<ViewCalculateGarnerWithdrawalDto>), (int)HttpStatusCode.OK)]
        public APIResponse ViewChangeRequestWithdrawal(int policyId, decimal amountMoney)
        {
            try
            {
                var result = _garnerWithdrawalServices.ViewChangeRequestWithdrawal(policyId, amountMoney);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Yêu cầu rút vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("withdrawal-request")]
        [ProducesResponseType(typeof(APIResponse<CalculateGarnerWithdrawalDto>), (int)HttpStatusCode.OK)]
        public async Task<APIResponse> AppWithdrawlRequest([FromBody] AppWithdrawalRequestDto input)
        {
            try
            {
                var result = await _garnerWithdrawalServices.AppRequestWithdrawal(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// App hủy duyệt trọng trạng thái chờ thanh toán
        /// </summary>
        /// <param name="orderId"></param>
        [HttpDelete("cancel/{orderId}")]
        public APIResponse AppOrderCancel(long orderId)
        {
            try
            {
                 _garnerOrderServices.AppOrderCancel(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem tạm hợp đồng trước khi đặt lệnh
        /// </summary>
        /// <param name="totalValue">Tổng tiền đầu tư ban đầu</param>
        /// <param name="policyId">Id chính sách</param>
        /// <param name="bankAccId">Id tài khoản thụ hưởng của nhà đầu tư</param>
        /// <param name="identificationId">Id giấy tờ cá nhân</param>
        /// <param name="contractTemplateId">Id mẫu hợp đồng</param>
        /// <returns></returns>
        [HttpGet("export-contract")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ExportContract([Range(1, double.MaxValue)] decimal totalValue, [Range(1, int.MaxValue)] int policyId, [Range(1, int.MaxValue)] int bankAccId, [Range(1, int.MaxValue)] int identificationId, [Range(1, int.MaxValue)] int contractTemplateId)
        {
            try
            {
                var result = await _garnerContractDataServices.ExportContractApp(totalValue, policyId, bankAccId, identificationId, contractTemplateId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }
    
        /// <summary>
        /// Lấy danh sách mẫu hợp đồng đặt lệnh
        /// </summary>
        /// <param name="policyId">Id chính sách</param>
        /// <returns></returns>
        [HttpGet("find-contract-template")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerContractTemplateAppDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllContractTemplate([Range(1, int.MaxValue)] int policyId)
        {     
            try
            {
                var result = _garnerContractTemplateServices.FindAllForApp(policyId, ContractTypes.DAT_LENH);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mẫu hợp đồng rút tiền
        /// </summary>
        /// <param name="policyId">Id chính sách</param>
        /// <returns></returns>
        [HttpGet("find-contract-template-withdrawal")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerContractTemplateAppDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllContractTemplateWithdrawal([Range(1, int.MaxValue)] int policyId)
        {
            try
            {
                var result = _garnerContractTemplateServices.FindAllForApp(policyId, ContractTypes.RUT_TIEN);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem tạm hợp đồng trước khi rút tiền
        /// </summary>
        /// <param name="orderId">Id lệnh</param>
        /// <param name="contractTemplateId">Id mẫu hợp đồng</param>
        /// <param name="amountMoney">Số tiền rút</param>
        /// <param name="investorBankAccId">id tài khoản ngân hàng rút</param>
        /// <returns></returns>
        [HttpGet("export-contract-withdrawal")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ExportContractWithdrawal([Range(1, int.MaxValue)] long orderId, [Range(1, int.MaxValue)] int contractTemplateId, [Range(1, double.MaxValue)] decimal amountMoney, [Range(1, int.MaxValue)] int investorBankAccId)
        {
            try
            {
                var result = await _garnerContractDataServices.ExportContractWithDrawal(orderId, contractTemplateId, amountMoney, investorBankAccId);
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
        /// <param name="orderContractFileId">id file trong bảng GARNER_ORDER_CONTRACT_FILE</param>
        /// <returns></returns>
        [HttpGet("export-contract-signature")]
        public IActionResult ExportFileSignature([Range(1, int.MaxValue)] int orderContractFileId)
        {
            try
            {
                var result = _garnerContractDataServices.ExportFileContract(orderContractFileId, ContractFileTypes.SIGNATURE);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// Get danh sách file hợp đồng đã ký
        /// </summary>
        /// <param name="orderId">Id sổ lệnh</param>
        /// <returns></returns>
        [HttpGet]
        [Route("contract/find-file-signature")]
        public APIResponse FindAllFileSignatureForApp([Range(1, int.MaxValue)] int orderId)
        {
            try
            {
                var contracts = _garnerContractTemplateServices.FindAllFileSignature(orderId);
                return new APIResponse(Utils.StatusCode.Success, contracts, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm hợp đồng mới nhất để đánh giá
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("rating/find-last-order")]
        public APIResponse FindLastOrder()
        {
            try
            {
                var result = _garnerRatingServices.FindLastOrder();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm đánh giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("rating/add")]
        public APIResponse AddRating(CreateGarnerRatingDto input)
        {
            try
            {
                _garnerRatingServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
