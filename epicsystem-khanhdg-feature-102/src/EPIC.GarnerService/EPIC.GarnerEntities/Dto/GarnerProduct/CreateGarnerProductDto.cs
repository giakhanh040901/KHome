using EPIC.Utils.Attributes;
using EPIC.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.Utils.Validation;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;

namespace EPIC.GarnerEntities.Dto.GarnerProduct
{
    public class CreateGarnerProductDto
    {
        #region Chọn loại hình sản phẩm tích lũy
        [Required(ErrorMessage = "Loại hình dự án không được bỏ trống")]
        [IntegerRange(AllowableValues = new int[] { GarnerProductTypes.CO_PHAN, GarnerProductTypes.CO_PHIEU, GarnerProductTypes.TRAI_PHIEU, GarnerProductTypes.BAT_DONG_SAN },
        ErrorMessage = "Vui lòng các loại hình dự án sau: 1: Cổ phần, 2: Cổ phiếu, 3: Trái phiếu, 4: Bất động sản")]
        public int ProductType { get; set; }

        private string _icon;
        public string Icon
        {
            get => _icon;
            set => _icon = value?.Trim();
        }
        #endregion

        #region Thông tin chung
        private string _name;
        [Required(ErrorMessage = "Tên sản phẩm không được bỏ trống")]
        public string Name 
        { 
            get => _name; 
            set => _name = value?.Trim(); 
        }

        private string _code;
        [Required(ErrorMessage = "Mã sản phẩm không được bỏ trống")]
        public string Code 
        { 
            get => _code; 
            set => _code = value?.Trim(); 
        }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? MaxInvestor { get; set; }

        public int? MinInvestDay { get; set; }

        [IntegerRange(AllowableValues = new int[] { GarnerCountTypes.NGAY_PHAT_HANH, GarnerCountTypes.NGAY_THANH_TOAN }, ErrorMessage = "Hình thức tính lã của tổ chức phát hành? 1: Ngày phát hành, 2: Ngày thanh toán")]
        public int CountType { get; set; }

        private string _guaranteeOrganization;
        public string GuaranteeOrganization 
        { 
            get => _guaranteeOrganization; 
            set => _guaranteeOrganization = value?.Trim(); 
        }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES }, ErrorMessage = "Có bảo lãnh thanh toán không? Y: có, N: không")]
        public string IsPaymentGurantee { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES }, ErrorMessage = "Có tài sản đảm bảo không toán không? Y: có, N: không")]
        public string IsCollateral { get; set; }
        #endregion

        #region Tích lũy cổ phần
        public int? CpsIssuerId { get; set; }
        public int? CpsDepositProviderId { get; set; }
        public int? CpsParValue { get; set; }
        public int? CpsPeriod { get; set; }
        public string CpsPeriodUnit { get; set; }
        public decimal? CpsInterestRate { get; set; }
        public int? CpsInterestRateType { get; set; }
        public int? CpsInterestPeriod { get; set; }
        public string CpsInterestPeriodUnit { get; set; }
        public int? CpsNumberClosePer { get; set; }
        public long? CpsQuantity { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES }, ErrorMessage = "Có niêm yết không? Y: có, N: không")]
        public string CpsIsListing { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES }, ErrorMessage = "Có bán trước hạn không? Y: có, N: không")]
        public string CpsIsAllowSBD { get; set; }
        #endregion

        #region Tích lũy Invest
        public int? InvOwnerId { get; set; }
        public int? InvGeneralContractorId { get; set; }
        public decimal? InvTotalInvestmentDisplay { get; set; }
        public decimal? InvTotalInvestment { get; set; }
        public string InvArea { get; set; }
        public string InvLocationDescription { get; set; }
        public string InvLatitude { get; set; }
        public string InvLongitude { get; set; }
        public List<int?> InvProductTypes { get; set; }
        #endregion

        private string _summary;
        /// <summary>
        /// Nội dung sản phẩm để xem lịch sử
        /// </summary>
        [Required(ErrorMessage = "Nội dung lịch sử không được bỏ trống")]
        public string Summary
        {
            get => _summary;
            set => _summary = value?.Trim();
        }
    }
}
