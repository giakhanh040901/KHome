using EPIC.DataAccess.Models;
using EPIC.LoyaltyEntities.Dto.LoyPointInvestor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyPointInvestorServices
    {
        /// <summary>
        /// Lịch sử quy đổi điểm của nhà đầu tư/ Tab Danh sách ưu đãi
        /// </summary>
        PagingResult<PointInvestorConversionHistoryDto> FindAllVoucherConversionHistory(FilterPointInvestorConversionHistoryDto dto);

        /// <summary>
        /// Tìm kiếm nhà đầu tư theo số điện thoại và điểm ranh của theo đại lý
        /// </summary>
        List<FindLoyPointInvestorDto> FindInvestorByPhone(string phone, int? investorId);

        /// <summary>
        /// Xem chi tiết Tab Danh sách ưu đãi
        /// </summary>
        PointInvestorConversionHistoryInfoDto VoucherConversionHistoryGetById(int conversionPointDetailId);
    }
}
