using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.Pvcb
{
    public class CallbackDataDto
    {
        public string FtType { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }
        public string SenderBankId { get; set; }
        public string Description { get; set; }
        public string TranId { get; set; }
        public DateTime? TranDate { get; set; }
        public string Currency { get; set; }
        public string TranStatus { get; set; }
        public decimal? ConAmount { get; set; }
        public string NumberOfBeneficiary { get; set; }
        public string Account { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RequestIP { get; set; }
        public decimal Status { get; set; }
    }
}
