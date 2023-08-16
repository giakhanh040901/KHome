using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.ManagerInvestor
{
    public class UpdateManagerInvestorDto
    {
        private string _name { get; set; }
        private string _bori { get; set; }
        private string _dorf { get; set; }
        private string _eduLevel { get; set; }
        private string _occupation { get; set; }
        private string _address { get; set; }
        private string _contactAddress { get; set; }
        private string _nationality { get; set; }
        private string _mobile { get; set; }
        private string _email { get; set; }
        private string _taxCode { get; set; }
        private string _phone { get; set; }
        private string _fax { get; set; }
        private string _stockTradingAccount { get; set; }
        private string _representativePhone { get; set; }
        private string _representativeEmail { get; set; }
        private string _referralCodeSelf { get; set; }

        public int InvestorId { get; set; }

        public int InvestorGroupId { get; set; }

        public int TradingProviderId { get; set; }

        public string Name { get => _name; set => _name = value?.Trim(); }

        public string Bori { get => _bori; set => _bori = value?.Trim(); }
        public string Dorf { get => _dorf; set => _dorf = value?.Trim(); }

        public string EduLevel { get => _eduLevel; set => _eduLevel = value?.Trim(); }
        public string Occupation { get => _occupation; set => _occupation = value?.Trim(); }
        public string Address { get => _address; set => _address = value?.Trim(); }
        public string ContactAddress { get => _contactAddress; set => _contactAddress = value?.Trim(); }
        public string Nationality { get => _nationality; set => _nationality = value?.Trim(); }

        public string Mobile { get => _mobile; set => _mobile = value?.Trim(); }

        //[PhoneEpic]
        public string Phone { get => _phone; set => _phone = value?.Trim(); }

        public string Fax { get => _fax; set => _fax = value?.Trim(); }

        //[Email]     
        public string Email { get => _email; set => _email = value?.Trim(); }

        public string TaxCode { get => _taxCode; set => _taxCode = value?.Trim(); }

        public bool IsTemp { get; set; }
        public int? SecurityCompany { get; set; }
        public string StockTradingAccount { get => _stockTradingAccount; set => _stockTradingAccount = value?.Trim(); }

        public string RepresentativePhone { get => _representativePhone; set => _representativePhone = value?.Trim(); }
        public string RepresentativeEmail { get => _representativeEmail; set => _representativeEmail = value?.Trim(); }
        public string ReferralCodeSelf { get => _referralCodeSelf; set => _referralCodeSelf = value?.Trim(); }

        /// <summary>
        /// Identification mặc định
        /// </summary>
        public UpdateIdentificationDto DefaultIdentification { get; set; }

        public UpdateDefaultBankDto DefaultBank { get; set; }

    }
}
