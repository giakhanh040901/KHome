using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicyTemp;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstDistributionPolicyTempServices
    {
        /// <summary>
        /// Thêm chính sách phân phối
        /// </summary>
        RstDistributionPolicyTemp Add(CreateRstDistributionPolicyTempDto input);

        /// <summary>
        /// Cập nhật chính sách phân phối
        /// </summary>
        RstDistributionPolicyTemp Update(UpdateRstDistributionPolicyTempDto input);

        /// <summary>
        /// Cập nhật trạng thái chính sách phân phối
        /// </summary>
        RstDistributionPolicyTemp ChangeStatus(int id);

        /// <summary>
        /// Xóa chính sách phân phối
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Lấy danh sách chính sách phân phối
        /// </summary>
        PagingResult<RstDistributionPolicyTempDto> FindAll(FilterRstDistributionPolicyTempDto input);

        /// <summary>
        /// Tìm thông tin chính sách phân phối
        /// </summary>
        RstDistributionPolicyTemp FindById(int id);
    }
}
