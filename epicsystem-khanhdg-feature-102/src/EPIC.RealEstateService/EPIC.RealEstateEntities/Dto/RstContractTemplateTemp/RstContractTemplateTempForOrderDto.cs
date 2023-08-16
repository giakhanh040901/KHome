using DocumentFormat.OpenXml.Office2013.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstContractTemplateTemp
{
    public class RstContractTemplateTempForOrderDto
    {
        public int Id { get; set; }
        /// <summary>
        /// ID mở bán
        /// </summary>
        public int OpenSellId { get; set; }
        /// <summary>
        /// ID chi tiết mở bán
        /// </summary>
        public int OpenSellDetailId { get; set; }
        /// <summary>
        /// ID đại lý
        /// </summary>
        public int? TradingProviderId { get; set; }
        /// <summary>
        /// Đường dẫn file
        /// </summary>
        public string ContractTemplateUrl { get; set; }
        /// <summary>
        /// ID mẫu hợp đồng
        /// </summary>
        public int ContractTemplateTempId { get; set; }
        /// <summary>
        /// Tên mẫu hợp đồng
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// File khách hàng cá nhân
        /// </summary>
        public string FileInvestor { get; set; }
        /// <summary>
        /// File khách hàng doanh nghiệp
        /// </summary>
        public string FileBusinessCustomer { get; set; }
        /// <summary>
        /// Kiểu hợp đồng (1: ONLINE, 2: OFFLINE, 3: ALL)
        /// </summary>
        public int ContractSource { get; set; }
        /// <summary>
        /// Loại hợp đồng (1: HĐ MUA BĐS, 2: HĐ BÁN BĐS, 3: HĐ THANH LÝ, 4: HĐ ĐẶT CỌC)
        /// </summary>
        public int ContractType { get; set; }
        /// <summary>
        /// Id cấu trúc mã HĐ
        /// </summary>
        public int ConfigContractId { get; set; }
        /// <summary>
        /// Loại hiển thị HĐ
        /// </summary>
        public string DisplayType { get; set; }
        /// <summary>
        /// TRạng thái A: ACTIVE, D:DEACTIVE
        /// </summary>
        public string Status { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
