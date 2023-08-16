using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Entities.DataEntities;
using EPIC.InvestSharedEntites.Dto.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Order
{
    public class AppOrderDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public string ContractCode { get; set; }
        public string BankName { get; set; }
        public string BankAccNo { get; set; }
        public decimal? TotalValue { get; set; }
        public string BankAccName { get; set; }
        public string PaymentNote { get; set; }
        public int DistributionId { get; set; }
        public List<AppTradingBankAccountDto> TradingBankAccounts { get; set; }
    }
}
