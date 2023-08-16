using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPIC.RealEstateEntities.DataEntities
{
    /// <summary>
    /// Bảng hàng - sản phẩm bán
    /// </summary>
    [Table("RST_PRODUCT_ITEM", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Deleted), nameof(PartnerId), nameof(ProjectId), IsUnique = false, Name = "IX_RST_PRODUCT_ITEM")]
    public class RstProductItem : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProductItem).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(PartnerId))]
        public int PartnerId { get; set; }

        [ColumnSnackCase(nameof(ProjectId))]
        public int ProjectId { get; set; }

        /// <summary>
        /// Trạng thái sổ đỏ (1: có sổ đỏ, 2: sổ đỏ 50 năm, 3: sổ lâu dài, 4: chưa có sổ đỏ)
        /// <see cref="RstRedBookTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(RedBookType))]
        public int RedBookType { get; set; }

        /// <summary>
        /// Mã căn/mã sản phẩm
        /// </summary>
        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Code))]
        public string Code { get; set; }

        /// <summary>
        /// Số căn/tên
        /// </summary>
        [Required]
        [MaxLength(256)]
        [ColumnSnackCase(nameof(Name))]
        public string Name { get; set; }

        /// <summary>
        /// Số tầng bao nhiêu
        /// </summary>
        [ColumnSnackCase(nameof(NumberFloor), TypeName = "VARCHAR2")]
        [MaxLength(10)]
        public string NumberFloor { get; set; }

        /// <summary>
        /// Kiểu phòng ngủ - số phòng (1: 1 phòng ngủ, 2: 2 phòng ngủ, 3: 3 phòng ngủ, 4: 4 phòng ngủ, 5: 5 phòng ngủ, 6: 6 phòng ngủ, 
        /// 7: 7 phòng ngủ, 8: 8 phòng ngủ, 9: 1 phòng ngủ + 1, 10: 2 phòng ngủ + 1, 11: 3 phòng ngủ + 1, 12: 4 phòng ngủ + 1)
        /// <see cref="RstRoomTypes"/>
        /// <see cref="RstProductItemData"/>
        /// </summary>
        [ColumnSnackCase(nameof(RoomType))]
        public int? RoomType { get; set; }

        /// <summary>
        /// Hướng cửa (1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Nam, 
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)
        /// <see cref="RstDirections"/>
        /// <see cref="RstProductItemData"/>
        /// </summary>
        [ColumnSnackCase(nameof(DoorDirection))]
        public int? DoorDirection { get; set; }

        /// <summary>
        /// Hướng ban công (1: Đông, 2: Tây, 3: Nam, 4: Bắc, 5: Đông Nam, 6: Đông Bắc, 7: Tây Nam, 8: Tây Bắc, 9: Đông Nam + Tây Bắc, 
        /// 10: Đông Nam + Đông Bắc, 11: Tây Nam + Tây Bắc, 12: Đông Nam + Tây Bắc, 13: Đông Bắc + Tây Bắc, 14: Đông Bắc + Tây Nam)
        /// <see cref="RstDirections"/>
        /// <see cref="RstProductItemData"/>
        /// </summary>
        [ColumnSnackCase(nameof(BalconyDirection))]
        public int? BalconyDirection { get; set; }

        /// <summary>
        /// Vị trí căn/sản phẩm (1: Căn giữa, 2: Căn góc, 3: Cổng chính, 4: Toà riêng, 5: Căn thông tầng)
        /// <see cref="RstProductLocations"/>
        /// </summary>
        [ColumnSnackCase(nameof(ProductLocation))]
        public int? ProductLocation { get; set; }

        /// <summary>
        /// Loại hình căn/sản phẩm (1: Căn đơn, 2: Căn ghép)
        /// <see cref="RstProductTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(ProductType))]
        public int? ProductType { get; set; }

        /// <summary>
        /// Loại bàn giao (1: Bàn giao thô, 2: Nột thất cơ bản, 3: Nội thất liền tường, 4: Nội thất cao cấp, 5: Full nội thất)
        /// <see cref="RstHandingTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(HandingType))]
        public int? HandingType { get; set; }

        /// <summary>
        /// Hướng view - mô tả
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(ViewDescription))]
        public string ViewDescription { get; set; }

        /// <summary>
        /// Diện tích thông thuỷ
        /// </summary>
        [ColumnSnackCase(nameof(CarpetArea), TypeName = "NUMBER(18, 6)")]
        public decimal CarpetArea { get; set; }

        /// <summary>
        /// Diện tích tim tường
        /// </summary>
        [ColumnSnackCase(nameof(BuiltUpArea), TypeName = "NUMBER(18, 6)")]
        public decimal BuiltUpArea { get; set; }

        /// <summary>
        /// Diện tích đất
        /// </summary>
        [ColumnSnackCase(nameof(LandArea), TypeName = "NUMBER(18, 6)")]
        public decimal LandArea { get; set; }

        /// <summary>
        /// Diện tích xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(ConstructionArea), TypeName = "NUMBER(18, 6)")]
        public decimal ConstructionArea { get; set; }

        /// <summary>
        /// Giá bán nhập giá hoặc không nếu nhập giá thì luồng xử lý là có đặt cọc,
        /// nếu không nhập giá thì luồng xử lý lúc giao dịch là liên hệ
        /// </summary>
        [ColumnSnackCase(nameof(Price), TypeName = "NUMBER(18, 6)")]
        public decimal? Price { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(MaterialContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string MaterialContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả vật liệu thi công
        /// </summary>
        [ColumnSnackCase(nameof(MaterialContent), TypeName = "CLOB")]
        public string MaterialContent { get; set; }

        /// <summary>
        /// Loại nội dung sơ đồ thiết kế: MARKDOWN, HTML
        /// </summary>
        [ColumnSnackCase(nameof(DesignDiagramContentType), TypeName = "VARCHAR2")]
        [MaxLength(20)]
        public string DesignDiagramContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả sơ đồ thiết kế
        /// </summary>
        [ColumnSnackCase(nameof(DesignDiagramContent), TypeName = "CLOB")]
        public string DesignDiagramContent { get; set; }

        /// <summary>
        /// Nhập căn ghép
        /// </summary>
        [ColumnSnackCase(nameof(CompoundRoom), TypeName = "VARCHAR2")]
        public string CompoundRoom { get; set; }

        /// <summary>
        /// Tầng ghép
        /// </summary>
        [ColumnSnackCase(nameof(CompoundFloor), TypeName = "VARCHAR2")]
        public string CompoundFloor { get; set; }

        ///// <summary>
        ///// Có khoá hay không, có thể khoá thủ công hoặc tự động
        ///// </summary>
        [Required]
        [MaxLength(1)]
        [ColumnSnackCase(nameof(IsLock), TypeName = "VARCHAR2")]
        public string IsLock { get; set; }

        /// <summary>
        /// Trạng thái của sản phẩm (1: Khởi tạo (có thể là chưa mở bán hoặc đang mở bán) 2: Giữ chỗ, 3: Khóa căn, 4: Đã cọc, 5: Đã bán) 
        /// 2 trạng thái tự logic 6: chưa mở bán và 7: đang bán<br/>
        /// <see cref="RstProductItemStatus"/>
        /// </summary>
        [ColumnSnackCase(nameof(Status))]
        public int Status { get; set; }

        /// <summary>
        /// Mật độ xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(BuildingDensityId))]
        public int? BuildingDensityId { get; set; }

        /// <summary>
        /// Phân loại sản phẩm (1: Căn hộ thông thường, 2: Căn hộ Studio, 3: Căn hộ Officetel, 4: Căn hộ Shophouse, 5: Căn hộ Penthouse, 6: Căn hộ Duplex, 
        /// 7: Căn hộ Sky Villa, 8: Nhà ở nông thôn, 9: Biệt thự nhà ở, 10: Liền kề, 11: Chung cư thấp tầng, 12: Căn Shophouse, 13: Biệt thự nghỉ dưỡng, 14: Villa, 15: DuplexPool)
        /// <see cref="RstClassifyType"/>
        /// <see cref="RstProductItemData"/>
        /// </summary>
        [ColumnSnackCase(nameof(ClassifyType))]
        public int ClassifyType { get; set; }

        /// <summary>
        /// Diện tích tính giá
        /// </summary>
        [ColumnSnackCase(nameof(PriceArea), TypeName = "NUMBER(18, 6)")]
        public decimal? PriceArea { get; set; }

        /// <summary>
        /// Đơn giá
        /// </summary>
        [ColumnSnackCase(nameof(UnitPrice), TypeName = "NUMBER(18, 6)")]
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Tầng số
        /// </summary>
        [ColumnSnackCase(nameof(NoFloor))]
        public string NoFloor { get; set; }

        /// <summary>
        /// Thời gian bàn giao
        /// </summary>
        [ColumnSnackCase(nameof(HandoverTime), TypeName = "DATE")]
        public DateTime? HandoverTime { get; set; }

        /// <summary>
        /// Tổng lượt view của bảng hàng
        /// </summary>
        [ColumnSnackCase(nameof(ViewCount))]
        public int ViewCount { get; set; }

        /// <summary>
        /// Diện tích sàn xây dựng
        /// </summary>
        [ColumnSnackCase(nameof(FloorBuildingArea), TypeName = "NUMBER(18, 6)")]
        public decimal? FloorBuildingArea { get; set; }
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
