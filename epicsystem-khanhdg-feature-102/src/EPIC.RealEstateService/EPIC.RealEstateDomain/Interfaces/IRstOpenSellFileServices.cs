using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOpenSellFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOpenSellFileServices
    {
        List<RstOpenSellFile> Add(CreateRstOpenSellFilesDto input);
        RstOpenSellFile ChangeStatus(int id, string status);
        void Delete(int id);
        PagingResult<RstOpenSellFile> FindAll(FilterRstOpenSellFileDto input);
        RstOpenSellFile FindById(int id);
        RstOpenSellFile Update(UpdateRstOpenSellFileDto input);
    }
}
