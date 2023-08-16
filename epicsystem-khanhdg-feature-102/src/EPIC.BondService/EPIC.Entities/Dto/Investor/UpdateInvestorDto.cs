using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class UpdateInvestorDto
    {
		public int InvestorId { get; set; }
		public string Name { get; set; }
		public string Bori { get; set; }
		public string Sex { get; set; }
		public DateTime BirthDate { get; set; }
		public string Address { get; set; }
		public string ContactAddress { get; set; }
		public string Nationality { get; set; }
		public string IdNo { get; set; }
		public DateTime IdDate { get; set; }
		public string IdIssuer { get; set; }
	}
}
