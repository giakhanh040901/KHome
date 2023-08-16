using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstHistoryUpdate
{
    /// <summary>
    /// Cập nhật lý do khóa căn hộ
    /// </summary>
    public class RstUpdateStatusLockDtoBase
    {
        /// <summary>
        /// Lý do khóa căn
        /// </summary>
        public int? LockingReason { get; set; }

        private string _summary;
        [StringLength(256, ErrorMessage = "Nội dung khoá căn không được dài hơn 256 ký tự")]
        public string Summary
        {
            get => _summary;
            set => _summary = value?.Trim();
        }
    }
}
