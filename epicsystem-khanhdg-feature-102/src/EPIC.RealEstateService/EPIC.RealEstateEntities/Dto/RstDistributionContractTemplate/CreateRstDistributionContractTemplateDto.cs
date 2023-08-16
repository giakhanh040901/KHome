using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate
{
    public class CreateRstDistributionContractTemplateDto
    {
        /// <summary>
        /// Id chính sách
        /// </summary>
        public int DistributionPolicyId { get; set; }
        /// <summary>
        /// ID bán phân phối
        /// </summary>
        public int DistributionId { get; set; }
        /// <summary>
        /// Id hợp đồng mẫu
        /// </summary>
        public int ContractTemplateTempId { get; set; }
        /// <summary>
        /// Id cấu hình mã hợp đồng
        /// </summary>
        public int ConfigContractCodeId { get; set; }
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
