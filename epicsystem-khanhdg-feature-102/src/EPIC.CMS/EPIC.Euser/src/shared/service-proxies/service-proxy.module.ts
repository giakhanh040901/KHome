import { PartnerAccountService } from '@shared/service-proxies/partner-account-service';
import { PartnerServiceProxy } from '@shared/service-proxies/partner-service';
import { NgModule } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
//import { AbpHttpInterceptor } from "abp-ng2-module";

import * as ApiServiceProxies from "./service-proxies";
import * as FileServiceProxies from "./file-service";
import * as UserServiceProxies from "./user-service";
import * as TradingProvidertServiceProxy from "./trading-provider-service"
import { UserRoleService } from "./role-service";
import * as BusinessCustomerServiceProxies from "./business-customer-service"
import * as BankServiceProxies from "./bank-service";
import * as BondManagerServiceproxies from "./bond-manager-service";
import * as ApproveServiceProxies from "./approve-service";

@NgModule({
    providers: [
        //core
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        //File
        FileServiceProxies.FileServiceProxy,
        //User
        UserServiceProxies.UserServiceProxy,
        UserRoleService,
        //TradingProvider
        TradingProvidertServiceProxy.TradingProviderServiceProxy,

        BusinessCustomerServiceProxies.BusinessCustomerApproveServiceProxy,
        BusinessCustomerServiceProxies.BusinessCustomerServiceProxy,
        BusinessCustomerServiceProxies.BusinessCustomerBankServiceProxy,
        BankServiceProxies.BankServiceProxy,
        BondManagerServiceproxies.ContractTemplateServiceProxy,

        ApproveServiceProxies.ApproveServiceProxy,
        PartnerServiceProxy,
        PartnerAccountService,
    ]
})
export class ServiceProxyModule { }
