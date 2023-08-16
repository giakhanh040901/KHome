using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Sale
{
    public class AppContractOrderFilterDto
    {
        [FromQuery(Name = "tradingProviderId")]
        public int TradingProviderId { get; set; }

        [FromQuery(Name = "status")]
        public int? Status { get; set; }

        /// <summary>
        /// 1: BOND, 2 INVEST, 3 COMPANY)SHARES, 4: GARNER, 5: REAL_ESTATE
        /// </summary>
        [FromQuery(Name = "productType")]
        public int? ProductType { get; set; }


        [FromQuery(Name = "isOverview")]
        public bool IsOverview { get; set; }

        /// <summary>
        /// Ngày lọc
        /// </summary>
        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }

        /// <summary>
        /// Lọc ra danh sách hợp đồng sắp đến hạn
        /// </summary>
        public bool OrderDueSoon { get; set; }
    }
}
