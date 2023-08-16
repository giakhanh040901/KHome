using EPIC.Entities.Dto.TradingProvider;
using EPIC.RealEstateEntities.Dto.RstDistributionProductItem;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstDistribution
{
    public class RstDistributionDto
    {
        public int Id { get; set; }
        public int TradingProviderId { get; set; }
        public int ProjectId { get; set; }

        /// <summary>
        /// Loại phân phối 1: Phân phối không độc quyền, 2: Phân phối độc quyền
        /// </summary>
        public int? DistributionType { get; set; }

        /// <summary>
        /// Tổng số lượng sản phẩm 
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Số lượng đã cọc
        /// </summary>
        public int QuantityDeposit { get; set; }

        /// <summary>
        /// Số lượng đã bán
        /// </summary>
        public int QuantitySold { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ngày phân phối
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc phân phối
        /// </summary>
        public DateTime? EndDate { get; set; }

        public int Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        /// <summary>
        /// Danh sách ngân hàng của chủ đầu tư
        /// </summary>
        public List<int> PartnerBankAccountIds { get; set; }

        public TradingProviderDto TradingProvider { get; set; }
        public RstProjectDto Project { get; set; }

        #region Các thông tin khác
        /// <summary>
        /// Được duyệt bởi
        /// </summary>
        public string ApproveBy { get; set; }

        /// <summary>
        /// Được tạm dừng bởi
        /// </summary>
        public string PauseBy { get; set; }
        #endregion
    }
}
