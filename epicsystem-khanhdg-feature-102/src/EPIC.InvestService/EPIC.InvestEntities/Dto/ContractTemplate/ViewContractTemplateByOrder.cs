using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ContractTemplate
{
    public class ViewContractTemplateByOrder
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string ContractTempUrl { get; set; }
        public int TradingProviderId { get; set; }
        public int ConfigContractId { get; set; }
        public string Type { get; set; }
        public string DisplayType { get; set; }
        public string Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Status { get; set; }
        public int? OrderContractFileId { get; set; }
        public string FileTempUrl { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileScanUrl { get; set; }
        public string IsSign { get; set; }
        public long? WithdrawalId { get; set; }
        public long? RenewalsId { get; set; }
        public int? Times { get; set; }
        public string ContractCode { get; set; }
        public int ContractType { get; set; }

    }

    public class ViewContractForApp
    {
        public int OrderContractFileId { get; set; }
        public int ContractTempId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TradingProviderId { get; set; }  
        public int PolicyId { get; set; }  
        public string IsSign { get; set; }
    }
}
