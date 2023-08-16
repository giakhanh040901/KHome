using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.IdentityEntities.DataEntities
{
    [Table("USERS", Schema = DbSchemas.EPIC)]
    public class Users
    {
        public static string SEQ { get; } = $"{DbSchemas.EPIC}.SEQ_USERS";

        [Key]
        [ColumnSnackCase(nameof(UserId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Column("USERNAME")]
        public string UserName { get; set; }

        [ColumnSnackCase(nameof(DisplayName))]
        public string DisplayName { get; set; }

        [ColumnSnackCase(nameof(Password))]
        public string Password { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(IsDeleted))]
        public string IsDeleted { get; set; }

        [ColumnSnackCase(nameof(SupperUser))]
        public string SupperUser { get; set; }

        [ColumnSnackCase(nameof(IpAddress))]
        public string IpAddress { get; set; }

        [ColumnSnackCase(nameof(Email))]
        public string Email { get; set; }

        [ColumnSnackCase(nameof(CaUsername))]
        public string CaUsername { get; set; }

        [ColumnSnackCase(nameof(Description))]
        public string Description { get; set; }

        [Column("USERTYPE")]
        public string UserType { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public decimal? PartnerId { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public decimal? TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(DataSearchRole))]
        public string DataSearchRole { get; set; }

        /// <summary>
        /// Ảnh đại diện
        /// </summary>
        [ColumnSnackCase(nameof(AvatarImageUrl))]
        public string AvatarImageUrl { get; set; }

        [ColumnSnackCase(nameof(LastLogin))]
        public DateTime? LastLogin { get; set; }

        [ColumnSnackCase(nameof(LastFailedLogin))]
        public DateTime? LastFailedLogin { get; set; }

        [ColumnSnackCase(nameof(FailAttemp))]
        public decimal? FailAttemp { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(DeletedBy))]
        public string DeletedBy { get; set; }

        [ColumnSnackCase(nameof(DeletedDate))]
        public DateTime? DeletedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ResetPwRequire))]
        public string ResetPwRequire { get; set; }

        [ColumnSnackCase(nameof(SubstituteType))]
        public string SubstituteType { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public decimal? InvestorId { get; set; }

        [ColumnSnackCase(nameof(SaleId))]
        public decimal? SaleId { get; set; }

        [ColumnSnackCase(nameof(IsFirstTime))]
        public string IsFirstTime { get; set; }

        [ColumnSnackCase(nameof(IsTempPassword))]
        public string IsTempPassword { get; set; }

        [ColumnSnackCase(nameof(ResetPasswordToken))]
        public string ResetPasswordToken { get; set; }

        [ColumnSnackCase(nameof(ResetPasswordTokenExp))]
        public DateTime? ResetPasswordTokenExp { get; set; }

        [ColumnSnackCase(nameof(PinCode))]
        public string PinCode { get; set; }

        [ColumnSnackCase(nameof(IsVerifiedEmail))]
        public string IsVerifiedEmail { get; set; }

        [ColumnSnackCase(nameof(LockedDate))]
        public DateTime? LockedDate { get; set; }

        [ColumnSnackCase(nameof(IsTempPin))]
        public string IsTempPin { get; set; }

        [ColumnSnackCase(nameof(VerifyEmailCode))]
        public string VerifyEmailCode { get; set; }

        [ColumnSnackCase(nameof(LastDevice))]
        public string LastDevice { get; set; }
        public List<UserData> UserDatas { get; set; } = new();
    }
}
