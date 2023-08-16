using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    /// <summary>
    /// Kết quả tính toán
    /// </summary>
    public class GarnerInterestPaymentCalculateResultDto
    {
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        public string ContractCode { get; set; }

        /// <summary>
        /// Ngày tích lũy
        /// </summary>
        public DateTime? InvestDate { get; set; }
    }
}
