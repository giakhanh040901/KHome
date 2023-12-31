import { Component, OnInit } from '@angular/core';
import { FormNotificationConst, OBJECT_CONFIRMATION_DIALOG } from '@shared/AppConsts';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

const { DEFAULT_IMAGE } = OBJECT_CONFIRMATION_DIALOG;

@Component({
  selector: 'form-notification-description',
  templateUrl: './form-notification-description.component.html',
  styleUrls: ['./form-notification-description.component.scss'],
})
export class FormNotificationDescriptionComponent implements OnInit {
  constructor(public ref: DynamicDialogRef, public configDialog: DynamicDialogConfig) {}

  default: any;
  submitted = false;

  showApproveBy: boolean = false;
  acceptStatus: boolean = true;

  DEFAULT_IMAGE = DEFAULT_IMAGE;

  data = {
    title: null,
    icon: null,
    description: '',
  };

  ngOnInit(): void {
    this.data.title = this.configDialog.data.title;
    this.default = this.configDialog.data.icon;
    if (this.default == FormNotificationConst.IMAGE_APPROVE) {
      this.data.icon = DEFAULT_IMAGE.IMAGE_APPROVE;
    } else if (this.default == FormNotificationConst.IMAGE_CLOSE) {
      this.data.icon = DEFAULT_IMAGE.IMAGE_CLOSE;
    }
  }

  hideDialog() {}

  accept() {
    this.acceptStatus = true;
    this.onAccept();
  }

  cancel() {
    this.acceptStatus = false;
    this.onAccept();
  }

  onAccept() {
    this.ref.close({ data: this.data, accept: this.acceptStatus });
  }
}
