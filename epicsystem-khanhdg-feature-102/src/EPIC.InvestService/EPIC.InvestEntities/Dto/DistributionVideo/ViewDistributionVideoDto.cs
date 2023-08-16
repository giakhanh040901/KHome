using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.DistributionVideo
{
    public class ViewDistributionVideoDto
    {
        public int Id { get; set; }
        public int DistributionId { get; set; }
        public int TradingProviderId { get; set; }
        public string UrlVideo { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Feature { get; set; }
    }
}
