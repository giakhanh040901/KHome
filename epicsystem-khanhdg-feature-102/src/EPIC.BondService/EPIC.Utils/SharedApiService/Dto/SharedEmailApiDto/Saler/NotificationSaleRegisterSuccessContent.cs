using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.Saler
{
    public class NotificationSaleRegisterSuccessContent
    {
        public string SaleRegisterPhone { get; set; }
        public string SaleRegisterEmail { get; set; }
        public string SaleRegisterReferralCode { get; set; }
        public string SaleRegisterName { get; set; }
        public string TradingProviderName { get; set; }
        public string DepartmentName { get; set; }
        public string SaleBankAccNo { get; set; }
        public string BankName { get; set; }
        public string ConfirmDate { get; set; }
    }
}
