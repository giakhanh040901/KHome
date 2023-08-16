using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreApprove
{
    public class CloseRequestDto
    {
        public decimal ApproveID { get; set; }
        public string CloseNote { get; set; }
    }

    public class CloseStatusDto
    {
        /// <summary>
        /// ReferId
        /// </summary>
        public int Id { get; set; }
        public string CloseNote { get; set; }
    }
}
