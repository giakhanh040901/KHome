using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class CreateContactAddressDto
    {
		private string _contactAddress;
		private string _detailAddress;


		public int InvestorId { get; set; }

		public string ContactAddress { get => _contactAddress; set => _contactAddress = value?.Trim(); }

        [StringRange(AllowableValues = new string[] {YesNo.YES, YesNo.NO})]
		public string IsDefault { get; set; }

        public string DetailAddress { get => _detailAddress; set => _detailAddress = value?.Trim(); }
		public string ProvinceCode { get; set; }
		public string DistrictCode { get; set; }

		public string WardCode { get; set; }
	}
}
