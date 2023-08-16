using EPIC.RealEstateEntities.Dto.RstProducItemDesignDiagramFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstProductItemDesignDiagramFileService
    {
        void Add(CreateRstProductItemDesignDiagramFileDto input);
        void Update(UpdateRstProductItemDesignDiagramFileDto input);
        void Delete(int id);
        IEnumerable<RstProductItemDesignDiagramFileDto> GetAll(int productItemId);
    }
}
