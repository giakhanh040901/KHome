import { SignalrService } from './services/signalr.service';
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
import { ProjectOverviewService } from './services/project-overview.service';
import { ProjectStructureService } from './services/project-structure.service';
import { ProjectPolicyService } from './services/project-policy.service';
import { ProjectFileService } from './services/project-file.service';
import { ContractCodeStructureService } from './services/contract-code-structure.service';
import { ProjectMediaService } from './services/project-media.service';
import { ProductPolicyIncentivesService } from './services/product-policy-incentives.service';
import { ProductService } from './services/product.service';
import { ProjectListService } from './services/project-list.service';
import { ProductUtilityService } from './services/product-utility.service';
import { ProductMaterialService } from './services/product-material.service';
import { ProductDiagramService } from './services/product-diagram.service';
import { ProjectSaleService } from './services/project-sale.service';
import { ProductMediaService } from './services/product-media.service';
import { OpenSellService } from './services/open-sell.service';
import { ProductDistributionService } from './services/product-distribution.service';
import { OpenSellPolicyService } from './services/open-sell-policy.service';
import { OpenSellContractService } from './services/open-sell-contract.service';
import { OpenSellFileService } from './services/open-sell-file.service';
import { OrderStepService } from './services/order-step-service';
import { OrderSellingPolicyService } from './services/order-selling-policy.service';
import { FunctionForDevService } from './services/function-for-dev.service';
import { MonthViewPipe } from './pipes/monthview.pipe';
import { FacebookPostService } from './services/project-post.service';
import { ProjectShareService } from './services/project-share.service';
import { InputSearchComponent } from './components/input-search/input-search.component';
import { FormsModule } from '@angular/forms';
import { ECalendarComponent } from './components/e-calendar/e-calendar.component';
import { BrowserModule } from '@angular/platform-browser';
import { CalendarModule } from 'primeng/calendar';
import { InputTextModule } from 'primeng/inputtext';
import { InputSwitchModule } from 'primeng/inputswitch';
import { EInputSwitchComponent } from './components/e-input-switch/e-input-switch.component';
import { InvestorServiceProxy } from './service-proxies/investor-service';
import { EMarkdownHtmlComponent } from './components/e-markdown-html/e-markdown-html.component';
import { EConfirmComponent } from './components/e-comfirm/e-comfirm.component';
import { PrimeNGModules } from './export-modules/primeng.modules';
import { MarkdownModule } from 'ngx-markdown';
import { EViewComponent } from './components/e-view/e-view.component';
import { EUploadComponent } from './components/e-upload/e-upload.component';
import { AcceptFilePipe } from './pipes/accept-file.pipe';
import { HelpersService } from './services/helper.service';
import { DialogService } from 'primeng/dynamicdialog';
import { FileService } from './services/file-service';
// import { AppLocalStorageService } from './services/storage.service';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        ...PrimeNGModules,
        MarkdownModule.forRoot(),
    ],
    declarations: [
		DateViewPipe,
        DateTimeViewPipe,
        MonthViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        SexNamPipe,
        BusyDirective,
        EqualValidator,
        LoadingPageComponent,
        InputSearchComponent,
        ECalendarComponent,
        EInputSwitchComponent,
        EMarkdownHtmlComponent,
        EConfirmComponent,
        EViewComponent,
        EUploadComponent,
        AcceptFilePipe,
    ],
    exports: [
		DateViewPipe,
        DateTimeViewPipe,
        MonthViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        SexNamPipe,
        BusyDirective,
        EqualValidator,
        LoadingPageComponent,
        InputSearchComponent,
        ECalendarComponent,
        EInputSwitchComponent,
        EUploadComponent,
        AcceptFilePipe,
        EMarkdownHtmlComponent
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
                ProjectOverviewService,
                ProjectStructureService,
                ProjectPolicyService,
                ProjectFileService,
                ContractCodeStructureService,
                ProjectMediaService,
                ProductPolicyIncentivesService,
                ProductService,
                ProjectListService,
                ProductUtilityService,
                ProductMaterialService,
                ProductDiagramService,
                ProjectSaleService,
                ProductMediaService,
                OpenSellService,
                ProductDistributionService,
                OpenSellPolicyService,
                OpenSellContractService,
                OpenSellFileService,
                OrderStepService,
                OrderSellingPolicyService,
                SignalrService,
                FunctionForDevService,
                FacebookPostService,
                ProjectShareService,
                InvestorServiceProxy,
                HelpersService,
                DialogService,
                FileService 
            ]
        };
    }
}
