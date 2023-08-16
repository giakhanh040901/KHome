using System.ComponentModel.DataAnnotations;

namespace EPIC.CompanySharesEntities.Dto.Issuer
{
    public class BaseCpsIssuerDto
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
