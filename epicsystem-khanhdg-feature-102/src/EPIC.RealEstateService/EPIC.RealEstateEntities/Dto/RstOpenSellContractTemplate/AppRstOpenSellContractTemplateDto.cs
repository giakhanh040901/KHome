using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate
{
    public class AppRstOpenSellContractTemplateDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string SellingPolicyName { get; set; }

        /// <summary>
        /// Tên hợp đồng
        /// </summary>
        public string ContractTemplateTempName { get; set; }

        /// <summary>
        /// Ngày hiệu lực
        /// </summary>
        public DateTime EffectiveDate { get; set; }
    }
}
