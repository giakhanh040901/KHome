using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Dự án
    /// </summary>
    [Table("RST_PROJECT", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(Status), IsUnique = false, Name = "IX_RST_PROJECT")]
    public class RstProject : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProject).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        /// <summary>
        /// Trang thai (1: Khoi tao, 2: Cho duyet, 3: Hoat dong, 4: Huy duyet, 5:Dong)
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Chủ đầu tư
        /// </summary>
        [ColumnSnackCase(nameof(OwnerId))]
        public int OwnerId { get; set; }

        [MaxLength(256)]
        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Da kiem tra (Y, N)
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(IsCheck), TypeName = "VARCHAR2")]
        [MaxLength(1)]
        public string IsCheck { get; set; }

        #region thông tin chung

        /// <summary>
        /// Tên tổng thầu
        /// </summary>
        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(ContractorName))]
        public string ContractorName { get; set; }

        /// <summary>
        /// Liên kết giới thiệu Tổng thầu xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(ContractorLink))]
        [MaxLength(512)]
        public string ContractorLink { get; set; }

        /// <summary>
        /// Mô tả thông tin Tổng thầu xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(ContractorDescription))]
        [MaxLength(1024)]
        public string ContractorDescription { get; set; }

        /// <summary>
        /// Tên đơn vị vận hành
        /// </summary>
        [MaxLength(256)]
        [ColumnSnackCase(nameof(OperatingUnit))]
        public string OperatingUnit { get; set; }

        /// <summary>
        /// Liên kết giới thiệu đơn vị vận hành
        /// </summary>
        [ColumnSnackCase(nameof(OperatingUnitLink))]
        [MaxLength(512)]
        public string OperatingUnitLink { get; set; }

        /// <summary>
        /// Mô tả thông tin Đơn vị vận hành
        /// </summary>
        [ColumnSnackCase(nameof(OperatingUnitDescription))]
        [MaxLength(1024)]
        public string OperatingUnitDescription { get; set; }

        /// <summary>
        /// Liên kết website đến dự án  
        /// </summary>
        [ColumnSnackCase(nameof(Website))]
        [MaxLength(512)]
        public string Website { get; set; }

        /// <summary>
        /// Đường dẫn facebook
        /// </summary>
        [ColumnSnackCase(nameof(FacebookLink))]
        [MaxLength(512)]
        public string FacebookLink { get; set; }

        /// <summary>
        /// Điện thoại Hotline liên hệ dự án
        /// </summary>
        [ColumnSnackCase(nameof(Phone))]
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// Loại hình dự án: 1: Đất đấu giá, 2: Đất BT, 3: Đất giao<br/>
        /// <see cref="RstProjectTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(ProjectType))]
        public int? ProjectType { get; set; }

        #endregion

        #region Thông số dự án

        /// <summary>
        /// Trạng thái tiến độ dự án: 1: Đang xây dựng, 2: Đang bán, 3: Đã hết hàng, 4: Tạm dừng bán, 5: Sắp mở bán
        /// </summary>
        [ColumnSnackCase(nameof(ProjectStatus))]
        public int? ProjectStatus { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        [ColumnSnackCase(nameof(LandArea))]
        [MaxLength(50)]
        public string LandArea { get; set; }

        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(ConstructionArea))]
        [MaxLength(50)]
        public string ConstructionArea { get; set; }

        /// <summary>
        /// % Mật độ xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(BuildingDensity))]
        public decimal? BuildingDensity { get; set; }

        /// <summary>
        /// Thửa đất số ...
        /// </summary>
        [ColumnSnackCase(nameof(LandPlotNo))]
        [MaxLength(128)]
        public string LandPlotNo { get; set; }

        /// <summary>
        /// Tờ bản đồ số ...
        /// </summary>
        [ColumnSnackCase(nameof(MapSheetNo))]
        [MaxLength(128)]
        public string MapSheetNo { get; set; }

        /// <summary>
        /// Ngày khởi công
        /// </summary>
        [ColumnSnackCase(nameof(StartDate), TypeName = "DATE")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Thời gian dự kiến hoàn thành
        /// </summary>
        [ColumnSnackCase(nameof(ExpectedHandoverTime))]
        [MaxLength(128)]
        public string ExpectedHandoverTime { get; set; }

        /// <summary>
        /// Tổng mức đầu tư
        /// </summary>
        [ColumnSnackCase(nameof(TotalInvestment))]
        public decimal? TotalInvestment { get; set; }

        /// <summary>
        /// Giá bán dự kiến
        /// </summary>
        [ColumnSnackCase(nameof(ExpectedSellingPrice))]
        public decimal? ExpectedSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối thiểu
        /// </summary>
        [ColumnSnackCase(nameof(MinSellingPrice))]
        public decimal? MinSellingPrice { get; set; }

        /// <summary>
        /// Giá bán tối đa
        /// </summary>
        [ColumnSnackCase(nameof(MaxSellingPrice))]
        public decimal? MaxSellingPrice { get; set; }

        /// <summary>
        /// Số căn
        /// </summary>
        [ColumnSnackCase(nameof(NumberOfUnit))]
        public int? NumberOfUnit { get; set; }

        /// <summary>
        /// Mã thành phố
        /// </summary>
        [ColumnSnackCase(nameof(ProvinceCode))]
        [MaxLength(6)]
        public string ProvinceCode { get; set; }

        /// <summary>
        /// Địa chỉ dự án
        /// </summary>
        [ColumnSnackCase(nameof(Address))]
        [MaxLength(1024)]
        public string Address { get; set; }

        /// <summary>
        /// Kinh độ
        /// </summary>
        [ColumnSnackCase(nameof(Latitude))]
        [MaxLength(512)]
        public string Latitude { get; set; }

        /// <summary>
        /// Vĩ độ
        /// </summary>
        [ColumnSnackCase(nameof(Longitude))]
        [MaxLength(512)]
        public string Longitude { get; set; }
        #endregion

        #region Mô tả thông tin dự án
        /// <summary>
        /// Loại nội dung tổng quan: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(ContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string ContentType { get; set; }

        /// <summary>
        /// Nội dung tổng quan
        /// </summary>
        [ColumnSnackCase(nameof(OverviewContent), TypeName = "CLOB")]
        public string OverviewContent { get; set; }
        #endregion

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }

        public override bool Equals(object obj)
        {
            RstProject project2 = obj as RstProject;
            if (obj == null)
            {
                return base.Equals(obj);
            }
            else
                return Id == project2.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        #endregion

        /// <summary>
        /// Chia sẻ dự án
        /// </summary>
        public List<RstProjectInformationShare> RstProjectInformationShares { get; } = new();
    }

    [Table("RST_PROJECT_WITH_GROUP_ATTR", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(ProjectId), nameof(AttributeType), nameof(GroupAttrId), IsUnique = false, Name = "IX_RST_PROJECT_WITH_GROUP_ATTR")]
    public class RstProjectWithGroupAttr : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectWithGroupAttr).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Loại thuộc tính (1: nhóm cho thông tin dự án, 2: nhóm cho tiện ích dự án)<br/>
        /// <see cref="RstProjectAttributeTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(AttributeType))]
        public int AttributeType { get; set; }

        /// <summary>
        /// Chọn nhóm thuộc tính
        /// </summary>
        [ColumnSnackCase(nameof(GroupAttrId))]
        public int? GroupAttrId { get; set; }

        /// <summary>
        /// Hoặc chọn trực tiếp thuộc tính
        /// </summary>
        [ColumnSnackCase(nameof(DefineAttrId))]
        public int? DefineAttrId { get; set; }

        /// <summary>
        /// Sắp xếp thứ tự
        /// </summary>
        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }

        #region audit
        [ColumnSnackCase(nameof(CreatedDate), TypeName = "DATE")]
        public DateTime? CreatedDate { get; set; }

        [ColumnSnackCase(nameof(CreatedBy))]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [ColumnSnackCase(nameof(ModifiedDate), TypeName = "DATE")]
        public DateTime? ModifiedDate { get; set; }

        [ColumnSnackCase(nameof(ModifiedBy))]
        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [ColumnSnackCase(nameof(Deleted), TypeName = "VARCHAR2")]
        [Required]
        [MaxLength(1)]
        public string Deleted { get; set; }
        #endregion
    }
}
