using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace EPIC.InvestAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/invest/distribution")]
    [ApiController]
    public class DistributionController : BaseController
    {
        private readonly IDistributionServices _distributionServices;
        private readonly IConfigContractCodeServices _configContractCodeServices;
        public DistributionController(ILogger<DistributionController> logger, IDistributionServices distributionServices, IConfigContractCodeServices configContractCodeServices)
        {
            _logger = logger;
            _distributionServices = distributionServices;
            _configContractCodeServices = configContractCodeServices;
        }


        /// <summary>
        /// Get all bán theo kỳ hạn, nếu là dlsc thì không cần truyền vào tradingProviderId tự lấy trong token,
        /// nếu là supper admin thì sẽ truyền vào tradingProviderId để lọc
        /// lọc theo kỳ hạn, chính sách đang trong trạng thái hoạt động và đang được mở bán
        /// </summary>
        /// <returns></returns>
        [HttpGet("find")]
        [PermissionFilter(Permissions.InvestPPDT_DanhSach)]
        public APIResponse FindAllDistribution([FromQuery] FilterInvestDistributionDto input)
        {
            try
            {
                var result = _distributionServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách bán theo kỳ hạn để đặt lệnh
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-distribution-order")]
        [ProducesResponseType(typeof(APIResponse<List<ViewDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllOrder([FromQuery] FilterInvestDistributionDto input)
        {
            try
            {
                var result = _distributionServices.FindAllOrder(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết bán theo kỳ hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _distributionServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// chi tiết bán theo kì hạn tổng quan tìm theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("over-view-distribution-by-id")]
        [PermissionFilter(Permissions.InvestPPDT_TongQuan)]
        public APIResponse FindOverViewById(int id)
        {
            try
            {
                var result = _distributionServices.FindOverViewById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách chính sách kèm theo kỳ hạn
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpGet("find-policy")]
        [ProducesResponseType(typeof(APIResponse<ViewPolicyDto>), (int)HttpStatusCode.OK)]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_DanhSach)]
        public APIResponse FindPolicyAndPolicyDetailByPolicyId(int policyId)
        {
            try
            {
                var result = _distributionServices.FindPolicyAndPolicyDetailByPolicyId(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách kỳ hạn theo id chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpGet("policy-detail/find-by-policy")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_DanhSach)]
        public APIResponse FindPolicyDetailByPolicyId(int policyId)
        {
            try
            {
                var result = _distributionServices.GetAllListPolicyDetailByPolicy(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem kỳ hạn theo id
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpGet("policy-detail/find-by-id")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_DanhSach)]
        public APIResponse FindPolicyDetailById(int policyDetailId)
        {
            try
            {
                var result = _distributionServices.FindPolicyDetailById(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo mới bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.InvestPPDT_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<Distribution>), (int)HttpStatusCode.OK)]
        public APIResponse AddDistribution([FromBody] CreateDistributionDto body)
        {
            try
            {
                var result = _distributionServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật bán theo kỳ hạn khi ở trạng thái tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("update")]
        [PermissionFilter(Permissions.InvestPPDT_CapNhat)]
        public APIResponse UpdateDistribution([FromBody] UpdateDistributionDto body)
        {
            try
            {
                _distributionServices.Update(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chỉnh sửa ngân hàng thụ hưởng của Bán phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessCustomerBankAccId"></param>
        /// <returns></returns>
        [HttpPut("update-bank")]
        [PermissionFilter(Permissions.InvestPPDT_CapNhat)]
        public APIResponse UpdateBank(int id, int businessCustomerBankAccId)
        {
            try
            {
                _distributionServices.UpdateBank(id, businessCustomerBankAccId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm nội dung tổng quan của sản phẩm
        /// overview 1- MARKDOWN 2- HTML
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-overview")]
        [PermissionFilter(Permissions.InvestPPDT_TongQuan_CapNhat)]
        public APIResponse UpdateOverviewContent(UpdateDistributionOverviewContentDto input)
        {
            try
            {
                _distributionServices.UpdateOverviewContent(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Có đóng không cho đặt lệnh không
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpPut("is-close/{distributionId}")]
        [PermissionFilter(Permissions.InvestPPDT_DongTam)]
        public APIResponse IsClose(int distributionId)
        {
            try
            {
                _distributionServices.IsClose(distributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Có hiện lên app không
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpPut("is-show-app/{distributionId}")]
        [PermissionFilter(Permissions.InvestPPDT_BatTatShowApp)]
        public APIResponse IsShowApp(int distributionId)
        {
            try
            {
                _distributionServices.IsShowApp(distributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Thêm chính sách khi ở trạng thái tạm
        /// </summary>
        /// <returns></returns>
        [HttpPost("add-policy")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<ViewPolicyDto>), (int)HttpStatusCode.OK)]
        public APIResponse AddPolicy([FromBody] CreatePolicySpecificDto body)
        {
            try
            {
                var policyDetailTemps = _distributionServices.AddPolicy(body);
                return new APIResponse(Utils.StatusCode.Success, policyDetailTemps, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách khi ở trạng thái tạm
        /// </summary>
        /// <returns></returns>
        [HttpPost("add-policy-detail")]
        [PermissionFilter(Permissions.InvestPPDT_KyHan_ThemMoi)]
        public APIResponse AddPolicyDetail([FromBody] CreatePolicyDetailDto body)
        {
            try
            {
                _distributionServices.AddPolicyDetail(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách khi bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update-policy/{policyId}")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_CapNhat)]
        public APIResponse UpdatePolicy( [FromBody] UpdatePolicyDto body)
        {
            try
            {
                _distributionServices.UpdatePolicy(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật kỳ hạn khi bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update-policy-detail")]
        [PermissionFilter(Permissions.InvestPPDT_KyHan_CapNhat)]
        public APIResponse UpdatePolicyDetail([FromBody] UpdatePolicyDetailDto body)
        {
            try
            {
                _distributionServices.UpdatePolicyDetail(body);
                return new APIResponse(Utils.StatusCode.Success, body, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa chính sách khi bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpDelete("delete-policy/{policyId}")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_Xoa)]
        public APIResponse DeletePolicy(int policyId)
        {
            try
            {
                _distributionServices.DeletePolicy(policyId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa kỳ hạn khi bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpDelete("delete-policy-detail/{policyDetailId}")]
        [PermissionFilter(Permissions.InvestPPDT_KyHan_Xoa)]
        public APIResponse DeletePolicyDetail(int policyDetailId)
        {
            try
            {
                _distributionServices.DeletePolicyDetail(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Có hiện lên app không
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpPut("policy-is-show-app/{policyId}")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_BatTatShowApp)]
        public APIResponse PolicyIsShowApp(int policyId)
        {
            try
            {
                _distributionServices.PolicyIsShowApp(policyId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Có hiện lên app không
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpPut("policy-detail-is-show-app/{policyDetailId}")]
        [PermissionFilter(Permissions.InvestPPDT_KyHan_BatTatShowApp)]
        public APIResponse PolicyDetailIsShowApp(int policyDetailId)
        {
            try
            {
                _distributionServices.PolicyDetailIsShowApp(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        public APIResponse AddRequest([FromBody] RequestStatusDto input)
        {
            try
            {
                _distributionServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt bán theo kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approve")]
        [PermissionFilter(Permissions.InvestPDPPDT_PheDuyetOrHuy, Permissions.InvestPDSPDT_PheDuyetOrHuy)]
        public APIResponse Approve(InvestApproveDto input)
        {
            try
            {
                _distributionServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
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
        [PermissionFilter(Permissions.InvestPPDT_EpicXacMinh)]
        public APIResponse Check([FromBody] CheckStatusDto input)
        {
            try
            {
                
                _distributionServices.Check(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Xác minh thành công");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Hủy duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("cancel")]
        [PermissionFilter(Permissions.InvestPDPPDT_PheDuyetOrHuy, Permissions.InvestPDSPDT_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] CancelStatusDto input)
        {
            try
            {
                _distributionServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpPut("policy/change-status")]
        [PermissionFilter(Permissions.InvestPPDT_ChinhSach_KichHoatOrHuy)]
        public APIResponse ChangeStatusPolicy(int policyId)
        {
            try
            {
                var result = _distributionServices.ChangeStatusPolicy(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái Kỳ hạn
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <returns></returns>
        [HttpPut("policyDetail/change-status")]
        [PermissionFilter(Permissions.InvestPPDT_KyHan_KichHoatOrHuy)]
        public APIResponse ChangeStatusPolicyDetail(int policyDetailId)
        {
            try
            {
                var result = _distributionServices.ChangeStatusPolicyDetail(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy chính sách theo distributionId chưa phân trang
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("find-policy/{id}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ViewPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindPolicyByDistribution(int id, string status)
        {
            try
            {
                var result = _distributionServices.GetAllPolicyByDistri(new InvestPolicyFilterDto
                {
                    DistributionId = id,
                    Status = status,
                    PageSize = -1
                });
                return new APIResponse(Utils.StatusCode.Success, result.Items, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy chính sách theo distributionId phân trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-policy-by-ditribution")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewPolicyDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindPolicyByDistribution([FromQuery] InvestPolicyFilterDto input)
        {
            try
            {
                var result = _distributionServices.GetAllPolicyByDistri(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("list-bank")]
        [ProducesResponseType(typeof(APIResponse<List<BusinessCustomerBankDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindBankByTradingInvest(int? distributionId = null, int? type = null)
        {
            try
            {
                var result = _distributionServices.FindBankByTradingInvest(distributionId, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Add cấu hình mã hợp đồng (Config contract sCode)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("config-contract-code/add")]
        [ProducesResponseType(typeof(APIResponse<InvConfigContractCodeDto>), (int)HttpStatusCode.OK)]
        public APIResponse AddConfigContractCode(CreateConfigContractCodeDto input)
        {
            try
            {
                var result = _configContractCodeServices.AddConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpPut("config-contract-code/update")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateConfigContractCode(UpdateConfigContractCodeDto input)
        {
            try
            {
                _configContractCodeServices.UpdateConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("config-contract-code/get-all")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCode([FromQuery] FilterInvConfigContractCodeDto input)
        {
            try
            {
                var result = _configContractCodeServices.GetAllConfigContractCode(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("config-contract-code/get-all-status-active")]
        [ProducesResponseType(typeof(APIResponse<List<InvConfigContractCodeDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllConfigContractCodeStatusActive()
        {
            try
            {
                var result = _configContractCodeServices.GetAllConfigContractCodeStatusActive();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("config-contract-code/{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse<InvConfigContractCodeDto>), (int)HttpStatusCode.OK)]
        public APIResponse GetConfigContractCodeById(int configContractCodeId)
        {
            try
            {
                var result = _configContractCodeServices.GetConfigContractCodeById(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái config theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <returns></returns>
        [HttpPut("config-contract-code/update-status/{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            try
            {
                _configContractCodeServices.UpdateConfigContractCodeStatus(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <returns></returns>
        [HttpDelete("config-contract-code/delete/{configContractCodeId}")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse DeleteConfigContractCode(int configContractCodeId)
        {
            try
            {
                _configContractCodeServices.DeleteConfigContractCode(configContractCodeId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách ngân hàng thu chi của đại lý theo Phân phối sản phẩm
        /// type: 1: Thu, 2: chi
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet("find-bank-distribution")]
        public APIResponse FindBankByDistribution (int distributionId, int type)
        {
            try
            {
                var result = _distributionServices.FindBankByDistributionId(distributionId, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách phân phối theo trading (cả bán hộ)
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-all")]
        [PermissionFilter(Permissions.InvestPPDT_DanhSach)]
        public APIResponse FindAllDistributionTrading()
        {
            try
            {
                var result = _distributionServices.FindDistributionBanHoByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
