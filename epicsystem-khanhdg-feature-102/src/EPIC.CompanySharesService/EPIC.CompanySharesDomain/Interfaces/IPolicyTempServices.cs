using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.PolicyTemp;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ProductBondPolicyTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface IPolicyTempServices
    {
        PagingResult<ViewPolicyTempDto> FindAll(int pageSize, int pageNumber, bool isNoPaging, string keyword, string status, decimal? classify);
        ViewPolicyTempDto FindById(int id);
        PolicyTemp FindPolicyTempById(int id);
        PolicyDetailTemp FindPolicyDetailTempById(int id);
        void Add(CreatePolicyTempDto input);
        void AddPolicyDetailTemp(PolicyDetailTempDto input);
        int UpdatePolicyTemp(int id, UpdatePolicyTempDto input);
        int UpdatePolicyDetailTemp(int id, UpdatePolicyDetailTempDto input);
        int DeletePolicyTemp(int id);
        int DeletePolicyDetailTemp(int id);
        int ChangeStatusPolicyTemp(int id);
        int ChangeStatusPolicyDetailTemp(int id);

    }
}
