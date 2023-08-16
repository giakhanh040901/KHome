using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyHisAccumulatePoint
{
    public class ViewFindAllHisAccumulatePointDto
    {
        /// <summary>
        /// Id tích/tiêu điểm
        /// </summary>
        public int Id { get; set; }
        public int InvestorId { get; set; }

        /// <summary>
        /// Tên khách hàng
        /// </summary>
        public string Fullname { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Số điểm đổi
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Điểm hiện tại
        /// </summary>
        public int? LoyCurrentPoint { get; set; }

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
        /// Ngày tiếp nhận
        /// </summary>
        public DateTime? PendingDate { get; set; }

        /// <summary>
        /// Ngày đang giao
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Ngày hoàn thành
        /// </summary>
        public DateTime? FinishedDate { get; set; }

        /// <summary>
        /// Ngày hủy
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// Trạng thái yêu cầu đổi điểm
        /// </summary>
        public int? ExchangedPointStatus { get; set; }
    }
}
