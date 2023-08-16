import { CommonModule } from '@angular/common';
import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgxPaginationModule } from 'ngx-pagination';

import { AppSessionService } from './session/app-session.service';
import { AppUrlService } from './nav/app-url.service';
import { AppAuthService } from './auth/app-auth.service';
import { AppRouteGuard } from './auth/auth-route-guard';
import { DateViewPipe } from '@shared/pipes/dateview.pipe';
import { DateTimeViewPipe } from '@shared/pipes/datetimeview.pipe';
import { TruncatePipe } from '@shared/pipes/truncate.pipe';

import { AbpPaginationControlsComponent } from './components/pagination/abp-pagination-controls.component';
import { AbpValidationSummaryComponent } from './components/validation/abp-validation.summary.component';
import { AbpModalHeaderComponent } from './components/modal/abp-modal-header.component';
import { AbpModalFooterComponent } from './components/modal/abp-modal-footer.component';
import { LayoutStoreService } from './layout/layout-store.service';

import { BusyDirective } from './directives/busy.directive';
import { EqualValidator } from './directives/equal-validator.directive';
import { SwitchButtonComponent } from './components/element/switch-button/switch-button.component';
import { AppUtilsService } from '@shared/services/utils.service';
import { WizardStepComponent } from './components/element/angular2-wizard/wizard-step.component';
import { WizardComponent } from './components/element/angular2-wizard/wizard.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        NgxPaginationModule,
    ],
    declarations: [
        AbpPaginationControlsComponent,
        AbpValidationSummaryComponent,
        AbpModalHeaderComponent,
        AbpModalFooterComponent,
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
        BusyDirective,
        EqualValidator,
        SwitchButtonComponent,
        WizardStepComponent,
        WizardComponent,
    ],
    exports: [
        AbpPaginationControlsComponent,
        AbpValidationSummaryComponent,
        AbpModalHeaderComponent,
        AbpModalFooterComponent,
		DateViewPipe,
        DateTimeViewPipe,
        TruncatePipe,
        BusyDirective,
        EqualValidator,
        SwitchButtonComponent,
        WizardStepComponent,
        WizardComponent
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
                LayoutStoreService,
                AppUtilsService,
            ]
        };
    }
}
