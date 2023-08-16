using EPIC.GarnerDomain.Implements;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using EPIC.WebAPIBase.FIlters;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerProductPrice;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.DataEntities;

namespace EPIC.GarnerAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/garner/distribution")]
    [ApiController]
    public class GarnerDistributionController : BaseController
    {
        private readonly IGarnerDistributionServices _garnerDistributionServices;
        private readonly IGarnerContractTemplateServices _garnerContractTemplateServices;

        public GarnerDistributionController(ILogger<GarnerDistributionController> logger,
            IGarnerDistributionServices garnerDistributionServices, 
            IGarnerContractTemplateServices garnerContractTemplateServices)
        {
            _logger = logger;
            _garnerDistributionServices = garnerDistributionServices;
            _garnerContractTemplateServices = garnerContractTemplateServices;
        }

        /// <summary>
        /// Lấy danh sách phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.GarnerPPDT_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterGarnerDistributionDto input)
        {
            try
            {
                var result = _garnerDistributionServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// thay đổi close của distribution
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpPut("close/{distributionId}")]
        [PermissionFilter(Permissions.GarnerPPDT_DongOrMo)]

        public APIResponse DistributionClose(int distributionId)
        {
            try
            {
                _garnerDistributionServices.DistributionClose(distributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem thông tin chi tiết phân phối sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id")]
        [PermissionFilter(Permissions.GarnerPPDT_ChiTiet)]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _garnerDistributionServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary> 
        /// Lấy thông tin phân phối sản phẩm để đặt lệnh CMS theo Đại lý
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-all-distribution")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllDistribution([FromQuery] GarnerDistributionFilterDto input)
        {
            try
            {
                var result = _garnerDistributionServices.FindAllDistribution(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        public APIResponse FindBankByDistributionId(int distributionId, int type)
        {
            try
            {
                var result = _garnerDistributionServices.FindBankByDistributionId(distributionId, type);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chọn tài khoản chi tự động
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("choose-auto-account")]
        public APIResponse ChooseAutoAccount(int id)
        {
            try
            {
                var result = _garnerDistributionServices.ChooseAutoAccount(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy thông tin tổng quan của sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-product-overview")]
        [PermissionFilter(Permissions.GarnerPPDT_TongQuan)]
        [ProducesResponseType(typeof(APIResponse<GarnerProductOverviewDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindProductOverview(int id)
        {
            try
            {
                var result = _garnerDistributionServices.FindProductOverview(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.GarnerPPDT_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<GarnerDistribution>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromBody] CreateGarnerDistributionDto input)
        {
            try
            {
                var result = _garnerDistributionServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.GarnerPPDT_CapNhat)]
        public APIResponse Update([FromBody] UpdateGarnerDistributionDto input)
        {
            try
            {
                var result = _garnerDistributionServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật tổng quan sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-product-overview")]
        [PermissionFilter(Permissions.GarnerPPDT_TongQuan_CapNhat)]
        public APIResponse UpdateProductOverview([FromBody] UpdateGarnerProductOverviewDto input)
        {
            try
            {
                _garnerDistributionServices.UpdateProductOverview(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo yêu cầu trình duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("request")]
        [PermissionFilter(Permissions.GarnerPPDT_TrinhDuyet)]
        public APIResponse RequestProduct(CreateGarnerRequestDto input)
        {
            try
            {
                _garnerDistributionServices.Request(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("approve")]
        [PermissionFilter(Permissions.GarnerPDPPDT_PheDuyetOrHuy)]

        public APIResponse ApproveProduct(GarnerApproveDto input)
        {
            try
            {
                _garnerDistributionServices.Approve(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
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
        [HttpPut("cancel")]
        public APIResponse CancelProduct(GarnerCancelDto input)
        {
            try
            {
                _garnerDistributionServices.Cancel(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Check sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("check")]
        public APIResponse CheckProduct(GarnerCheckDto input)
        {
            try
            {
                _garnerDistributionServices.Check(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Show app 
        /// </summary>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        [HttpPut("show-app/{distributionId}")]
        [PermissionFilter(Permissions.GarnerPPDT_BatTatShowApp)]

        public APIResponse IsShowApp(int distributionId)
        {
            try
            {
                _garnerDistributionServices.IsShowApp(distributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// EPIC, ROOT_EPIC Xét phân phối sản phẩm làm mặc định 
        /// </summary>
        [HttpPut("set-default/{distributionId}")]
        public APIResponse SetDefault(int distributionId)
        {
            try
            {
                _garnerDistributionServices.SetDefault(distributionId);
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
        [PermissionFilter(Permissions.GarnerPPDT_ChinhSach_ThemMoi)]
        [ProducesResponseType(typeof(APIResponse<GarnerPolicyMoreInfoDto>), (int)HttpStatusCode.OK)]
        public APIResponse AddPolicy([FromBody] CreatePolicyDto input)
        {
            try
            {
                var policyDetailTemps = _garnerDistributionServices.AddPolicy(input);
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
        [PermissionFilter(Permissions.GarnerPPDT_KyHan_ThemMoi)]

        public APIResponse AddPolicyDetail([FromBody] CreatePolicyDetailDto input)
        {
            try
            {
                _garnerDistributionServices.AddPolicyDetail(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chính sách khi ở trạng thái tạm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-policy")]
        [PermissionFilter(Permissions.GarnerPPDT_ChinhSach_CapNhat)]
        public APIResponse UpdatePolicy([FromBody] UpdatePolicyDto input)
        {
            try
            {
                _garnerDistributionServices.UpdatePolicy(input);
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
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-policy-detail")]
        [PermissionFilter(Permissions.GarnerPPDT_KyHan_CapNhat)]

        public APIResponse UpdatePolicyDetail([FromBody] UpdatePolicyDetailDto input)
        {
            try
            {
                _garnerDistributionServices.UpdatePolicyDetail(input);
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
        [HttpDelete("delete-policy/{policyId}")]
        [PermissionFilter(Permissions.GarnerPPDT_ChinhSach_Xoa)]
        public APIResponse DeletePolicy(int policyId)
        {
            try
            {
                _garnerDistributionServices.DeletePolicy(policyId);
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
        [PermissionFilter(Permissions.GarnerPPDT_KyHan_Xoa)]

        public APIResponse DeletePolicyDetail(int policyDetailId)
        {
            try
            {
                _garnerDistributionServices.DeletePolicyDetail(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi trạng thái của chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpPut("policy/change-status")]
        [PermissionFilter(Permissions.GarnerPPDT_ChinhSach_KichHoatOrHuy)]
        public APIResponse ChangeStatusPolicy(int policyId)
        {
            try
            {
                var result = _garnerDistributionServices.UpdateStatusPolicy(policyId);
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
        [PermissionFilter(Permissions.GarnerPPDT_KyHan_KichHoatOrHuy)]

        public APIResponse ChangeStatusPolicyDetail(int policyDetailId)
        {
            try
            {
                var result = _garnerDistributionServices.UpdateStatusPolicyDetail(policyDetailId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra danh sách chính sách và kỳ hạn theo Distribution
        /// </summary>
        [HttpGet("find-policy/{distributionId}")]
        [PermissionFilter(Permissions.GarnerPPDT_ChinhSach_DanhSach)]
        [ProducesResponseType(typeof(APIResponse<List<GarnerPolicyMoreInfoDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindPolicyByDistribution(int distributionId,[FromQuery] GarnerDistributionFilterDto input)
        {
            try
            {
                var result = _garnerDistributionServices.GetAllPolicy(distributionId, input);
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
        [HttpGet("find-policy-by-id/{policyId}")]
        public APIResponse FindPolicyAndPolicyDetailByPolicyId(int policyId)
        {
            try
            {
                var result = _garnerDistributionServices.FindPolicyAndPolicyDetailByPolicyId(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
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
        [PermissionFilter(Permissions.GarnerPPDT_ChinhSach_BatTatShowApp)]
        public APIResponse PolicyIsShowApp(int policyId)
        {
            try
            {
                _garnerDistributionServices.PolicyIsShowApp(policyId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lây danh sách kỳ hạn theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpGet("policy-detail/find-by-policy")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerPolicyMoreInfoDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindPolicyDetailByPolicyId(int policyId)
        {
            try
            {
                var result = _garnerDistributionServices.GetByPolicyId(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách hợp đồng theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [HttpGet("contract-template/find-by-policy")]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse FindByPolicyId(int policyId)
        {
            try
            {
                var result = _garnerContractTemplateServices.FindAll(policyId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách distribution theo chính sách (Không phân trang)
        /// </summary>
        /// <returns></returns>
        [HttpGet("distribution/get-all")]
        [ProducesResponseType(typeof(APIResponse<List<GarnerDistributionDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllDistibution([FromQuery] GarnerDistributionFilterDto input)
        {
            try
            {
                var result = _garnerDistributionServices.GetAllDistribution(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Import file excel bảng giá
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ditributionId"></param>
        /// <returns></returns>
        [HttpPost("import-price-from-excel/{ditributionId}")]
        [PermissionFilter(Permissions.Garner_TTCT_BangGia_ImportExcelBG)]

        public APIResponse ReadFileExcel([Required(ErrorMessage = "Không tìm thấy đầu vào file dữ liệu! ")] IFormFile file, int ditributionId)
        {
            try
            {
                _garnerDistributionServices.ImportProductPrice(file, ditributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa bảng giá theo ditributionId
        /// </summary>
        /// <param name="ditributionId"></param>
        /// <returns></returns>
        [HttpDelete("delete-product-price/{ditributionId}")]
        [PermissionFilter(Permissions.Garner_TTCT_BangGia_XoaBangGia)]

        public APIResponse DeleteProductPrice([Required(ErrorMessage = " Id của phân phối sản phẩm không hợp lệ ")] int ditributionId)
        {
            try
            {
                _garnerDistributionServices.DeleteProductPrice(ditributionId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem bảng giá theo ditributionId
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-product-price")]
        [PermissionFilter(Permissions.Garner_TTCT_BangGia_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterGarnerProductPriceDto input)
        {
            try
            {
                var result = _garnerDistributionServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật giá
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-product-price")]
        [PermissionFilter(Permissions.GarnerPPDT_FileChinhSach_CapNhat)]
        public APIResponse UpdateProductPrice([FromBody] UpdateProductPriceDto input)
        {
            try
            {
                _garnerDistributionServices.UpdateProductPrice(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region File Chính sách
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("policy-file/add")]
        public APIResponse AddDistributionPolicyFile([FromBody] CreateGarnerProductOverviewFileDto input)
        {
            try
            {
                _garnerDistributionServices.AddDistributionPolicyFile(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("policy-file/update")]
        public APIResponse UpdateDistributionPolicyFile([FromBody] CreateGarnerProductOverviewFileDto input)
        {
            try
            {
                _garnerDistributionServices.UpdateDistributionPolicyFile(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("policy-file/delete/{id}")]
        public APIResponse DeleteDistributionPolicyFile(int id)
        {
            try
            {
                _garnerDistributionServices.DeleteDistributionPolicyFile(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// GEt All File chính sách bán theo kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("get-all-policy-file")]
        public APIResponse FindAllDistributionPolicyFile([FromQuery] FilterGarnerDistributionFileDto input)
        {
            try
            {
                var result = _garnerDistributionServices.FindAllDistributionFile(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Lịch sử
        [HttpGet("get-all-history")]
        public APIResponse FindAllHistory([FromQuery] FilterGarnerDistributionHistoryDto input)
        {
            try
            {
                var result = _garnerDistributionServices.FindAllDistributionHistory(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #endregion

        [HttpGet("find-list-ban-ho")]
        public APIResponse FindListBanHoByTrading()
        {
            try
            {
                var result = _garnerDistributionServices.FindAllDistributionBanHoByTrading();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
