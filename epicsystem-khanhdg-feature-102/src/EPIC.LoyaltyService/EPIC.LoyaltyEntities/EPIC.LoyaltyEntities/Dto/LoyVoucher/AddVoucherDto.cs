using EPIC.Utils.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.Validation;
using EPIC.Utils.ConstantVariables.Loyalty;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils;

namespace EPIC.LoyaltyEntities.Dto.LoyVoucher
{
    public class AddVoucherDto
    {
        private string _code;
        private string _name;
        private string _displayName;
        private string _avatar;
        private string _descriptionContent;
        private string _linkVoucher;
        private string _bannerImageUrl;

        /// <summary>
        /// Mã lô voucher
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã lô voucher không được bỏ trống")]
        public string Code { get => _code; set => _code = value?.Trim(); }

        /// <summary>
        /// Tên voucher
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên voucher không được bỏ trống")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        /// <summary>
        /// Tên hiển thị
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên hiển thị không được bỏ trống")]
        public string DisplayName { get => _displayName; set => _displayName = value?.Trim(); }

        /// <summary>
        /// Loại voucher (C: Cứng; DT: Điện tử)
        /// <see cref="LoyVoucherTypes"/>
        /// </summary>
        [StringRange(AllowableValues = new string[] { LoyVoucherTypes.DIEN_TU, LoyVoucherTypes.CUNG })]
        public string VoucherType { get; set; }

        /// <summary>
        /// Link voucher
        /// </summary>
        public string LinkVoucher { get => _linkVoucher; set => _linkVoucher = value?.Trim(); }

        /// <summary>
        /// Loại voucher (TD: Tiêu dùng; MS: Mua sắm; AT: Ẩm thực; DV: Dịch vụ)
        /// <see cref="LoyVoucherUseTypes"/>
        /// </summary>
        [StringRange(AllowableValues = new string[] { LoyVoucherUseTypes.TIEU_DUNG, LoyVoucherUseTypes.MUA_SAM, LoyVoucherUseTypes.AM_THUC, LoyVoucherUseTypes.DICH_VU})]
        public string UseType { get; set; }

        /// <summary>
        /// Đơn vị của giá trị voucher (P: phần trăm; VND: VND)
        /// <see cref="LoyVoucherValueUnits"/>
        /// </summary>
        [StringRange(AllowableValues = new string[] {LoyVoucherValueUnits.PERCENT, LoyVoucherValueUnits.VND})]
        public string Unit { get; set; }

        /// <summary>
        /// Giá trị voucher
        /// </summary>
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Giá trị phải lớn hơn 0")]
        [Required(ErrorMessage = "Giá trị ko được bỏ trống")]
        public decimal? Value { get; set; }

        /// <summary>
        /// Ngày áp dụng
        /// </summary>
        [Required(ErrorMessage = "Ngày áp dụng ko được bỏ trống")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        [Required(ErrorMessage = "Ngày hết hạn ko được bỏ trống")]
        public DateTime ExpiredDate { get; set; }

        /// <summary>
        /// Ngày nhập lô voucher
        /// </summary>
        [Required(ErrorMessage = "Ngày nhập lô voucher không được bỏ trống")]
        public DateTime BatchEntryDate { get; set; }

        /// <summary>
        /// Điểm quy đổi
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Điểm quy đổi phải lớn hơn 0")]
        [Required(ErrorMessage = "Điểm quy đổi ko được bỏ trống")]
        public int Point { get; set; }

        /// <summary>
        /// Ảnh đại diện voucher
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ảnh minh họa không được để trống")]
        public string Avatar { get => _avatar; set => _avatar = value?.Trim(); }

        /// <summary>
        /// Ảnh Banner voucher
        /// </summary>
        public string BannerImageUrl { get => _bannerImageUrl; set => _bannerImageUrl = value?.Trim(); }

        /// <summary>
        /// Loại nội dung: MARKDOWN, HTML
        /// </summary>
        [StringRange(AllowableValues = new string[] { null, "", ContentTypes.HTML, ContentTypes.MARKDOWN })]
        public string DescriptionContentType { get; set; }

        /// <summary>
        /// Nội dung mô tả
        /// </summary>
        public string DescriptionContent { get => _descriptionContent; set => _descriptionContent = value?.Trim(); }

        /// <summary>
        /// Số lượng phát hành
        /// </summary>
        [Required(ErrorMessage = "Số lượng phát hành ko được bỏ trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phát hành phải lớn hơn 0")]
        public int? PublishNum { get; set; }

        /// <summary>
        /// Số lượt đổi tối đa mỗi khách
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Số lượt đổi tối đa mỗi khách phải lớn hơn 0")]
        public int? ExchangeRoundNum { get; set; }

        /// <summary>
        /// Hiển thị trên app
        /// </summary>
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsShowApp { get; set; }

        /// <summary>
        /// Đặt làm nổi bật
        /// </summary>
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsHot { get; set; }

        /// <summary>
        /// Sử dụng mã cho vòng quay may mắn
        /// </summary>
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO })]
        public string IsUseInLuckyDraw { get; set; }
    }
}
