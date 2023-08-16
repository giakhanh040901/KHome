using EPIC.EventEntites.Dto.EvtConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtConfigContractCode
{
    public class EvtConfigContractCodeDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }/// <summary>
        /// ID đại lý
        /// </summary>
        public int TradingProviderId { get; set; }
        /// <summary>
        /// Tên cấu trúc mã
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người hủy
        /// </summary>
        public string CancelBy { get; set; }
        /// <summary>
        /// Ngày hủy
        /// </summary>
        public DateTime? CancelDate { get; set; }
        /// <summary>
        /// Danh sách chit tiết cấu trúc mã
        /// </summary>
        public List<EvtConfigContractCodeDetailDto> ConfigContractCodeDetails { get; set; }
    }
}
