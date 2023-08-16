using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.LoyaltyEntities.Dto.LoyVoucherInvestor;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    public class ViewVoucherDto
    {
        public int VoucherId { get; set; }

        /// <summary>
        /// Tên voucher
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        public string VoucherType { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        public decimal? Value { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// Link
        /// </summary>
        public string LinkVoucher { get; set; }

        /// <summary>
        /// Ảnh đại diện voucher
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Ảnh Banner voucher
        /// </summary>
        public string BannerImageUrl { get; set; }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string DescriptionContent { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? ExpiredDate { get; set; }

        /// <summary>
        /// Ngày nhập lô voucher
        /// </summary>
        public DateTime? BatchEntryDate { get; set; }

        /// <summary>
        /// Ngày tạo voucher
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo voucher
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Mã lô voucher
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Loại voucher
        /// </summary>
        public string  UseType { get; set; }

        /// <summary>
        /// Hiển thị trên app
        /// </summary>
        public string IsShowApp { get; set; }

        /// <summary>
        /// Đặt làm nổi bật
        /// </summary>
        public string IsHot { get; set; }

        /// <summary>
        /// Sử dụng mã cho vòng quay may mắn
        /// </summary>
        public string IsUseInLuckyDraw { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Số lượng phát hành
        /// </summary>
        public int? PublishNum { get; set; }

        /// <summary>
        /// Số lượt đổi tối đa mỗi khách
        /// </summary>
        public int? ExchangeRoundNum { get; set; }

        /// <summary>
        /// Tên hiển thị trên ứng dụng
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Đơn vị của giá trị voucher (P: phần trăm; VND: VND)
        /// <see cref="LoyVoucherValueUnits"/>
        /// </summary>
        public string Unit { get; set; }
    }
}
