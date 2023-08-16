using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto.NotifyInvestorProf
{
    public class NotifyInvestorProfDto
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ProfStartDate { get; set; }
        public string ProfDueDate { get; set; }
    }
}
