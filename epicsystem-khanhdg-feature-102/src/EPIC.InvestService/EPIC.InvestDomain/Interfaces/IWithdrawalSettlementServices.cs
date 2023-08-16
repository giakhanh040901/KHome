using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Withdrawal;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IWithdrawalServices
    {
        /// <summary>
        /// Thêm rút vốn
        /// </summary>
        /// <param name="input"></param>
        void WithdrawalAdd(CreateWithdrawalDto input);

        /// <summary>
        /// Lấy danh sách yêu cầu rút vốn
        /// </summary>
        /// <returns></returns>
        PagingResult<WithdrawalDto> FindAll(InvestWithdrawalFilterDto input);
        
        /// <summary>
        /// Phê duyệt rút vốn
        /// </summary>
        /// <returns></returns>
        Task WithdrawalApprove(InvestApproveRequestWithdrawalDto input);

        /// <summary>
        /// Hủy duyệt rút vốn
        /// </summary>
        /// <returns></returns>
        int WithdrawalCancel(long id);

        /// <summary>
        /// Yêu cầu rút vốn từ Nhà đầu tư trên App
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Withdrawal> AppWithdrawalRequest(AppWithdrawalRequestDto input);

        /// <summary>
        /// Gửi thông báo rút vốn/tất toán trước hạn thành công
        /// </summary>
        /// <returns></returns>
        Task ResendNotifyInvestWithdrawalSuccess(int withdrawalId, int withdrawalType);

        /// <summary>
        /// Phê duyệt rút vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task ApproveRequestWithdrawal(InvestApproveRequestWithdrawalDto input);

        /// <summary>
        /// Chuẩn bị rút vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<MsbRequestPaymentWithErrorDto> PrepareApproveRequestWithdrawal(PrepareApproveRequestWithdrawalDto input);
    }
}
