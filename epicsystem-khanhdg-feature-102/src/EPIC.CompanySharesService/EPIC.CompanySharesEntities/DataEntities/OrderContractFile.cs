using EPIC.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class OrderContractFile : IFullAudited
    {
        public int Id { get; set; }
        public int? TradingProviderId { get; set; }
        public int? OrderId { get; set; }
        public int? ContractTempId { get; set; }
        public string FileTempUrl { get; set; }
        public string FileTempPdfUrl { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileScanUrl { get; set; }
        public string IsSign { get; set; }
        public int PageSign { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
    }
}
