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
    [Table("EP_CORE_BUSINESS_CUSTOMER_TEMP", Schema = DbSchemas.EPIC)]
    public class BusinessCustomerTemp : IFullAudited
    {
        [Key]
        [ColumnSnackCase(nameof(BusinessCustomerTempId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessCustomerTempId { get; set; }

        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        [ColumnSnackCase(nameof(ShortName))]
        public string ShortName { get; set; }

        [NotMapped]
        public string Type { get; set; }
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

        [ColumnSnackCase(nameof(BankAccNo))]
        public string BankAccNo { get; set; }

        [ColumnSnackCase(nameof(BankAccName))]
        public string BankAccName { get; set; }

        [ColumnSnackCase(nameof(BankName))]
        public string BankName { get; set; }

        [ColumnSnackCase(nameof(BankId))]
        public int? BankId { get; set; }

        [ColumnSnackCase(nameof(BankBranchName))]
        public string BankBranchName { get; set; }

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

        [ColumnSnackCase(nameof(CancelDate))]
        public DateTime? CancelDate { get; set; }

        [ColumnSnackCase(nameof(CancelBy))]
        public string CancelBy { get; set; }

        [NotMapped]
        public int? BusinessCustomerBankId { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }

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

        [ColumnSnackCase(nameof(BusinessRegistrationImg))]
        public string BusinessRegistrationImg { get; set; }

        [ColumnSnackCase(nameof(Website))]
        public string Website { get; set; }

        [ColumnSnackCase(nameof(Fanpage))]
        public string Fanpage { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }

        [ColumnSnackCase(nameof(Server))]
        public string Server { get; set; }

        [ColumnSnackCase(nameof(Key))]
        public string Key { get; set; }

        [ColumnSnackCase(nameof(Secret))]
        public string Secret { get; set; }

        [ColumnSnackCase(nameof(AvatarImageUrl))]
        public string AvatarImageUrl { get; set; }

        [ColumnSnackCase(nameof(StampImageUrl))]
        public string StampImageUrl { get; set; }
    }
}


