using AutoMapper;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.AppOrder;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.BondEntities.Dto.BondOrder;
using EPIC.BondEntities.Dto.BondPolicy;
using EPIC.BondEntities.Dto.BondSecondary;
using EPIC.BondEntities.Dto.BondSecondaryOverviewFile;
using EPIC.BondEntities.Dto.BondSecondaryOverviewOrg;
using EPIC.BondEntities.Dto.RenewalsRequest;
using EPIC.BondEntities.Dto.SaleInvestor;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.Bank;
using EPIC.Entities.Dto.BlockadeLiberation;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.BusinessLicenseFile;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.CoreCollabContractTemp;
using EPIC.Entities.Dto.Department;
using EPIC.Entities.Dto.DistributionContract;
using EPIC.Entities.Dto.GuaranteeAsset;
using EPIC.Entities.Dto.GuaranteeFile;
using EPIC.Entities.Dto.Investor;
using EPIC.Entities.Dto.Issuer;
using EPIC.Entities.Dto.JuridicalFile;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.Order;
using EPIC.Entities.Dto.ProductBond;
using EPIC.Entities.Dto.ProductBondPrimary;
using EPIC.Entities.Dto.ProductBondSecondPrice;
using EPIC.Entities.Dto.Sale;
using EPIC.Entities.Dto.User;
using EPIC.IdentityEntities.DataEntities;

namespace EPIC.BondEntities.MapperProfiles
{
    public class BondMapperProfile : Profile
    {
        public BondMapperProfile()
        {
            CreateMap<BondIssuer, ViewIssuerDto>();
            CreateMap<BondOrder, AppOrderInvestorDetailDto>();
            CreateMap<CashFlowDto, ProfitAppDto>();
            CreateMap<ProfitAppDto, CashFlowDto>();

            CreateMap<Investor, InvestorDto>();
            CreateMap<BondInfo, ProductBondInfoDto>();
            CreateMap<BondInfoOverviewFile, ViewBondSecondaryOverViewFileDto>();
            CreateMap<BondInfoOverviewOrg, ViewBondSecondaryOverViewOrgDto>();

            CreateMap<BondSecondary, ViewProductBondSecondaryDto>();
            CreateMap<BondPolicy, ViewProductBondPolicyDto>();
            CreateMap<BondPolicyDetail, ViewProductBondPolicyDetailDto>();
            CreateMap<BondPrimary, ProductBondPrimaryDto>();
            CreateMap<ProductBondSecondaryView, ViewProductBondSecondaryDto>();

            CreateMap<BondPolicyFile, AppPolicyFileDto>();
            CreateMap<BondGuaranteeFile, AppGuaranteeFileDto>();
            CreateMap<AppBondPolicyDto, AppBondPolicyDto>();
            CreateMap<AppBondPolicyDetailDto, AppBondPolicyDetailDto>();
            CreateMap<BondInfo, AppBondInfoDto>();
            CreateMap<BondInfoSecondaryDto, BondInfoSecondaryDto>();

            CreateMap<InvestorTemporary, ViewManagerInvestorTemporaryDto>();
            CreateMap<Investor, ViewManagerInvestorDto>();
            CreateMap<InvestorIdentification, ViewIdentificationDto>();
            CreateMap<DistributionContractDto, DistributionContractDto>();
            CreateMap<DistributionContractFile, DistributionContractFileDto>();
            CreateMap<BondGuaranteeAsset, GuaranteeAssetDto>();
            CreateMap<GuaranteeAssetDto, AppGuaranteeAssetDto>();
            CreateMap<BondGuaranteeFile, GuaranteeFileDto>();
            CreateMap<BondJuridicalFile, JuridicalFileDto>();
            CreateMap<BondOrder, ViewOrderDto>();
            CreateMap<BondOrder, SCBondOrderDto>();
            CreateMap<BusinessCustomer, BusinessCustomerDto>();
            CreateMap<BusinessCustomerBank, BusinessCustomerBankDto>();
            CreateMap<BondSecondPrice, ProductBondSecondPriceDto>();
            CreateMap<BondPolicy, ProductBondPolicyDto>();
            CreateMap<BondPolicy, BondPolicyDetail>();
            CreateMap<BondPolicy, SCBondPolicyDto>();
            CreateMap<BondPolicyDetail, SCBondPolicyDetailDto>();
            CreateMap<BondPolicyDetail, BondPolicyDetailTemp>();
            CreateMap<BondPolicyDetail, ProductBondPolicyDetailDto>();
            CreateMap<BondSecondary, BondSecondaryOverviewDto>();

            

            CreateMap<CoreApprove, ViewCoreApproveDto>();
            CreateMap<BondPolicyFile, AppPolicyFileDto>();
            CreateMap<BondGuaranteeFile, AppGuaranteeFileDto>();
            CreateMap<AppBondPolicyDto, AppBondPolicyDto>();
            CreateMap<AppBondPolicyDetailDto, AppBondPolicyDetailDto>();
            CreateMap<BondInfo, AppBondInfoDto>();
            CreateMap<BondInfo, SCBondInfoDto>();
            CreateMap<BondInfoSecondaryDto, BondInfoSecondaryDto>();
            CreateMap<BondSecondary, SCBondSecondaryDto>();
            CreateMap<BondInterestPaymentDate, InterestPaymentDateDto>();
            CreateMap<InvestorBankAccount, InvestorBankAccountDto>();
            CreateMap<InvestorIdentification, IdDto>();
            CreateMap<CoreBank, BankSupportDto>();
            CreateMap<InvestorContactAddress, TransactionAddessDto>();
            CreateMap<InvestorTemporary, InvestorMyInfoDto>();
            CreateMap<Investor, ViewManagerInvestorTemporaryDto>();
            CreateMap<InvestorTemporary, AppShortSalerInfoDto>();
            CreateMap<BusinessCustomerBankTemp, BusinessCustomerBankTempDto>();
            CreateMap<BondIssuer, ViewIssuerDto>();
            CreateMap<BondOrder, AppOrderInvestorDetailDto>();
            CreateMap<CashFlowDto, ProfitAppDto>();
            CreateMap<ProfitAppDto, CashFlowDto>();
            CreateMap<InvestorIdentification, InvestorIdentificationDto>();
            CreateMap<BusinessCustomerTemp, BusinessCustomerTempDto>();
            CreateMap<DistributionContractFile, DistributionContractFileDto>();
            CreateMap<TradingRecentlyDto, TradingRecentlyDto>();
            CreateMap<BusinessCustomerBank, AppPaymentInfoDto>();
            CreateMap<ViewOrderDto, AppOrderInvestorDetailDto>();
            CreateMap<GuaranteeAssetDto, AppGuaranteeAssetDto>();
            CreateMap<BondBlockadeLiberation, BlockadeLiberationDto>();

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
            CreateMap<BondInfoSecondaryFindDto, BondInfoSecondaryDto>();
            CreateMap<BondInterestPayment, DanhSachChiTraDto>();
            CreateMap<CoreApprove, ViewRenewalsRequestDto>();
            CreateMap<Department, ViewDepartmentDto>();
            CreateMap<Department, SCDepartmentDto>();

            CreateMap<Users, UserDto>();
            CreateMap<SaleInvestorAddOrderDto, CreateOrderAppDto>();
        }
    }
}
