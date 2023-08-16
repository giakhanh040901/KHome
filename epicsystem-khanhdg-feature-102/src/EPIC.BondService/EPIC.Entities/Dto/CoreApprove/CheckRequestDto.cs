using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreApprove
{
    /// <summary>
    /// Xac minh
    /// </summary>
    public class CheckRequestDto
    {
        public decimal ApproveID { get; set; }
        public int? UserCheckId { get; set; }
    }

    public class CheckStatusDto
    {
        public int Id { get; set; }
    }
}
