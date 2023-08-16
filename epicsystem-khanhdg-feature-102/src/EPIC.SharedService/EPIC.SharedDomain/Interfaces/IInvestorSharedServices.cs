using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.SharedDomain.Interfaces
{
    public interface IInvestorSharedServices
    {
        /// <summary>
        /// Đổi trạng thái giao nhận hợp đồng từ đang giao thành nhận hợp đồng
        /// </summary>
        Task ChangeDeliveryStatusRecevired(string deliveryCode);
    }
}
