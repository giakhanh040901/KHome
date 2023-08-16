using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils;
using EPIC.Utils.Validation;

namespace EPIC.CoreSharedEntities.Dto.Investor
{
    public class UpdateContactAddressDto 
    {
        private string _contactAddress;
        private string _detailAddress;

        public int InvestorId { get; set; }
        public int ContactAddressId { get; set; }

        public string ContactAddress { get => _contactAddress; set => _contactAddress = value?.Trim(); }
        public string DetailAddress { get => _detailAddress; set => _detailAddress = value?.Trim(); }
        public string ProvinceCode { get; set; }
        public string DistrictCode { get; set; }
        public string WardCode { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES })]
        public string IsDefault { get; set; }
    }
}
