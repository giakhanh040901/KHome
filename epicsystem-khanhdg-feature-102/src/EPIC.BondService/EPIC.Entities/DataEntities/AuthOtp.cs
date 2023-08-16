using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_AUTH_OTP", Schema = DbSchemas.EPIC)]
    public class AuthOtp
    {
        public static string SEQ { get; } = $"SEQ_AUTH_OTP";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        public int Id { get; set; }

        [MaxLength(50)]
        [ColumnSnackCase(nameof(Phone))]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        [ColumnSnackCase(nameof(OtpCode))]
        public string OtpCode { get; set; }

        [Required]
        [ColumnSnackCase(nameof(UserId))]
        public int UserId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(CreatedTime))]
        public DateTime CreatedTime { get; set; }

        [Required]
        [ColumnSnackCase(nameof(ExpiredTime))]
        public DateTime ExpiredTime { get; set; }

        [Required]
        [ColumnSnackCase(nameof(IsActive))]
        public string IsActive { get; set; }
    }
}
