using EPIC.RealEstateDomain.Implements;
using EPIC.RealEstateDomain.Interfaces;
using EPIC.RealEstateEntities.Dto.RstGenContractCode;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.RealEstateEntities.Dto.RstOrder;
using EPIC.RealEstateEntities.Dto.RstOrderContractFile;
using EPIC.RealEstateEntities.Dto.RstOrderCoOwner;
using EPIC.RealEstateSharedDomain.Interfaces;
using EPIC.Shared.Filter;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.RealEstateAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/real-estate/order")]
    [ApiController]
    public class RstOrderController : BaseController
    {
        private readonly IRstOrderServices _rstOrderServices;
        private readonly RstOrderContractFileServices _rstOrderContractFileServices;
        private readonly IRstContractCodeServices _rstContractCodeServices;
        public RstOrderController(ILogger<RstOrderController> logger,
            IRstOrderServices rstOrderServices,
            RstOrderContractFileServices rstOrderContractFileServices)
        {
            _logger = logger;
            _rstOrderServices = rstOrderServices;
            _rstOrderContractFileServices = rstOrderContractFileServices;
        }
        #region CMS
        /// <summary>
        /// Lấy tất cả sổ lệnh có trạng thái 1,2,3,4
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_DanhSach)]
        public APIResponse FindAll([FromQuery] FilterRstOrderDto input)
        {
            try
            {
                var result = _rstOrderServices.FindAll(input, new int[] { RstOrderStatus.KHOI_TAO, RstOrderStatus.CHO_THANH_TOAN_COC});
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThemMoi)]
        public async Task<APIResponse> Add([FromBody] CreateRstOrderDto input)
        {
            try
            {
                var result = await _rstOrderServices.Add(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinChung_ChinhSua)]
        public APIResponse Update([FromBody] UpdateRstOrderDto input)
        {
            try
            {
                var result = _rstOrderServices.Update(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật hình thức thanh toán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update/payment-type")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinChung_DoiHinhThucThanhToan)]
        public APIResponse UpdatePaymentType([FromBody] UpdateRstOrderPaymentTypeDto input)
        {
            try
            {
                _rstOrderServices.UpdatePaymentType(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật đồng sở hữu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update/co-owner")]
        public APIResponse UpdateOrderCoOwner([FromBody]  UpdateListRstOrderCoOwnerDto input)
        {
            try
            {
                _rstOrderServices.UpdateOrderCoOwner(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa hợp đồng ở trạng thái khởi tạo
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpDelete("deleted/{orderId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_Xoa)]
        public APIResponse Deleted(int orderId)
        {
            try
            {
                var result = _rstOrderServices.Deleted(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy sổ lệnh theo Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("find-by-id/{orderId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_ThongTinChung_ChiTiet)]
        public APIResponse FindOrderById(int orderId)
        {
            try
            {
                var result = _rstOrderServices.FindById(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xoá đồng sở hữu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpDelete("delete-co-owner/{id}")]
        public APIResponse DeleteCoOwner(int id, int orderId)
        {
            try
            {
                _rstOrderServices.DeleteCoOwner(id, orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm đồng sở hữu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-co-owner")]
        public APIResponse AddCoOwner([FromBody] CreateRstOrderCoOwnerDto input)
        {
            try
            {
                var result = _rstOrderServices.AddCoOwner(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thay đổi nguồn đặt lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("change-source/{orderId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_HSKHDangKy_ChuyenOnline)]
        public APIResponse ChangeSource(int orderId)
        {
            try
            {
                var result = _rstOrderServices.ChangeSource(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("approve/{orderId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_HSKHDangKy_DuyetHS)]
        [WhiteListIpFilter(WhiteListIpTypes.RstDuyetHopDong)]
        public async Task<APIResponse> OrderApprove(int orderId)
        {
            try
            {
                var result = await _rstOrderServices.OrderApprove(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        /// <summary>
        /// Huỷ duyệt hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("cancel/{orderId}")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_HSKHDangKy_HuyDuyetHS)]
        public APIResponse OrderCancel(int orderId)
        {
            try
            {
                var result = _rstOrderServices.OrderCancel(orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra order có trạng thái  4
        /// </summary>
        /// <returns></returns>
        [HttpGet("processing/find-all")]
        [PermissionFilter(Permissions.RealStateGDC_XLDC_DanhSach)]
        public APIResponse FindAllConTractProcessing([FromQuery] FilterRstOrderDto input)
        {
            try
            {
                var result = _rstOrderServices.FindAll(input, new int[] { RstOrderStatus.CHO_DUYET_HOP_DONG_COC });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy ra order có trạng thái  5
        /// </summary>
        /// <returns></returns>
        [HttpGet("active/find-all")]
        [PermissionFilter(Permissions.RealStateGDC_HDDC_DanhSach)]
        public APIResponse FindAllConTractActive([FromQuery] FilterRstOrderDto input)
        {
            try
            {
                var result = _rstOrderServices.FindAll(input, new int[] { RstOrderStatus.DA_COC });
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Gia hạn thêm thời gian giữ chỗ
        /// </summary>
        /// <returns></returns>
        [HttpPut("extended-keep-time")]
        [PermissionFilter(Permissions.RealStateMenuSoLenh_GiaHanGiuCho)]
        public APIResponse ExtendedKeepTime([FromBody] RstOrderExtendedKeepTimeDto input)
        {
            try
            {
                _rstOrderServices.ExtendedKeepTime(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion

        #region Hợp đồng
        /// <summary>
        /// Update file Scan hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("upload-file-scan")]
        [HttpPut]
        public APIResponse UploadFileScanContract([FromBody] RstUpdateOrderContractFileDto input)
        {
            try
            {
                _rstOrderContractFileServices.UpdateFileScanContract(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            { 
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update (file lại data) hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPut("update-contract-file/{orderId}")]
        public async Task<APIResponse> UpdateContractFile(int orderId)
        {
            try
            {
                await _rstOrderContractFileServices.UpdateContractFile(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
