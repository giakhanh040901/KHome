using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_OWNER", Schema = DbSchemas.EPIC)]
    public class Owner : IFullAudited
    {
        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int BusinessCustomerId { get; set; }
        public BusinessCustomer BusinessCustomer { get; set; }
        [ColumnSnackCase(nameof(PartnerId))]
        public int? PartnerId { get; set; }
        [ColumnSnackCase(nameof(BusinessTurnover))]
        public decimal? BusinessTurnover { get; set; }
        [ColumnSnackCase(nameof(BusinessProfit))]
        public decimal? BusinessProfit { get; set; }
        [ColumnSnackCase(nameof(Roa))]
        public decimal? Roa { get; set; }
        [ColumnSnackCase(nameof(Roe))]
        public decimal? Roe { get; set; }
        [ColumnSnackCase(nameof(Image))]
        public string Image { get; set; }

        [ColumnSnackCase(nameof(Website))]
        public string Website { get; set; }
        [ColumnSnackCase(nameof(Hotline))]
        public string Hotline { get; set; }
        [ColumnSnackCase(nameof(Fanpage))]
        public string Fanpage { get; set; }
        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }
        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }
        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }
        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }
        public List<Project> Projects { get; } = new();
    }
}
