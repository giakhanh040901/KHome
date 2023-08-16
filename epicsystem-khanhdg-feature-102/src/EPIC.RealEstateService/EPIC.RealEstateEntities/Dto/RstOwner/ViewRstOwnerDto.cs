using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOwner
{
    public class ViewRstOwnerDto
    {
        /// <summary>
        /// Id chủ đầu tư
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tên chủ đầu tư
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// Id Doanh nghiệp
        /// </summary>
        public int BusinessCustomerId { get; set; }

        /// <summary>
        /// Thông tin tài chính - doanh thu
        /// </summary>
        public decimal? BusinessTurnover { get; set; }

        /// <summary>
        /// Thông tin tài chính - lợi nhuận sau thuế
        /// </summary>
        public decimal? BusinessProfit { get; set; }

        /// <summary>
        /// Thông tin tài chính - chỉ số ROA
        /// </summary>
        public decimal? Roa { get; set; }

        /// <summary>
        /// Thông tin tài chính - chỉ số ROE
        /// </summary>
        public decimal? Roe { get; set; }

        /// <summary>
        /// Website
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Đường dây nóng
        /// </summary>
        public string Hotline { get; set; }

        /// <summary>
        /// Link page
        /// </summary>
        public string Fanpage { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string DescriptionContent { get; set; }

        public BusinessCustomerDto BusinessCustomer { get; set; }

        /// <summary>
        /// danh sách Ngân hàng của chủ đầu tư
        /// </summary>
        public List<BusinessCustomerBankDto> BusinessCustomerBanks { get; set; }
    }
}
