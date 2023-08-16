using EPIC.Entities.Dto.BusinessCustomer;

namespace EPIC.CompanySharesEntities.Dto.Issuer
{
    public class ViewCpsIssuerDto
    {
        public int Id { get; set; }
        public int BusinessCustomerId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? Status { get; set; }
        public decimal? BusinessTurnover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? ROA { get; set; }
        public decimal? ROE { get; set; }
        public string Image { get; set; }
        public BusinessCustomerDto BusinessCustomer { get; set; }
    }
}
