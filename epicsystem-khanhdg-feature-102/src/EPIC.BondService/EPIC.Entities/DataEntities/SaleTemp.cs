using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CORE_SALE_TEMP", Schema = DbSchemas.EPIC)]
    public class SaleTemp : IFullAudited
	{
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(InvestorId))]
        public int? InvestorId { get; set; }
        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }
        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        [ColumnSnackCase(nameof(DepartmentId))]
        public int? DepartmentId { get; set; }
        [ColumnSnackCase(nameof(EmployeeCode))]

        public string EmployeeCode { get; set; }
        [ColumnSnackCase(nameof(SaleType))]
        public int? SaleType { get; set; }
        [ColumnSnackCase(nameof(SaleParentId))]
        public int? SaleParentId { get; set; }
        [ColumnSnackCase(nameof(Status))]

        public int? Status { get; set; }
        [ColumnSnackCase(nameof(Source))]
        public int Source { get; set; }

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
