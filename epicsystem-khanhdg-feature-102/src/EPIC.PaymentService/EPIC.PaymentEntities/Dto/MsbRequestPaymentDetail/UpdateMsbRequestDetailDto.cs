using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail
{
    public class UpdateMsbRequestDetailDto : CreateMsbRequestDetailDto
    {
        public long Id { get; set; }
    }
}
