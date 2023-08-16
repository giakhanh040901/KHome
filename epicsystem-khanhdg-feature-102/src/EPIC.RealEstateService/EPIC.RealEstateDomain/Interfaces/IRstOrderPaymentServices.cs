using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOrderPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOrderPaymentServices
    {
        RstOrderPayment Add(CreateRstOrderPaymentDto input);
        Task ApproveOrCancel(int orderPaymentId, int status);
        void Delete(int id);
        PagingResult<RstOrderPaymentDto> FindAll(FilterRstOrderPaymentDto input);
        RstOrderPaymentDto FindById(int id);
        RstOrderPayment Update(UpdateRstOrderPaymentDto input);
    }
}
