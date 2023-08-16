using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.Entities;
using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;

namespace EPIC.EventEntites.Entites
{
    /// <summary>
    /// Lịch sử thay đổi, chỉ lưu 1 bản ghi mỗi lần thay đổi
    /// </summary>
    [Table("EVT_HISTORY_UPDATE", Schema = DbSchemas.EPIC_EVENT)]
    [Index(nameof(RealTableId), nameof(UpdateTable), IsUnique = false, Name = "IX_EVT_HISTORY_UPDATE")]
    public class EvtHistoryUpdate : ICreatedBy
    {
        public static string SEQ { get; } = $"SEQ_{nameof(EvtHistoryUpdate).ToSnakeUpperCase()}";

        [Key]
        [ColumnSnackCase(nameof(Id))]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Update bảng nào<br/>
        /// <see cref="EvtHistoryUpdateTables"/>
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
        /// <see cref="EvtFieldName"/>
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
        /// Loại hành động cập nhật (nếu có miêu tả)
        /// <see cref="EvtActionUpdateTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(ActionUpdateType))]
        public int? ActionUpdateType { get; set; }

        /// <summary>
        /// Lý do cập nhật (nếu có miêu tả)
        /// <see cref="EvtUpdateReasons"/>
        /// </summary>
        [ColumnSnackCase(nameof(UpdateReason))]
        public int? UpdateReason { get; set; }

        /// <summary>
        /// Nội dung tổng quan là làm cái gì (vd: khởi tạo lệnh, tạo thanh toán, cập nhật(cập nhật trạng thái))
        /// </summary>
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

        /// <summary>
        /// Loại hình
        /// <see cref="EvtHistoryTypes"/>
        /// </summary>
        [ColumnSnackCase(nameof(Type))]
        public int? Type { get; set; }

        public EvtHistoryUpdate()
        {
        }

        public EvtHistoryUpdate(int realTableId, string oldValue, string newValue, string fieldName, int updateTable, int action, string summary, DateTime date)
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

        public EvtHistoryUpdate(int realTableId, string oldValue, string newValue, string fieldName, int updateTable, int action, string summary, DateTime date, int? type)
        {
            RealTableId = realTableId;
            OldValue = oldValue;
            NewValue = newValue;
            FieldName = fieldName;
            UpdateTable = updateTable;
            Action = action;
            Summary = summary;
            CreatedDate = date;
            Type = type;
        }
    }
}
