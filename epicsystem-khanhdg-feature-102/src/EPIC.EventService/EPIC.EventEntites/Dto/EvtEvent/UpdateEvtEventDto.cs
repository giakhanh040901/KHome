using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class UpdateEvtEventDto : CreateEvtEventDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Địa điểm tổ chức
        /// </summary>
        
        private string _location;
        [Required(ErrorMessage = "Vui lòng nhập địa điểm tổ chức")]
        [MaxLength(128)]
        public string Location
        {
            get => _location;
            set => _location = value?.Trim();
        }
        /// <summary>
        /// Thành phố
        /// </summary>
        private string _provinceCode;
        [Required(ErrorMessage = "Vui lòng chọn thành phố tổ chức")]
        public string ProvinceCode
        {
            get => _provinceCode;
            set => _provinceCode = value?.Trim();
        }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        private string _address;
        [Required(ErrorMessage = "Vui lòng địa chỉ tổ chức")]
        [MaxLength(128)]
        public string Address
        {
            get => _address;
            set => _address = value?.Trim();
        }
        /// <summary>
        /// Kinh độ
        /// </summary>
        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => _latitude = value?.Trim();
        }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => _longitude = value?.Trim();
        }
        /// <summary>
        /// Đối tượng xem
        /// </summary>
        public int Viewing { get; set; }
        /// <summary>
        /// Cấu trúc mã hợp đồng
        /// </summary>
        public int ConfigContractCodeId { get; set; }
        /// <summary>
        /// Tài khoản nhận tiền
        /// </summary>
        public int BackAccountId { get; set; }
        /// <summary>
        /// Website sự kiện
        /// </summary>
        private string _website;
        public string Website
        {
            get => _website;
            set => _website = value?.Trim();
        }
        /// <summary>
        /// Faecbook sự kiện
        /// </summary>
        private string _facebook;
        public string Facebook
        {
            get => _facebook;
            set => _facebook = value?.Trim();
        }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        private string _phone;
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại liên hệ")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
        /// <summary>
        /// Email
        /// </summary>
        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value?.Trim();
        }
        /// <summary>
        /// Show App hay không
        /// </summary>
        private string _isShowApp;
        public string IsShowApp
        {
            get => _isShowApp;
            set => _isShowApp = value?.Trim();
        }
        public bool IsHighlight { get; set; }
        /// <summary>
        /// Cho phép khác hàng xuất vé bản cứng
        /// </summary>
        public bool CanExportTicket { get; set; }
        /// <summary>
        /// Cho phép khách hàng yêu cầu lấy hóa đơn
        /// </summary>
        public bool CanExportRequestRecipt { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }

        /// <summary>
        /// Chính sách mua vé
        /// </summary>
        private string _ticketPurchasePolicy;
        public string TicketPurchasePolicy
        {
            get => _ticketPurchasePolicy;
            set => _ticketPurchasePolicy = value?.Trim();
        }
        /// <summary>
        /// Danh sách ngân hàng
        /// </summary>
        public List<int> TradingBankAccounts { get; set; }

        /// <summary>
        /// Danh sách nhan vien soat ve
        /// </summary>
        public List<int> TicketInspectorIds { get; set; }
    }
}
