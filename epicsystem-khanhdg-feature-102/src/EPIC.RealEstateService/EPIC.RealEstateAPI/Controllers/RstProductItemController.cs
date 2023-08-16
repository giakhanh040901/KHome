using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using EPIC.Utils.Net.MimeTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EPIC.Shared.Filter;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.RealEstateEntities.Dto.RstProductItemMaterialFile;
using EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/product-item")]
    [ApiController]
    public class RstProductItemController : BaseController
    {
        private readonly IRstProductItemServices _rstProductItemServices;

        private readonly IRstProductItemUtilityServices _rstProductItemUtilityServices;
        private readonly IRstProductItemMaterialFileService _rstProductItemMaterialFileService;
        private readonly IRstProductItemDesignDiagramFileService _rstProductItemDesignDiagramFileService;
        public RstProductItemController(ILogger<RstProductItemController> logger,
            IRstProductItemServices rstProductItemServices, IRstProductItemUtilityServices rstProductItemUtilityServices,
            IRstProductItemMaterialFileService rstProductItemMaterialFileService, IRstProductItemDesignDiagramFileService rstProductItemDesignDiagramFileService)
        {
            _logger = logger;
            _rstProductItemServices = rstProductItemServices;
            _rstProductItemUtilityServices = rstProductItemUtilityServices;
            _rstProductItemMaterialFileService = rstProductItemMaterialFileService;
            _rstProductItemDesignDiagramFileService = rstProductItemDesignDiagramFileService;
        }

        /// <summary>
        /// Tìm danh sách sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstProductItemDto input)
        {
            try
            {
                var result = _rstProductItemServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin hợp đồng mới nhất được tạo trong dự án
        /// </summary>
        [HttpGet("order-new-project/{projectId}")]
        public APIResponse InfoOrderNewInProject(int projectId)
        {
            try
            {
                var result = _rstProductItemServices.InfoOrderNewInProject(projectId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm dự án có thể phân phối cho đại lý (Lọc những căn đã phân phối cho đại lý trước đó)
        /// </summary>
        [HttpGet("can-distribution")]
        public APIResponse GetAllProductItemCanDistributionForTrading([FromQuery] FilterRstProductItemCanDistributionDto input)
        {
            try
            {
                var result = _rstProductItemServices.GetAllProductItemCanDistributionForTrading(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_ThemMoi)]
        public APIResponse Add([FromBody] CreateRstProductItemDto input)
        {
            try
            {
                var result = _rstProductItemServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật sản phẩm bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_ThongTinChung_CapNhat)]
        public APIResponse Update([FromBody] UpdateRstProductItemDto input)
        {
            try
            {
                var result = _rstProductItemServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái của căn hộ về khởi tạo
        /// </summary>
        /// <returns></returns>
        [HttpPut("reset-status/{productItemId}")]
        public async Task<APIResponse> ResetStatusProductItem(int productItemId)
        {
            try
            {
                await _rstProductItemServices.ResetStatusProductItem(productItemId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá sản phẩm bán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        public APIResponse Delete(int id)
        {
            try
            {
                _rstProductItemServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm sản phẩm bán theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{id}")]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _rstProductItemServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật mô tả sơ đồ thiết kế cho căn hộ
        /// </summary>
        [HttpPut("update-design-diagram-content")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_SoDoTK_CapNhat)]
        public APIResponse UpdateProductItemDesignDiagramContent([FromBody] UpdateRstProductItemDesignDiagramDto input)
        {
            try
            {
                _rstProductItemServices.UpdateProductItemDesignDiagramContent(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm file sơ đồ thiết kế
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-design-diagram-file")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_SoDoTK_CapNhat)]
        public APIResponse AddDesignDiagram([FromBody] CreateRstProductItemDesignDiagramFileDto input)
        {
            try
            {
                _rstProductItemDesignDiagramFileService.Add(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật file sơ đồ thiết kế
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-design-diagram-file")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_SoDoTK_CapNhat)]
        public APIResponse UpdateDesignDiagramFile([FromBody] UpdateRstProductItemDesignDiagramFileDto input)
        {
            try
            {
                _rstProductItemDesignDiagramFileService.Update(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa file sơ đồ thiết kế
        /// </summary>
        /// <param name="id">Id file</param>
        /// <returns></returns>
        [HttpDelete("delete-design-diagram-file/{id}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_SoDoTK_CapNhat)]
        public APIResponse DeleteDesignDiagramFile(int id)
        {
            try
            {
                _rstProductItemDesignDiagramFileService.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// get danh sách file sơ đồ thiết kế
        /// </summary>
        /// <param name="productItemId">Id căn hộ</param>
        /// <returns></returns>
        [HttpGet("get-design-diagram-file/{productItemId}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_SoDoTK_CapNhat)]
        public APIResponse GetAllDesignDiagramFile(int productItemId)
        {
            try
            {
                var result = _rstProductItemDesignDiagramFileService.GetAll(productItemId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật vật liệu thi công cho căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-material-content")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_VatLieu_CapNhat)]
        public APIResponse UpdateProductItemMaterialContent([FromBody] UpdateRstProductItemMaterialDto input)
        {
            try
            {
                _rstProductItemServices.UpdateProductItemMaterialContent(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm file vật liệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-material-file")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_VatLieu_CapNhat)]
        public APIResponse AddMaterialFile([FromBody] CreateRstProductItemMaterialFileDto input)
        {
            try
            {
                _rstProductItemMaterialFileService.AddMaterialFile(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật file vật liệu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update-material-file")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_VatLieu_CapNhat)]
        public APIResponse UpdateMaterialFile([FromBody] UpdateRstProductItemMaterialFileDto input)
        {
            try
            {
                _rstProductItemMaterialFileService.UpdateMaterialFile(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa file vật liệu
        /// </summary>
        /// <param name="id">Id file</param>
        /// <returns></returns>
        [HttpDelete("delete-material-file/{id}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_VatLieu_CapNhat)]
        public APIResponse DeleteMaterialFile(int id)
        {
            try
            {
                _rstProductItemMaterialFileService.Delete(id);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// get danh sách file vật liệu
        /// </summary>
        /// <param name="productItemId">Id căn hộ</param>
        /// <returns></returns>
        [HttpGet("get-material-file/{productItemId}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_VatLieu_CapNhat)]
        public APIResponse GetAllMaterialFile(int productItemId)
        {
            try
            {
                var result = _rstProductItemMaterialFileService.FindAll(productItemId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm tiện ích căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("add-utility")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_TienIch_CapNhat)]
        public APIResponse AddProductItemProjectUitlity([FromBody] CreateRstProductItemUtilityDto input)
        {
            try
            {
                _rstProductItemServices.AddProductItemProjectUtility(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách tiện ích được chọn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-utility")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_TienIch)]
        public APIResponse FindAllProductItemProjectUitlity([FromQuery] FilterProductItemProjectUtilityDto input)
        {
            try
            {
                var result = _rstProductItemServices.FindAllProjectUtility(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách tiện ích được chọn
        /// </summary>
        /// <param name="input"></param>
        /// <param name="productItemId"></param>
        /// <returns></returns>
        [HttpGet("find-all-selected")]
        public APIResponse FindAllSelectedProductItemProjectUitlity([FromQuery] FilterProductItemProjectUtilityDto input, int productItemId)
        {
            try
            {
                var restul = _rstProductItemServices.FindAllProjectUtilitySelected(input, productItemId);
                return new APIResponse(Utils.StatusCode.Success, restul, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///Thay đổi trạng thái tiện ích căn hộ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status-utility/{id}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_TienIch_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusProductItemProjectUitlity(int id)
        {
            try
            {
                _rstProductItemServices.ChangeStatusProductItemUtility(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm chính sách ưu đãi
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("add-policy")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_ChinhSach_CapNhat)]
        public APIResponse AddProductItemProjectPolicy([FromBody] CreateRstProductItemProjectPolicyDto input)
        {
            try
            {
                _rstProductItemServices.AddProductItemProjectPolicy(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get danh sách chính sách được chọn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all-policy")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_ChinhSach)]
        public APIResponse FindAllProductItemProjectPolicy([FromQuery] FilterProductItemProjectPolicyDto input)
        {
            try
            {
                var result = _rstProductItemServices.FindAllProjectPolicy(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        ///Thay đổi trạng thái chính sách ưu đãi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("change-status-policy/{id}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_ChinhSach_DoiTrangThai)]
        [ProducesResponseType(typeof(APIResponse), (int)HttpStatusCode.OK)]
        public APIResponse ChangeStatusProductItemProjectPolicy(int id)
        {
            try
            {
                _rstProductItemServices.ChangeStatusProductItemPolicy(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Khoá căn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("lock-product-item")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_KhoaCan)]
        public APIResponse LockProductItem([FromBody] RstProductItemLockingDto input)
        {
            try
            {
                _rstProductItemServices.LockProductItem(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Nhân bản căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("replicate-product-item")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_NhanBan)]
        public APIResponse ReplicateProductItem([FromBody] CreateRstListProductItemReplicationDto input)
        {
            try
            {
                var result = _rstProductItemServices.ReplicateProductItem(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Đổi trạng thái căn hộ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("change-status")]
        public APIResponse ChangeStatus(int id, int status)
        {
            try
            {
                var result = _rstProductItemServices.ChangeStatus(id, status);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm căn hộ từ file excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("import")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_UploadFile)]
        public APIResponse ImportExcelProductItem([FromForm] ImportExcelProductItemDto dto)
        {
            try
            {
                _rstProductItemServices.ImportExcelProductItem(dto);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Mẫu file import
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("import-template/{projectId}")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_TaiFileMau)]
        [AllowAnonymous]
        public IActionResult ImportFileTemplate(int projectId)
        {
            try
            {
                var result = _rstProductItemServices.ImportFileTemplate(projectId);
                return File(result.fileData, MimeTypeNames.ApplicationOctetStream, result.fileDownloadName);
            }
            catch (Exception ex)
            {
                return Ok(OkException(ex));
            }
        }

        /// <summary>
        /// lấy tất cả thông tin của tiện ích theo căn hộ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("utility/find-all")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_TienIch)]
        public APIResponse ProductItemUtilityFindAll([FromQuery] FilterProductItemProjectUtilityDto input)
        {
            try
            {
                var result = _rstProductItemUtilityServices.FindAll(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// cập nhập tiện ích của căn hộ
        /// </summary>
        /// <param name="input"></param>s
        /// <returns></returns>
        [HttpPut("utility/update")]
        [PermissionFilter(Permissions.RealStateMenuProjectListDetail_TienIch_CapNhat)]
        public APIResponse UpdateProductItemUtility(CreateRstProductItemUtilityDto input)
        {
            try
            {
                var result = _rstProductItemUtilityServices.UpdateProductItemUtility(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa tiện tích của căn hộ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("utility/delete")]
        public APIResponse DeleteProductItem(int id)
        {
            try
            {
                _rstProductItemUtilityServices.DeleteProductItemUtility(id);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
