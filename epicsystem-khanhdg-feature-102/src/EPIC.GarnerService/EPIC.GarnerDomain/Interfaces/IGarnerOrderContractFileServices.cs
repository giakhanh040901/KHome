using EPIC.CoreSharedEntities.Dto.BankAccount;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerOrder;
using EPIC.GarnerEntities.Dto.GarnerOrderContractFile;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.Utils.DataUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerOrderContractFileServices
    {
        Task CreateContractFileByOrderAdd(long orderId, List<ReplaceTextDto> data);
        Task CreateContractFileByOrderAdd(GarnerOrder order, List<ReplaceTextDto> data);
        Task CreateContractFileByWithdrawal(long orderId, int? withDrawalId, List<ReplaceTextDto> data);
        Task UpdateContractFile(long orderId);
        void UpdateContractFileSignPdf(long orderId);
        Task UpdateContractFileUpdateSource(long orderId);
        void UpdateFileScanContract(UpdateOrderContractFileDto input);
    }
}
