using EPIC.RealEstateEntities.Dto.RstProjectExtend;
using EPIC.Utils;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProject
{
    /// <summary>
    /// Tạo dự án
    /// </summary>
    public class RstCreateProjectDto
    {
        /// <summary>
        /// Chủ đầu tư
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// Mã dự án
        /// </summary>
        private string _code;
        public string Code 
        { 
            get => _code; 
            set => _code = value?.Trim();
        }

        /// <summary>
        /// Tên dự án
        /// </summary>
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        #region thông tin chung

        /// <summary>
        /// Tên tổng thầu
        /// </summary>
        private string _contractName;
        public string ContractorName
        {
            get => _contractName;
            set => _contractName = value?.Trim();
        }

        /// <summary>
        /// Liên kết giới thiệu Tổng thầu xây dựng
        /// </summary>
        private string _contractLink;
        public string ContractorLink
        {
            get => _contractLink;
            set => _contractLink = value?.Trim();
        }

        /// <summary>
        /// Mô tả thông tin Tổng thầu xây dựng
        /// </summary>
        private string _contractorDescription;
        public string ContractorDescription
        {
            get => _contractorDescription;
            set => _contractorDescription = value?.Trim();
        }

        /// <summary>
        /// Tên đơn vị vận hành
        /// </summary>
        private string _operatingUnit;
        public string OperatingUnit
        {
            get => _operatingUnit;
            set => _operatingUnit = value?.Trim();
        }

        /// <summary>
        /// Liên kết giới thiệu đơn vị vận hành
        /// </summary>
        private string _operatingUnitLink;
        public string OperatingUnitLink
        {
            get => _operatingUnitLink;
            set => _operatingUnitLink = value?.Trim();
        }

        /// <summary>
        /// Mô tả thông tin Đơn vị vận hành
        /// </summary>
        private string _operatingUnitDescription;
        public string OperatingUnitDescription
        {
            get => _operatingUnitDescription;
            set => _operatingUnitDescription = value?.Trim();
        }

        /// <summary>
        /// Liên kết website đến dự án  
        /// </summary>
        private string _website;
        public string Website
        {
            get => _website;
            set => _website = value?.Trim();
        }

        /// <summary>
        /// Đường dẫn Facebook của dự án
        /// </summary>
        private string _facebookLink;
        public string FacebookLink
        {
            get => _facebookLink;
            set => _facebookLink = value?.Trim();
        }

        /// <summary>
        /// Điện thoại Hotline liên hệ dự án
        /// </summary>
        private string _phone;
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        /// <summary>
        /// Loại hình dự án: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao
        /// </summary>
        [IntegerRange(AllowableValues = new int[] { RstProjectTypes.DAT_ĐAU_GIA, RstProjectTypes.DAT_BT, RstProjectTypes.DAT_GIAO},
        ErrorMessage = "Vui lòng các loại hình dự án sau: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao")]
        public int? ProjectType { get; set; }

        /// <summary>
        /// (Sản phẩm dự án) Loại hình sản phẩm của dự án
        /// </summary>
        public List<int> ProductTypes { get; set; }

        /// <summary>
        /// Loại hình phân phối: 1: Chủ đầu tư phân phối, 2: Đại lý phân phối
        /// </summary>
        public List<int> DistributionTypes { get; set; }

        /// <summary>
        /// Ngân hàng đảm bảo
        /// </summary>
        public List<int> GuaranteeBanks { get; set; }
        #endregion

        #region Thông số dự án

        /// <summary>
        /// Trạng thái tiến độ dự án: 1: Đang xây dựng, 2: Đang bán, 3: Đã hết hàng, 4: Tạm dừng bán, 5: Sắp mở bán
        /// </summary>
        public int? ProjectStatus { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        /// 
        private string _landArea;
        public string LandArea
        {
            get => _landArea;
            set => _landArea = value?.Trim();
        }

        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        private string _constructionArea;
        public string ConstructionArea
        {
            get => _constructionArea;
            set => _constructionArea = value?.Trim();
        }

        /// <summary>
        /// % Mật độ xây dựng
        /// </summary>
        public decimal? BuildingDensity { get; set; }

        /// <summary>
        /// Thửa đất số ...
        /// </summary>
        private string _landPlotNo;
        public string LandPlotNo
        {
            get => _landPlotNo;
            set => _landPlotNo = value?.Trim();
        }

        /// <summary>
        /// Tờ bản đồ số ...
        /// </summary>
        private string _mapSheetNo;
        public string MapSheetNo
        {
            get => _mapSheetNo;
            set => _mapSheetNo = value?.Trim();
        }

        /// <summary>
        /// Ngày khởi công
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Thời gian dự kiến hoàn thành
        /// </summary>
        private string _expectedHandoverTime;
        public string ExpectedHandoverTime
        {
            get => _expectedHandoverTime;
            set => _expectedHandoverTime = value?.Trim();
        }

        /// <summary>
        /// Tổng mức đầu tư
        /// </summary>
        public decimal? TotalInvestment { get; set; }

        /// <summary>
        /// Giá bán dự kiến
        /// </summary>
        public decimal? ExpectedSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Số căn
        /// </summary>
        public int? NumberOfUnit { get; set; }

        /// <summary>
        /// Mã thành phố
        /// </summary>
        private string _provinceCode;
        public string ProvinceCode
        {
            get => _provinceCode;
            set => _provinceCode = value?.Trim();
        }

        /// <summary>
        /// Địa chỉ dự án
        /// </summary>
        private string _address;
        public string Address
        {
            get => _address;
            set => _address = value?.Trim();
        }

        /// <summary>
        /// Kinh độ
        /// </summary>
        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => _latitude = value?.Trim();
        }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => _longitude = value?.Trim();
        }
        #endregion

        #region Thông tin khác của dự án
        /// <summary>
        /// Các thông tin khác của dự án
        /// </summary>
        public List<CreateRstProjectExtendDto> ProjectExtends { get; set; }
        #endregion
    }
}
