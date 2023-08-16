using EPIC.BondEntities.DataEntities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Order;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondEntities.Dto.AppOrder
{
    public class DanhSachChiTraDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Ngày chi trả
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// Kỳ chi trả
        /// </summary>
        public int PeriodIndex { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long OrderId { get; set; }
        
        /// <summary>
        /// Số tiền dự chi
        /// </summary>
        public decimal AmountMoney { get; set; }
        public string CifCode { get; set; }
        public int PolicyDetailId { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Ngày chi trả của kì trước
        /// </summary>
        public DateTime PreviousPeriodPayDate { get; set; }
        public int SoNgay { get; set; }

        /// <summary>
        /// Số tiền thực nhận
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Thuế TN
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// Lợi nhuận
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Có phải là kỳ cuối hay không? Y/N
        /// </summary>
        public string IsLastPeriod { get; set; }
        public DataEntities.BondOrder BondOrder { get; set; }
        public BondPolicyDetail PolicyDetail { get; set; }
    }
}
