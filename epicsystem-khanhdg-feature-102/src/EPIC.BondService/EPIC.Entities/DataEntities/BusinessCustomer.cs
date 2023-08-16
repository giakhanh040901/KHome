using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_BUSINESS_CUSTOMERS", Schema = DbSchemas.EPIC)]
    public class BusinessCustomer : IFullAudited
    {
        public BusinessCustomer BusinessCustomers;

        public static string SEQ { get; } = $"{DbSchemas.EPIC}.SEQ_BUSINESS_CUSTOMERS";

        [Key]
        [ColumnSnackCase(nameof(BusinessCustomerId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? BusinessCustomerId { get; set; }

        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(ShortName))]
        public string ShortName { get; set; }

        [ColumnSnackCase(nameof(Address))]
        public string Address { get; set; }

        [ColumnSnackCase(nameof(TradingAddress))]
        public string TradingAddress { get; set; }

        [ColumnSnackCase(nameof(Nation))]
        public string Nation { get; set; }

        [ColumnSnackCase(nameof(Phone))]
        public string Phone { get; set; }

        [ColumnSnackCase(nameof(Mobile))]
        public string Mobile { get; set; }

        [ColumnSnackCase(nameof(Email))]
        public string Email { get; set; }

        [ColumnSnackCase(nameof(TaxCode))]
        public string TaxCode { get; set; }

        [ColumnSnackCase(nameof(LicenseDate))]
        public DateTime? LicenseDate { get; set; }

        [ColumnSnackCase(nameof(LicenseIssuer))]
        public string LicenseIssuer { get; set; }

        [ColumnSnackCase(nameof(Capital))]
        public decimal? Capital { get; set; }

        [ColumnSnackCase(nameof(RepName))]
        public string RepName { get; set; }

        [ColumnSnackCase(nameof(RepPosition))]
        public string RepPosition { get; set; }

        [ColumnSnackCase(nameof(DecisionNo))]
        public string DecisionNo { get; set; }

        [ColumnSnackCase(nameof(DecisionDate))]
        public DateTime? DecisionDate { get; set; }

        [ColumnSnackCase(nameof(NumberModified))]
        public int? NumberModified { get; set; }

        [ColumnSnackCase(nameof(DateModified))]
        public DateTime? DateModified { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }

        [ColumnSnackCase(nameof(IsCheck))]
        public string IsCheck { get; set; }

        [ColumnSnackCase(nameof(RepIdNo))]
        public string RepIdNo { get; set; }

        [ColumnSnackCase(nameof(RepIdDate))]
        public DateTime? RepIdDate { get; set; }

        [ColumnSnackCase(nameof(RepIdIssuer))]
        public string RepIdIssuer { get; set; }

        [ColumnSnackCase(nameof(RepAddress))]
        public string RepAddress { get; set; }

        [ColumnSnackCase(nameof(RepSex))]
        public string RepSex { get; set; }

        [ColumnSnackCase(nameof(RepBirthDate))]
        public DateTime? RepBirthDate { get; set; }

        [ColumnSnackCase(nameof(Website))]
        public string Website { get; set; }

        [ColumnSnackCase(nameof(BusinessRegistrationImg))]
        public string BusinessRegistrationImg { get; set; }

        [ColumnSnackCase(nameof(Fanpage))]
        public string Fanpage { get; set; }

        [ColumnSnackCase(nameof(Server))]
        public string Server { get; set; }

        [ColumnSnackCase(nameof(Key))]
        public string Key { get; set; }

        [ColumnSnackCase(nameof(BankId))]
        public int? BankId { get; set; }

        [ColumnSnackCase(nameof(Secret))]
        public string Secret { get; set; }

        [ColumnSnackCase(nameof(AvatarImageUrl))]
        public string AvatarImageUrl { get; set; }

        [ColumnSnackCase(nameof(StampImageUrl))]
        public string StampImageUrl { get; set; }

        [ColumnSnackCase(nameof(ReferralCodeSelf))]
        public string ReferralCodeSelf { get; set; }

        [ColumnSnackCase(nameof(AllowDuplicate))]
        public string AllowDuplicate { get; set; }

        public List<BusinessCustomerBank> BusinessCustomerBanks { get; }
        //public List<BusinessCustomerBankTemp> BusinessCustomerBanks { get; set; }
    }
}
