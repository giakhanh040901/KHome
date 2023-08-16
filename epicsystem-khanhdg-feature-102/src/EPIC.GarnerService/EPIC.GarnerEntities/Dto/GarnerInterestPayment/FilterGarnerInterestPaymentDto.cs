using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerInterestPayment
{
    public class FilterGarnerInterestPaymentDto : PagingRequestBaseDto
    {
        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone 
        { 
            get => _phone; 
            set => _phone = value?.Trim();
        }

        private string _cifCode;
        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }

        /// <summary>
        /// Nhóm trạng thái chi trả
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Trạng thái tri trả
        /// </summary>
        [FromQuery(Name = "interestPaymentStatus")]
        public int? InterestPaymentStatus { get; set; }

        /// <summary>
        /// Loại sản phẩm tích lũy
        /// </summary>
        [FromQuery(Name = "distributionId")]
        public int? DistributionId { get; set; }

        /// <summary>
        /// Id chính sách
        /// </summary>
        [FromQuery(Name = "policyId")]
        public int? PolicyId { get; set; }

        /// <summary>
        /// Ngày chi trả
        /// </summary>
        [FromQuery(Name = "payDate")]
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// Lọc theo ngày chính xác: Y, các ngày trước đó N
        /// </summary>
        [FromQuery(Name = "isExactDate")]
        public string IsExactDate { get; set; }

        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
    }
}
