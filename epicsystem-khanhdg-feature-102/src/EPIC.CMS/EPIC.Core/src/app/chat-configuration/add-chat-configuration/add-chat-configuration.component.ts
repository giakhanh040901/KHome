import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { MSBPrefixAccountServiceProxy, WhitelistIpServiceProxy } from '@shared/service-proxies/whitelist-ip-service';
import { ConfigureSystemConst, MSBPrefixAccountConst, UserTypes } from '@shared/AppConsts';
import { ConfigureSystemService } from '@shared/services/configure-system.service';
import { ChatConfigurationService } from '@shared/services/chat-configuration.service';

@Component({
  selector: 'app-add-chat-configuration',
  templateUrl: './add-chat-configuration.component.html',
  styleUrls: ['./add-chat-configuration.component.scss']
})
export class AddChatConfigurationComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    private _chatConfigurationService: ChatConfigurationService,
  ) {
    super(injector, messageService);
    this.userLogin = this.getUser();
    console.log('userLogin____', this.userLogin);
  }
  userLogin: any = {};
  configureSystem: any = {
    tradingBankAccountId: null,
  };
  UserTypes = UserTypes;
  submitted: boolean;
  view: boolean;
  checkKey: any[] = [];
  tradingProviders: any[] = [];
  ConfigureSystemConst = ConfigureSystemConst;

  ngOnInit(): void {
    this.view = this.configDialog?.data?.view;
    this.tradingProviders = this.configDialog?.data?.tradingProviders;
    if(this.configDialog?.data?.configureSystem) {
      this.configureSystem = this.configDialog?.data?.configureSystem;
      this.configureSystem.isUpdate = true
    }
    if(this.configDialog?.data?.checkKey){
      this.checkKey = this.configDialog?.data?.checkKey;
    } else {
      this.checkKey = this.tradingProviders
    }
  }

  save() {
    this.submitted = true;
    if (this.configureSystem.isUpdate) {
      this._chatConfigurationService.create(this.configureSystem).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
            this.ref.close(true);
          } else {
            this.submitted = false;
          }
        }, (err) => {
          console.log('err----', err);
          this.submitted = false;
        }
      );
    } else {
      this._chatConfigurationService.create(this.configureSystem).subscribe((response) => {
          if (this.handleResponseInterceptor(response,"Thêm thành công")) {
            this.ref.close(true);
          } else {
            this.submitted = false;
          }
        }, (err) => {
          console.log('err----', err);
          this.submitted = false;
        }
      );
    }
  }

  close() {
    this.ref.close();
  }

  validForm(): boolean {
    let validRequired;
    if(UserTypes.TYPE_TRADING_PROVIDERS.includes(this.userLogin.user_type)) {
      validRequired = this.configureSystem?.message;
    } else {
      validRequired = this.configureSystem?.message && this.configureSystem?.tradingProviderId;
    }
                        
    return validRequired;
  }

}


