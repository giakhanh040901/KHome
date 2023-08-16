using EPIC.DataAccess.Models;
using EPIC.PaymentEntities.Dto.Msb;
using EPIC.PaymentEntities.Dto.MsbRequestPaymentDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Interfaces
{
    public interface IMsbRequestDetailServices
    {
        void AddRequestDetail(CreateMsbRequestDetailDto input);
        void UpdateRequestDetail(UpdateMsbRequestDetailDto input);
        MsbRequestDetailWithErrorDto FindById(long requestDetailId);
        PagingResult<MsbCollectionPaymentDto> FindAllCollectionPayment(MsbCollectionPaymentFilterDto input);
    }
}
