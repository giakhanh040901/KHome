using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.Utils.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerDistribution
{
    public class AppGarnerDistributionDto
    {
        /// <summary>
        /// Id phân phối sản phẩm
        /// </summary>
        public int DistributionId { get; set; }

        public int TradingProviderId { get; set; }

        /// <summary>
        /// Loại hình tích lũy
        /// </summary>
        public int ProductType { get; set; }

        /// <summary>
        /// Loại hình tích lũy
        /// </summary>
        public string ProductTypeName { get; set; }

        /// <summary>
        /// Id chính sách
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Tên đại lý
        /// </summary>
        public string TradingProviderName { get; set; }

        /// <summary>
        /// % loi tuc
        /// </summary>
        public decimal? Profit { get; set; }

        /// <summary>
        /// Tên kiểu trả lợi tức
        /// </summary>
        public string InterestTypeName { get; set; }

        /// <summary>
        /// Ảnh logo của dự án
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Có là mặc định
        /// </summary>
        public string IsDefault { get; set; }

        /// <summary>
        /// Ảnh của phân phối dự án
        /// </summary>
        public string Image { get; set; }
        #region Thông tin chính sách
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Số tiền tích lũy tối thiểu
        /// </summary>
        public decimal MinMoney { get; set; }

        /// <summary>
        /// So ngay tich luy toi thieu
        /// </summary>
        public int MinInvestDay { get; set; }

        /// <summary>
        /// Thue loi nhuan
        /// </summary>
        public decimal IncomeTax { get; set; }

        /// <summary>
        /// Loai nha dau tu (P: chuyen nghiep, A: tat ca)
        /// </summary>
        public string InvestorType { get; set; }

        /// <summary>
        /// Phan loai chinh sach san pham (1: hop tac, 2: mua ban), mac dinh la hop tac
        /// </summary>
        public int Classify { get; set; }

        /// <summary>
        /// Loai hinh tinh loi tuc (1: Net, 2: Gross)
        /// </summary>
        public int CalculateType { get; set; }

        /// <summary>
        /// Loai hinh ky han (1: khong chon ky han (tich luy theo thoi gian), 2: chon ky han (ky han), 3, 4...
        /// </summary>
        public int GarnerType { get; set; }

        /// <summary>
        /// (Không chọn kỳ hạn) Kiểu trả lợi tức lấy trong InterestType const
        /// </summary>
        public int? InterestType { get; set; }

        /// <summary>
        /// (GarnerType = khong chon ky han va InterestType = dinh ky) Số kỳ trả lợi nhuận
        /// </summary>
        public int? InterestPeriodQuantity { get; set; }

        /// <summary>
        /// (GarnerType = khong chon ky han va InterestType = dinh ky) Don vi ky tra loi nhuan (Y, M, D)
        /// </summary>
        public string InterestPeriodType { get; set; }

        /// <summary>
        /// (GarnerType = khong chon ky han va InterestType = ngay co dinh) Ngày trả cố định
        /// </summary>
        public int? RepeatFixedDate { get; set; }

        /// <summary>
        /// So tien rút tối thiểu
        /// </summary>
        public decimal MinWithdraw { get; set; }

        /// <summary>
        /// Phí rút tối thiểu (theo số tiền hoặc theo năm
        /// </summary>
        public decimal WithdrawFee { get; set; }

        /// <summary>
        /// Loai phi rut von (1: so tien, 2: theo nam)
        /// </summary>
        public int WithdrawFeeType { get; set; }

        /// <summary>
        /// Loai hinh linh hoat: Thu tu rut tien (1: moi nhat den cu nhat, 2: cu nhat den moi nhat, 3: gia tri gan nhat gia tri rut (uu tien gia tri HD nao to hon gia tri rut)))
        /// </summary>
        public int OrderOfWithdrawal { get; set; }

        /// <summary>
        /// % phí chuyển đổi tài sản
        /// </summary>
        public decimal TransferAssetsFee { get; set; }
        #endregion

        /// <summary>
        /// Danh sách kỳ hạn
        /// </summary>
        public List<AppGarnerPolicyDetailDto> PolicyDetail { get; set; }
    }
}
