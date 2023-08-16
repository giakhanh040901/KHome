using EPIC.Entities;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Shared;

namespace EPIC.InvestEntities.DataEntities
{
    [Table("EP_INV_PROJECT", Schema = DbSchemas.EPIC)]
    public class Project : IFullAudited
    {
        public static string SEQ { get; } = $"SEQ_INV_PROJECT";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(OwnerId))]
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }

        [ColumnSnackCase(nameof(GeneralContractorId))]
        public int GeneralContractorId { get; set; }
        public GeneralContractor GeneralContractor { get; set; }

        [ColumnSnackCase(nameof(InvCode))]
        public string InvCode { get; set; }

        [ColumnSnackCase(nameof(InvName))]
        public string InvName { get; set; }

        [ColumnSnackCase(nameof(Content))]
        public string Content { get; set; }

        [ColumnSnackCase(nameof(StartDate))]
        public DateTime? StartDate { get; set; }

        [ColumnSnackCase(nameof(EndDate))]
        public DateTime? EndDate { get; set; }

        [ColumnSnackCase(nameof(Image))]
        public string Image { get; set; }

        [ColumnSnackCase(nameof(IsPaymentGuarantee))]
        public string IsPaymentGuarantee { get; set; }

        [ColumnSnackCase(nameof(Area))]
        public string Area { get; set; }

        [ColumnSnackCase(nameof(Longitude))]
        public string Longitude { get; set; }

        [ColumnSnackCase(nameof(Latitude))]
        public string Latitude { get; set; }

        [ColumnSnackCase(nameof(LocationDescription))]
        public string LocationDescription { get; set; }

        [ColumnSnackCase(nameof(TotalInvestment))]
        public decimal? TotalInvestment { get; set; }

        [ColumnSnackCase(nameof(TotalInvestmentDisplay))]
        public decimal? TotalInvestmentDisplay { get; set; }

        [ColumnSnackCase(nameof(ProjectType))]
        public int? ProjectType { get; set; }

        [ColumnSnackCase(nameof(ProjectProgress))]
        public string ProjectProgress { get; set; }

        [ColumnSnackCase(nameof(GuaranteeOrganization))]
        public string GuaranteeOrganization { get; set; }

        [ColumnSnackCase(nameof(IsCheck))]
        public string IsCheck { get; set; }

        [ColumnSnackCase(nameof(Status))]
        public int? Status { get; set; }

        /// <summary>
        /// Có yêu cầu tổng đầu tư cho từng đại lý hay không
        /// </summary>
        [ColumnSnackCase(nameof(HasTotalInvestmentSub))]
        public string HasTotalInvestmentSub { get; set; }

        [ColumnSnackCase(nameof(TradingProviderId))]
        public int? TradingProviderId { get; set; }

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
        public List<Distribution> Distributions { get; } = new();
        public List<ProjectTradingProvider> ProjectTradingProviders { get; } = new();
        /// <summary>
        /// Chia sẻ thong tin dự án
        /// </summary>
        public List<InvestProjectInformationShare> ProjectInformationShares { get; } = new();
    }
}
