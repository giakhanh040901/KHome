using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.Sale
{
    public class AppSaleFilterContractDto
    {
        public string SaleReferralCode { get; set; }
        public int TradingProviderId { get; set; }
        public int? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> ListTradingIds { get; set; }
        public List<int> OrderListStatus { get; set; } = null;
        public int? DepartmentId { get; set; } = null;
        public string Keyword { get; set; }
    }
}
