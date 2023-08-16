using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.Utils.ConstantVariables.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtOrderValidService
    {
        /// <summary>
        /// danh sach ve ban hop le
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtOrderValidDto> FindAll(FilterEvtOrderValidDto input);

        /// <summary>
        /// khoa tam so lenh
        /// </summary>
        /// <param name="input"></param>
        void OrderLock(EvtOrderLockDto input);
        /// <summary>
        /// Mở khóa sổ lệnh
        /// </summary>
        /// <param name="id"></param>
        void OrderUnLock(int id);

        /// <summary>
        /// thay doi ma gioi thieu
        /// </summary>
        /// <param name="input"></param>
        void ChangeReferralCode(UpdateReferralCode input);

        /// <summary>
        /// danh sach quan ly tham gia
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtOrderTicketDto> FindAllOrderTicket(FilterEvtOrderTicketDto input);

        /// <summary>
        /// khoa ve cua lenh
        /// </summary>
        /// <param name="input"></param>
        void OrderTickLock(EvtOrderTickLockDto input);

        /// <summary>
        /// mo khoa ve cua lenh, xac nhan tham gia su kien, checkout su kien 
        /// </summary>
        /// <param name="orderTickId"></param>
        void OrderTickChangeStatus(int orderTickId, int status = EvtOrderTicketStatus.CHUA_THAM_GIA);
    }
}
