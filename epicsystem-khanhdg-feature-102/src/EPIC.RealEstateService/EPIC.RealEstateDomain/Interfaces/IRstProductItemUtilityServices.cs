using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstProductItem;
using EPIC.RealEstateEntities.Dto.RstProductItemProjectUtility;
using EPIC.Utils.ConstantVariables.RealEstate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProductItemUtilityServices
    {
        void DeleteProductItemUtility(int id);
        PagingResult<RstProductItemUtilityDto> FindAll(FilterProductItemProjectUtilityDto input);

        /// <summary>
        /// Cập nhập tiện ích
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<RstProductItemUtilityDto> UpdateProductItemUtility(CreateRstProductItemUtilityDto input);

    }
}
