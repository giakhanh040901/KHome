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
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { MessageModule } from 'primeng/message';
import { MessagesModule } from 'primeng/messages';
import { ToastModule} from 'primeng/toast';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { OtpCodeComponent } from './otp-code/otp-code.component';
import { CheckboxModule } from 'primeng/checkbox';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        SharedModule,
        ServiceProxyModule,
        AccountRoutingModule,
        ButtonModule,
        InputTextModule,
        PasswordModule,
        MessageModule,
        MessagesModule,
        ToastModule,
        CheckboxModule,
        ProgressSpinnerModule,
    ],
    declarations: [
        AccountComponent,
        LoginComponent,
        ChangePasswordComponent,
        ForgotPasswordComponent,
        OtpCodeComponent,
    ],
    providers: [
        { provide: API_BASE_URL, useFactory: () => AppConsts.remoteServiceBaseUrl },
    ],
    entryComponents: [
    ]
})
export class AccountModule {
}
