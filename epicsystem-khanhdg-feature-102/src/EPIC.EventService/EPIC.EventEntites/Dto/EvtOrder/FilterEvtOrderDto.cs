using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.EntitiesBase.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtOrder
{
    public class FilterEvtOrderDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// loc nhieu su kien
        /// </summary>
        [FromQuery(Name = "eventIds")]
        public List<int> EventIds { get; set; }

        /// <summary>
        /// loc nhieu trang thai
        /// </summary>
        [FromQuery(Name = "statuses")]
        public List<int> Statuses { get; set; }

        /// <summary>
        /// loc nhieu nguon dat lenh
        /// </summary>
        [FromQuery(Name = "orderers")]
        public List<int> Orderers { get; set; }

        /// <summary>
        /// Loc ban ghi order het thoi gian
        /// </summary>
        [FromQuery(Name = "timeOut")]
        public bool? TimeOut { get; set; }
       
        /// <summary>
        ///  Lọc theo người tạo hợp đồng : 1: Quản trị viên, 2: Khách hàng, 3 Tư vấn viên
        /// </summary>
        [FromQuery(Name = "orderer")]
        public int? Orderer { get; set; }

        /// <summary>
        /// lọc theo sự kiện
        /// </summary>
        [FromQuery(Name = "eventId")]
        public int? EventId { get; set; }

        /// <summary>
        /// lọc theo số điện thoại
        /// </summary>
        private string _phone;
        [FromQuery(Name = "phone")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        /// <summary>
        /// lọc theo tên sự kiện
        /// </summary>
        private string _eventName;
        [FromQuery(Name = "eventName")]
        public string EventName
        {
            get => _eventName;
            set => _eventName = value?.Trim();
        }

        /// <summary>
        /// lọc theo mã yêu cầu
        /// </summary>
        private string _contractCode;
        [FromQuery(Name = "contractCode")]
        public string ContractCode
        {
            get => _contractCode;
            set => _contractCode = value?.Trim();
        }

        /// <summary>
        /// lọc theo loại sự kiện (1: Khởi tạo, 2: Đang mở bán, 3: Tạm dừng, 4: Huỷ sự kiện, 5: Kết thúc)
        /// </summary>
        [FromQuery(Name = "eventStatus")]
        public int? EventStatus { get; set; }
    }

    public class FilterEvtOrderValidDto : FilterEvtOrderDto
    {
        /// <summary>
        /// loc cho trang thai tam khoa, hop le
        /// </summary>
        [FromQuery(Name = "isLock")]
        public bool? IsLock { get; set; }
    }

    public class FilterEvtOrderDeliveryTicketDto : FilterEvtOrderDto
    {
        /// <summary>
        /// loc nhieu loai su kien
        /// </summary>
        [FromQuery(Name = "eventType")]
        public List<int> EventType { get; set; }
        /// <summary>
        /// loc nhieu trang thai
        /// </summary>
        [FromQuery(Name = "deliveryStatus")]
        public List<int> DeliveryStatus { get; set; }
        /// <summary>
        /// Ngày thao tác
        /// </summary>
        [FromQuery(Name = "processingDate")]
        public DateTime? ProcessingDate { get; set; }
    }

    public class FilterEvtOrderDeliveryInvoiceDto : FilterEvtOrderDto
    {
        /// <summary>
        /// loc nhieu loai su kien
        /// </summary>
        [FromQuery(Name = "eventType")]
        public List<int> EventType { get; set; }
        /// <summary>
        /// loc nhieu trang thai
        /// </summary>
        [FromQuery(Name = "deliveryInvoiceStatus")]
        public List<int> DeliveryInvoiceStatus { get; set; }
        /// <summary>
        /// Ngày thao tác
        /// </summary>
        [FromQuery(Name = "processingInvoiceDate")]
        public DateTime? ProcessingInvoiceDate { get; set; }
    }
}
