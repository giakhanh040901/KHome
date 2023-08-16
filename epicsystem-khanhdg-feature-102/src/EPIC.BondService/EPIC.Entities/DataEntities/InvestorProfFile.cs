using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    /// <summary>
    /// Lưu file của nhà đầu tư chuyên nghiệp
    /// </summary>
    public class InvestorProfFile
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }
        public string ProfFileUrl { get; set; }
        public string ProfFileType { get; set; }
        public string ProfFileName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Deleted { get; set; }
    }
}
