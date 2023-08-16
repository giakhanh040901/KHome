using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy
{
    public class CreateRstOrderSellingPolicyDto
    {
        /// <summary>
        /// List Chính sách ưu đãi CĐT 
        /// </summary>
        public List<int> ProjectPolicyIds { get; set; }
        /// <summary>
        /// List Chính sách mở bán
        /// </summary>
        public List<int> SellingPolicyIds { get; set; }
        public int OrderId { get; set; }
    }
}
