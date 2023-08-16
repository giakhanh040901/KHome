using System;
using System.ComponentModel.DataAnnotations;

namespace EPIC.CompanySharesEntities.Dto.PolicyFile
{
    public class CreateCpsPolicyFileDto
    {
        public int SecondaryId { get; set; }
        public int TradingProviderId { get; set; }

        private string _name;
        [StringLength(512, ErrorMessage = "Tên file không được dài hơn 512 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => _url = value?.Trim();
        }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
