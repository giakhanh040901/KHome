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
import * as CalenderServiceProxies from "./calendar-service"
import * as DashBoardServiceProxies from "./dashboard-service";
import { NotificationService } from "./notification-service";

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
        TradingAccountService,
        NotificationService,
    ]
})
export class ServiceProxyModule { }
