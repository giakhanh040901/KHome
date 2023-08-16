using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ViewInvestorStockDto
    {
        public int Id { get; set; }
        public int? InvestorId { get; set; }
        public int? InvestorGroupId { get; set; }
        public string IsDefault { get; set; }
        public int? SecurityCompany { get; set; }
        public string StockTradingAccount { get; set; }
    }
}
