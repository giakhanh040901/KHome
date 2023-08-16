using EPIC.RealEstateEntities.DataEntities;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstSignalRBroadcastServices
    {
        Task BroadcastOpenSellDetailChangeStatus(int openSellDetailId);

        /// <summary>
        /// Hợp đồng mới nhất trong bảng hàng của dự án
        /// </summary>
        Task BroadcastUpdateOrderNewInProject(int orderId);

        /// <summary>
        /// Khi căn hộ có thanh toán trong trạng thái giữ chố
        /// </summary>
        /// <param name="productItemId"></param>
        /// <param name="openSellId"></param>
        /// <returns></returns>
        Task BroadcatProductItemHasPaymentOrder(int productItemId, int openSellId);
    }
}
