using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.JuridicalFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondJuridicalFileService
    {
        BondJuridicalFile Add(CreateJuridicalFileDto input);
        BondJuridicalFile FindJuridicalFileById(int id);
        PagingResult<BondJuridicalFile> FindAllJuridicalFile(int productBondId, int pageSize, int pageNumber, string keyword);
        int JuridicalFileUpdate(int id, UpdateJuridicalFileDto entity);
        
        int DeleteJuridicalFile(int id);
    }
}
