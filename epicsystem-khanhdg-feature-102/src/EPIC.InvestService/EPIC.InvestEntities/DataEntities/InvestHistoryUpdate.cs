using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils;
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
    [Table("EP_INV_HISTORY_UPDATE", Schema = DbSchemas.EPIC)]
    public class InvestHistoryUpdate : ICreatedBy
    {
        public static string SEQ { get; } = $"SEQ_INV_HISTORY_UPDATE";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Update bảng nào<br/>
        /// <see cref="RstHistoryUpdateTables"/>
        /// </summary>
        [ColumnSnackCase(nameof(UpdateTable))]
        [MaxLength(128)]
        public int UpdateTable { get; set; }

        /// <summary>
        /// Id Bảng thật
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(RealTableId))]
        public int RealTableId { get; set; }

        /// <summary>
        /// Giá trị cũ
        /// </summary>
        [ColumnSnackCase(nameof(OldValue))]
        [MaxLength(128)]
        public string OldValue { get; set; }

        /// <summary>
        /// Giá trị mới
        /// </summary>
        [ColumnSnackCase(nameof(NewValue))]
        [MaxLength(128)]
        public string NewValue { get; set; }

        /// <summary>
        /// Tên trường
        /// <see cref="RstFieldName"/>
        /// </summary>
        [ColumnSnackCase(nameof(FieldName))]
        [MaxLength(128)]
        public string FieldName { get; set; }

        /// <summary>
        /// Hành động (1: thêm mới, 2: cập nhật, 3: xoá)
        /// <see cref="ActionTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(Action))]
        public int Action { get; set; }
        /// <summary>
        /// Nội dung tổng quan là làm cái gì (vd: khởi tạo lệnh, tạo thanh toán, cập nhật(cập nhật trạng thái))
        /// </summary>
        [Required]
        [ColumnSnackCase(nameof(Summary))]
        [MaxLength(512)]
        public string Summary { get; set; }

        /// <summary>
        /// Ngay thuc hien
        /// </summary>
        [ColumnSnackCase(nameof(CreatedDate))]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người thực hiện
        /// </summary>
        [ColumnSnackCase(nameof(CreatedBy))]
        public string CreatedBy { get; set; }

        public InvestHistoryUpdate()
        {
        }

        public InvestHistoryUpdate(int realTableId, string oldValue, string newValue, string fieldName, int updateTable, int action, string summary, DateTime date)
        {
            RealTableId = realTableId;
            OldValue = oldValue;
            NewValue = newValue;
            FieldName = fieldName;
            UpdateTable = updateTable;
            Action = action;
            Summary = summary;
            CreatedDate = date;
        }
    }
}
