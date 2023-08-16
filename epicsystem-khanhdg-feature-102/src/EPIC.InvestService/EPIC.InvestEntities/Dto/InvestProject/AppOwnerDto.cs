using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    public class AppOwnerDto
    {
        /// <summary>
        /// Bảng BusinessCustomer
        /// </summary>
        /// Tên doanh nghiệp
        public string Name { get; set; }
        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// Địa chỉ trụ sở
        /// </summary>
        public string TradingAddress { get; set; }
        /// <summary>
        /// Người đại diện phát luật
        /// </summary>
        public string RepName { get; set; }
        /// <summary>
        /// Vốn điều lệ
        /// </summary>
        public decimal? Capital { get; set; }
        /// <summary>
        /// Giấy phép đăng ký kinh doanh
        /// </summary>
        public string BusinessRegistrationImg { get; set; }
        /// <summary>
        /// Bảng Owner
        /// </summary>
        /// Id chủ đầu tư
        public int Id { get; set; }
        /// <summary>
        /// Doanh thu
        /// </summary>
        public decimal? BusinessTurover { get; set; }
        /// <summary>
        /// Lợi nhuận sau thuế
        /// </summary>
        public decimal? BusinessProfit { get; set; }
        /// <summary>
        /// Chỉ số ROA
        /// </summary>
        public decimal? ROA { get; set; }
        /// <summary>
        /// Chỉ số ROE
        /// </summary>
        public decimal? ROE { get; set; }
        /// <summary>
        /// Hình ảnh đại diện
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Đường dây nóng
        /// </summary>
        public string Hotline { get; set; }
        /// <summary>
        /// Website
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// Url Fanpage
        /// </summary>
        public string Fanpage { get; set; }
    }
}
