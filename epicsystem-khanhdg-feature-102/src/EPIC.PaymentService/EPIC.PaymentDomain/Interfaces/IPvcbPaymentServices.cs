using EPIC.DataAccess.Models;
using EPIC.PaymentEntities.Dto.Pvcb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.PaymentDomain.Interfaces
{
    public interface IPvcbPaymentServices
    {
        bool VerifyToken(string data, string signature);
        PvcbCallbackDto PvcbCallbackAdd(string data, string token);
        PagingResult<CallbackDataDto> FindAll(int pageSize, int pageNumber, string keyword, string status);
        string PvcbCallbackEncode(CreatePvcbCallbackDto input);
    }
}
