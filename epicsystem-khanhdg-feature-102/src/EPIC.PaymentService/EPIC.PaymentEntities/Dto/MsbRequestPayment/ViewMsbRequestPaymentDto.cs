using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EPIC.PaymentEntities.Dto.MsbRequestPayment
{
    public class ViewMsbRequestPaymentDto
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public int ProductType { get; set; }
        public int RequestType { get; set; }
        public int DataType { get; set; }
        public long ReferId { get; set; }
        public int Status { get; set; }
        public List<ViewGarnerOrderDto> GarnerOrders { get; set; }
        public decimal AmountMoney { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string Exception { get; set; }
        public string Bin { get; set; }
        public string Note { get; set; }
        public string OwnerAccount { get; set; }
        public string OwnerAccountNo { get; set; }
        /// <summary>
        /// Ngày tạo yêu cầu hay ngày tạo bản ghi ở bảng request payment detail
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Ngày phản hồi của notify bank hay ngày tạo bản ghi notify
        /// </summary>
        public DateTime? ResponseDate { get; set; }
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Ngày yêu cầu rút vốn
        /// </summary>
        public DateTime? WithdrawalDate { get; set; }
        public string ContractCode { get; set; }
        public ViewInvestOrderDto InvestOrder { get; set; } // OrderInvest
    }

    public class ViewInvestOrderDto
    {
        public long Id { get; set; }
        public string ContractCode { get; set; }
        public string GenContractCode { get; set; }
        public DateTime? ApproveDate { get; set; }
    }

    public class ViewGarnerOrderDto
    {
        public long Id { get; set; }
        public string ContractCode { get; set; }
        public string GenContractCode { get; set; }
        public DateTime? ApproveDate { get; set; }
    }
}
