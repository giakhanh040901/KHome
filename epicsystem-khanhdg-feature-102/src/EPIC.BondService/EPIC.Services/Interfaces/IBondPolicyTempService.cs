using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ProductBondPolicyTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondPolicyTempService
    {
        PagingResult<ViewProductBondPolicyTempDto> FindAll(int pageSize, int pageNumber, bool isNoPaging, string keyword, string status, decimal? classify);
        ViewProductBondPolicyTempDto FindById(int id);
        BondPolicyTemp FindProductBondPolicyTempById(int id);
        BondPolicyDetailTemp FindProductBondPolicyDetailTempById(int id);
        void Add(CreateProductBondPolicyTempDto input);
        void AddBondPolicyDetailTemp(ProductBondPolicyDetailTempDto input);
        int UpdateProductBondPolicyTemp(int id, UpdateProductBondPolicyTempDto input);
        int UpdateProductBondPolicyDetailTemp(int id, UpdateProductBondPolicyDetailTempDto input);
        int DeleteProductBondPolicyTemp(int id);
        int DeleteProductBondPolicyDetailTemp(int id);
        int ChangeStatusProductBondPolicyTemp(int id);
        int ChangeStatusProductBondPolicyDetailTemp(int id);
        
    }
}
