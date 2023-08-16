using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedEntities.Dto.InvestorTelesale
{
    public class GarnerInfoDto
    {
        /// <summary>
        /// Số tiền đầu tư ban đầu
        /// </summary>
        public decimal? InitTotalValue { get; set; }
        public DateTime? BuyDate { get; set; }
        public GarnerPolicyDto Policy { get; set; }
        public GarnerPolicyDetailDto PolicyDetail { get; set; }
    }
}
