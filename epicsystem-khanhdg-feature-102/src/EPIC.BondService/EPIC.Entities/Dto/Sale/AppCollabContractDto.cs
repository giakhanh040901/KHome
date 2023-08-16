using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppCollabContractSignDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string TradingProviderName { get; set; }
        public int SaleId { get; set; }
        public int CollabContractTempId { get; set; }
        public string FileTempUrl { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileScanUrl { get; set; }
    }
}
