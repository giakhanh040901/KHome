import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, BlockageLiberationConst, FormNotificationConst } from '@shared/AppConsts';
import { OBJECT_CONFIRMATION_DIALOG } from '@shared/base-object';
import { CrudComponentBase } from '@shared/crud-component-base';
import { MessageService } from 'primeng/api';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {DynamicDialogConfig} from 'primeng/dynamicdialog';

const { DEFAULT_IMAGE } = OBJECT_CONFIRMATION_DIALOG;

@Component({
  selector: 'app-contract-blockage-detail',
  templateUrl: './contract-blockage-detail.component.html',
  styleUrls: ['./contract-blockage-detail.component.scss']
})
export class ContractBlockageDetailComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef, 
    public configDialog: DynamicDialogConfig
  ) { 
    super(injector, messageService);
  }

  default: any;
  submitted = false;


  showApproveBy: boolean = false;
  acceptStatus: boolean = true;

  AppConsts = AppConsts;
  BlockageLiberationConst = BlockageLiberationConst;
  DEFAULT_IMAGE = DEFAULT_IMAGE;

  data = {
    title: null,
    icon: null,
  }

  blockageLiberation: any;

  ngOnInit(): void {
    this.blockageLiberation = this.configDialog.data.inputData
    this.blockageLiberation = {
      ...this.blockageLiberation,
      blockadeDate: (this.blockageLiberation.blockadeDate || this.blockageLiberation.blockadeTime) ? new Date(this.blockageLiberation.blockadeDate || this.blockageLiberation.blockadeTime) : null,
      liberationDate: this.blockageLiberation.liberationDate ? new Date(this.blockageLiberation.liberationDate) : null,
    }
    this.default = this.configDialog.data.icon;
    if(this.default == FormNotificationConst.IMAGE_APPROVE){
      this.data.icon = DEFAULT_IMAGE.IMAGE_APPROVE;
    } else if(this.default == FormNotificationConst.IMAGE_CLOSE) {
      this.data.icon = DEFAULT_IMAGE.IMAGE_CLOSE;
    }
  } 

  hideDialog() {
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
    this.ref.close({data: this.data,accept: this.acceptStatus});
  }
}

