using EPIC.BondEntities.DataEntities;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.DistributionContractPayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondDistributionContractService
    {
        DistributionContractDto FindById(int id);
        PagingResult<DistributionContractDto> FindAll(int pageSize, int pageNumber, string keyword, int? status);
        BondDistributionContract Add(CreateDistributionContractDto input);
        int Update(int id, UpdateDistributionContractDto input);
        int Delete(int id);
        int Duyet(int id);

        BondDistributionContractPayment FindPaymentById(int id);
        PagingResult<DistributionContractPaymentDto> FindAllContractPayment(int contractId, int pageSize, int pageNumber, string keyword);
        BondDistributionContractPayment Add(CreateDistributionContractPaymentDto input);
        int UpdateContractPayment(int id, UpdateDistributionContractPaymentDto input);
        int ContractPaymentApprove(int id);
        int ContractPaymentCancel(int id);
        int DeleteContractPayment(int id);

        DistributionContractFile Add(CreateDistributionContractFileDto input);
        DistributionContractFile FindContractFileById(int id);
        PagingResult<DistributionContractFile> FindAllContractFile(int contractId,int pageSize, int pageNumber, string keyword);
        int ContractFileApprove(int id);
        void ContractFileCancel(CancelStatusDto input);
        int DeleteContractFile(int id);

        CouponDto FindCouponById(int id);
    }
}
