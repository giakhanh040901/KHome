using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSellContractTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOpenSellContractTemplateServices
    {
        /// <summary>
        /// Thêm mẫu hợp đồng
        /// </summary>
        RstOpenSellContractTemplate Add(CreateRstOpenSellContractTemplateDto input);

        /// <summary>
        /// Cập nhật
        /// </summary>
        void Update(UpdateRstOpenSellContractTemplateDto input);

        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        void ChangeStatus(int id);
        void Delete(int id);
        PagingResult<RstOpenSellContractTemplateDto> FindAll(FilterRstOpenSellContractTemplateDto input);
        RstOpenSellContractTemplateDto FindById(int id);
    }
}
