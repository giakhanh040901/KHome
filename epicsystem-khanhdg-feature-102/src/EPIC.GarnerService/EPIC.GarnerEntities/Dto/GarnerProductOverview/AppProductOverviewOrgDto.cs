using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class AppProductOverviewOrgDto
    {
        /// <summary>
        /// Id thông tin tổ chức
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Mã tổ chức
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Vai trò của đối tác
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Icon của đối tác
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Đường dẫn
        /// </summary>
        public string Url { get; set; }
    }
}
