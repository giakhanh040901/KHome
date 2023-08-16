using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesEntities.Dto.CpsSecondary
{
    /// <summary>
    /// Thoong tin chung của lô cổ phần theo bán theo kỳ hạn
    /// </summary>
    public class CpsInfoSecondaryDto
    {
        public int SecondaryId { get; set; }
        public string CpsCode { get; set; }
        public string TradingProviderName { get; set; }
        public decimal? Profit { get; set; }
        public string Icon { get; set; }
    }
}
