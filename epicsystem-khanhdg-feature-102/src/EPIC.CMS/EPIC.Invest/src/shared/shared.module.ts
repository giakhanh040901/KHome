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
import { FormatCurrencyPipe } from './pipes/currencyEpic.pipe';
import { BaseApiUrlPipe } from './pipes/baseApiUrl.pipe';
import { SexNamPipe } from './pipes/sexName.pipe';
import { PolicyTemplateService } from './services/policy-template.service';
import { DistributionService } from './services/distribution.service';
import { SaleService } from './services/sale.service';
import { DeliveryContractService } from './services/delivery-contract.service';
import { BlockageLiberationService } from './services/blockade-liberation.service';
import { ExportReportService } from './services/export-report.service';
import { ContractFormService } from './services/contract-form.service';
import { SampleContractService } from './services/sample-contract.service';
import { WithdrawalService } from './services/withdrawal-service';
import { CollectMoneyBankService } from './services/collect-money-bank.service';
import { QueryMoneyBankService } from './services/query-bank.service';
import { FunctionForDevService } from './services/function-for-dev.service';
import { ApproveService } from './services/approve.service';
import { BankService } from './services/bank.service';
import { OrderService } from './services/order.service';
import { OrderPaymentService } from './services/order-payment.service';
import { ContractTemplateService } from './services/contract-template.service';
import { InterestPaymentService } from './services/interest-payment.service';
import { GeneralContractorService } from './services/general-contractor.service';
import { TradingProviderService } from './services/trading-provider.service';
import { MediaService } from './services/media.service';
import { ProjectShareService } from './services/project-share.service';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { TagModule } from 'primeng/tag';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { ETableComponent } from './components/e-table/e-table.component';
import { WrapperPageDetailComponent } from './components/wrapper-page-detail/wrapper-page-detail.component';
import { ImageModule } from 'primeng/image';
import { InputSearchComponent } from './components/input-search/input-search.component';
import { InputTextModule } from 'primeng/inputtext';
import { ELoadingComponent } from './components/e-loading/e-loading.component';
import { ECheckboxSingleComponent } from './components/e-checkbox-single/e-checkbox-single.component';
import { EMultiSelectComponent } from './components/e-multi-select/e-multi-select.component';
import { MultiSelectModule } from 'primeng/multiselect';
import { FunctionPipe } from './pipes/function.pipe';

// import { AppLocalStorageService } from './services/storage.service';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        TableModule,
        PaginatorModule,
        TagModule,
        MenuModule,
        ButtonModule,
        CheckboxModule,
        ImageModule,
        InputTextModule,
        MultiSelectModule,
    ],
    declarations: [
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        SexNamPipe,
        BusyDirective,
        EqualValidator,
        LoadingPageComponent,
        ETableComponent,
        WrapperPageDetailComponent,
        InputSearchComponent,
        ELoadingComponent,
        ECheckboxSingleComponent,
        EMultiSelectComponent,
        FunctionPipe,
    ],
    exports: [
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        SexNamPipe,
        BusyDirective,
        EqualValidator,
        LoadingPageComponent,
        ETableComponent,
        WrapperPageDetailComponent,
        InputSearchComponent,
        ELoadingComponent,
        ECheckboxSingleComponent,
        EMultiSelectComponent,
        FunctionPipe,
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
                ContractFormService,
                SampleContractService,
                WithdrawalService,
                CollectMoneyBankService,
                QueryMoneyBankService,
                FunctionForDevService,
                ApproveService,
                BankService,
                OrderService,
                OrderPaymentService,
                ContractTemplateService,
                InterestPaymentService,
                GeneralContractorService,
                TradingProviderService,
                MediaService,
                ProjectShareService,
            ]
        };
    }
}
