using EPIC.RealEstateEntities.Dto.RstProductItem;
using System.Collections.Generic;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstCountServices
    {
        /// <summary>
        /// Đếm số lượng từng loại của sản phẩm trong dự án
        /// </summary>
        RstCountProductItemSignalRDtoBase CountProductItemByPartner(int projectId);

        /// <summary>
        /// Đếm số lượng từng loại của sản phẩm của mở bán trong dự án theo đại lý
        /// </summary>
        AppCountProductItemSignalRDto AppCountProductItemByTrading(int openSellId);
        int CountStatusProductItemForTrading(int openSellDetailId, int status);
        int CountStatusProductItemForPartner(int projectId, int listStatus);
    }
}
