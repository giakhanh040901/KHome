using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy;
using EPIC.RealEstateEntities.Dto.RstSellingPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstSellingPolicyServices
    {
        #region Chính sách mở bán
        void AddSellingPolicy(CreateRstSellingPolicyDto input);
        PagingResult<RstSellingPolicyDto> FindAllSellingPolicy(FilterRstSellingPolicyDto input);
        void ChangeStatusSellingPolicy(int id);
        #endregion
    }
}
