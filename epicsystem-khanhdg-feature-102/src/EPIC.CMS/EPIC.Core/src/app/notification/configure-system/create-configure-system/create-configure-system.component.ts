import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { PageBondPolicyTemplate } from '@shared/model/pageBondPolicyTemplate';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { MSBPrefixAccountServiceProxy, WhitelistIpServiceProxy } from '@shared/service-proxies/whitelist-ip-service';
import { ConfigureSystemConst, MSBPrefixAccountConst } from '@shared/AppConsts';
import { ConfigureSystemService } from '@shared/services/configure-system.service';

@Component({
  selector: 'app-create-configure-system',
  templateUrl: './create-configure-system.component.html',
  styleUrls: ['./create-configure-system.component.scss'],
})
export class CreateConfigureSystemComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    private _configureSystemService: ConfigureSystemService,
  ) {
    super(injector, messageService);
  }

  configureSystem: any = {
    tradingBankAccountId: null,
 
  };

  submitted: boolean;
  view: boolean;
  checkKey: any[] = [];
  offset = 0;
  listBanks: any[] = [];
  MSBPrefixAccountConst = MSBPrefixAccountConst;
  ConfigureSystemConst = ConfigureSystemConst;

  ngOnInit(): void {
    this.view = this.configDialog?.data?.view;
    if(this.configDialog?.data?.configureSystem) {
      this.configureSystem = this.configDialog?.data?.configureSystem;
      this.configureSystem.isUpdate = true
    }
    if(this.configDialog?.data?.checkKey){
      this.checkKey = this.configDialog?.data?.checkKey;
    } else {
      this.checkKey = ConfigureSystemConst.key;
    }
  }

  save() {
    this.submitted = true;
    if(ConfigureSystemConst.keyDates.includes(this.configureSystem.key)){
      let date = new Date(this.configureSystem.value);

      this.configureSystem.value = `${date.getHours()}:${date.getMinutes()}`;
    }

    if (this.configureSystem.isUpdate) {
      this._configureSystemService.update(this.configureSystem).subscribe((response) => {
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
      this._configureSystemService.create(this.configureSystem).subscribe((response) => {
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
    const validRequired = this.configureSystem?.value       ;
                        
    return validRequired;
  }

}

