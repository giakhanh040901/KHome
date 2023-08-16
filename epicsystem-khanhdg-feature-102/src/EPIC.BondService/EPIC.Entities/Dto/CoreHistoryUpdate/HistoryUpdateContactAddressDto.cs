using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.CoreHistoryUpdate
{
    public class HistoryUpdateContactAddressDto
    {
		public string ContactAddress { get; set; }
		public string DetailAddress { get; set; }
		public string ProvinceCode { get; set; }
		public string DistrictCode { get; set; }
		public string WardCode { get; set; }
		public string IsDefault { get; set; }
		public string Deleted { get; set; }
		public int? InvestorGroupId { get; set; }
	}
}
