using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BusinessCustomer
{
    public class BusinessCustomerBankDto
    {
        public int BusinessCustomerBankAccId { get; set; }
        public int BusinessCustomerId { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
        public string BankCode { get; set; }
        public string Logo { get; set; }
        public int BankId { get; set; }
        public string FullBankName { get; set; }
        public int? Status { get; set; }
        public string IsDefault { get; set; }
        /// <summary>
        /// Avatar đại lý
        /// </summary>
        public string AvatarTradingImageUrl { get; set; }
    }
    
    public class BusinessCustomerBankDefault
    {
        public int? BusinessCustomerBankAccId { get; set; }
        public int? BusinessCustomerId { get; set; }
    }

    public class BusinessCustomerBankTempDefault
    {
        public int? Id { get; set; }
        public int? BusinessCustomerTempId { get; set; }
    }
}
