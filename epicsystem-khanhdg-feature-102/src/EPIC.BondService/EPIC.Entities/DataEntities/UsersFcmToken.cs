using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    /// <summary>
    /// Lưu fcm token từ app bắn lên khi login
    /// </summary>
    [Table("USERS_FCM_TOKEN", Schema = DbSchemas.EPIC)]
    public class UsersFcmToken
    {
        public static string SEQ { get; } = $"SEQ_USERS_FCM_TOKEN";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(UserId))]
        public int UserId { get; set; }

        [ColumnSnackCase(nameof(FcmToken))]
        public string FcmToken { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get ; set ; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get ; set ; }
    }
}
