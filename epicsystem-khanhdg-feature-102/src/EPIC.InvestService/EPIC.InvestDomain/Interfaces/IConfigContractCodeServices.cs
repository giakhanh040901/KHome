using EPIC.DataAccess.Models;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IConfigContractCodeServices
    {
        #region Config Contract Code
        InvConfigContractCodeDto AddConfigContractCode(CreateConfigContractCodeDto input);
        /// <summary>
        /// Cập nhật config contract code
        /// </summary>
        /// <param name="input"></param>
        void UpdateConfigContractCode(UpdateConfigContractCodeDto input);

        PagingResult<InvConfigContractCodeDto> GetAllConfigContractCode(FilterInvConfigContractCodeDto input);
        List<InvConfigContractCodeDto> GetAllConfigContractCodeStatusActive();
        InvConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId);
        void UpdateConfigContractCodeStatus(int configContractCodeId);
        void DeleteConfigContractCode(int configContractCodeId);
        #endregion
    }
}
