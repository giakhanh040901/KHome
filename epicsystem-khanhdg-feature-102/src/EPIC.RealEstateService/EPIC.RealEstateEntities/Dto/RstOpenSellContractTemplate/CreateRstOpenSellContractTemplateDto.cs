using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate
{
    public class CreateRstOpenSellContractTemplateDto
    {
        public int OpenSellId { get; set; }

        public int ContractTemplateTempId { get; set; }

        /// <summary>
        /// Chính sách mở bán
        /// </summary>
        public int DistributionPolicyId { get; set; }

        /// <summary>
        /// Cấu trúc mã 
        /// </summary>
        public int ConfigContractCodeId { get; set; }

        /// <summary>
        /// Loai hien thi cua hop dong mau (B : Hien thi truoc khi hop dong duoc duyet; A : Hien thi sau khi hop dong duoc duyet)
        /// </summary>
        private string _displayType;
        public string DisplayType 
        { 
            get => _displayType; 
            set => _displayType = value?.Trim(); 
        }

        /// <summary>
        /// Ngày hiệu lực
        /// </summary>
        public DateTime EffectiveDate { get; set; }
    }
}
