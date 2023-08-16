using EPIC.Utils.Attributes;
using EPIC.Utils.ConstantVariables.Loyalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    public class ViewListVoucherDto
    {
        //public int? VoucherInvestorId { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã lô
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Id của voucher
        /// </summary>
        public int VoucherId { get; set; }

        /// <summary>
        /// Loại hình (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }

        /// <summary>
        /// Loại voucher
        /// <see cref="LoyVoucherUseTypes"/>
        /// </summary>
        public string UseType { get; set; }

        /// <summary>
        /// Số lượng phát hành
        /// </summary>
        public int? PublishNum { get; set; }

        /// <summary>
        /// Số lượng cấp phát
        /// </summary>
        public int? DeliveryNum { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Trạng thái của voucher
        /// <see cref="LoyVoucherStatus"/>
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Ngày nhập lô voucher
        /// </summary>
        public DateTime? BatchEntryDate { get; set; }

        /// <summary>
        /// Có hết hạn không
        /// </summary>
        public bool IsExpiredDate { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Ngày tạo voucher
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Hiển thị trên app
        /// </summary>
        public string IsShowApp { get; set; }

        /// <summary>
        /// Tên hiển thị trên ứng dụng
        /// </summary>
        public string DisplayName { get; set; }
    
        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        public string LinkVoucher { get; set; }

        /// <summary>
        /// Số lượt đổi tối đa mỗi khách
        /// </summary>
        public int? ExchangeRoundNum { get; set; }

        /// <summary>
        /// Đặt làm nổi bật
        /// </summary>
        public string IsHot { get; set; }

        /// <summary>
        /// Sử dụng mã cho vòng quay may mắn
        /// </summary>
        public string IsUseInLuckyDraw { get; set; }

        /// <summary>
        /// Đơn vị của giá trị voucher (P: phần trăm; VND: VND)
        /// <see cref="LoyVoucherValueUnits"/>
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// Số lượng cấp phát (Lấy theo trạng thái hoàn thành trong Yêu cầu đổi điểm)
        /// </summary>
        public int ConversionQuantiy { get; set; }
    }
}
