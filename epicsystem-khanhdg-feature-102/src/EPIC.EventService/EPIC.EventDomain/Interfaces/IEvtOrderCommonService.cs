using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtOrder;
using EPIC.Utils.ConstantVariables.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtOrderCommonService
    {
        /// <summary>
        /// gen mảng ticket code
        /// </summary>
        /// <param name="number"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        List<string> ListTicketCodeGenerate(int number, int length);
        /// <summary>
        /// tạo contract code
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string ContractCode(int orderId);

        /// <summary>
        /// sinh contractCodeGen
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string GenContractCode(OrderContractCodeDto input);

        /// <summary>
        /// kiểm tra số vé còn lại lúc đặt lệnh
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        bool CheckRemainingTickets(int ticketId);
        /// <summary>
        /// kiểm tra số vé còn lại lúc đặt lệnh trên app
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        bool AppCheckRemainingTickets(int ticketId);
    }
}
