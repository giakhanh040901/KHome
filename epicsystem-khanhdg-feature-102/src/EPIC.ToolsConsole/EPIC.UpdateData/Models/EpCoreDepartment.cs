using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpCoreDepartment
    {
        public decimal DepartmentId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string DepartmentName { get; set; }
        public decimal? ParentId { get; set; }
        public decimal? DepartmentLevel { get; set; }
        public decimal? ManagerId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Deleted { get; set; }
        public string DepartmentAddress { get; set; }
        public string Area { get; set; }
        public decimal? ManagerId2 { get; set; }
    }
}
