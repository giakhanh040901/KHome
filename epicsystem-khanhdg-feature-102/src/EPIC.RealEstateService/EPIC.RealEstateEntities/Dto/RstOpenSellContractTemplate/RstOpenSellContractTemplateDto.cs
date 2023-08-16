using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using EPIC.RealEstateEntities.Dto.RstContractTemplateTemp;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate
{
    public class RstOpenSellContractTemplateDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Id chính sách
        /// </summary>
        public int DistributionPolicyId { get; set; }
        /// <summary>
        /// ID đại lý
        /// </summary>
        public int? TradingProviderId { get; set; }
        /// <summary>
        /// ID đối tác
        /// </summary>
        public int? PartnerId { get; set; }
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string DistributionPolicyName { get; set; }
        /// <summary>
        /// Id hợp đồng mẫu
        /// </summary>
        public int ContractTemplateTempId { get; set; }
        /// <summary>
        /// Tên hợp đồng
        /// </summary>
        public string ContractTemplateTempName { get; set; }
        /// <summary>
        /// Id cấu hình mã hợp đồng
        /// </summary>
        public int ConfigContractCodeId { get; set; }
        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Danh sách detail cấu hình mã hợp đồng
        /// </summary>
        public List<RstConfigContractCodeDetailDto> ConfigContractCodes { get; set; }

        /// <summary>
        /// Loai hien thi cua hop dong mau (B : Hien thi truoc khi hop dong duoc duyet; A : Hien thi sau khi hop dong duoc duyet)
        /// </summary>
        public string DisplayType { get; set; }
        /// <summary>
        /// 1. Từ Đại lý, 2. Từ Đối tác
        /// </summary>
        public int? ContractSource { get; set; }

        /// <summary>
        /// Ngày hiệu lực
        /// </summary>
        public DateTime EffectiveDate { get; set; }
        
        /// <summary>
        /// Chính sách bán hàng
        /// </summary>
        public RstDistributionPolicyDto RstDistributionPolicy { get; set; }

        /// <summary>
        /// Mẫu hợp đồng mẫu
        /// </summary>
        public RstContractTemplateTempDto RstContractTemplateTemp { get; set; }
    }
}
