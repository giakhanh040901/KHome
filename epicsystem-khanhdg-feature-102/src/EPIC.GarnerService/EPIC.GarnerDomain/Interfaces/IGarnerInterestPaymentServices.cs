using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerInterestPayment;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerInterestPaymentServices
    {
        GarnerInterestPaymentSetUpDto FindById(int interestPaymentId);
        List<GarnerInterestPaymentSetUpDto> Add(List<CreateGarnerInterestPaymentDto> input);

        /// <summary>
        /// Lấy danh sách đã lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<GarnerInterestPaymentDto> FindAll(FilterGarnerInterestPaymentDto input);

        /// <summary>
        /// Danh sách lệnh phải chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagingResult<GarnerInterestPaymentByPolicyDto>> FindAllInterestPaymentPay(FilterGarnerInterestPaymentDto input);
        Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestInterestPayment(PrepareApproveRequestInterestPaymentDto input);
        Task ApproveInterestPaymentOrder(InvestEntities.Dto.InterestPayment.ApproveInterestPaymentRenewalsOrderDto input);
    }
}
