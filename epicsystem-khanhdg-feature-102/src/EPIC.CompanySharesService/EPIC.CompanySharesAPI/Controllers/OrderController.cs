using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using EPIC.FileEntities.Settings;
using EPIC.Notification.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.Dto.Order;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.CompanySharesEntities.Dto.OrderContractFile;

namespace EPIC.CompanySharesAPI.Controllers
{
    [Authorize]
    [Route("api/company-shares/order")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly NotificationServices _sendEmailServices;
        private readonly IConfiguration _configuration;
        private readonly IOptions<FileConfig> _fileConfig;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderServices orderServices,
            NotificationServices sendEmailServices,
            IConfiguration configuration,
            IOptions<FileConfig> fileConfig)
        {
            _logger = logger;
            _orderServices = orderServices;
            _sendEmailServices = sendEmailServices;
            _configuration = configuration;
            _fileConfig = fileConfig;
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái đang đầu tư
        /// </summary>
        /// <returns></returns>
        [Route("find-active")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondHDPP_HopDong_DanhSach)]
        public APIResponse FindAllOrderActive(int pageNumber, int? pageSize, string keyword, string taxCode, string idNo, string cifCode, string phone, int? status, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.DANG_DAU_TU, source, cpsSecondaryId, cpsPolicy?.Trim(), cpsPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, null, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// tim kiếm giao nhận hợp đồng
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="taxCode"></param>
        /// <param name="idNo"></param>
        /// <param name="cifCode"></param>
        /// <param name="phone"></param>
        /// <param name="status"></param>
        /// <param name="source"></param>
        /// <param name="cpsSecondaryId"></param>
        /// <param name="cpsPolicy"></param>
        /// <param name="cpsPolicyDetailId"></param>
        /// <param name="customerName"></param>
        /// <param name="contractCode"></param>
        /// <param name="tradingDate"></param>
        /// <param name="deliveryStatus"></param>
        /// <returns></returns>
        [Route("find-delivery-status")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondHDPP_GiaoNhanHopDong_DanhSach)]
        public APIResponse FindAllDeliveryStatus(int pageNumber, int? pageSize, string keyword, string taxCode, string idNo, string cifCode, string phone, int? status, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus)
        {
            try
            {
                var result = _orderServices.FindAllDeliveryStatus(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.DANG_DAU_TU, source, cpsSecondaryId, cpsPolicy?.Trim(), cpsPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, deliveryStatus);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm trong trạng thái đang xử lý hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find-contract-processing")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondHDPP_XLHD_DanhSach)]
        public APIResponse FindAllConTractProcessing(int pageNumber, int? pageSize, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.XU_LY_HOP_DONG, source, cpsSecondaryId, cpsPolicy?.Trim(), cpsPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, null, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái khởi tạo và chờ ký hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find")]
        [HttpGet]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_DanhSach, Permissions.BondHDPP_GiaoNhanHopDong_TTC)]
        public APIResponse FindAllOrder(int pageNumber, string taxCode, string idNo, string cifCode, string phone, int? pageSize, string keyword, int? status, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.SO_LENH, source, cpsSecondaryId, cpsPolicy?.Trim(), cpsPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, deliveryStatus, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái phong tỏa hoặc giải tỏa
        /// </summary>
        /// <returns></returns>
        [Route("find-cancel")]
        [HttpGet]
        public APIResponse FindAllCancel(int pageNumber, string taxCode, string idNo, string cifCode, string phone, int? pageSize, string keyword, int? status, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.PHONG_TOA, source, cpsSecondaryId, cpsPolicy?.Trim(), cpsPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, null, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sổ lệnh trong trạng thái giao nhận hợp đồng
        /// </summary>
        /// <returns></returns>
        [Route("find-delivery-contract")]
        [HttpGet]

        public APIResponse FindAllDeliveryContract(int pageNumber, string taxCode, string idNo, string cifCode, string phone, int? pageSize, string keyword, int? status, int? source, int? cpsSecondaryId, string cpsPolicy, int? cpsPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer)
        {
            try
            {
                var result = _orderServices.FindAll(pageSize ?? 100, pageNumber, taxCode, idNo, cifCode, phone, keyword?.Trim(), status, OrderGroupStatus.SO_LENH, source, cpsSecondaryId, cpsPolicy?.Trim(), cpsPolicyDetailId, customerName?.Trim(), contractCode?.Trim(), tradingDate, deliveryStatus, orderer);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm lệnh theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        //[PermissionFilter(Permissions.Bond_HDPP_TTC_ChiTiet)]
        public APIResponse GetOrder(int id)
        {
            try
            {
                var result = _orderServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tạo lệnh offline, sinh mã hợp đồng, dò cif code có nằm trong tập khách hàng của partner không
        /// các điều kiện where kèm theo partner id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_ThemMoi)]
        public APIResponse AddOrder([FromBody] CreateOrderDto body)
        {
            try
            {
                //var result = await _orderServices.Add(body);
                var result = _orderServices.Add(body);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_TTCT_TTC_CapNhat)]
        public APIResponse UpdateOrder([FromBody] UpdateOrderDto body, int orderId)
        {
            try
            {
                var result = _orderServices.Update(body, orderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// xóa order theo trạng thái khởi tạo
        /// </summary>
        /// <returns></returns>
        [Route("order/delete/{id}")]
        [HttpDelete]
        //[PermissionFilter(Permissions.BondHDPP_SoLenh_Xoa)]
        public APIResponse Delete(int id)
        {
            try
            {
                var result = _orderServices.Delete(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        #region hợp đồng
        /// <summary>
        /// Update (file lại data) hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("update-contract-file")]
        [HttpPut]
        //[PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_CapNhatHS)]
        public async Task<APIResponse> UpdateContractFile(int orderId)
        {
            try
            {
                await _orderServices.UpdateContractFile(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Ký điện tử
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("sign-contract-file")]
        [HttpPut]
        //[PermissionFilter(Permissions.InvestHDPP_SoLenh_TTCT_HSKHDangKy_KyDienTu)]

        public APIResponse SignContractFile(int orderId)
        {
            try
            {
                _orderServices.UpdateContractFileSignPdf(orderId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Success");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm file scan hợp đồng
        /// </summary>
        /// <returns></returns>
        [HttpPost("order-contract-file/add")]
        public APIResponse OrderContractFileAdd([FromBody] CreateOrderContractFileDto input)
        {
            try
            {
                var result = _orderServices.CreateOrderContractFileScan(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Update đường dẫn file scan
        /// </summary>
        /// <returns></returns>
        [HttpPut("order-contract-file/update")]
        public APIResponse OrdeContractFileUpdate([FromBody] UpdateOrderContractFileDto input)
        {
            try
            {
                var result = _orderServices.UpdateOrderContractFileScan(input);
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
