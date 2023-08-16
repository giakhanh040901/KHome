using AutoMapper;
//using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bank;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.BusinessLicenseFile;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.Partner;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.TradingProvider;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityEntities.Dto.PartnerPermission;
using EPIC.IdentityEntities.Dto.Permissions;
using EPIC.IdentityEntities.Dto.Roles;
using EPIC.IdentityEntities.Dto.TradingProviderPermission;
using EPIC.IdentityEntities.Dto.UsersChat;
using EPIC.IdentityEntities.Dto.UsersPartner;
using EPIC.IdentityEntities.Dto.WhiteListIp;

namespace EPIC.IdentityEntities.MapperProfiles
{
    public class IdentityMapperProfile : Profile
    {
        public IdentityMapperProfile()
        {
            CreateMap<Investor, InvestorDto>();
            CreateMap<InvestorTemporary, ViewManagerInvestorTemporaryDto>();
            CreateMap<Investor, ViewManagerInvestorDto>();
            CreateMap<InvestorIdentification, ViewIdentificationDto>();

            CreateMap<BusinessCustomer, BusinessCustomerDto>();
            CreateMap<BusinessCustomerBank, BusinessCustomerBankDto>();

            CreateMap<CoreApprove, ViewCoreApproveDto>();

            CreateMap<InvestorBankAccount, InvestorBankAccountDto>();
            CreateMap<InvestorIdentification, IdDto>();
            CreateMap<CoreBank, BankSupportDto>();
            CreateMap<InvestorContactAddress, TransactionAddessDto>();
            CreateMap<InvestorTemporary, InvestorMyInfoDto>();
            CreateMap<Investor, ViewManagerInvestorTemporaryDto>();
            CreateMap<InvestorTemporary, AppShortSalerInfoDto>();
            CreateMap<BusinessCustomerBankTemp, BusinessCustomerBankTempDto>();

            CreateMap<InvestorIdentification, InvestorIdentificationDto>();
            CreateMap<BusinessCustomerTemp, BusinessCustomerTempDto>();

            CreateMap<TradingRecentlyDto, TradingRecentlyDto>();
            CreateMap<BusinessCustomerBank, AppPaymentInfoDto>();


            CreateMap<Department, DepartmentDto>();
            CreateMap<SaleInfoBySaleIdDto, AppSaleInfoDto>();
            CreateMap<SaleInfoDto, AppSaleManagerDto>();
            CreateMap<SaleTempDto, SaleTempDto>();
            CreateMap<ViewSaleDto, ViewSaleDto>();
            CreateMap<AppSaleByReferralCodeDto, ViewSaleDto>();
            CreateMap<SaleInfoBySaleIdDto, AppSaleStatusByTrading>();
            CreateMap<AppCollabContractDto, AppCollabContractDto>();
            CreateMap<AppSaleByReferralCodeDto, SaleInvestorDto>();
            CreateMap<ViewCollabContractTempDto, AppCollabContractDto>();
            CreateMap<AppListSaleRegisterDto, SaleRegisterWithTradingDto>();
            CreateMap<SaleInfoDto, SaleRegisterDirectionToTradingProviderDto>();
            CreateMap<BusinessLicenseFile, BusinessLicenseFileDto>();
            CreateMap<Department, AppDeparmentDto>();
            CreateMap<Partner, PartnerDto>();
            CreateMap<TradingProvider, TradingProviderDto>();
            CreateMap<Users, UserDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<PartnerPermission, PartnerPermissionDto>();
            CreateMap<RolePermission, RolePermissionDto>();
            CreateMap<Role, RolePermissionInfoDto>();
            CreateMap<UsersPartner, UsersPartnerDto>();
            CreateMap<Users, MyInfoDto>();
            CreateMap<Users, UsersPartnerDto>();
            CreateMap<UsersPartnerDto, Users>();
            CreateMap<TradingProviderPermission, TradingProviderPermissionDto>();
            CreateMap<UsersChatRoom, ViewUsersChatInfoDto>();
            CreateMap<UpdateUserDto, Users>().ReverseMap();

            CreateMap<WhiteListIp, WhiteListIpDto>();
        }
    }
}
