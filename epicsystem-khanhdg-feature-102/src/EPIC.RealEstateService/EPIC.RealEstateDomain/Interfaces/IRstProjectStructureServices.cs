using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstProjectStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProjectStructureServices
    {
        RstProjectStructure Add(CreateRstProjectStructureDto input);
        void Delete(int id);
        PagingResult<RstProjectStructure> FindAll(FilterRstProjectStructureDto input);
        RstProjectStructure FindById(int id);
        RstProjectStructure Update(UpdateRstProjectStructureDto input);
        ViewRstProjectStructureDto FindByProjectId(int projectId);
        /// <summary>
        /// Lấy danh sách cấu trúc là con trong các node
        /// </summary>
        /// <returns></returns>
        List<RstProjectStructureChildDto> FindAllChild(int projectId);
        /// <summary>
        /// Lấy danh sách cấu trúc theo mức mật độ
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        List<RstProjectStructureChildDto> FindAllByLevel(int level, int projectId);
    }
}
