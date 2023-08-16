using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class DistributionDto
    {
		public int Id { get; set; }
		public int ProjectId { get; set; }
		public int TradingProviderId { get; set; }
		public int? BusinessCustomerBankAccId { get; set; }
		public int Status { get; set; }
		public DateTime? OpenCellDate { get; set; }
		public DateTime? CloseCellDate { get; set; }
		public string IsClose { get; set; }
		public string IsShowApp { get; set; }
		public string IsCheck { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
    }
}
