using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppListCollabContractSignDto
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int TradingProviderId { get; set; }
        public string TradingProviderName { get; set; }
        //public int CollabContractTempId { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 1. Online, 2. Offline
        /// </summary>
        public int SaleSource { get; set; }
        public string FileSignatureUrl { get; set; }
        public string FileScanUrl { get; set; }
    }
}
