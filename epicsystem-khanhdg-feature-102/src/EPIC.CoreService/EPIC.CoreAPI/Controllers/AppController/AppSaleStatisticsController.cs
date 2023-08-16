using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.DataAccess;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.SaleAppStatistical;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace EPIC.CoreAPI.Controllers.AppController
{
    /// <summary>
    /// Thống kê doanh số
    /// </summary>
    [Authorize]
    [Route("api/core/sale/statistics")]
    [ApiController]
    public class AppSaleStatisticsController : BaseController
    {
        private readonly ISaleServices _saleServices;

        public AppSaleStatisticsController(ILogger<AppSaleStatisticsController> logger, ISaleServices saleServices)
        {
            _logger = logger;
            _saleServices = saleServices;
        }

        /// <summary>
        /// Thống kê dữ liệu nhân sự của nút hệ thống trên sale app
        /// </summary>
        /// <returns></returns>
        [HttpGet("personnel")]
        public APIResponse FindNhanSuBySaleId([Range(1, int.MaxValue)] int? tradingProviderId, int? saleType, DateTime? startDate, DateTime? endDate, string keyword)
        {
            try
            {
                var result = _saleServices.StatisticPersonnelBySale(tradingProviderId, saleType, startDate, endDate, keyword?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem chi tiết thông tin nhân viên Sale trong đại lý
        /// </summary>
        [HttpGet("personnel-detail")]
        public APIResponse FindPersonnelSaleById(int saleId, int tradingProviderId)
        {
            try
            {
                var result = _saleServices.FindPersonnelSaleById(saleId, tradingProviderId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thống kê dữ liệu hợp đồng cho nút hợp đồng sale app 
        /// </summary>
        /// <returns></returns>
        [HttpGet("contract")]
        [ProducesResponseType(typeof(APIResponse<AppStatisticContractBySaleDto>), (int)HttpStatusCode.OK)]
        public APIResponse ThongKeHopDongSaleApp([FromQuery] AppContractOrderFilterDto input)
        {
            try
            {
                var result = _saleServices.ThongKeHopDongSaleApp(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tổng quan số liệu doanh số, hợp đồng... của Sale tại đại lý đấy
        /// </summary>
        /// <param name="tradingProviderId"></param>
        /// <param name="typeTime">Loại thời gian lọc biểu đồng W , M, Y : tuần tháng năm</param>
        /// <param name="filterNumberTime"> Số tuần số tháng số năm</param>
        /// <returns></returns>
        [HttpGet("overview")]
        public APIResponse AppSalerOverview([Range(1, int.MaxValue)] int tradingProviderId, string typeTime, int? filterNumberTime = null)
        {
            try
            {
                var result = _saleServices.AppSalerOverview(tradingProviderId, typeTime, filterNumberTime);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Màn thống kê doanh số của sale, xem của cá nhân hoặc xem của cả hệ thống
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("overview/turnover")]
        [ProducesResponseType(typeof(APIResponse<AppSaleProceedDto>), (int)HttpStatusCode.OK)]
        public APIResponse AppThongKeDoanhSo([FromQuery] AppSaleProceedFilterDto input)
        {
            try
            {
                var result = _saleServices.AppThongKeDoanhSo(input);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("view-order")]
        public APIResponse SaleViewOrder([Range(1, int.MaxValue)] int orderId, int projectType)
        {
            try
            {
                var result = _saleServices.SaleViewOrder(orderId, projectType);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
