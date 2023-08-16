import { NgModule } from "@angular/core";
import { HTTP_INTERCEPTORS } from "@angular/common/http";
import * as ApiServiceProxies from "./service-proxies";
import * as FileServiceProxies from "../services/file-service";
import { TradingAccountService } from "../services/trading-account-service";
import { OwnerServiceProxy } from "../services/owner-service";
import { ProjectServiceProxy } from "../services/project-manager-service";
import * as ProjectMangerServiceProxies from "../services/project-manager-service"
import * as CalenderServiceProxies from "../services/calender-service"
import * as DashBoardServiceProxies from "../services/dashboard-service";
import { NotificationService } from "../services/notification-service";

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
        CalenderServiceProxies.CalendarServiceProxy,

        // DashBoardService
        DashBoardServiceProxies.DashBoardServiceProxy,
        TradingAccountService,
        NotificationService,
        ProjectMangerServiceProxies.ProjectServiceProxy,
    ]
})
export class ServiceProxyModule { }
