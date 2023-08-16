using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    public class CreateInvestorStockDto
    {
        private string _stockTradingAccount { get; set; }

        public int InvestorId { get; set; }
        public int InvestorGroupId { get; set; }
        public string IsDefault { get; set; }
        public bool IsTemp { get; set; }

        [Required]
        public int? SecurityCompany { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string StockTradingAccount { get => _stockTradingAccount; set => _stockTradingAccount = value?.Trim(); }
    }
}
