using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Distribution
{
    public class AppFilterProjectDistribution
    {
        /// <summary>
        /// True: sale xem danh sách bảng hàng
        /// </summary>
        [FromQuery(Name = "isSaleView")]
        public bool IsSaleView { get; set; }

        private string _keyword;
        [FromQuery(Name = "keyword")]
        public string Keyword 
        { 
            get => _keyword; 
            set => _keyword = value?.Trim(); 
        }
        /// <summary>
        /// Sắp xếp theo lợi tức
        /// </summary>
        [FromQuery(Name = "orderByInterestDesc")]
        public bool? OrderByInterestDesc { get;set; }

        /// <summary>
        /// Sắp xếp theo sản phẩm
        /// </summary>
        [FromQuery(Name = "orderByIdDesc")]
        public bool? OrderByIdDesc { get; set; }

        /// <summary>
        /// Kỳ hạn đầu tư: 3M:3 Tháng, 6M: 6 tháng, 9M: 9 tháng, 12M: 12 tháng...
        /// </summary>
        private string _periodType;
        [FromQuery(Name = "periodType")]
        public string PeriodType
        {
            get => _periodType;
            set => _periodType = value?.Trim();
        }
    }
}
