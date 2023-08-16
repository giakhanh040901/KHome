using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerPolicyDetail
{
    public class AppGarnerPolicyDetailDto
    {
        /// <summary>
        /// Id Kỳ hạn
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Số thứ tự
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên viết tắt
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Số kỳ đầu tư
        /// </summary>
        public int PeriodQuantity { get; set; }

        /// <summary>
        /// Đơn vị kỳ đầu tư
        /// </summary>
        public string PeriodTypeName { get; set; }

        /// <summary>
        /// % loi tuc
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Ngày đáo hạn
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Tính toán lợi tức
        /// </summary>
        public decimal? ActuallyProfit { get; set; }

        /// <summary>
        /// Tên kiểu trả lợi tức
        /// </summary>
        public string InterestTypeName { get; set; }
    }
}
