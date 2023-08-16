import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CompanyShareInfoServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-form-company-share-info-approve',
  templateUrl: './form-company-share-info-approve.component.html',
  styleUrls: ['./form-company-share-info-approve.component.scss']
})
export class FormCompanyShareInfoApproveComponent extends CrudComponentBase {
  constructor(
    public ref: DynamicDialogRef,
    private routeActive: ActivatedRoute,
    messageService: MessageService,
    injector: Injector,
    private dialogService: DialogService,
    public configDialog: DynamicDialogConfig,
    private _companyShareInfoService : CompanyShareInfoServiceProxy,
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
    } else if (this.configDialog.data.productCompanyShareId != null && this.configDialog.data.companySharePrimaryId == null && this.configDialog.data.companyShareSecondaryId == null) {
      this.approveId = this.configDialog.data.productCompanyShareId;
    } else if (this.configDialog.data.companySharePrimaryId != null && this.configDialog.data.productCompanyShareId != null && this.configDialog.data.companyShareSecondaryId == null) {
      this.approveId = this.configDialog.data.companySharePrimaryId;
    } else if (this.configDialog.data.companyShareSecondaryId != null) {
      this.approveId = this.configDialog.data.companyShareSecondaryId;
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