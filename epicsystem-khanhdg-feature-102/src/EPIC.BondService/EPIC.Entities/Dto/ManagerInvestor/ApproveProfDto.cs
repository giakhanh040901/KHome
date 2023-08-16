using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ApproveProfDto
    {
        private string _approveNote { get; set; }

        public int ApproveId { get; set; }
        public string ApproveNote { get => _approveNote; set => _approveNote = value?.Trim(); }
        public int InvestorId { get; set; }
        public int InvestorIdTemp { get; set; }
        public DateTime ProfStartDate { get; set; }
        public DateTime ProfDueDate { get; set; }
    }
}
