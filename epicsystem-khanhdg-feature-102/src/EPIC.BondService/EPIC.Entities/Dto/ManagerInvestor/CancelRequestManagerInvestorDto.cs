using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class CancelRequestManagerInvestorDto
    {
        public int InvestorIdTemp { get; set; }
        public string Notice { get; set; }
        public List<string> IncorrectFields { get; set; }
    }
}
