using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDashboard
{
    public class RstDashboardActionsDto
    {
        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// Tên
        /// </summary>
        public string Fullname { get; set; }
        /// <summary>
        /// Hành động 
        /// 1: Đặt lệnh mới
        /// <see cref="RstDashboardActions"/>
        /// </summary>
        public int Action { get; set; }
        /// <summary>
        /// ngày đặt lệnh
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// id sổ lệnh
        /// </summary>
        public long OrderId { get; set; }
    }
}
