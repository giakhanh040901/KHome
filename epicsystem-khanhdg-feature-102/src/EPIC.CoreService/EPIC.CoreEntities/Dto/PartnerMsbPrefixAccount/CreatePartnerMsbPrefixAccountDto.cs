using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount
{
    public class CreatePartnerMsbPrefixAccountDto
    {
        public int PartnerBankAccountId { get; set; }

        private string _prefixMsb;
        [Required(ErrorMessage = "Mã tiền tố MSB không dược bỏ trống")]
        public string PrefixMsb
        {
            get => _prefixMsb;
            set => _prefixMsb = value?.Trim();
        }

        private string _mId;
        [Required(ErrorMessage = "MId không dược bỏ trống")]
        public string MId
        {
            get => _mId;
            set => _mId = value?.Trim();
        }

        private string _tId;
        [Required(ErrorMessage = "TId không dược bỏ trống")]
        public string TId
        {
            get => _tId;
            set => _tId = value?.Trim();
        }

        private string _accessCode;
        [Required(ErrorMessage = "AccessCode không dược bỏ trống")]
        public string AccessCode
        {
            get => _accessCode;
            set => _accessCode = value?.Trim();
        }
    }
}
