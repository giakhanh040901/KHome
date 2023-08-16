using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ExportReport
{
    public class SaleInfoExcelDto
    {
        public int? DepartmentId { get; set; }
        /// <summary>
        /// Tên sale của investor sale
        /// </summary>
        public string Fullname { get; set; } 
        
        /// <summary>
        /// tên phòng ban của investor sale hoặc business sale
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Tên sale của business sale
        /// </summary>
        public string Name { get; set; }

        public int? InvestorId { get; set; }
        public int? BusinessCustomerId { get; set; }
    }
}
