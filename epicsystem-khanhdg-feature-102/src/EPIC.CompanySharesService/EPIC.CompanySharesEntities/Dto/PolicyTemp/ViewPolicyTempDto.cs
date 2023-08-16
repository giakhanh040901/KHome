using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.PolicyTemp
{
    public class ViewPolicyTempDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string InvestorType { get; set; }
        public decimal? IncomeTax { get; set; }
        public decimal? TransferTax { get; set; }
        public decimal? Classify { get; set; }
        public decimal? MinMoney { get; set; }
        public string IsTransfer { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Deleted { get; set; }
        public List<ViewPolicyDetailTempDto> PolicyDetailTemp { get; set; }

    }

    public class ViewPolicyDetailTempDto
    {
        public int Id { get; set; }
        public int CPSPolicyTempId { get; set; }
        public int? Stt { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string InterestPeriodType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string PeriodType { get; set; }
        public int? PeriodQuantity { get; set; }
        public string Status { get; set; }
        public decimal? Profit { get; set; }
        public int? InterestDays { get; set; }
        public int? InterestType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Deleted { get; set; }

    }
}
