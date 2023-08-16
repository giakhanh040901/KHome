using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorProfFile
    {
        public decimal Id { get; set; }
        public decimal? InvestorId { get; set; }
        public string ProfFileUrl { get; set; }
        public string Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public decimal? InvestorGroupId { get; set; }
        public decimal? InvestorTempId { get; set; }
        public string ProfFileType { get; set; }
        public string ProfFileName { get; set; }
    }
}
