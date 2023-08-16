using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectTradingProvider;
using EPIC.InvestEntities.Dto.ProjectType;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Project
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Chủ đầu tư không được bỏ trống ")]
        public int? OwnerId { get; set; }

        [Required(ErrorMessage = "Tổng thầu không được bỏ trống ")]
        public int? GeneralContractorId { get; set; }

        [Required(ErrorMessage = "Mã sản phẩm đầu tư không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Mã sản phẩm đầu tư không được dài hơn 256 ký tự")]
        private string _invCode;
        public string InvCode
        {
            get => _invCode;
            set => _invCode = value?.Trim();
        }

        [Required(ErrorMessage = "Tên sản phẩm đầu tư không được bỏ trống")]
        [StringLength(256, ErrorMessage = "Mã sản phẩm đầu tư không được dài hơn 256 ký tự")]
        private string _invName;
        public string InvName
        {
            get => _invName;
            set => _invName = value?.Trim();
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => _content = value?.Trim();
        }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        private string _image;
        public string Image
        {
            get => _image;
            set => _image = value?.Trim();
        }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Có bảo lãnh thanh toán không? (Y: Có, N: Không)")]
        public string IsPaymentGuarantee { get; set; }

        public string _area;
        public string Area
        {
            get => _area;
            set => _area = value?.Trim();
        }
        [StringLength(256, ErrorMessage = "Kinh độ không được dài hơn 50 ký tự")]
        public string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => _longitude = value?.Trim();
        }

        [StringLength(256, ErrorMessage = "Vĩ độ không được dài hơn 50 ký tự")]
        public string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => _latitude = value?.Trim();
        }

        [StringLength(256, ErrorMessage = "Mô tả vị trí không được dài hơn 256 ký tự")]
        public string _locationDescription;
        public string LocationDescription
        {
            get => _locationDescription;
            set => _locationDescription = value?.Trim();
        }
        public decimal? TotalInvestment { get; set; }
        public decimal? TotalInvestmentDisplay { get; set; }
        public int? ProjectType { get; set; }


        [StringLength(256, ErrorMessage = "Tiến độ dự án không được dài hơn 256 ký tự")]
        private string _grojectProgress;
        public string ProjectProgress
        {
            get => _grojectProgress;
            set => _grojectProgress = value?.Trim();
        }


        [StringLength(256, ErrorMessage = "Tổ chức bảo lãnh không được dài hơn 256 ký tự")]
        private string _guaranteeOrganization;
        public string GuaranteeOrganization
        {
            get => _guaranteeOrganization;
            set => _guaranteeOrganization = value?.Trim();
        }

        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Có yêu cầu tổng mức đầu tư cho các đại lý hay không? (Y: Có, N: Không)")]
        public string HasTotalInvestmentSub { get; set; }

        public List<int?> ProjectTypes { get; set; }

        /// <summary>
        /// Danh sách đại lý của dự án
        /// </summary>
        public List<CreateProjectTradingProviderDto> ProjectTradingProvider { get; set; }
    }
}
