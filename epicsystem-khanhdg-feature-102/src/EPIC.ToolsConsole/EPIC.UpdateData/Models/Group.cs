using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class Group
    {
        public decimal? GroupId { get; set; }
        public string Groupname { get; set; }
        public string Status { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Lastmodify { get; set; }
        public string Usertype { get; set; }
        public string IsDeleted { get; set; }
        public string Grptype { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string GroupCode { get; set; }
    }
}
