import { Component, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppConsts } from '@shared/AppConsts';
import { MessageService } from 'primeng/api';
import { TokenService } from '@shared/services/token.service';
import { CookieManagerService } from '@shared/services/cookie.service';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [ MessageService ]
})
export class LoginComponent extends AppComponentBase {
    submitting = false;
    dark: boolean;
    isLoading: boolean = false;
    constructor(
        private injector: Injector,
        messageService: MessageService,
        public authService: AppAuthService,
    ) {
        super(injector, messageService);
    }

    login(): void {
        this.submitting = true;
        this.authService.authenticate(() => {
            this.submitting = false;
        });

        // this.isLoading = true;
        // setTimeout(() => {
        //     this.authService.authenticate(() => {
        //         this.submitting = false;
        //         this.isLoading = false;
        //     });
        // }, 150);
    }
}
