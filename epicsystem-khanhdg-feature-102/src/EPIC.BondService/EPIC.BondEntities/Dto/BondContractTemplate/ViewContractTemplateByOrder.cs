using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ContractTemplate
{
    public class ViewContractTemplateByOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ContractType { get; set; }
        public string ContractTempUrl { get; set; }
        public int TradingProviderId { get; set; }
        public int? Classify { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }
        public int? SecondaryContractFileId {get; set;}
        public string FileTempUrl {get; set;}
        public string FileSignatureUrl {get; set;}
        public string FileScanUrl {get; set;}
        public string IsSign {get; set;}
    }

    public class ViewOrderContractForApp
    {
        public int Id { get; set; }
        public string ContractTemplateCode { get; set; }
        public string ContractTemplateName { get; set; }
        public int TradingProviderId { get; set; }
        public int ContractTemplateId { get; set; }
        public string IsSign { get; set; }
    }
}
