using EPIC.Entities.Dto.TradingProvider;
using EPIC.RealEstateEntities.Dto.RstOpenSellBank;
using EPIC.RealEstateEntities.Dto.RstProject;
using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOpenSell
{
    public class RstOpenSellDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Ngày phân phối
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc phân phối
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Tổng số lượng sản phẩm 
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Số lượng đã cọc
        /// </summary>
        public int QuantityDeposit { get; set; }

        /// <summary>
        /// Số lượng đã bán
        /// </summary>
        public int QuantitySold { get; set; }

        /// <summary>
        /// Số lượng giữ chỗ
        /// </summary>
        public int QuantityHold { get; set; }

        /// <summary>
        /// Mô tả về mở bán
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Thời gian giữ chỗ
        /// </summary>
        public int? KeepTime { get; set; }

        public string Hotline { get; set; }

        public string IsOutstanding { get; set; }

        /// <summary>
        /// Bật tắt show App (Y/N)
        /// </summary>
        public string IsShowApp { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 1: Ngân hàng đại lý, 2: Ngân hàng đại lý đối tác, 3 tất cả
        /// </summary>
        public int FromType { get; set; }

        /// <summary>
        /// Chức năng đăng ký làm cộng tác viên bán hàng.
        /// Khi bật lên thì App sẽ hiện chức năng đăng ký làm CTV bán hàng
        /// Nếu bật mà nhà đầu tư đã là sale của đại lý thì cũng ko hiện trên App
        /// </summary>
        public bool IsRegisterSale { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int ProjectId { get; set; }

        /// <summary>
        /// Dự án
        /// </summary>
        public RstProjectDto Project { get; set; }

        /// <summary>
        /// Thông tin ngân hàng nhận tiền của mở bán
        /// </summary>
        public List<RstOpenSellBankDto> OpenSellBanks { get; set; }
    }
}
