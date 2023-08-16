using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.OperationalInfo
{
    public class OperationalInfoDto
    {
        /// <summary>
        /// Số lượng truy cập
        /// </summary>
        public int Visitors { get; set; }

        /// <summary>
        /// Số người bán
        /// </summary>
        public int Sellers { get; set; }

        /// <summary>
        /// Số người bán mới - tính từ đầu năm
        /// </summary>
        public int NewSellers { get; set; }

        /// <summary>
        /// Tổng số sản phẩm
        /// </summary>
        public int TotalProducts { get; set; }

        /// <summary>
        /// Số sản phẩm mới - tính từ đầu năm
        /// </summary>
        public int NewProducts { get; set; }

        /// <summary>
        /// Số lượng giao dịch
        /// </summary>
        public int Transactions { get; set; }

        /// <summary>
        /// Tổng số đơn hàng thành công
        /// </summary>
        public int SuccessfulOrders { get; set; }

        /// <summary>
        /// Tổng số đơn hàng không thành công
        /// </summary>
        public int FailedOrders { get; set; }

        /// <summary>
        /// Tổng giá trị giao dịch
        /// </summary>
        public long TotalTransactionValue { get; set; }
    }
}
