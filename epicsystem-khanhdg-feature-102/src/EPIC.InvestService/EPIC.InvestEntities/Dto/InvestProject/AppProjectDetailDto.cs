using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    /// <summary>
    /// Lấy thông tin của lô
    /// </summary>
    public class AppProjectDto
    {
        /// <summary>
        /// Id Bán phân phối
        /// </summary>
        public int DistributionId { get; set; }
        public int ProjectId { get; set; }

        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }
        /// <summary>
        /// Id đại lý
        /// </summary>
        public int? TradingProviderId { get; set; }
        /// <summary>
        /// Mã sản phẩm đầu tư
        /// </summary>
        public string InvCode { get; set; }

        /// <summary>
        /// Ảnh đại diện dự án
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Ảnh phân phối sản phẩm
        /// </summary>
        public string DistributionImage { get; set; }

        /// <summary>
        /// Diện tích
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Kinh độ
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Mô tả vị trí
        /// </summary>
        public string LocationDescription { get; set; }

        /// <summary>
        /// Hạn mức đầu tư
        /// </summary>
        public decimal? TotalInvestment { get; set; }

        /// <summary>
        /// Tổng mức đầu tư
        /// </summary>
        public decimal? TotalInvestmentDisplay { get; set; }

        /// <summary>
        /// Tiến độ dự án
        /// </summary>
        public string ProjectProgress { get; set; }
        
        /// <summary>
        /// Thông tin chủ đầu tư
        /// </summary>
        public AppOwnerDto Owner { get; set; }
        /// <summary>
        /// Thông tin chính sách
        /// </summary>
        public List<AppPolicyFileDto> PolicyFiles { get; set; }
        /// <summary>
        /// Số tiền đầu tư tối đa
        /// </summary>
        public decimal? MaxMoney { get; set; }
        /// <summary>
        /// Thông tin hồ sơ pháp lý
        /// </summary>
        public List<AppJuridicalFileDto> JuridicalFiles { get; set; }

        public List<AppDistributionFileDto> DistributionFiles { get; set; }
        public List<ProjectImageDto> ProjectImages { get; set; } 
        public List<ProjectTypeDto> ProjectTypes { get; set; }

        public AppOverViewProjectDto ProjectOverview { get; set; }
    }
}
