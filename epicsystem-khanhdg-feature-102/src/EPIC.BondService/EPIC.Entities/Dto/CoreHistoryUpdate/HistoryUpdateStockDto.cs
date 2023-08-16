using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreHistoryUpdate
{
    public class HistoryUpdateStockDto
    {
		public int? SecurityCompany { get; set; }
		public string StockTradingAccount { get; set; }
		public string IsDefault { get; set; }
		public string Deleted { get; set; }
	}
}
