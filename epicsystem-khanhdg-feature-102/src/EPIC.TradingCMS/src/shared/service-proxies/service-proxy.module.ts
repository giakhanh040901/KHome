import { NgModule } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { AbpHttpInterceptor } from "abp-ng2-module";

import * as ApiServiceProxies from "./service-proxies";
import * as SettingServiceProxies from "./setting-service";
import * as BondManagerServiceproxies from "./bond-manager-service";

@NgModule({
    providers: [
        //core
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        //SettingService
        SettingServiceProxies.IssuerServiceProxy,
        SettingServiceProxies.DepositProviderServiceProxy,
        SettingServiceProxies.CalendarServiceProxy,
        SettingServiceProxies.TradingProviderServiceProxy,
        SettingServiceProxies.DepositProviderServiceProxy,
        SettingServiceProxies.CalendarServiceProxy,
        SettingServiceProxies.TradingProviderServiceProxy,
        SettingServiceProxies.ProductCategoryServiceProxy,
        SettingServiceProxies.ProductBondTypeServiceProxy,
        SettingServiceProxies.ProductBondInterestServiceProxy,
        SettingServiceProxies.ProductPolicyServiceProxy,
        //BondManagerService
        BondManagerServiceproxies.ProductBondDetailServiceProxy,
        BondManagerServiceproxies.ProductBondInfoServiceProxy,
        BondManagerServiceproxies.ContractTemplateServiceProxy,
        BondManagerServiceproxies.ContractTypeServiceProxy,

        { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }


    ]
})
export class ServiceProxyModule {}
