using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstOrderSellingPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOrderSellingPolicyServices
    {
        /// <summary>
        /// Tìm kiếm danh sách chính sách của sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<RstOrderSellingPolicyDto> FindAll(FilterRstOrderSellingPolicyDto input);
        /// <summary>
        /// Thêm chính sách của đối tác hoặc đại lý vào sổ lệnh
        /// </summary>
        /// <param name="input"></param>
        void Add(CreateRstOrderSellingPolicyDto input);
    }
}
