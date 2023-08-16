using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.GuaranteeAsset;
using EPIC.Entities.Dto.GuaranteeFile;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondGuaranteeAssetService
    {
        GuaranteeAssetDto FindById(int id);
        void Add(CreateGuaranteeAssetDto input);
        void Update(UpdateGuaranteeAssetDto input);
        int Delete(int id);

        PagingResult<GuaranteeAssetDto> FindAll(int productBondId, int pageSize, int pageNumber, string keyword);
        GuaranteeFileDto FindByIdFile(int id);
        int UpdateFile(int id, UpdateGuaranteeFileDto input);
        int DeleteFile(int id);
    }
}
