using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    public class CustomerRootExcel
    {
        
        /// <summary>
        /// mã khách hàng
        /// </summary>
        public string CifCode { get; set; }

        /// <summary>
        /// Mã id của investor
        /// </summary>
        public int InvestorId { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        public string ReferralCode { get;set; }

        /// <summary>
        /// Nguồn tạo
        /// </summary>
        public int Source { get; set; }
        
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Xác minh EPIC
        /// </summary>
        public string IsCheck { get; set; }

        /// <summary>
        /// Có phải là nhà đầu tư chuyên nghiệp hay không
        /// </summary>
        public string IsProf { get; set; }
    }
}
