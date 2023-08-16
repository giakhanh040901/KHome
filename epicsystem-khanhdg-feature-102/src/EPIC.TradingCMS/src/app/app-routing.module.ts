import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { AppPermissionNames } from '@shared/AppConsts';
import { HomeComponent } from './home/home.component';
import { IssuerComponent } from './setting/issuer/issuer.component';
import { ProductBondDetailComponent } from './bond-manager/product-bond-detail/product-bond-detail.component';
import { ProductBondInfoComponent } from './bond-manager/product-bond-info/product-bond-info.component';
import { DepositProviderComponent } from './setting/deposit-provider/deposit-provider.component';
import { CalendarComponent } from './setting/calendar/calendar.component';
import { TradingProviderComponent } from './setting/trading-provider/trading-provider.component';
import { ProductCategoryComponent } from './setting/product-category/product-category.component';
import { ProductBondInterestComponent } from './setting/product-bond-interest/product-bond-interest.component';
import { ProductBondTypeComponent } from './setting/product-bond-type/product-bond-type.component';
import { UserComponent } from './user/user.component';
import { ContractTemplateComponent } from './bond-manager/contract-template/contract-template.component';
import { ContractTypeComponent } from './bond-manager/contract-type/contract-type.component';
import { ProductPolicyComponent } from './setting/product-policy/product-policy.component';

const permissionName = new AppPermissionNames();

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'home', component: HomeComponent, canActivate: [AppRouteGuard] },
                    { path: 'user', component: UserComponent, canActivate: [AppRouteGuard] },
                    {
                        path: 'setting',
                        children: [
                            { path: 'issuer', component: IssuerComponent, },
                            { path: 'trading-provider', component: TradingProviderComponent},

                            { path: 'deposit-provider', component: DepositProviderComponent, },
                            { path: 'calendar', component: CalendarComponent, },
                            { path: 'product-category', component: ProductCategoryComponent, },
                            { path: 'product-bond-type', component: ProductBondTypeComponent, },
                            { path: 'product-policy', component: ProductPolicyComponent, },
                            { path: 'product-bond-interest', component: ProductBondInterestComponent, },
                        ]
                    },
                    {
                        path: 'bond-manager',
                        children: [
                            { path: 'product-bond-detail', component: ProductBondDetailComponent, },
                            { path: 'product-bond-info', component: ProductBondInfoComponent},
                            { path: 'contract-template', component: ContractTemplateComponent},
                            { path: 'contract-type', component: ContractTypeComponent},
                        ]
                    }
                ]
            }
        ]),
    ],
    exports: [RouterModule]
})

export class AppRoutingModule {}
