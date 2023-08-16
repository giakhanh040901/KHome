using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsInfo
{
    public class DividendCpsDto
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal? ParValue { get; set; }
        public long? Quantity { get; set; }
        public int? Period { get; set; }
        public string PeriodUnit { get; set; }
        public decimal? InterestRate { get; set; }
        public string InterestPeriodUnit { get; set; }
        public int? InterestPeriod { get; set; }
        public int? InterestRateType { get; set; }
        public List<DividendRecurentDto> DividendRecurents { get; set; }
    }

    public class DividendRecurentDto
    {
        /// <summary>
        /// Ngày trả
        /// </summary>
        public DateTime? PayDate { get; set; }
        public decimal? Dividend { get; set; }
        /// <summary>
        /// Số ngày
        /// </summary>
        public int NumberOfDays { get; set; }
        /// <summary>
        /// Ngày chốt quyền
        /// </summary>
        public DateTime? ClosePerDate { get; set; }
    }
}
