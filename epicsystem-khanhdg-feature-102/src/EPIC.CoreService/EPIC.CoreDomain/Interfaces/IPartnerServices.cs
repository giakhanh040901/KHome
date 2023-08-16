using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Partner;
using EPIC.Entities.Dto.TradingProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IPartnerServices
    {
        PagingResult<Partner> FindAll(int pageSize, int pageNumber, string keyword);

        Partner FindById(int id);
        Partner Add(CreatePartnerDto entity);
        int Update(int id, UpdatePartnerDto entity);
        int Delete(int id);
        /// <summary>
        /// Đổ danh sách DLSC theo Đối tác đang đăng nhập
        /// </summary>
        /// <returns></returns>
        List<TradingProviderDto> FindTradingProviderByPartner();
    }
}
