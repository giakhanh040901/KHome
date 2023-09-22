import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ApproveServiceProxy } from '@shared/service-proxies/approve-service';
import { ServiceProxyBase } from '@shared/service-proxies/service-proxies-base';
import { CookieManagerService } from '@shared/services/cookie.service';
import { ProjectOverviewService } from '@shared/services/project-overview.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent extends CrudComponentBase{

  submitting = false;
  dark: boolean;

  constructor(
      private injector: Injector,
      messageService: MessageService,
      // private serviceBase: ServiceProxyBase,
      private approveservice: ApproveServiceProxy,
      private router?: Router,
      // private projectOverviewService?: ProjectOverviewService,
  ) {
      super(injector, messageService);
  }

  public register: {
    email: string;
    phone: string;
    password: string;
    name: string;
  } = {
    email: "",
    phone: "",
    password: "",
    name: "",
  };

  ngOnInit(): void {
  }

  public save(event?: any) {
    console.log("event!!!!", event);
    console.log("Thông tin!!!!", this.register);
    this.approveservice.registerCustomer(this.register).subscribe((res) => {
      if (this.handleResponseInterceptor(res, "Thêm thành công")) {
        this.router.navigate(['account/login']);
      }
      else {
        this.submitted = false;
      }
      
      
    }) 
	}

  onClickLogin() {
    this.router.navigate(['account/login']);
  }

}
