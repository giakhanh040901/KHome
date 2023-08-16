using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.Entities.Dto.Issuer
{
    public class BaseIssuerDto
    {
        [Required(ErrorMessage = "Mã doanh nghiệp không được bỏ trống")]
        public int BusinessCustomerId { get; set; }
        public decimal? BusinessTurnover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? ROA { get; set; }
        public decimal? ROE { get; set; }
        public string Image { get; set; }
    }
}
