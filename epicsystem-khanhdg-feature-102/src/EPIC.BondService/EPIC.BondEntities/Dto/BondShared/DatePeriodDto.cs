using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.BondShared
{
    /// <summary>
    /// Ngày chốt quyền và ngày đáo hạn
    /// </summary>
    public class DatePeriodDto
    {
        /// <summary>
        /// Ngày chốt quyền
        /// </summary>
        public DateTime ClosePerDate { get; set; }

        /// <summary>
        /// Ngày trả trái tức
        /// </summary>
        public DateTime PayDate { get; set; }
    }
}
