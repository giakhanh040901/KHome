import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AccountComponent } from './account.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { OtpCodeComponent } from './otp-code/otp-code.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { ChangePasswordTempComponent } from './change-password-temp/change-password-temp.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AccountComponent,
                children: [
                    { path: 'login', component: LoginComponent },
                    { path: 'change-password', component: ChangePasswordComponent },
                    { path: 'forgot-password', component: ForgotPasswordComponent },
                    { path: 'verify-policy', component: OtpCodeComponent },
                    { path: 'change-password-temp', component: ChangePasswordTempComponent,  canActivate: [AppRouteGuard]}
                ]
            }
        ])
    ],
    exports: [
        RouterModule
    ]
})
export class AccountRoutingModule { }
