using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtConfigContractCode;
using EPIC.EventEntites.Dto.EvtConfigContractCodeDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtConfigContractCodeServices
    {
        EvtConfigContractCodeDto AddConfigContractCode(CreateEvtConfigContractCodeDto input);
        void DeleteConfigContractCode(int configContractCodeId);
        PagingResult<EvtConfigContractCodeDto> GetAllConfigContractCode(FilterEvtConfigContractCodeDto input);
        List<EvtConfigContractCodeDto> GetAllConfigContractCodeStatusActive();
        EvtConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId);
        void UpdateConfigContractCode(UpdateEvtConfigContractCodeDto input);
        void UpdateConfigContractCodeDetail(List<CreateEvtConfigContractCodeDetailDto> input, int configContractCodeId);
        void UpdateConfigContractCodeStatus(int configContractCodeId);
    }
}
