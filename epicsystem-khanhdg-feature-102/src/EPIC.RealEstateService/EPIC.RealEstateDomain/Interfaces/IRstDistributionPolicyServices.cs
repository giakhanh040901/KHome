using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstDistributionPolicy;
using EPIC.RealEstateEntities.Dto.RstProjectPolicy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstDistributionPolicyServices
    {
        /// <summary>
        /// Thêm chính sách phân phối
        /// </summary>
        RstDistributionPolicy Add(CreateRstDistributionPolicyDto input);

        /// <summary>
        /// Cập nhật chính sách phân phối
        /// </summary>
        RstDistributionPolicy Update(UpdateRstDistributionPolicyDto input);

        /// <summary>
        /// Cập nhật trạng thái chính sách phân phối
        /// </summary>
        RstDistributionPolicy ChangeStatus(int id);

        /// <summary>
        /// Xóa chính sách phân phối
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Lấy danh sách chính sách phân phối
        /// </summary>
        PagingResult<RstDistributionPolicyDto> FindAll(FilterRstDistributionPolicyDto input, int distributionId);

        /// <summary>
        /// Tìm thông tin chính sách phân phối
        /// </summary>
        RstDistributionPolicy FindById(int id);

        /// <summary>
        /// đổi trạng thái kích hoạt cho sản phẩm phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <param name="DistributionId"></param>
        void ActiveDistributionPolicy(int id, int DistributionId);

        /// <summary>
        /// Lấy danh sách chính sách theo mở bán
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<RstDistributionPolicyDto> GetAllPolicy(FilterDistrobutionPolicyDto input);

    }
}
