using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.GarnerEntities.Dto.GarnerPolicy;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class GarnerDistributionDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int TradingProviderId { get; set; }
        public DateTime? OpenCellDate { get; set; }
        public DateTime? CloseCellDate { get; set; }
        public int Status { get; set; }
        public string IsClose { get; set; }
        public string IsShowApp { get; set; }
        public string IsCheck { get; set; }
        public string IsDefault { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        public string OverviewImageUrl { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }

        #region Thông tin phân phối
        /// <summary>
        /// (Chung) Có phân phối hạn mức không
        /// </summary>
        public string HasTotalInvestmentSub { get; set; }

        /// <summary>
        /// (Chung)Hạn mức số tiền phân phối
        /// </summary>
        public decimal? TotalInvestmentSub { get; set; }

        /// <summary>
        /// (Bond, Cps) Hạn mức số lượng phân phối
        /// </summary>
        public long? Quantity { get; set; }

        /// <summary>
        /// Số tiền đã đầu tư
        /// </summary>
        public decimal? IsInvested { get; set; }
        #endregion

        /// <summary>
        /// Thông tin dự án
        /// </summary>
        public GarnerProductDto GarnerProduct { get; set; }

        /// <summary>
        /// Danh sách ngân hàng thụ hưởng 
        /// </summary>
        public List<int> TradingBankAccountCollects { get; set; }

        /// <summary>
        /// Danh sách ngân hàng chi tiền
        /// </summary>
        public List<int> TradingBankAccountPays { get; set; }

        public List<GarnerPolicyMoreInfoDto> Policies { get; set; }
    }
}
