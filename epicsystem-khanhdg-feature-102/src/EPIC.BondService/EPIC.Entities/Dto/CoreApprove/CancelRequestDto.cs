using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreApprove
{
    /// <summary>
    /// Huy yeu cau duyet
    /// </summary>
    public class CancelRequestDto
    {
        public decimal ApproveID { get; set; }
        public string CancelNote { get; set; }

        /// <summary>
        /// Người hủy
        /// </summary>
        public int UserApproveId { get; set; }
    }

    public class CancelStatusDto
    {
        /// <summary>
        /// ReferId
        /// </summary>
        public int Id { get; set; }
        public string CancelNote { get; set; }
    }
}
