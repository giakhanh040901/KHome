using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerReceiveContractTemp
{
    public class GarnerReceiveContractTemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TradingProviderId { get; set; }
        public string FileUrl { get; set; }
        public int DistributionId { get; set; }
    }
}
