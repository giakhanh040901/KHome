import { NgModule } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { NgxPaginationModule } from 'ngx-pagination';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';
import { NgbModule, NgbNavbar } from '@ng-bootstrap/ng-bootstrap';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgxDatatableModule } from '@swimlane/ngx-datatable';
//ngx-boostrap
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerConfig, BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';

import { HomeComponent } from './home/home.component';
import { SidebarComponent } from './layout/sidebar/sidebar.component';
import { NavbarComponent } from './layout/navbar/navbar.component';
import { SettingsPanelComponent } from './layout/settings-panel/settings-panel.component';
import { FooterComponent } from './layout/footer/footer.component';
import { CreateOrUpdateIssuerComponent } from './setting/issuer/create-or-update-issuer/create-or-update-issuer.component';
import { IssuerComponent } from './setting/issuer/issuer.component';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { viLocale } from 'ngx-bootstrap/locale';
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { CreateOrUpdateTradingProviderComponent } from './setting/trading-provider/create-or-update-trading-provider/create-or-update-trading-provider.component';
import { ProductBondDetailComponent } from './bond-manager/product-bond-detail/product-bond-detail.component';
import { CreateOrUpdateProductBondDetailComponent } from './bond-manager/product-bond-detail/create-or-update-product-bond-detail/create-or-update-product-bond-detail.component';
import { ProductBondInfoComponent } from './bond-manager/product-bond-info/product-bond-info.component';
import { CreateOrUpdateProductBondInfoComponent } from './bond-manager/product-bond-info/create-or-update-product-bond-info/create-or-update-product-bond-info.component';
import { DepositProviderComponent } from './setting/deposit-provider/deposit-provider.component';
import { CreateOrUpdateDepositProviderComponent } from './setting/deposit-provider/create-or-update-deposit-provider/create-or-update-deposit-provider.component';
import { CalendarComponent } from './setting/calendar/calendar.component';
import { CreateOrUpdateCalendarComponent } from './setting/calendar/create-or-update-calendar/create-or-update-calendar.component';
import { ProductCategoryComponent } from './setting/product-category/product-category.component';
import { CreateOrUpdateProductCategoryComponent } from './setting/product-category/create-or-update-product-category/create-or-update-product-category.component';
import { ProductBondInterestComponent } from './setting/product-bond-interest/product-bond-interest.component';
import { CreateOrUpdateProductBondInterestComponent } from './setting/product-bond-interest/create-or-update-product-bond-interest/create-or-update-product-bond-interest.component';
import { ProductBondTypeComponent } from './setting/product-bond-type/product-bond-type.component';
import { CreateOrUpdateProductBondTypeComponent } from './setting/product-bond-type/create-or-update-product-bond-type/create-or-update-product-bond-type.component';
import { AppConsts } from '@shared/AppConsts';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { UserComponent } from './user/user.component';
import { CreateOrUpdateUserComponent } from './user/create-or-update-user/create-or-update-user.component';
import { ContractTemplateComponent } from './bond-manager/contract-template/contract-template.component';
import { CreateOrUpdateContractTemplateComponent } from './bond-manager/contract-template/create-or-update-contract-template/create-or-update-contract-template.component';
import { ContractTypeComponent } from './bond-manager/contract-type/contract-type.component';
import { CreateOrUpdateContractTypeComponent } from './bond-manager/contract-type/create-or-update-contract-type/create-or-update-contract-type.component';
import { AngularFileUploaderModule } from 'angular-file-uploader';
import { ProductPolicyComponent } from './setting/product-policy/product-policy.component';
import { CreateProductPolicyComponent } from './setting/product-policy/create-product-policy/create-product-policy.component';

export const options: Partial<IConfig> | (() => Partial<IConfig>) = null;
@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        SidebarComponent,
        NavbarComponent,
        SettingsPanelComponent,
        FooterComponent,
        //
        IssuerComponent,
        CreateOrUpdateIssuerComponent,
        TradingProviderComponent,
        CreateOrUpdateTradingProviderComponent,
        ProductBondDetailComponent,
        CreateOrUpdateProductBondDetailComponent,
        //
        ProductBondInfoComponent,
        CreateOrUpdateProductBondInfoComponent,
        DepositProviderComponent,
        CreateOrUpdateDepositProviderComponent,
        CalendarComponent,
        CreateOrUpdateCalendarComponent,
        ProductCategoryComponent,
        CreateOrUpdateProductCategoryComponent,
        ProductBondInterestComponent,
        CreateOrUpdateProductBondInterestComponent,
        ProductBondTypeComponent,
        CreateOrUpdateProductBondTypeComponent,
        UserComponent,
        CreateOrUpdateUserComponent,

        ContractTemplateComponent,
        CreateOrUpdateContractTemplateComponent,
        ContractTypeComponent,
        CreateOrUpdateContractTypeComponent,
        ProductPolicyComponent,
        CreateProductPolicyComponent,
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        AppRoutingModule,
        ServiceProxyModule,
        SharedModule,
        NgxPaginationModule,
        NgbModule,
        NgxMaskModule.forRoot(),
        NgSelectModule,
        FormsModule,
        NgxDatatableModule,
        ModalModule,
        BsDropdownModule,
        BsDatepickerModule.forRoot(),
        CollapseModule,
        TabsModule,
        AngularFileUploaderModule,
    ],
    providers: [
        BsDatepickerConfig,
        { provide: API_BASE_URL, useFactory: () => AppConsts.remoteServiceBaseUrl },
    ],
    entryComponents: [
    ],
    exports: [
    ]
})
export class AppModule {
    constructor() {
        defineLocale('vi', viLocale);
    }
 }
