using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class EpInvestorToDo
    {
        public int Id { get; set; }
        public int InvestorId { get; set; }
        public int Type { get; set; }
        public string Detail { get; set; }
        public int Status { get; set; }
    }
}
