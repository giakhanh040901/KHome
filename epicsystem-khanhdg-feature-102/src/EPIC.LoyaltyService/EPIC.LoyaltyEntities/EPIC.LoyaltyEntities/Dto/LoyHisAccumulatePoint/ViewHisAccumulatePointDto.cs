using EPIC.LoyaltyEntities.Dto.LoyVoucher;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class ViewHisAccumulatePointDto
    {
        /// <summary>
        /// Id tích/tiêu điểm
        /// </summary>
        public int Id { get; set; }
        public int InvestorId { get; set; }

        /// <summary>
        /// Số điểm
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Tích điểm hay tiêu điểm
        /// 1: Tích điểm, 2: Tiêu điểm<br/>
        /// <see cref="LoyPointTypes"/>
        /// </summary>
        public int PointType { get; set; }

        /// <summary>
        /// Lý do
        /// </summary>
        public int? Reason { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ngày áp dụng thực tế
        /// </summary>
        public DateTime? ApplyDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// khcn
        /// </summary>
        public ViewHisAccumulatePointInvestorDto Investor { get; set; }

        /// <summary>
        /// khdn
        /// </summary>
        public ViewHisAccumulatePointInvestorDto BussinessCustomer { get; set; }

        /// <summary>
        /// Chi tiết tiến trình
        /// </summary>
        public List<ViewHisAccumulatePointStatusLogDto> Logs { get; set; }

        /// <summary>
        /// Voucher Ưu đãi
        /// </summary>
        public ViewVoucherDto Voucher { get; set; }
    }
   
}
