using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.OrderPayment;
using EPIC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface ICpsOrderPaymentServices
    {
        #region thanh toán
        CpsOrderPayment AddPayment(CreateOrderPaymentDto input);
        int ApprovePayment(int orderPaymentId, int status);
        int DeleteOrderPayment(int id);
        PagingResult<CpsOrderPayment> FindAll(int orderId, int pageSize, int pageNumber, string keyword, int? status);
        OrderPaymentDto FindPaymentById(int id);
        #endregion
    }
}
