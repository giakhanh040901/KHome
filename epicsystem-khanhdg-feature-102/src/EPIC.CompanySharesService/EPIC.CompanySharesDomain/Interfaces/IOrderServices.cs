
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.Order;
using EPIC.CompanySharesEntities.Dto.OrderContractFile;
using EPIC.CompanySharesEntities.Dto.SaleInvestor;
using EPIC.DataAccess.Models;
using EPIC.Entities.Dto.Bank;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Sale;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.CompanySharesDomain.Interfaces
{
    public interface IOrderServices
    {
        CompanySharesEntities.DataEntities.Order Add(CreateOrderDto input);
        int Delete(int id);
        int UpdateTotalValue(int orderId, decimal? totalValue);
        int UpdateReferralCode(int orderId, string referralCode);
        int UpdatePolicyDetail(int orderId, int? bondPolicyDetailId);
        int UpdateSource(int orderId);
        int UpdateSettlementMethod(int orderId, SettlementMethodDto input);
        int AppUpdateSettlementMethod(int orderId, SettlementMethodDto input);
        int OrderApprove(int orderId);
        public int OrderCancel(int orderId);
        PagingResult<ViewOrderDto> FindAll(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus, int? orderer);
        PagingResult<ViewOrderDto> FindAllDeliveryStatus(int pageSize, int pageNumber, string taxCode, string idNo, string cifCode, string phone, string keyword, int? status, int? groupStatus, int? source, int? bondSecondaryId, string bondPolicy, int? bondPolicyDetailId, string customerName, string contractCode, DateTime? tradingDate, int? deliveryStatus);
        public ViewOrderDto FindById(int id);
        public int Update(UpdateOrderDto input, int orderId);
        void CheckOrder(CheckOrderAppDto input);
        List<InvestorBankAccountDto> FindAllListBank();
        List<BankSupportDto> GetListBankSupport(string keyword);
        OrderAppDto InvestorAdd(CreateOrderAppDto input);
        OrderAppDto SaleAddInvestorOrder(SaleInvestorAddOrderDto input);
        ViewCheckSaleBeforeAddOrderDto CheckSaleBeforeAddOrder(string referralCode, int secondaryId);
        AppSaleByReferralCodeDto AppSaleOrderFindReferralCode(string referralCode, int policyDetailId);
        void UpdateContractFileSignPdf(int orderId);
        Task UpdateContractFile(int orderId);
        int UpdateOrderContractFileScan(UpdateOrderContractFileDto input);
        OrderContractFile CreateOrderContractFileScan(CreateOrderContractFileDto input);
    }
}
