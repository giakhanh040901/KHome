import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-form-bond-info-approve',
  templateUrl: './form-bond-info-approve.component.html',
  styleUrls: ['./form-bond-info-approve.component.scss']
})
export class FormBondInfoApproveComponent extends CrudComponentBase {
  constructor(
    public ref: DynamicDialogRef,
    private routeActive: ActivatedRoute,
    messageService: MessageService,
    injector: Injector,
    private dialogService: DialogService,
    public configDialog: DynamicDialogConfig,
    private _bondInfoService : ProductBondInfoServiceProxy,
  ) {
    super(injector, messageService)
  }

  count = 0;
  isLoadingPage: boolean = false;
  title: string;
  submitted = false;
  approveId: number;
  acceptStatus: boolean = true;
  check_approve: boolean;
  dataRequest = {
    id: 0,
    actionType: 0,
    userApproveId: 1,
    requestNote: null,
    summary: null,
  }

  ngOnInit(): void {
    if(this.configDialog.data.referId != null) {
      this.approveId = this.configDialog.data.referId;
    } else if (this.configDialog.data.productBondId != null && this.configDialog.data.bondPrimaryId == null && this.configDialog.data.bondSecondaryId == null) {
      this.approveId = this.configDialog.data.productBondId;
    } else if (this.configDialog.data.bondPrimaryId != null && this.configDialog.data.productBondId != null && this.configDialog.data.bondSecondaryId == null) {
      this.approveId = this.configDialog.data.bondPrimaryId;
    } else if (this.configDialog.data.bondSecondaryId != null) {
      this.approveId = this.configDialog.data.bondSecondaryId;
    }
    
    this.dataRequest.id = this.approveId;
    this.check_approve = true;
    this.dataRequest.summary = this.configDialog.data.summary;
    this.dataRequest.actionType = this.configDialog.data.actionType;
  }

  accept() {
    this.acceptStatus = true;
    this.onAccept();
  }

  cancel() {
    this.acceptStatus = false;
    this.onAccept();
  }

  onAccept() {
    this.dataRequest.id = this.approveId;
    this.ref.close({ data: this.dataRequest, accept: this.acceptStatus, checkApprove:this.check_approve  });
  }

  validForm(): boolean {
    const validRequired = this.dataRequest?.requestNote?.trim();
    return validRequired;
  }

}