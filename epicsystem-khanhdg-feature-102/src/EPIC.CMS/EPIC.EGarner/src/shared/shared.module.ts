import { CommonModule } from '@angular/common';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppSessionService } from './session/app-session.service';
import { AppUrlService } from './nav/app-url.service';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { DateViewPipe } from './pipes/dateview.pipe';
import { DateTimeViewPipe } from './pipes/datetimeview.pipe';
import { TruncatePipe } from './pipes/truncate.pipe';

import { BusyDirective } from './directives/busy.directive';
import { EqualValidator } from './directives/equal-validator.directive';
import { AppUtilsService } from './services/utils.service';
import { MessageService } from 'primeng/api';
import { LoadingPageComponent } from './components/validation/loadingPage/loading-page.component';
import { FormatCurrencyPipe } from './pipes/formatCurrency.pipe';
import { BaseApiUrlPipe } from './pipes/baseApiUrl.pipe';
import { PolicyTemplateService } from './services/policy-template.service';
import { DistributionService } from './services/distribution.service';
import { SaleService } from './services/sale.service';
import { DeliveryContractService } from './services/delivery-contract.service';
import { BlockageLiberationService } from './services/blockade-liberation.service';
import { ExportReportService } from './services/export-report.service';
import { ProductService } from './services/product.service';
import { GeneralService } from './services/general-service';
import { WithdrawalService } from './services/withdrawal-service';
import { InterestPaymentService } from './services/interest-payment-service';
import { ContractFormService } from './services/contract-form.service';
import { CollectMoneyBankService } from './services/collect-money-bank.service';
import { PaymentBankService } from './services/payment-bank.service';
import { QueryMoneyBankService } from './services/query-bank.service';
import { GeneralDescriptionService } from './services/general-description.service';
import { SampleContractService } from './services/sample-contract.service';
import { DistributionContractService } from './services/distribution-contract.service';
import { ProductCollateralService } from './services/product-collateral.service';
import { PolicyFileService } from './services/policy-file.service';
import { FunctionForDevService } from './services/function-for-dev.service';
import { SpinnerLoadingComponent } from './components/spinner-loading/spinner-loading.component';
import { SpinnerService } from './services/spinner.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
// import { AppLocalStorageService } from './services/storage.service';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        ProgressSpinnerModule,
    ],
    declarations: [
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        BusyDirective,
        EqualValidator,
        LoadingPageComponent,
        SpinnerLoadingComponent,
    ],
    exports: [
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        BusyDirective,
        EqualValidator,
        LoadingPageComponent,
        SpinnerLoadingComponent,
    ]
})
export class SharedModule {
    static forRoot(): ModuleWithProviders<SharedModule> {
        return {
            ngModule: SharedModule,
            providers: [
                AppSessionService,
                AppUrlService,
                AppAuthService,
                AppRouteGuard,
                AppUtilsService,
                PolicyTemplateService,
                DistributionService,
                SaleService,
                MessageService,
                DeliveryContractService,
                BlockageLiberationService,
                ExportReportService,
                ProductService,
                GeneralService,
                WithdrawalService,
                InterestPaymentService,
                ContractFormService,
                CollectMoneyBankService,
                PaymentBankService,
                QueryMoneyBankService,
                GeneralDescriptionService,
                SampleContractService,
                DistributionContractService,
                ProductCollateralService,
                PolicyFileService,
                FunctionForDevService,
                SpinnerService,
            ]
        };
    }
}
