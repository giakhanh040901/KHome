using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInterestPaymentServices
    {
        List<InvestInterestPaymentDto> InterestPaymentAdd(InterestPaymentCreateListDto input);
        PagingResult<DanhSachChiTraDto> FindAll(InterestPaymentFilterDto input);
        InvestInterestPayment FindById(int id);
        Task ApproveInterestPayment(List<int> ids);

        /// <summary>
        /// Chi trả cuối kỳ (có tái tục)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ApprovePaymentLastPeriod(List<int> ids);
        void CancelRenewalRequest(List<int> interestPaymentId);

        /// <summary>
        /// Gửi lại thông báo chi trả thành công
        /// </summary>
        /// <returns></returns>
        Task ResendNotifyInvestInterestPaymentSuccess(int interestPaymentId);

        /// <summary>
        /// Phê duyệt chi trả của hợp đồng sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isRenewals"></param>
        /// <returns></returns>
        Task ApproveInterestPaymentOrder(ApproveInterestPaymentRenewalsOrderDto input, bool isRenewals);
        /// Gửi lại thông báo TÁI TỤC thành công
        /// </summary>
        /// <param name="interestPaymentId"></param>
        /// <returns></returns>
        Task ResendNotifyInvestRenewalsSuccess(int interestPaymentId);
        Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestInterestPayment(PrepareApproveRequestInterestPaymentDto input);

        /// <summary>
        /// Duyệt chi trả có tái tục vốn
        /// </summary>
        Task RenewalInterestPayment(List<InvestInterestPayment> interestPayments, int tradingProviderId, int? tradingBankAccId, int interestPaymentStatus, string username, int? approveNote = null, int? statusBank = null);

        /// <summary>
        /// Duyệt chi trả từng kỳ/ cuối kỳ không tái tục
        /// </summary>
        void InterestPayment(List<InvestInterestPayment> interestPayments, int tradingProviderId, int? tradingBankAccId, int interestPaymentStatus, string username, int? approveNote = null, int? statusBank = null);
    }
}
