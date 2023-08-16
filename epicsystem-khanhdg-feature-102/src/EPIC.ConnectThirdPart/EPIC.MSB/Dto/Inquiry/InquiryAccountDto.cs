using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.Inquiry
{
    public class InquiryAccountDto
    {
        private string _bankAccount;
        [Required]
        public string BankAccount 
        { 
            get => _bankAccount; 
            set => _bankAccount = value?.Trim(); 
        }

        private string _bin;
        [Required]
        public string Bin
        { 
            get => _bin;
            set => _bin = value?.Trim(); 
        }
    }
}
