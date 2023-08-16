using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class InterestPaymentFilterDto : PagingRequestBaseDto
    {

        [FromQuery(Name = "ngayChiTra")]
        public DateTime? NgayChiTra { get; set; }

        private string _contractCode;

        [FromQuery(Name = "contractCode")]
        public string ContractCode 
        {
            get => _contractCode; 
            set => _contractCode = value?.Trim(); 
        }

        private string _idNo;

        [FromQuery(Name = "idNo")]
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }

        private string _cifCode;

        [FromQuery(Name = "cifCode")]
        public string CifCode
        {
            get => _cifCode;
            set => _cifCode = value?.Trim();
        }

        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        private string _taxCode;
        [FromQuery(Name = "taxCode")]
        public string TaxCode
        {
            get => _taxCode;
            set => _taxCode = value?.Trim();
        }

        private string _isExactDate;
        /// <summary>
        /// Phạm vi lọc ngày: (Y/null: Lọc theo ngày chính xác. N: Lọc bằng ngày hoặc các ngày bé hơn)
        /// </summary>
        [FromQuery(Name = "isExactDate")]
        public string IsExactDate
        {
            get => _isExactDate;
            set => _isExactDate = value?.Trim();
        }

        /// <summary>
        /// Lọc trạng thái : 1 Lập chi, 2 đã chỉ trả (thủ công + tái tục)
        /// </summary>
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// Lọc trạng thái chi trả: 1 Lập chi, 5 chờ phản hồi, 2 chi tự động, 4 chi thủ công
        /// </summary>
        [FromQuery(Name = "interestPaymentStatus")]
        public int? InterestPaymentStatus { get; set; }

        /// <summary>
        /// Lọc có phải là kỳ cuối hay không khi đã lập chi (Y/N)
        /// </summary>
        [FromQuery(Name = "isLastPeriod")]
        public string IsLastPeriod { get; set; }
        /// <summary>
        /// Nếu là kỳ cuối lọc xem có loại cuối kỳ: 1 là đáo hạn, 2 tái tục
        /// </summary>
        [FromQuery(Name = "typeContractExpire")]
        public int? TypeContractExpire { get; set; }

        /// <summary>
        /// Phương thức tái tục: 1 Không tái tục, 2 tái tục gốc, 3 tái tục gốc và lợi nhuận
        /// </summary>
        [FromQuery(Name = "settlementMethod")]
        public int? SettlementMethod { get; set; }

        /// <summary>
        /// List id đại lý
        /// </summary>
        [FromQuery(Name = "tradingProviderIds")]
        public List<int> TradingProviderIds { get; set; }

        [FromQuery(Name = "projectId")]
        public int? ProjectId { get; set; }

        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn (1: có chi tiền, 2: không chi tiền)
        /// </summary>
        [FromQuery(Name = "methodInterest")]
        public int? MethodInterest { get; set; }
    }
}
