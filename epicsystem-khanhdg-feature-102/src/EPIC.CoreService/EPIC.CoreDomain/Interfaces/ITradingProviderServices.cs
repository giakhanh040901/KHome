using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.DigitalSign;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ITradingProviderServices
    {
        PagingResult<TradingProviderDto> Find(int pageSize, int pageNumber, string keyword, int? status);
        TradingProviderDto FindById(int id);
        List<ViewTradingProviderDto> FindByTaxCode(string taxCode);
        Task<TradingProvider> Add(CreateTradingProviderDto entity);
        int Delete(int id);
        void ChangePassword(ChangePasswordDto input);
        Users CreateUser(CreateUserDto model);
        int ActiveUser(int userId, bool isActive);
        PagingResult<Users> FindAll(int pageSize, int pageNumber, string keyword);
        int DeleteUser(int userId);
        int UpdateUser(int id, UpdateUserDto model);
        List<BusinessCustomerBankDto> FindBankByTrading(int? distributionId = null, int? bankId = null);
        public List<BusinessCustomerBankDto> FindBankByTradingInvest();
        DigitalSignDto GetTradingDigitalSign();
        DigitalSign UpdateTradingDigitalSign(DigitalSignDto input);
        TradingProvider ChangeStatus(int tradingProviderId, int status);

        /// <summary>
        /// Lấy thông tin doanh nghiệp của đại lý đang đăng nhập
        /// </summary>
        /// <returns></returns>
        BusinessCustomerDto GetInfoTradingProviderCurrent();
    }
}
