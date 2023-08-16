using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerWithdrawalServices
    {
        /// <summary>
        /// Duyệt lệnh chi trả
        /// </summary>
        Task ApproveRequestWithdrawal(GarnerApproveRequestWithdrawalDto input);
        /// <summary>
        /// Xem rút vốn tích lũy
        /// </summary>
        /// <param name="withdrawalId"></param>
        /// <returns></returns>
        GarnerWithdrawalByPolicyDto GetOrderWithdrawalById(long withdrawalId);
        /// <summary>
        /// Investor tạo yêu cầu rút
        /// </summary>
        Task<CalculateGarnerWithdrawalDto> AppRequestWithdrawal(AppWithdrawalRequestDto input);
        /// <summary>
        /// Tạo yêu cầu rút theo cifcode
        /// </summary>
        Task<CalculateGarnerWithdrawalDto> RequestWithdrawal(string cifCode, int policyId, decimal amount, DateTime withdrawDate, int source, int bankAccountId, bool isApp = false);
        /// <summary>
        /// Xem thay đổi trước khi yêu cầu rút vốn
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="amountMoney"></param>
        /// <returns></returns>
        ViewCalculateGarnerWithdrawalDto ViewChangeRequestWithdrawal(int policyId, decimal amountMoney); 

        PagingResult<GarnerWithdrawalDto> FindAll(FilterGarnerWithdrawalDto input);

        /// <summary>
        /// Chuẩn bị chi cho yêu cầu rút vốn, gọi sang bên bank để thực hiện validate và gửi otp
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestWithdrawal(PrepareApproveRequestWithdrawalDto input, int? tradingProviderId = null, bool isApp = false);

        /// <summary>
        /// Duyệt trạng thái ngân hàng sang thành công khi duyệt chi tiền
        /// </summary>
        /// <param name="ids"></param>
        void ApproveChangeStatusBank(List<long> ids);
    }
}
