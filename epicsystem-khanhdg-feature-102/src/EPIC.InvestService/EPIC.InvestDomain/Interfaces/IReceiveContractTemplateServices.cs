using EPIC.DataAccess.Models;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.ReceiveContractTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IReceiveContractTemplateServices
    { 
        ReceiveContractTemplate FindById(int id);
        ReceiveContractTemplate FindByDistributionId(int id);
        ReceiveContractTemplate Add(CreateReceiveContractTemplateDto input);
        int Update(UpdateReceiveContractTemplateDto input);
        int Delete(int id);
        int ChangeStatus(int id);

        /// <summary>
        /// Tìm danh sách hợp đồng có phân trang
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="keyword"></param>
        /// <param name="distributionId"></param>
        /// <returns></returns>
        PagingResult<ReceiveContractTemplate> GetAll(int pageSize, int pageNumber, string keyword, int distributionId);
    }
}
