using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicyDetail
{
    public class GarnerPolicyDetailDto
    {
        #region Id
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int DistributionId { get; set; }
        public int PolicyId { get; set; }
        #endregion

        #region name, status
        public int SortOrder { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string IsShowApp { get; set; }
        public string Status { get; set; }
        #endregion

        public decimal Profit { get; set; }

        public int? InterestDays { get; set; }
        public int PeriodQuantity { get; set; }
        public string PeriodType { get; set; }

        #region loại chọn kỳ hạn (với type của chính sách là loại chọn kỳ hạn)
        /// <summary>
        /// Kiểu trả lợi tức lấy trong InterestType const
        /// </summary>
        public int? InterestType { get; set; }
        public int? InterestPeriodQuantity { get; set; }
        public string InterestPeriodType { get; set; }
        public int? RepeatFixedDate { get; set; }
        #endregion

        #region audit
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        #endregion
    }
}
