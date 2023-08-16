using EPIC.DataAccess.Models;
using EPIC.MSB.Dto.PayMoney;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Interfaces
{
    public interface IMsbRequestPaymentServices
    {
        void UpdateRequest(UpdateMsbRequestPaymentDto input);
        MsbRequestPaymentDto FindById(long requestId);
        PagingResult<ViewMsbRequestPaymentDto> FindAll(FilterMsbRequestDetailDto input);
        /// <summary>
        /// Gửi lại Otp
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="tradingBankAccountId"></param>
        /// <returns></returns>
        Task SendOtp(long requestId, int tradingBankAccountId);
        /// <summary>
        /// truy vấn lô theo id lô
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        Task<List<ResponseInquiryBatchDetailDto>> InquiryBatch(long requestId);

        /// <summary>
        /// Hủy yêu cầu chi tiền 
        /// </summary>
        void CancelRequestPayment(long requestPaymentDetailId);
    }
}
