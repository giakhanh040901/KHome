using EPIC.MSB.Dto.PayMoney;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using System.Collections.Generic;

namespace EPIC.PaymentEntities.Dto.MsbRequestPayment
{
    public class MsbRequestPaymentWithErrorDto
    {
        /// <summary>
        /// Request id
        /// </summary>
        public long Id { get; set; }
        public string PrefixAccount { get; set; }
        public List<MsbRequestDetailWithErrorDto> Details { get; set; }
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Có gửi gửi otp không?
        /// </summary>
        public bool IsSubmitOtp { get; set; }
    }
}
