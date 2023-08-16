using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContractPayment
{
    public class DistributionContractPaymentDto : CreateDistributionContractPaymentDto
    {
        public decimal PaymentId { get; set; }

        public string ApproveBy { get; set; }

        public DateTime? ApproveDate { get; set; }

        public string CancelBy { get; set; }

        public DateTime? CancelDate { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string TotalValueContract { get; set; }
    }
}
