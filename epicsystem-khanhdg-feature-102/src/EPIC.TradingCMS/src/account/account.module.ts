import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { AccountRoutingModule } from './account-routing.module';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { AccountComponent } from './account.component';
import { LoginComponent } from './login/login.component';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { AppConsts } from '@shared/AppConsts';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    SharedModule,
    ServiceProxyModule,
    AccountRoutingModule,
  ],
  declarations: [
    AccountComponent,
    LoginComponent,
  ],
  providers: [
    { provide: API_BASE_URL, useFactory: () => AppConsts.remoteServiceBaseUrl },
  ],
  entryComponents: [
  ]
})
export class AccountModule {
}
