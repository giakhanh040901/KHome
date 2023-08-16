using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EPIC.CoreEntities.DataEntities
{
    [Table("EP_INVESTOR_REGISTER_LOG", Schema = DbSchemas.EPIC)]
    [Index(nameof(Deleted), IsUnique = false, Name = "IX_INVESTOR_REGISTER_LOG")]
    public class InvestorRegisterLog : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(InvestorRegisterLog).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(256)]
        [ColumnSnackCase(nameof(Phone), TypeName = "VARCHAR2")]
        public string Phone { get; set; }

        /// <summary>
        /// Loại tài khoản: 1: Đăng ký ngay, 2: OTP sent, 3: Nhập OTP thành công, 4: Thêm giấy tờ thành công
        /// 5: Start eKYC, 6: eKYC thành công, 7: Thêm ngân hàng thành công, 8: Hoàn thành đăng ký
        /// </summary>
        [ColumnSnackCase(nameof(Type))]
        public int Type { get; set; }

        /// <summary>
        /// Địa chỉ ip 
        /// </summary>
        [ColumnSnackCase(nameof(IpRequest), TypeName = "VARCHAR2")]
        [MaxLength(50)]
        public string IpRequest { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [DefaultValue(YesNo.NO)]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
