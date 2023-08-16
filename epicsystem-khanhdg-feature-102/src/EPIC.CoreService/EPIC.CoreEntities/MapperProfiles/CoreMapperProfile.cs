using AutoMapper;
using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.CollabContract;
using EPIC.CoreEntities.Dto.InvestorRegistorLog;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreEntities.Dto.PartnerBankAccount;
using EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount;
using EPIC.CoreEntities.Dto.Sale;
using EPIC.CoreEntities.Dto.SaleInvestor;
using EPIC.CoreEntities.Dto.TradingFirstMessage;
using EPIC.CoreEntities.Dto.TradingMSBPrefixAccount;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.CoreSharedEntities.Dto.TradingProvider;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bank;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.BusinessLicenseFile;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using EPIC.Entities.Dto.CoreHistoryUpdate;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.Sale;

namespace EPIC.CoreEntities.MapperProfiles
{
    public class CoreMapperProfile : Profile
    {
        public CoreMapperProfile()
        {
            CreateMap<Investor, InvestorDto>();
            CreateMap<InvestorTemporary, ViewManagerInvestorTemporaryDto>();
            CreateMap<Investor, ViewManagerInvestorDto>();
            CreateMap<InvestorIdentification, ViewIdentificationDto>();

            CreateMap<BusinessCustomer, BusinessCustomerDto>().ReverseMap();
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
            CreateMap<InvestorIdentification, EPIC.Entities.Dto.Investor.InvestorIdentificationDto>();
            CreateMap<BusinessCustomerTemp, BusinessCustomerTempDto>();
            CreateMap<TradingRecentlyDto, TradingRecentlyDto>();
            CreateMap<BusinessCustomerBank, AppPaymentInfoDto>();
            CreateMap<InvestorTemporary, SaleManagerInvestorDto>();

            CreateMap<Department, DepartmentDto>();
            CreateMap<SaleInfoBySaleIdDto, AppSaleInfoDto>();
            CreateMap<SaleInfoDto, AppSaleManagerDto>();
            CreateMap<SaleInfoDto, ViewSaleDto>();
            CreateMap<SaleTempDto, SaleTempDto>();
            CreateMap<ViewSaleDto, ViewSaleDto>();
            CreateMap<CoreHistoryUpdate, CoreHistorySaleUpdateDto>();
            CreateMap<AppSaleByReferralCodeDto, ViewSaleDto>();
            CreateMap<SaleInfoBySaleIdDto, AppSaleStatusByTrading>();
            CreateMap<AppCollabContractDto, AppCollabContractDto>();
            CreateMap<AppSaleByReferralCodeDto, SaleInvestorDto>();
            CreateMap<ViewCollabContractTempDto, AppCollabContractDto>();
            CreateMap<AppListSaleRegisterDto, SaleRegisterWithTradingDto>();
            CreateMap<SaleInfoDto, SaleRegisterDirectionToTradingProviderDto>();
            CreateMap<BusinessLicenseFile, BusinessLicenseFileDto>();
            CreateMap<Department, AppDeparmentDto>();
            CreateMap<CollabContract, CollabContractDto>();
            CreateMap<InvestorTemporary, HistoryUpdateInvestorDto>();
            CreateMap<InvestorIdentification, HistoryUpdateIdentificationsDto>();
            CreateMap<InvestorBankAccount, HistoryUpdateBankDto>();
            CreateMap<InvestorContactAddress, HistoryUpdateContactAddressDto>();
            CreateMap<BusinessCustomer, HistoryUpdateBusinessCustomerDto>();
            CreateMap<BusinessCustomerTemp, HistoryUpdateBusinessCustomerDto>();
            CreateMap<BusinessCustomerBankTemp, HistoryUpdateBusinessCustomerBankDto>();
            CreateMap<BusinessCustomerBank, HistoryUpdateBusinessCustomerBankDto>();
            CreateMap<BusinessCustomerBankDto, AppTradingBankAccountDto>();
            CreateMap<CreateBusinessCustomerTempDto, BusinessCustomer>().ReverseMap();

            CreateMap<InvestorBankAccount, ViewInvestorBankAccountDto>();
            CreateMap<InvestorContactAddress, ViewInvestorContactAddressDto>();
            CreateMap<InvestorTemporary, ViewInvestorInfoForSaleDto>();
            CreateMap<InvestorIdentification, ViewInvestorInfoForSaleDto>();
            CreateMap<InvestorIdentification, EkycRequiredFieldsDto>();

            CreateMap<Investor, ViewInvestorsBySaleDto>();
            CreateMap<InvestorTemporary, InvestorDto>();
            CreateMap<InvestorTemporary, EPIC.Entities.Dto.Investor.InvestorDto>();
            CreateMap<ViewSaleDto, GetDataSaleDto>();
            CreateMap<Investor, InvestorTemp>();
            CreateMap<InvestorIdentification, InvestorIdTemp>();
            CreateMap<InvestorBankAccount, InvestorBankAccountTemp>();
            CreateMap<InvestorContactAddress, InvestorContactAddressTemp>();
            //CreateMap<InvestEntities.Dto.Order.ViewOrderDto, AppInvestOrderInvestorDetailDto>();
            //CreateMap<EPIC.InvestEntities.Dto.InvestShared.CashFlowDto, EPIC.InvestEntities.Dto.Order.AppCashFlowDto>();

            CreateMap<ViewManagerInvestorTemporaryDto, UpdateManagerInvestorDto>();
            CreateMap<ViewIdentificationDto, UpdateIdentificationDto>();
            CreateMap<InvestorBankAccount, UpdateDefaultBankDto>();
            CreateMap<DepartmentSaleDto, ViewDepartmentDto>();
            CreateMap<DepartmentDto, ViewDepartmentDto>();
            CreateMap<Department, ViewDepartmentDto>();

            CreateMap<InvestorStock, ViewInvestorStockDto>();
            CreateMap<InvestorStock, HistoryUpdateStockDto>();
            CreateMap<InvestorIdentification, SCInvestorIdentificationDto>();

            CreateMap<CreateTradingMsbPrefixAccountDto, TradingMsbPrefixAccount>();
            CreateMap<UpdateTradingMsbPrefixAccountDto, TradingMsbPrefixAccount>();
            CreateMap<TradingMsbPrefixAccount, TradingMsbPrefixAccountDto>();

            CreateMap<CreatePartnerMsbPrefixAccountDto, PartnerMsbPrefixAccount>().ReverseMap();
            CreateMap<UpdatePartnerMsbPrefixAccountDto, PartnerMsbPrefixAccount>().ReverseMap();
            CreateMap<PartnerMsbPrefixAccount, PartnerMsbPrefixAccountDto>().ReverseMap();

            CreateMap<CreateInvestorRegisterLogDto, InvestorRegisterLog>().ReverseMap();

            CreateMap<TradingFirstMessage, ViewTradingFirstMessageDto>().ReverseMap();

            CreateMap<PartnerBankAccount, PartnerBankAccountDto>().ReverseMap();
            CreateMap<CreatePartnerBankAccountDto, PartnerBankAccount>();
            CreateMap<UpdatePartnerBankAccountDto, PartnerBankAccount>().ReverseMap();

            CreateMap<CoreApprove, CreateApproveRequestDto>().ReverseMap();
        }
    }
}
