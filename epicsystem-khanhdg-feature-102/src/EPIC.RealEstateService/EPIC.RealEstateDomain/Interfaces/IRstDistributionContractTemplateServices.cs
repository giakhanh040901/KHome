using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstDistributionContractTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstDistributionContractTemplateServices
    {
        /// <summary>
        /// Thêm biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RstDistributionContractTemplateDto Add(CreateRstDistributionContractTemplateDto input);
        /// <summary>
        /// Cập nhật biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RstDistributionContractTemplateDto Update(UpdateRstDistributionContractTemplateDto input);
        /// <summary>
        /// Xóa biểu mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Tìm biểu mẫu hợp đồng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RstDistributionContractTemplateDto FindById(int id);
        /// <summary>
        /// Tìm kiếm danh sách biểu mẫu hợp đồng
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<RstDistributionContractTemplateDto> FindAll(FilterRstDistributionContractTemplateDto input);
        void ChangeStatus(int id);
    }
}
