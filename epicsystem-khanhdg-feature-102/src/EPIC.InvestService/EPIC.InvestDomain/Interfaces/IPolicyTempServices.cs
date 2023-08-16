using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.PolicyTemp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IPolicyTempServices
    {
        ViewPolicyTempDto Add(CreatePolicyTempDto input);
        void AddBondPolicyDetailTemp(PolicyDetailTempDto input);
        int ChangeStatusPolicyDetailTemp(int id);
        int ChangeStatusPolicyTemp(int id);
        int DeletePolicyDetailTemp(int id);
        int DeletePolicyTemp(int id);
        PagingResult<ViewPolicyTempDto> FindAll(FilterPolicyTempDto input);
        ViewPolicyTempDto FindById(int id);
        PolicyDetailTemp FindPolicyDetailTempById(int id);
        PolicyTemp FindPolicyTempById(int id);
        int UpdatePolicyTemp(int id, UpdatePolicyTempDto input);
        int UpdateProductBondPolicyDetailTemp(int id, UpdatePolicyDetailTempDto input);
        PagingResult<ViewPolicyTemp> FindAllPolicyTempNoPermission(string status);
    }
}
