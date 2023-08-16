using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppSalerOverviewDto
    {
        /// <summary>
        /// Doanh số cả hệ thống
        /// </summary>
        public decimal TotalValueSystem { get; set; }

        /// <summary>
        /// Doanh số cả hệ thống hôm nay
        /// </summary>
        public decimal TotalValueSystemToday { get; set; }
        /// <summary>
        /// Ngày vào đại lý
        /// </summary>
        public DateTime? SignDate { get; set; }

        /// <summary>
        /// Tổng doanh số của sale
        /// </summary>
        public decimal? TotalValueMoney { get; set; }
        /// <summary>
        /// Tổng doanh số trong tháng của Sale
        /// </summary>
        public decimal? TotalValueMoneyMonth { get; set; }
        /// <summary>
        /// Tổng hợp đồng 
        /// </summary>
        public int? TotalContract { get; set; }
        /// <summary>
        /// Tổng số hợp đồng trong tháng
        /// </summary>
        public int? TotalContractMonth { get; set; }
        /// <summary>
        /// Tổng số khách hàng
        /// </summary>
        public int? TotalCustomer { get; set; }
        /// <summary>
        /// Tổng số khác hàng mới trong tháng
        /// </summary>
        public int? TotalCustomerMonth { get; set; }
        /// <summary>
        /// Tổng số sale cấp dưới
        /// </summary>
        public decimal? TotalSalerChild { get; set; }
        /// <summary>
        /// Tổng số sale cấp dưới mới trong tháng
        /// </summary>
        public decimal? TotalSalerChildMonth { get; set; }

        /// <summary>
        /// Sale có hợp đồng của sản phẩm invest không Y/N
        /// </summary>
        public string HasContractInvestSub { get; set; }

        /// <summary>
        /// Danh sách các loại hình sản phẩm của đại lý đang có: 1: BOND, 2 INVEST, 3 COMPANY)SHARES, 4: GARNER, 5: REAL_ESTATE
        /// </summary>
        public List<int> ProductTypes { get; set; }

        /// <summary>
        /// Thống kê top 10 sale cấp dưới có doanh số cao nhất
        /// </summary>
        public List<AppTopSaleDto> TopSaleChilds { get; set; }

        /// <summary>
        /// Thông kê biểu đồ doanh số theo ngày
        /// </summary>
        public List<AppStatisticOrderInDay> StatisticOrderInDay { get; set; }
    }
}
