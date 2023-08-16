using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EPIC.GarnerEntities.Dto.GarnerProductTradingProvider
{
    public class CreateGarnerProductTradingProviderDto
    {
        [Required(ErrorMessage = "Sản phẩm không được bỏ trống")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Đại lý không được bỏ trống")]
        public int TradingProviderId { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES }, ErrorMessage = "Có yêu cầu cài hạn mức không? Y: có, N: không")]
        public string HasTotalInvestmentSub { get; set; }

        [StringRange(AllowableValues = new string[] { YesNo.NO, YesNo.YES }, ErrorMessage = "Có nhận lợi nhuận từ tổ chức phát hành không? Y: có, N: không")]
        public string IsProfitFromPartner { get; set; }
        public long? Quantity { get; set; }
        public decimal? TotalInvestmentSub { get; set; }

        [Required(ErrorMessage = "Ngày phân phối không được bỏ trống")]
        public DateTime DistributionDate { get; set; }

        private string _summary;
        public string Summary
        {
            get => _summary;
            set => _summary = value?.Trim();
        }
    }
}
