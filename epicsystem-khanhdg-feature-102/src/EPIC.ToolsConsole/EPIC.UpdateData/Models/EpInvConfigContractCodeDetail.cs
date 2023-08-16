using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvConfigContractCodeDetail
    {
        public int Id { get; set; }
        public int ConfigContractCodeId { get; set; }
        public int SortOrder { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
