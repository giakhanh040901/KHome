using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class InvestOrderFilterDto : PagingRequestBaseDto
    {
        /// <summary>
        /// Loc theo Id
        /// </summary>
        public int? OrderId { get; set; }

        private string _policy;
        /// <summary>
        /// Nhiều loại chính sách cách nhau bởi dấu ,
        /// </summary>
        [FromQuery(Name = "policy")]
        public string Policy
        {
            get => _policy;
            set => _policy = value?.Trim();
        }

        [FromQuery(Name = "deliveryStatus")]
        public int? DeliveryStatus { get; set; }

        /// <summary>
        /// Trạng thái (1: khởi tạo, 2: chờ thanh toán, 3: chờ ký hợp đồng, 4: chờ duyệt hợp đồng, 5: đang đầu tư, 6: phong tỏa, 7: Giải tỏa, 8: tất toán, 9: đã hủy lệnh)
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        ///  Nguồn đặt lệnh = 1: Online, 2: Offline
        /// </summary>
        [FromQuery(Name = "source")]
        public int? Source { get; set; }

        /// <summary>
        /// Lọc theo ngày đặt lệnh
        /// </summary>
        [FromQuery(Name = "tradingDate")]
        public DateTime? TradingDate { get; set; }

        [FromQuery(Name = "pendingDate")]
        public DateTime? PendingDate { get; set; }

        [FromQuery(Name = "deliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [FromQuery(Name = "receivedDate")]
        public DateTime? ReceivedDate { get; set; }

        [FromQuery(Name = "finishedDate")]
        public DateTime? FinishedDate { get; set; }

        [FromQuery(Name = "date")]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Lọc theo ngày đầu tư
        /// </summary>
        [FromQuery(Name = "investDate")]
        public DateTime? InvestDate { get; set; }

        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _customerName;
        /// <summary>
        /// Tên khách hàng
        /// </summary>
        [FromQuery(Name = "customerName")]
        public string CustomerName
        {
            get => _customerName;
            set => _customerName = value?.Trim();
        }

        private string _contractCode;
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        [FromQuery(Name = "contractCode")]
        public string ContractCode
        {
            get => _contractCode;
            set => _contractCode = value?.Trim();
        }

        private string _cifCode;
        /// <summary>
        /// CifCode khách hàng
        /// </summary>
        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }

        /// <summary>
        ///  Lọc theo người tạo hợp đồng : 1: Quản trị viên, 2: Khách hàng, 3 Tư vấn viên
        /// </summary>
        [FromQuery(Name = "orderer")]
        public int? Orderer { get; set; }

        /// <summary>
        /// Lọc theo sản phẩm
        /// </summary>
        [FromQuery(Name = "distributionId")]
        public int? DistributionId { get; set; }
        
        /// <summary>
        /// Lọc theo chính sách
        /// </summary>
        [FromQuery(Name = "policyDetailId")]
        public int? PolicyDetailId { get; set; }

        private string _contractCodeGen;
        /// <summary>
        /// Mã hợp đồng
        /// </summary>
        [FromQuery(Name = "contractCodeGen")]
        public string ContractCodeGen
        {
            get => _contractCodeGen;
            set => _contractCodeGen = value?.Trim();
        }

        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }
        [FromQuery(Name = "tradingProviderId")]
        public int? TradingProviderId { get; set; }
        public int? GroupStatus { get; set; }
    }
}
