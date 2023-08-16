using EPIC.Entities;
using EPIC.Utils;
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
    /// Định nghĩa các thuộc tính linh hoạt của dự án
    /// </summary>
    [Table("RST_PROJECT_DEFINE_ATTR", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(AttributeType), nameof(DataType), nameof(Deleted), IsUnique = false, Name = "IX_RST_PROJECT_DEFINE_ATTR")]
    public class RstProjectDefineAttr : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectDefineAttr).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Tên thuộc tính hiển thị tiếng việt
        /// </summary>
        [Required]
        [MaxLength(512)]
        [ColumnSnackCase(nameof(AttributeDisplayName))]
        public string AttributeDisplayName { get; set; }

        /// <summary>
        /// Tên thuộc tính hiển thị tiếng anh
        /// </summary>
        [MaxLength(512)]
        [ColumnSnackCase(nameof(AttributeDisplayNameEn))]
        public string AttributeDisplayNameEn { get; set; }

        /// <summary>
        /// Loại thuộc tính (1: nhóm cho thông tin dự án, 2: nhóm cho tiện ích dự án)<br/>
        /// <see cref="RstProjectAttributeTypes"/>
        /// </summary>
        public int AttributeType { get; set; }

        /// <summary>
        /// Kiểu dữ liệu (1:number, 2:string, 3:url, 4:dropdown - select one, 5:dropdown - multiple select, 6:tree table - select one, 7:tree table - multiple select, 8: list image)
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// Mã Icon đại diện
        /// </summary>
        [MaxLength(128)]
        [ColumnSnackCase(nameof(IconCode), TypeName = "VARCHAR2")]
        public string IconCode { get; set; }

        //cần cấu hình thuộc tính nào nữa thì thêm vào đây

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

    /// <summary>
    /// Định nghĩa các giá trị cho attribute
    /// </summary>
    [Table("RST_PROJECT_DEFINE_ATTR_VALUE", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(Status), nameof(Deleted), nameof(ParentId), IsUnique = false, Name = "IX_RST_PROJECT_DEFINE_ATTR_VALUE")]
    public class RstProjectDefineAttrValue : IFullAudited
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectDefineAttrValue).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(128)]
        [ColumnSnackCase(nameof(DefineAttrName))]
        public string DefineAttrName { get; set; }

        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }

        [MaxLength(1)]
        [ColumnSnackCase(nameof(Status), TypeName = "VARCHAR2")]
        public string Status { get; set; }

        [ColumnSnackCase(nameof(ParentId))]
        public int? ParentId { get; set; }

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

    /// <summary>
    /// Gom nhóm thuộc tính
    /// </summary>
    [Table("RST_PROJECT_DEFINE_GROUP_ATTR", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(AttributeType), IsUnique = false, Name = "IX_RST_PROJECT_DEFINE_GROUP_ATTR")]
    public class RstProjectDefineGroupAttr
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectDefineGroupAttr).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Tên gom nhóm tiếng việt
        /// </summary>
        [Required]
        [MaxLength(128)]
        [ColumnSnackCase(nameof(GroupDisplayName))]
        public string GroupDisplayName { get; set; }

        /// <summary>
        /// Tên gom nhóm hiển thị tiếng anh
        /// </summary>
        [MaxLength(128)]
        [ColumnSnackCase(nameof(GroupDisplayNameEn))]
        public string GroupDisplayNameEn { get; set; }

        /// <summary>
        /// Loại thuộc tính (1: nhóm cho thông tin dự án, 2: nhóm cho tiện ích dự án)<br/>
        /// <see cref="RstProjectAttributeTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(AttributeType))]
        public int AttributeType { get; set; }
    }

    /// <summary>
    /// Quan hệ DefineAttr và DefineGroupAttr
    /// </summary>
    [Table("RST_PROJECT_DEFINE_ATTR_IN_GROUP", Schema = DbSchemas.EPIC_REAL_ESTATE)]
    [Index(nameof(DefineAttrId), nameof(GroupAttrId), IsUnique = false, Name = "IX_RST_PROJECT_DEFINE_ATTR_IN_GROUP")]
    public class RstProjectDefineAttrInGroup
    {
        public static string SEQ { get; } = $"{DbConsts.SEQ}{nameof(RstProjectDefineAttrInGroup).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ColumnSnackCase(nameof(DefineAttrId))]
        public int DefineAttrId { get; set; }

        [ColumnSnackCase(nameof(GroupAttrId))]
        public int GroupAttrId { get; set; }

        /// <summary>
        /// thứ tự thuộc tính
        /// </summary>
        [ColumnSnackCase(nameof(SortOrder))]
        public int SortOrder { get; set; }
    }
}
