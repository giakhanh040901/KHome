using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Invest;
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
    [Table("EP_INV_DISTRIBUTION", Schema = DbSchemas.EPIC)]
    public class Distribution : IFullAudited
	{
        public static string SEQ { get; } = $"SEQ_INV_DISTRIBUTION";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int TradingProviderId { get; set; }
        public TradingProvider TradingProvider { get; set; }

        [ColumnSnackCase(nameof(BusinessCustomerBankAccId))]
        public int? BusinessCustomerBankAccId { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        [ColumnSnackCase(nameof(OpenCellDate))]
        public DateTime? OpenCellDate { get; set; }

        [ColumnSnackCase(nameof(CloseCellDate))]
        public DateTime? CloseCellDate { get; set; }

        [ColumnSnackCase(nameof(ContentType))]
        public string ContentType{ get; set; }

        [ColumnSnackCase(nameof(OverviewContent))]
        public string OverviewContent { get; set; }

        [ColumnSnackCase(nameof(OverviewImageUrl))]
        public string OverviewImageUrl { get; set; }

        [ColumnSnackCase(nameof(IsClose))]
        public string IsClose { get; set; }

        [ColumnSnackCase(nameof(IsShowApp))]
        public string IsShowApp { get; set; }

        [ColumnSnackCase(nameof(IsCheck))]
        public string IsCheck { get; set; }

        /// <summary>
        /// Hình thức chi trả lợi tức, đáo hạn
        /// <see cref="InvestMethodInterests"/>
        /// </summary>
        [ColumnSnackCase(nameof(MethodInterest))]
        public int MethodInterest { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate))]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(Deleted))]
        public string Deleted { get; set; }

        [ColumnSnackCase(nameof(Image))]
        public string Image { get; set; }
        public List<Policy> Policies { get; } = new();
        public List<InvOrder> Orders { get; } = new();
        public List<PolicyDetail> PolicyDetails { get; } = new();

    }
}
