import { NgModule } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
//import { AbpHttpInterceptor } from "abp-ng2-module";

import * as ApiServiceProxies from "./service-proxies";
import * as SettingServiceProxies from "./setting-service";
import * as TradingContractServiceProxy from "./trading-contract-service";
import * as ApproveServiceProxies from "./approve-service";
import * as BankServiceProxies from "./bank-service";
import * as FileServiceProxies from "./file-service";
import { TradingAccountService } from "./trading-account-service";
import { OwnerServiceProxy } from "./owner-service";
import { ProjectServiceProxy } from "./project-manager-service";
import * as ProjectMangerServiceProxies from "./project-manager-service"
import * as CalenderServiceProxies from "./calender-service"
import * as DashBoardServiceProxies from "./dashboard-service";
import { NotificationService } from "./notification-service";
import { ProvinceServiceProxy } from "./province-service";
import { BusinessCustomerApproveServiceProxy, BusinessCustomerBankServiceProxy, BusinessCustomerServiceProxy } from "./business-customer-service";
import { ProductBondInfoFileServiceProxy } from "./bond-manager-service";

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

        OwnerServiceProxy,
        SettingServiceProxies.GeneralContractorServiceProxy,
        SettingServiceProxies.TradingProviderServiceProxy,
        SettingServiceProxies.MediaService,

        //{ provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true }
        ApproveServiceProxies.ApproveServiceProxy,
        BankServiceProxies.BankServiceProxy,
        CalenderServiceProxies.CalendarServiceProxy,

        // DashBoardService
        DashBoardServiceProxies.DashBoardServiceProxy,

        TradingContractServiceProxy.OrderServiceProxy,
        TradingContractServiceProxy.OrderPaymentServiceProxy,
        TradingContractServiceProxy.InterestPaymentServiceProxy,
        TradingContractServiceProxy.SettlementServiceProxy,
        TradingAccountService,
        NotificationService,
        ProjectMangerServiceProxies.ProjectServiceProxy,
        ProvinceServiceProxy,
        BusinessCustomerApproveServiceProxy,
        BusinessCustomerBankServiceProxy,
        ProductBondInfoFileServiceProxy,
        BusinessCustomerServiceProxy 
    ]
})
export class ServiceProxyModule { }
