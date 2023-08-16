using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.EventEntites.Dto.EvtOrderPayment;
using EPIC.EventEntites.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtOrderPaymentServices
    {
        EvtOrderPaymentDto Add(CreateEvtOrderPaymentDto input);
        Task ApprovePayment(int id, int status);
        void Delete(int id);
        PagingResult<EvtOrderPaymentDto> FindAll(FilterEvtOrderPaymentDto input);
        EvtOrderPaymentDto FindById(int id);
        IEnumerable<BusinessCustomerBankDto> FindListBank(int orderId);
        EvtOrderPaymentDto Update(UpdateEvtOrderPaymentDto input);
    }
}
