using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class ViewInvestorContactAddressDto
    {
		public int ContactAddressId { get; set; }

		public int InvestorId { get; set; }

		public string ContactAddress { get; set; }

		public string DetailAddress { get; set; }

		public string ProvinceCode { get; set; }

		public string DistrictCode { get; set; }

		public string WardCode { get; set; }

		public string ProvinceName { get; set; }

		public string DistrictName { get; set; }

		public string WardName { get; set; }

		public string IsDefault { get; set; }

		public int? InvestorGroupId { get; set; }
	}
}
