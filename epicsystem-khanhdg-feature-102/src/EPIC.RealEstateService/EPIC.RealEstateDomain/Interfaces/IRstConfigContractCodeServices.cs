using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.Dto.RstConfigContractCode;
using EPIC.RealEstateEntities.Dto.RstConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstConfigContractCodeServices
    {
        void AddConfigContractCode(CreateRstConfigContractCodeDto input);
        void DeleteConfigContractCode(int configContractCodeId);
        PagingResult<RstConfigContractCodeDto> GetAllConfigContractCode(FilterRstConfigContractCodeDto input);
        PagingResult<RstConfigContractCodeDto> GetAllConfig(FilterRstConfigContractCodeDto input);
        List<RstConfigContractCodeDto> GetAllConfigContractCodeStatusActive();
        RstConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId);
        void UpdateConfigContractCode(UpdateRstConfigContractCodeDto input);
        void UpdateConfigContractCodeDetail(List<CreateRstConfigContractCodeDetailDto> input, int configContractCodeId);
        void UpdateConfigContractCodeStatus(int configContractCodeId);
    }
}
