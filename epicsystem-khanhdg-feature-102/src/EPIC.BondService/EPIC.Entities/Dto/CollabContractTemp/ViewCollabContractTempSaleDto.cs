using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CollabContractTemp
{
    public class ViewCollabContractTempSaleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TradingProviderId { get; set; }
        public int? CollabContractId {get; set;}
        public string FileTempUrl {get; set;}
        public string FileSignatureUrl {get; set;}
        public string FileScanUrl {get; set;}
        public string IsSign {get; set;}
    }
}
