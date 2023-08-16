using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto;
using EPIC.CompanySharesEntities.Dto.CpsSecondary;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.CompanyShares;
using EPIC.Utils.Controllers;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/company-shares/secondary")]
    [ApiController]
    public class CpsSecondaryController : BaseController
    {
        private readonly ICpsSecondaryServices _cpsSecondaryService;
        public CpsSecondaryController(ILogger<CpsSecondaryController> logger, ICpsSecondaryServices cpsSecondaryService)
        {
            _logger = logger;
            _cpsSecondaryService = cpsSecondaryService;
        }

        /// <summary>
        /// Tạo mới bán theo kỳ hạn ở trạng thái tạm
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPost("add")]
        //[PermissionFilter(Permissions.CpsMenuQLTP_BTKH_ThemMoi)]
        public APIResponse AddSecondary([FromBody] CreateCpsSecondaryDto body)
        {
            try
            {
                var result = _cpsSecondaryService.Add(body);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_ThemMoi)]
        public APIResponse AddPolicy([FromBody] CreateCpsPolicySpecificDto body)
        {
            try
            {
                var policyDetailTemps = _cpsSecondaryService.AddPolicy(body);
                return new APIResponse(Utils.StatusCode.Success, policyDetailTemps, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        [HttpPost("add-list-policy")]

        public APIResponse AddPolicySecondary(int policytempId, List<int> cpssecondaryId)
        {
            try
            {
                var policyDetailTemps = _cpsSecondaryService.AddPolicySecondary(policytempId, cpssecondaryId);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_ThemMoi)]
        public APIResponse AddPolicyDetail([FromBody] CreateCpsPolicyDetailDto body)
        {
            try
            {
                _cpsSecondaryService.AddPolicyDetail(body);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_CapNhat)]
        public APIResponse UpdateSecondary([FromBody] UpdateCpsSecondaryDto body)
        {
            try
            {
                _cpsSecondaryService.Update(body);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_CapNhat)]
        public APIResponse UpdatePolicy(int policyId, [FromBody] UpdateCpsPolicyDto body)
        {
            try
            {
                _cpsSecondaryService.UpdatePolicy(policyId, body);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_Xoa)]
        public APIResponse DeletePolicy(int policyId)
        {
            try
            {
                _cpsSecondaryService.DeletePolicy(policyId);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_CapNhat)]
        public APIResponse UpdatePolicyDetail(int policyDetailId, [FromBody] UpdateCpsPolicyDetailDto body)
        {
            try
            {
                _cpsSecondaryService.UpdatePolicyDetail(policyDetailId, body);
                return new APIResponse(Utils.StatusCode.Success, body, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("find-all-policy-detail/{policyId}")]
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_DanhSach)]
        public APIResponse FindAllPolicyDetail(int policyId)
        {
            try
            {
                var result = _cpsSecondaryService.GetAllListPolicyDetailByPolicy(policyId);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_Xoa)]
        public APIResponse DeletePolicyDetail(int policyDetailId)
        {
            try
            {
                _cpsSecondaryService.DeletePolicyDetail(policyDetailId);
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
        /// lọc theo kỳ hạn, chính sách đang trong trạng thái hoạt động và đang được mở bán
        /// </summary>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpGet("find")]
        //[PermissionFilter(Permissions.CpsMenuQLTP_BTKH_DanhSach)]
        public APIResponse FindAllSecondary(int pageNumber, int? pageSize, string keyword, int? status, bool isActivate, string isClose)
        {
            try
            {
                var result = _cpsSecondaryService.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status, isActivate, isClose);
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
        //[PermissionFilter(Permissions.CpsHDPP_SoLenh_DanhSach)]
        public APIResponse FindAllOrder()
        {
            try
            {
                var result = _cpsSecondaryService.FindAllOrder();
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
        //[PermissionFilter(Permissions.CpsMenuQLTP_BTKH_ThongTinChiTiet)]

        public APIResponse FindById(int id)
        {
            try
            {
                var result = _cpsSecondaryService.FindById(id);
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
        /// <param name="cpsSecondaryId"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("trading-provider-submit/{cpsSecondaryId}")]
        public APIResponse TradingProviderSubmit(int cpsSecondaryId)
        {
            try
            {
                _cpsSecondaryService.TradingProviderSubmit(cpsSecondaryId);
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
        /// <param name="cpsSecondaryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.TRADING_PROVIDER })]
        [HttpPut("trading-provider-approve/{cpsSecondaryId}")]
        public APIResponse TradingProviderApprove(
            int cpsSecondaryId,
            [IntegerRange(AllowableValues = new int[] {
                CpsSecondaryStatus.TEMP, CpsSecondaryStatus.TRADING_PROVIDER_APPROVE
            })]
            int status)
        {
            try
            {
                _cpsSecondaryService.TradingProviderApprove(cpsSecondaryId, status);
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
        /// <param name="cpsSecondaryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("super-admin-approve/{cpsSecondaryId}")]
        public APIResponse SuperAdminApprove(
            int cpsSecondaryId,
            [IntegerRange(AllowableValues = new int[] {
                CpsSecondaryStatus.SUPER_ADMIN_APPROVE
            })]
            int status)
        {
            try
            {
                _cpsSecondaryService.SuperAdminApprove(cpsSecondaryId, status);
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
        /// <param name="cpsSecondaryId"></param>
        /// <param name="isClose"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpPut("is-close/{cpsSecondaryId}")]
        //[PermissionFilter(Permissions.CpsMenuQLTP_BTKH_DongTam)]
        public APIResponse IsCancel(
            int cpsSecondaryId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isClose)
        {
            try
            {
                _cpsSecondaryService.IsClose(cpsSecondaryId, isClose);
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
        /// <param name="cpsSecondaryId"></param>
        /// <param name="isShowApp"></param>
        /// <returns></returns>
        //[AuthorizeUserTypeFilter(new string[] { UserData.SUPER_ADMIN, UserData.TRADING_PROVIDER })]
        [HttpPut("is-show-app/{cpsSecondaryId}")]
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse IsShowApp(
            int cpsSecondaryId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isShowApp)
        {
            try
            {
                _cpsSecondaryService.IsShowApp(cpsSecondaryId, isShowApp);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse PolicyIsShowApp(
            int policyId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isShowApp)
        {
            try
            {
                _cpsSecondaryService.PolicyIsShowApp(policyId, isShowApp);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        public APIResponse PolicyDetailIsShowApp(
            int policyDetailId,
            [Required][StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })] string isShowApp)
        {
            try
            {
                _cpsSecondaryService.PolicyDetailIsShowApp(policyDetailId, isShowApp);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        //[HttpPost("import-price-from-excel/{cpsSecondaryId}")]
        ////[PermissionFilter(Permissions.Cps_BTKH_TTCT_BangGia_ImportExcelBG)]
        //public APIResponse ReadFileExcel([Required(ErrorMessage = "Không tìm thấy đầu vào file dữ liệu! ")][FromForm] IFormFile file, int cpsSecondaryId)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.ImportSecondPrice(file, cpsSecondaryId);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        //[HttpDelete("delete-second-price/{cpsSecondaryId}")]
        ////[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_BatTatShowApp)]
        //public APIResponse DeleteSecondPrice([Required(ErrorMessage = " Id của phát hành thứ cấp không hợp lệ ")] int cpsSecondaryId)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.DeleteSecondPrice(cpsSecondaryId);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        //[HttpGet("find-second-price")]
        ////[PermissionFilter(Permissions.Cps_BTKH_TTCT_BangGia_DanhSach)]
        //public APIResponse FindAllSecondPrice(int pageNumber, int? pageSize, int cpsSecondaryId)
        //{
        //    try
        //    {
        //        var result = _cpsSecondaryService.FindAll(pageSize ?? 100, pageNumber, cpsSecondaryId);
        //        return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        ///// <summary>
        ///// Tạo yêu cầu duyệt
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("request")]
        ////[PermissionFilter(Permissions.CpsMenuQLTP_BTKH_TrinhDuyet)]
        //public APIResponse AddRequest([FromBody] RequestStatusDto input)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.Request(input);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        ///// <summary>
        ///// Duyệt bán theo kỳ hạn
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("approve")]
        ////[PermissionFilter(Permissions.CpsQLPD_PDBTKH_PheDuyetOrHuy)]
        //public APIResponse Approve(ApproveStatusDto input)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.Approve(input);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        ///// <summary>
        ///// Epic xác minh
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("check")]
        ////[PermissionFilter(Permissions.CpsMenuQLTP_BTKH_EpicXacMinh)]
        //public APIResponse Check([FromBody] CheckStatusDto input)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.Check(input);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Xác minh thành công");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        ///// <summary>
        ///// Hủy duyệt
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("cancel")]
        ////[PermissionFilter(Permissions.CpsQLPD_PDBTKH_PheDuyetOrHuy)]
        //public APIResponse Cancel([FromBody] CancelStatusDto input)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.Cancel(input);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        ///// <summary>
        ///// Cập nhật giá
        ///// </summary>
        ///// <param name="body"></param>
        ///// <returns></returns>
        //[HttpPut("update-second-price")]

        //public APIResponse UpdatePrice([FromBody] UpdateSecondaryPriceDto body)
        //{
        //    try
        //    {
        //        _cpsSecondaryService.UpdatePrice(body);
        //        return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        return OkException(ex);
        //    }
        //}

        /// <summary>
        /// thay đổi trạng thái status của chính sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-policy-status")]
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_ChinhSach_KichHoatOrHuy)]
        public APIResponse ChangePolicyStatus(int id)
        {
            try
            {
                var result = _cpsSecondaryService.ChangePolicyStatus(id);
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
        //[PermissionFilter(Permissions.Cps_BTKH_TTCT_KyHan_KichHoatOrHuy)]
        public APIResponse ChangePolicyDetailStatus(int id)
        {
            try
            {
                var result = _cpsSecondaryService.ChangePolicyDetailStatus(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
