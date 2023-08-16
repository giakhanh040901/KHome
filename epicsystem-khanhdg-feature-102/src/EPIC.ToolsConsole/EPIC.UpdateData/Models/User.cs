using System;
using System.Collections.Generic;

#nullable disable

namespace EPIC.UpdateData.Models
{
    public partial class User
    {
        public decimal UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string IsDeleted { get; set; }
        public string SupperUser { get; set; }
        public string IpAddress { get; set; }
        public string Email { get; set; }
        public string CaUsername { get; set; }
        public string Description { get; set; }
        public string Usertype { get; set; }
        public decimal? PartnerId { get; set; }
        public decimal? TradingProviderId { get; set; }
        public string DataSearchRole { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastFailedLogin { get; set; }
        public decimal? FailAttemp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ResetPwRequire { get; set; }
        public string SubstituteType { get; set; }
        public decimal? InvestorId { get; set; }
        public decimal? SaleId { get; set; }
        public string IsFirstTime { get; set; }
        public string IsTempPassword { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExp { get; set; }
        public string PinCode { get; set; }
        public string IsVerifiedEmail { get; set; }
        public string IsTempPin { get; set; }
        public DateTime? LockedDate { get; set; }
        public string VerifyEmailCode { get; set; }
    }
}
