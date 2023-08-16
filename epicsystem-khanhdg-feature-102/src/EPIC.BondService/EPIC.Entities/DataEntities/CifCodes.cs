using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EPIC.Entities.DataEntities
{
    [Table("EP_CIF_CODE", Schema = DbSchemas.EPIC)]
    public class CifCodes
    {
        public static string SEQ { get; } = $"SEQ_CIF_CODE";

        [Key]
        [ColumnSnackCase(nameof(CifId))]
        public int CifId { get; set; }

        [Required]
        [ColumnSnackCase(nameof(CifCode))]
        public string CifCode { get; set; }

        [ColumnSnackCase(nameof(InvestorId))]
        public int? InvestorId { get; set; }
        [JsonIgnore]
        public Investor Investor { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerId))]
        public int? BusinessCustomerId { get; set; }
        [JsonIgnore]
        public BusinessCustomer BusinessCustomer { get; set; }

        [Required]
        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; } = YesNo.NO;
    }
}
