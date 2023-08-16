using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyLuckyProgram
{
    public class ViewLuckyProgramDto
    {
        public int Id { get; set; }
        /// <summary>
        /// Mã chương trình
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Thời gian bắt đầu
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Thời gian kết thúc
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Trạng thái: 1 Khởi tạo
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Số kịch bản
        /// </summary>
        public int NumberLuckyScenario { get; set; }
    }
}
