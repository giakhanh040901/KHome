import { PermissionsService } from './services/permissions.service';
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
import { InvestorServiceProxy } from './service-proxies/investor-service';
import { SafePipe } from './pipes/safe.pipe';
import { ExportReportService } from './services/export-report.service';
import { FacebookServiceProxy } from './service-proxies/facebook-service';
import { GeneralService } from './services/general-service';
import { ConfigureSystemService } from './services/configure-system.service';
import { NotificationExtendService } from './services/notification-extend.service';
import { SpinnerLoadingComponent } from './components/spinner-loading/spinner-loading.component';
import { SpinnerService } from './services/spinner.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ChatConfigurationService } from './services/chat-configuration.service';
import { VideoCallService } from './services/video-call.service';
import { SignalingService } from './services/signaling.service';
import { PartnerBankService } from './services/partner-bank-account.service';
import { CallSupportService } from './services/call-support.service';
import { CallCenterConfigService } from './services/call-center-config.service';
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
        SexNamPipe,
        SafePipe,
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
        SexNamPipe,
        SafePipe,
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
                PermissionsService,
                // AppLocalStorageService,
                MessageService,
                InvestorServiceProxy,
                ExportReportService,
                FacebookServiceProxy,
                GeneralService,
                ConfigureSystemService,
                NotificationExtendService,
                SpinnerService,
                ChatConfigurationService,
                VideoCallService,
                SignalingService,
                PartnerBankService,
                CallSupportService,
                CallCenterConfigService,
            ]
        };
    }
}
