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
import { SafePipe } from './pipes/safe.pipe';
import { UserInfoService } from './services/user.service';
import { DistributionService } from './services/distribution.service';
import { FileService } from './services/file-service';
import { DashboardService } from './services/dashboard-service';
import { BondSecondaryService } from './services/bond-secondary.service';
import { AvatarService } from './services/avatar.service';
// import { AppLocalStorageService } from './services/storage.service';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
    ],
    declarations: [
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        BusyDirective,
		SafePipe,
        EqualValidator,
        LoadingPageComponent
    ],
    exports: [
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
		FormatCurrencyPipe,
		BaseApiUrlPipe,
        BusyDirective,
		SafePipe,
        EqualValidator,
        LoadingPageComponent
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
                UserInfoService,
				// AppLocalStorageService,
                MessageService,
                DistributionService,
                BondSecondaryService,
                FileService,
                DashboardService,
                AvatarService
            ]
        };
    }
}
