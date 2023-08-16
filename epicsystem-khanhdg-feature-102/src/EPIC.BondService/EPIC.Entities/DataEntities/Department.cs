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
    [Table("EP_CORE_DEPARTMENT", Schema = DbSchemas.EPIC)]
    public class Department
    {
        [Key]
        [ColumnSnackCase(nameof(DepartmentId))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DepartmentId { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }

        [ColumnSnackCase(nameof(DepartmentName))]
        public string DepartmentName { get; set; }

        [ColumnSnackCase(nameof(DepartmentAddress))]
        public string DepartmentAddress { get; set; }

        [ColumnSnackCase(nameof(Area))]
        public string Area { get; set; }

        [ColumnSnackCase(nameof(ParentId))]
        public int? ParentId { get; set; }

        [ColumnSnackCase(nameof(DepartmentLevel))]
        public int? DepartmentLevel { get; set; }

        [ColumnSnackCase(nameof(ManagerId))]
        public int? ManagerId { get; set; }

        [ColumnSnackCase(nameof(ManagerId2))]
        public int? ManagerId2 { get; set; }

        [ColumnSnackCase(nameof(ManagerStartDate))]
        public DateTime? ManagerStartDate { get; set; }

        [ColumnSnackCase(nameof(Manager2StartDate))]
        public DateTime? Manager2StartDate { get; set; }
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
	}

}
