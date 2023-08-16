using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreApprove
{
    /// <summary>
    /// Trinh duyet => Da duyet
    /// </summary>
    public class ApproveRequestDto
    {
        public decimal ApproveID { get; set; }
        public string ApproveNote { get; set; }
        public int UserApproveId { get; set; }
        public int? ReferId { get; set; }
    }

    /// <summary>
    /// Duyệt từ bảng tạm chưa có Id Thật
    /// </summary>
    public class ApproveRequestTempDto
    {
        public decimal ApproveID { get; set; }
        public string ApproveNote { get; set; }
        public int ReferIdTemp { get; set; }
    }   

    public class ApproveStatusDto
    {
        /// <summary>
        /// ReferId
        /// </summary>
        public int Id { get; set; }
        public string ApproveNote { get; set; }
    }
}
