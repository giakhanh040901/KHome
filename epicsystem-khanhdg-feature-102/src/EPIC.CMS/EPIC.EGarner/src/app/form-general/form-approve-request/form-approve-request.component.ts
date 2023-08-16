import { ApproveServiceProxy } from '@shared/service-proxies/approve-service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit } from '@angular/core';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { ApproveConst, WithdrawalConst } from '@shared/AppConsts';
import { WithdrawalService } from '@shared/services/withdrawal-service';

@Component({
  selector: 'app-form-approve-request',
  templateUrl: './form-approve-request.component.html',
  styleUrls: ['./form-approve-request.component.scss']
})
export class FormApproveRequestComponent extends CrudComponentBase {

  constructor(
		injector: Injector,
		messageService: MessageService,
    public ref: DynamicDialogRef, 
    public configDialog: DynamicDialogConfig,
    private _approveServcie: ApproveServiceProxy,
    private _withdrawalService: WithdrawalService,
	) {
		super(injector, messageService);
	}

  radioApprove: boolean = true;
  dataType: number;

  item: any;

  dataApprove = {
    id: 0,
    approveNote: null,
    cancelNote: null,
  }

  ngOnInit(): void {
      console.log('item___', this.configDialog.data.item);
      this.item = this.configDialog.data?.item;
      
      this.dataApprove.id = this.configDialog.data.item?.referId ?? this.configDialog.data.item?.id;
      this.dataType = this.configDialog.data?.dataType;
  } 
  
  cancel() {
    this.ref.close();
  }

  save() {
    this.submitted = true;

    console.log('this.dataApprove___', this.dataApprove);
    
    // Phê duyệt yêu cầu tái tục
    if(this.dataType == ApproveConst.STATUS_REINSTATEMENT) {
      let actionApprove = this.radioApprove ? 'approve' : 'cancel';
      if(actionApprove == 'cancel') {
        this.dataApprove.cancelNote = this.dataApprove.approveNote;
      }
      this._approveServcie.reinstatementApprove(this.dataApprove, actionApprove).subscribe(res => {
        if(this.handleResponseInterceptor(res)) {
          this.ref.close(res);
        }
        this.submitted = false;
      }, (err) => {
        this.submitted = false;
        this.messageError('Có lỗi xảy ra vui lòng thử lại sau!');
      });
    }

    // Phê duyệt yêu cầu rút vốn
    if(this.dataType == ApproveConst.STATUS_WITHDRAWAL) {
      let status = this.radioApprove ? WithdrawalConst.APPROVE : WithdrawalConst.CANCEL; 
      let body = {
        withdrawalId: this.item?.id,
        status: status,
      }
      //
      this._withdrawalService.approve(body).subscribe(res => {
        if(this.handleResponseInterceptor(res)) {
          this.ref.close(res);
        }
        this.submitted = false;
      }, (err) => {
        this.submitted = false;
        this.messageError('Có lỗi xảy ra vui lòng thử lại sau!');
      });
    }
  }
}
