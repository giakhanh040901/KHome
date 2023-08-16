using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSell
{
    public class CreateRstOpenSellDto
    {
        /// <summary>
        /// Chọn dự án
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// Chọn phân phối
        /// </summary>
        public int DistributionId { get; set; }

        /// <summary>
        /// Ngày phân phối
        /// </summary>
        [Required(ErrorMessage = "Ngày phân phối không được bỏ trống")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc phân phối
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Thời gian giữ thanh toán cọc (giây)
        /// </summary>
        public int? KeepTime { get; set; }

        private string _hotline;
        /// <summary>
        /// Hotline liên hệ
        /// </summary>
        public string Hotline 
        { 
            get => _hotline; 
            set => _hotline = value?.Trim(); 
        }

        /// <summary>
        /// 1: Ngân hàng đại lý, 2: Ngân hàng đại lý đối tác, 3 tất cả
        /// </summary>
        public int FromType { get; set; }

        private string _description;
        /// <summary>
        /// Mô tả về mở bán
        /// </summary>
        public string Description
        {
            get => _description;
            set => _description = value?.Trim();
        }
        /// <summary>
        /// Tài khoản ngân hàng nhận tiền của mở bán
        /// </summary>
        public List<CreateRstOpenSellBankDto> OpenSellBanks { get; set; }
    }
}
