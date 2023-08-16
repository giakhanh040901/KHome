import { Component, Injector, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { TradingAccountService } from '@shared/service-proxies/trading-account-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-create-trading-provider-account',
  templateUrl: './create-trading-provider-account.component.html',
  styleUrls: ['./create-trading-provider-account.component.scss']
})
export class CreateTradingProviderAccountComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private userService: UserServiceProxy,
    private ref: DynamicDialogRef, 
    private configDialog: DynamicDialogConfig,
    private _tradingAccountService: TradingAccountService,
  ) {
    super(injector, messageService);
  }

  user: any = {};

  submitted = false;

  ngOnInit() {
    this.user = this.configDialog.data.user;
    console.log('user', this.user);
  }

  cancel() {
    this.ref.destroy();
  }
 
  save() {
    this.submitted = true;
    //
    if (this.user.userId >= 0) {
      this._tradingAccountService.update(this.user).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
            this.submitted = false;
          } else {
            this.submitted = false;
          }
        }, () => {
          this.submitted = false;
        }
      );
    } else {
      this._tradingAccountService.create(this.user).subscribe((response) => {
          if(this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.ref.close(response)
          }
          this.submitted = false;
        }, (err) => {
          console.log('err---', err);
          this.submitted = false;
        }
      );
    }
  }

  validatePassword(): boolean {
    return this.user?.password?.trim() && this.user?.confirmPassword?.trim() && this.user?.confirmPassword?.trim() != this.user?.password?.trim();
  }

  validForm(): boolean {

    const validIfCreate = this.user.confirmPassword === this.user.password && this.user?.userName?.trim() && this.user?.displayName?.trim() && this.user?.password?.trim() && this.user?.email?.trim();
    const validIfUpdate = this.user?.userName?.trim() && this.user?.displayName?.trim() && this.user?.email?.trim();

    return this.user.userId >= 0 ? validIfUpdate : validIfCreate;
  }

}
