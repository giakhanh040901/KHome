using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestRenewalsRequestServices
    {
        PagingResult<ViewInvRenewalsRequestDto> Find(FilterInvRenewalsRequestDto dto);

        /// <summary>
        /// Tạo yêu cầu thay đổi phương thức tất toán trên Cms
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<int> CreateRenewalsRequestCms(CreateRenewalsRequestDto input);

        /// <summary>
        /// Duyệt yêu cầu
        /// </summary>
        /// <param name="input"></param>
        void ApproveRequest(InvestApproveDto input);

        /// <summary>
        /// Hủy yêu cầu
        /// </summary>
        /// <param name="input"></param>
        void CancelRequest(InvestCancelDto input);

        /// <summary>
        /// Yêu cầu gửi thay đổi phương thức tất toán từ App
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<InvRenewalsRequest> AppRenewalsRequest(AppCreateRenewalsRequestDto input);

        /// <summary>
        /// Check hạn ngày gửi yêu cầu tái tục
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        bool AppRenewalsRequestNotification(long orderId);
        InvestRenewalsRequestInfoDto AppRenewalsRequestInfo(int orderId);
    }
}
