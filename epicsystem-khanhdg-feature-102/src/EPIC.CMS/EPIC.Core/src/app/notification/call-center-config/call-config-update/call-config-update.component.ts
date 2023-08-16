import { Component, Injector, Input, OnInit } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService, ConfirmationService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ConfigureSystemConst, MSBPrefixAccountConst } from '@shared/AppConsts';
import { ConfigureSystemService } from '@shared/services/configure-system.service';
import { CallCenterConfigService } from '@shared/services/call-center-config.service';

@Component({
  selector: 'app-call-config-update',
  templateUrl: './call-config-update.component.html',
  styleUrls: ['./call-config-update.component.scss']
})
export class CallConfigUpdateComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    public configDialog: DynamicDialogConfig,
    public ref: DynamicDialogRef,
    private _callCenterConfigService: CallCenterConfigService,
  ) {
    super(injector, messageService);
  }

  submitted: boolean;
  rows: any[] = [];

  ngOnInit(): void {
      this.rows = this.configDialog?.data?.rows.sort((a, b) => {
        if (a.sortOrder === null) {
          return 1; 
        }
        if (b.sortOrder === null) {
          return -1; 
        }
        return a.sortOrder - b.sortOrder; 
      });
  }

  save() {
    this.submitted = true;
    let body = {
      details :  this.rows.filter(item => item.sortOrder !== null)
    }
      this._callCenterConfigService.update(body).subscribe((response) => {
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
  
  }

  close() {
    this.ref.close();
  }
}

