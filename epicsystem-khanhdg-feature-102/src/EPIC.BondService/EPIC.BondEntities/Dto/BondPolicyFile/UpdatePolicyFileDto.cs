using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.PolicyFile
{
    public class UpdatePolicyFileDto
    {
        public  int PolicyFileId { get; set; }

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
