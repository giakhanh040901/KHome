using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtHistoryUpdate
{
    /// <summary>
    /// Lý do tạm dựng hoặc hủy ự kiện
    /// </summary>
    public class CreateHistoryUpdateDto
    {
        /// <summary>
        /// Id sự kiện
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Lý do
        /// </summary>
        public int Reason { get; set; }
        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        private string _summary;
        public string Summary
        {
            get => _summary;
            set => _summary = value?.Trim();
        }
    }
}
