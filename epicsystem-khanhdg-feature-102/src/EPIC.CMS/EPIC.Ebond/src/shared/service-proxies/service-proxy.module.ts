import { NgModule } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
//import { AbpHttpInterceptor } from "abp-ng2-module";

import * as ApiServiceProxies from "./service-proxies";
import * as SettingServiceProxies from "./setting-service";
import * as BondManagerServiceproxies from "./bond-manager-service";
import * as TradingContractServiceProxy from "./trading-contract-service";
import * as ApproveServiceProxies from "./approve-service";
import * as BankServiceProxies from "./bank-service";
import * as FileServiceProxies from "./file-service";
import * as DashBoardServiceProxies from "./dashboard-service";
import { TradingAccountService } from "./trading-account-service";
import { NotificationService } from "./notification-service";

@NgModule({
    providers: [
        //core f
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        //File
        FileServiceProxies.FileServiceProxy,
        //SettingService
        SettingServiceProxies.IssuerServiceProxy,
        SettingServiceProxies.DepositProviderServiceProxy,
        SettingServiceProxies.TradingProviderServiceProxy,
        SettingServiceProxies.DepositProviderServiceProxy,
        SettingServiceProxies.CalendarServiceProxy,
        SettingServiceProxies.TradingProviderServiceProxy,
        SettingServiceProxies.ProductCategoryServiceProxy,
        SettingServiceProxies.ProductBondTypeServiceProxy,
        SettingServiceProxies.ProductBondInterestServiceProxy,
        SettingServiceProxies.ProductPolicyServiceProxy,
        SettingServiceProxies.BusinessCustomerServiceProxy,
        SettingServiceProxies.MediaService,
        //BondManagerService
        BondManagerServiceproxies.ProductBondSecondaryServiceProxy,
        BondManagerServiceproxies.ProductBondDetailServiceProxy,
        BondManagerServiceproxies.ProductBondInfoServiceProxy,
        BondManagerServiceproxies.ContractTemplateServiceProxy,
        BondManagerServiceproxies.ContractTypeServiceProxy,
        BondManagerServiceproxies.ProductBondPrimaryServiceProxy,
        BondManagerServiceproxies.ProductBondPolicyTemplateServiceProxy,
        BondManagerServiceproxies.DistributionContractServiceProxy,
        BondManagerServiceproxies.DistributionContractPaymentServiceProxy,
        BondManagerServiceproxies.DistributionContractFileServiceProxy,
        BondManagerServiceproxies.ProductBondSecondaryServiceProxy,
        BondManagerServiceproxies.ProductBondPolicyServiceProxy,
        BondManagerServiceproxies.GuaranteeAssetServiceProxy,
        BondManagerServiceproxies.ProductBondInfoFileServiceProxy,
        BondManagerServiceproxies.ProductBondSecondaryFileServiceProxy,
        BondManagerServiceproxies.ReceiveContractTemplateServiceProxy,
        //{ provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
        ApproveServiceProxies.ApproveServiceProxy,
        BankServiceProxies.BankServiceProxy,

        // DashBoardService
        DashBoardServiceProxies.DashBoardServiceProxy,

        TradingContractServiceProxy.OrderServiceProxy,
        TradingContractServiceProxy.OrderPaymentServiceProxy,
        TradingContractServiceProxy.InterestPaymentServiceProxy,
        TradingAccountService,
        NotificationService
    ]
})
export class ServiceProxyModule { }
