using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ProductBondSecondPrice;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/secondary")]
    [ApiController]
    public class BondSecondaryController : BaseController
    {
        private readonly IBondSecondaryService _productBondSecondaryService;
        public BondSecondaryController(ILogger<BondSecondaryController> logger, IBondSecondaryService productBondSecondaryService)
        {
            _logger = logger;
            _productBondSecondaryService = productBondSecondaryService;
        }

        /// <summary>
        /// Tạo mới bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPost("add")]
        [PermissionFilter(Permissions.BondMenuQLTP_BTKH_ThemMoi)]
        public APIResponse AddSecondary([FromBody] CreateProductBondSecondaryDto body)
        {
            try
            {
                var result = _productBondSecondaryService.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPost("add-policy")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_ThemMoi)]
        public APIResponse AddPolicy([FromBody] CreateProductBondPolicySpecificDto body)
        {
            try
            {
                var policyDetailTemps = _productBondSecondaryService.AddPolicy(body);
                return new APIResponse(Utils.StatusCode.Success, policyDetailTemps, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        [HttpPost("add-list-policy")]
       
        public APIResponse AddPolicySecondary(int policytempId, List<int> bondsecondaryId)
        {
            try
            {
                var policyDetailTemps = _productBondSecondaryService.AddPolicySecondary(policytempId, bondsecondaryId);
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
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPost("add-policy-detail")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_ThemMoi)]
        public APIResponse AddPolicyDetail([FromBody] CreateProductBondPolicyDetailDto body)
        {
            try
            {
                _productBondSecondaryService.AddPolicyDetail(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_CapNhat)]
        public APIResponse UpdateSecondary([FromBody] UpdateProductBondSecondaryDto body)
        {
            try
            {
                _productBondSecondaryService.Update(body);
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
        /// <param name="policyId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("update-policy/{policyId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_CapNhat)]
        public APIResponse UpdatePolicy(int policyId, [FromBody] UpdateProductBondPolicyDto body)
        {
            try
            {
                _productBondSecondaryService.UpdatePolicy(policyId, body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpDelete("delete-policy/{policyId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_Xoa)]
        public APIResponse DeletePolicy(int policyId)
        {
            try
            {
                _productBondSecondaryService.DeletePolicy(policyId);
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
        /// <param name="policyDetailId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("update-policy-detail/{policyDetailId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_CapNhat)]
        public APIResponse UpdatePolicyDetail(int policyDetailId, [FromBody] UpdateProductBondPolicyDetailDto body)
        {
            try
            {   
                _productBondSecondaryService.UpdatePolicyDetail(policyDetailId, body);
                return new APIResponse(Utils.StatusCode.Success, body, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-all-policy-detail/{policyId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_DanhSach)]
        public APIResponse FindAllPolicyDetail(int policyId)
        {
            try
            {
                var result = _productBondSecondaryService.GetAllListPolicyDetailByPolicy(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpDelete("delete-policy-detail/{policyDetailId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_Xoa)]
        public APIResponse DeletePolicyDetail(int policyDetailId)
        {
            try
            {
                _productBondSecondaryService.DeletePolicyDetail(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get all bán theo kỳ hạn, nếu là dlsc thì không cần truyền vào tradingProviderId tự lấy trong token,
        /// nếu là supper admin thì sẽ truyền vào tradingProviderId để lọc
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="status"></param>
        /// <param name="isActivate">lọc theo kỳ hạn, chính sách đang trong trạng thái hoạt động và đang được mở bán</param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpGet("find")]
        [PermissionFilter(Permissions.BondMenuQLTP_BTKH_DanhSach)]
        public APIResponse FindAllSecondary(int pageNumber, int? pageSize, string keyword, int? status, bool isActivate, string isClose)
        {
            try
            {
                var result = _productBondSecondaryService.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status, isActivate, isClose);
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
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpGet("find-secondary-order")]
        [PermissionFilter(Permissions.BondHDPP_SoLenh_DanhSach)]
        public APIResponse FindAllOrder()
        {
            try
            {
                var result = _productBondSecondaryService.FindAllOrder();
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
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER, UserData.SUPER_ADMIN })]
        [HttpGet("{id}")]
        [PermissionFilter(Permissions.BondMenuQLTP_BTKH_ThongTinChiTiet)]

        public APIResponse FindById(int id)
        {
            try
            {
                var result = _productBondSecondaryService.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Trình duyệt trong đại lý
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("trading-provider-submit/{bondSecondaryId}")]
        public APIResponse TradingProviderSubmit(int bondSecondaryId)
        {
            try
            {
                _productBondSecondaryService.TradingProviderSubmit(bondSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đại lý duyệt, hoặc chuyển về trạng thái tạm khi chưa được super admin duyệt
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("trading-provider-approve/{bondSecondaryId}")]
        public APIResponse TradingProviderApprove(
            int bondSecondaryId,
            [IntegerRange(AllowableValues = new int[] {
                BondSecondaryStatus.TEMP, BondSecondaryStatus.TRADING_PROVIDER_APPROVE
            })]
            int status)
        {
            try
            {
                _productBondSecondaryService.TradingProviderApprove(bondSecondaryId, status);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// super admin duyệt khi đại lý đã duyệt
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("super-admin-approve/{bondSecondaryId}")]
        public APIResponse SuperAdminApprove(
            int bondSecondaryId,
            [IntegerRange(AllowableValues = new int[] {
                BondSecondaryStatus.SUPER_ADMIN_APPROVE
            })]
            int status)
        {
            try
            {
                _productBondSecondaryService.SuperAdminApprove(bondSecondaryId, status);
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
        /// <param name="bondSecondaryId"></param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpPut("is-close/{bondSecondaryId}")]
        [PermissionFilter(Permissions.BondMenuQLTP_BTKH_DongTam)]
        public APIResponse IsCancel(
            int bondSecondaryId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isClose)
        {
            try
            {
                _productBondSecondaryService.IsClose(bondSecondaryId, isClose);
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
        /// <param name="bondSecondaryId"></param>
        /// <param name="isShowApp"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpPut("is-show-app/{bondSecondaryId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse IsShowApp(
            int bondSecondaryId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isShowApp)
        {
            try
            {
                _productBondSecondaryService.IsShowApp(bondSecondaryId, isShowApp);
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
        /// <param name="isShowApp"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpPut("policy-is-show-app/{policyId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse PolicyIsShowApp(
            int policyId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isShowApp)
        {
            try
            {
                _productBondSecondaryService.PolicyIsShowApp(policyId, isShowApp);
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
        /// <param name="isShowApp"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpPut("policy-detail-is-show-app/{policyDetailId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse PolicyDetailIsShowApp(
            int policyDetailId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isShowApp)
        {
            try
            {
                _productBondSecondaryService.PolicyDetailIsShowApp(policyDetailId, isShowApp);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        [HttpPost("import-price-from-excel/{bondSecondaryId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_BangGia_ImportExcelBG)]
        public APIResponse ReadFileExcel([Required(ErrorMessage = "Không tìm thấy đầu vào file dữ liệu! ")] [FromForm] IFormFile file, int bondSecondaryId)
        {
            try
            {
                _productBondSecondaryService.ImportSecondPrice(file, bondSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpDelete("delete-second-price/{bondSecondaryId}")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse DeleteSecondPrice([Required(ErrorMessage = " Id của phát hành thứ cấp không hợp lệ ")] int bondSecondaryId )
        {
            try
            {
                _productBondSecondaryService.DeleteSecondPrice(bondSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-second-price")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_BangGia_DanhSach)]
        public APIResponse FindAllSecondPrice(int pageNumber, int? pageSize, int bondSecondaryId)
        {
            try
            {
                var result = _productBondSecondaryService.FindAll(pageSize ?? 100, pageNumber, bondSecondaryId);
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
        [PermissionFilter(Permissions.BondMenuQLTP_BTKH_TrinhDuyet)]
        public APIResponse AddRequest([FromBody] RequestStatusDto input)
        {
            try
            {
                _productBondSecondaryService.Request(input);
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
        [PermissionFilter(Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy)]
        public APIResponse Approve(ApproveStatusDto input)
        {
            try
            {
                _productBondSecondaryService.Approve(input);
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
        [PermissionFilter(Permissions.BondMenuQLTP_BTKH_EpicXacMinh)]
        public APIResponse Check([FromBody] CheckStatusDto input)
        {
            try
            {
                _productBondSecondaryService.Check(input);
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
        [PermissionFilter(Permissions.BondQLPD_PDBTKH_PheDuyetOrHuy)]
        public APIResponse Cancel([FromBody] CancelStatusDto input)
        {
            try
            {
                _productBondSecondaryService.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật giá
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("update-second-price")]
        
        public APIResponse UpdatePrice([FromBody] UpdateSecondaryPriceDto body)
        {
            try
            {
                _productBondSecondaryService.UpdatePrice(body);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// thay đổi trạng thái status của chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-policy-status")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_ChinhSach_KichHoatOrHuy)]
        public APIResponse ChangePolicyStatus(int id)
        {
            try
            {
                var result = _productBondSecondaryService.ChangePolicyStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thay đổi trạng thái status của kì hạn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-policy-detail-status")]
        [PermissionFilter(Permissions.Bond_BTKH_TTCT_KyHan_KichHoatOrHuy)]
        public APIResponse ChangePolicyDetailStatus(int id)
        {
            try
            {
                var result = _productBondSecondaryService.ChangePolicyDetailStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
