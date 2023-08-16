using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.ProductBondPrimary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondPrimaryService
    {
        PagingResult<BondPrimary> FindAll(int pageSize, int pageNumber, string keyword);
        PagingResult<ProductBondPrimaryDto> FindAllProductBondPrimary(int pageSize, int pageNumber, string keyword, string status);
        BondPrimary Add(CreateProductBondPrimaryDto input);
        int Update(int id, UpdateProductBondPrimaryDto input);
        int Delete(int id);
        int DuyetBondPrimary(int bondPrimaryId, string status);
        ProductBondPrimaryDto FindById(int id);
        IEnumerable<ProductBondPrimaryDto> GetAllByTradingProvider(int tradingproviderId);
        IEnumerable<ProductBondPrimaryDto> GetAllByCurrentTradingProvider();
        void Request(RequestStatusDto input);
        void Approve(ApproveStatusDto input);
        void Check(CheckStatusDto input);
        void Cancel(CancelStatusDto input);
    }
}
