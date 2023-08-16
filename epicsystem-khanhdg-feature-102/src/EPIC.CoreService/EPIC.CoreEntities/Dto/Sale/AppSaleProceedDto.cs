using EPIC.Entities.Dto.Sale;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class AppSaleProceedFilterDto
    {
        [Range(1, int.MaxValue)]
        [FromQuery(Name = "tradingProviderId")]
        public int TradingProviderId { get; set; }

        /// <summary>
        /// 1: BOND, 2 INVEST, 3 COMPANY)SHARES, 4: GARNER, 5: REAL_ESTATE
        /// </summary>
        [Range(1, int.MaxValue)]
        [FromQuery(Name = "project")]
        public int? Project { get; set; }


        [FromQuery(Name = "isOverview")]
        public bool IsOverview { get; set; }

        /// <summary>
        /// Ngày lọc
        /// </summary>
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        private string _keyword;
        /// <summary>
        /// Tìm kiếm doanh số theo Tên hoặc mã giới thiệu
        /// </summary>
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }
    }

    public class AppSaleProceedDto
    {
        public DateTime? SignDate { get; set; }
        /// <summary>
        /// Tổng doanh số của sale
        /// </summary>
        public decimal TotalValueMoney { get; set; }

        /// <summary>
        /// Số dư
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// Tổng hợp đồng 
        /// </summary>
        public int TotalContract { get; set; }
        /// <summary>
        /// Tổng số khách hàng
        /// </summary>
        public int TotalCustomer { get; set; }

        /// <summary>
        /// REAL_ESTATE: Tổng số căn hộ
        /// </summary>
        public int RstTotalProductItem { get; set; }

        /// <summary>
        /// Top 5 Sale có doanh số cao nhất khi xem Hệ thống
        /// </summary>
        public List<AppTopSaleDto> TopSales { get; set; }

        /// <summary>
        /// Top 5 Khach hàng đầu tư cao nhất khi xem Cá nhân
        /// </summary>
        public List<AppTopInvestorOrderDto> TopInvestors { get; set; }

        /// <summary>
        /// Thông kê doanh số hợp đồng
        /// </summary>
        public List<AppStatisticOrderOfSaleDto> StatisticOrders { get; set; }
    }
}
