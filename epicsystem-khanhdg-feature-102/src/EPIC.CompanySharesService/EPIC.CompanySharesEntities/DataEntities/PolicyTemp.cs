using EPIC.Entities;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.DataEntities
{
    public class PolicyTemp : IFullAudited
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
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Deleted { get; set; }
    }

    /// <summary>
    /// KET QUA TRA RA TU PROC GET ALL
    /// </summary>
    public class PolicyTempView : EntityTypeMapper
    {
        //PolicyTemp
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

        //PolicyDetailTemp

        public int PolicyDetailTempId { get; set; }
        public int? DeStt { get; set; }
        public string DeShortName { get; set; }
        public string DeName { get; set; }
        public string DeInterestPeriodType { get; set; }
        public int? DeInterestPeriodQuantity { get; set; }
        public string DePeriodType { get; set; }
        public int DePeriodQuantity { get; set; }
        public string DeStatus { get; set; }
        public decimal DeProfit { get; set; }
        public int? DeInterestDays { get; set; }
        public int? DeInterestType { get; set; }
        public DateTime DeCreatedDate { get; set; }
        public string DeCreatedBy { get; set; }
        public string DeModifiedBy { get; set; }
        public DateTime DeModifiedDate { get; set; }
    }
}
