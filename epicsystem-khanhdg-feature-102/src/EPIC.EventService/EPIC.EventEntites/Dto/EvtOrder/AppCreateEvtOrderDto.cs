using EPIC.EventEntites.Dto.EvtOrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class AppCreateEvtOrderDto
    {
        /// <summary>
        /// Khung giờ 
        /// </summary>
        public int EventDetailId { get; set; }

        /// <summary>
        /// dia chi nhan ve
        /// </summary>
        public int? ContractAddressId { get; set; }

        /// <summary>
        /// Id Giấy tờ khách hàng
        /// </summary>
        public int InvestorIdenId { get; set; }
        /// <summary>
        /// Nhận vé bản cứng (Yes/No)
        /// </summary>
        public bool IsReceiveHardTicket { get; set; }
        /// <summary>
        /// Yêu cầu nhận hóa đơn (Yes/No)
        /// </summary>
        public bool IsRequestReceiveRecipt { get; set; }
        /// <summary>
        /// Mã giới thiệu đặt lệnh
        /// </summary>
        private string _saleReferralCode;
        public string SaleReferralCode
        {
            get => _saleReferralCode;
            set => _saleReferralCode = value?.Trim();
        }
        /// <summary>
        /// Chi tiết lệnh
        /// </summary>
        public List<CreateEvtOrderDetailDto> OrderDetails { get; set; }
    }

    public class InvoiceTicketRequestDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Nhận vé bản cứng (Yes/No)
        /// </summary>
        public bool IsReceiveHardTicket { get; set; }
        /// <summary>
        /// Yêu cầu nhận hóa đơn (Yes/No)
        /// </summary>
        public bool IsRequestReceiveRecipt { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public int ContractAddressId { get; set; }
    }
}
