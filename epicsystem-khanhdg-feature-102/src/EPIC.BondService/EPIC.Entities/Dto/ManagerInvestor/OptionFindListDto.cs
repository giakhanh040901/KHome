using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class OptionFindDto
    {
        /// <summary>
        /// trả thêm object approve status
        /// </summary>
        [FromQuery(Name = "isNeedApproveStatus")]
        public bool IsNeedApproveStatus { get; set; }

        /// <summary>
        /// trả thêm giấy tờ mặc định
        /// </summary>

        [FromQuery(Name = "isNeedDefaultIdentification")]
        public bool IsNeedDefaultIdentification { get; set; }

        /// <summary>
        /// trả thêm list giấy tờ
        /// </summary>

        [FromQuery(Name = "isNeedListIdentification")]
        public bool IsNeedListIdentification { get; set; }

        /// <summary>
        /// trả thêm list ngân hàng
        /// </summary>
        [FromQuery(Name = "isNeedListBank")]
        public bool IsNeedListBank { get; set; }

        /// <summary>
        /// trả thêm ngân hàng mặc định
        /// </summary>
        [FromQuery(Name = "isNeedDefaultBank")]
        public bool IsNeedDefaultBank { get; set; }

        /// <summary>
        /// trả thêm list địa chỉ
        /// </summary>
        [FromQuery(Name = "isNeedListAddress")]
        public bool IsNeedListAddress { get; set; }

        /// <summary>
        /// trả thêm địa chỉ mặc định
        /// </summary>
        [FromQuery(Name = "isNeedDefaultAddress")]
        public bool IsNeedDefaultAddress { get; set; }

        /// <summary>
        /// trả thêm list cty chứng khoán
        /// </summary>
        [FromQuery(Name = "isNeedListStock")]
        public bool IsNeedListStock { get; set; }

        /// <summary>
        /// trả thêm cty chứng khoán mặc định
        /// </summary>
        [FromQuery(Name = "isNeedDefaultStock")]
        public bool IsNeedDefaultStock { get; set; }

        /// <summary>
        /// trả thêm thông tin người giới thiệu
        /// </summary>
        [FromQuery(Name = "isNeedReferralInvestor")]
        public bool IsNeedReferralInvestor { get; set; }

        /// <summary>
        /// trả thêm thông tin loyalty
        /// </summary>
        [FromQuery(Name = "isNeedLoyalty")]
        public bool IsNeedLoyalty { get; set; }
    }
}
