using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate
{
    public class RstDistributionContractTemplateDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Id chính sách
        /// </summary>
        public int DistributionPolicyId { get; set; }
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
        /// Loại hợp đồng
        /// </summary>
        public int ContractType { get; set; }
        /// <summary>
        /// Danh sách detail cấu hình mã hợp đồng
        /// </summary>
        public List<RstConfigContractCodeDetailDto> ConfigContractCodes { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ngày hiệu lực
        /// </summary>
        public DateTime EffectiveDate { get; set; }

    }
}
