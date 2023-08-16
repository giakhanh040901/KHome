using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPIC.InvestEntities.Dto.ContractTemplate
{
    public class CreateContractTemplateDto
    {
        public List<int> PolicyIds { get; set; }

        [StringRange(AllowableValues = new string[] { Utils.DisplayType.TRUOC_KHI_DUYET, EPIC.Utils.DisplayType.SAU_KHI_DUYET })]
        public string DisplayType { get; set; }

        [IntegerRange(AllowableValues = new int[] { ContractSources.ONLINE, ContractSources.OFFLINE, ContractSources.ALL })]
        public int ContractSource { get; set; }
        public List<int> ContractTemplateTempIds { get; set; }
        public int ConfigContractId { get; set; }
        public DateTime? StartDate { get; set; }
        public List<DistributionPolicyContractUpload> DistributionPolicyContractUploads { set; get; }
    }

    /// <summary>
    /// File chính sách phân phối tải lên
    /// </summary>
    public class DistributionPolicyContractUpload
    {
        /// <summary>
        /// Tên file
        /// </summary>
        public string DistributionPolicyUploadName { get; set; }
        public IFormFile InvestorFile { get; set; }
        public IFormFile BusinessCustomerFile { get; set; }
        public int ContractType { get; set; }
    }
}
