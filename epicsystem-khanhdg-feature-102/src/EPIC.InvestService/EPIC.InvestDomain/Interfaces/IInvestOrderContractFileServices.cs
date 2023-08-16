using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InterestPayment;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestDomain.Interfaces
{
    public interface IInvestOrderContractFileServices
    {
        Task CreateContractFileRenewals(InvOrder order, long renewalsId, int renewalsTimes, List<ReplaceTextDto> replaceTexts, int settlementMethod);
        Task UpdateContractFile(InvOrder order, List<ReplaceTextDto> replaceTexts, int? contractType = null);
        Task UpdateContractFile(int orderId);

        /// <summary>
        /// Ký điện tử
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contractType">nếu null thì ký hết</param>
        void UpdateContractFileSignPdf(long orderId, int? contractType = null);
    }
}
