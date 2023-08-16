using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EntitiesBase.Interfaces.Policy
{
    /// <summary>
    /// Kỳ trả lợi tức lấy trong InterestType const (sau bao lâu trả lãi một lần)
    /// </summary>
    public interface IInterestPeriod
    {
        /// <summary>
        /// Kiểu kỳ trả lợi tức
        /// </summary>
        public int? InterestType { get; set; }

        /// <summary>
        /// Số kỳ trả
        /// </summary>
        public int? InterestPeriodQuantity { get; set; }

        /// <summary>
        /// Y M D
        /// </summary>
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// Ngày trả cố định hằng tháng
        /// </summary>
        public int? RepeatFixedDate { get; set; }
    }
}
