using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.PolicyFile
{
    public class DistriPolicyFileDto
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public int TradingProviderId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
