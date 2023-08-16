import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
//import { AbpHttpInterceptor } from "abp-ng2-module";

import * as ApiServiceProxies from './service-proxies';
import * as FileServiceProxies from './file-service';
import * as DashBoardServiceProxies from './dashboard-service';
import { FileService } from '@shared/services/file-service';

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
    FileService,
    FileServiceProxies.FileServiceProxy,
    // DashBoardService
    DashBoardServiceProxies.DashBoardServiceProxy,
  ],
})
export class ServiceProxyModule {}
