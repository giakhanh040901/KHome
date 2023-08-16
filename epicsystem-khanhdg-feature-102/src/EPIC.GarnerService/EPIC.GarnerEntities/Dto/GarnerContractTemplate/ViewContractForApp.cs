using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerContractTemplate
{
    public class ViewContractForApp
    {
        public long OrderContractFileId { get; set; }
        public int ContractTempId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TradingProviderId { get; set; }
        public string IsSign { get; set; }
    }
}
