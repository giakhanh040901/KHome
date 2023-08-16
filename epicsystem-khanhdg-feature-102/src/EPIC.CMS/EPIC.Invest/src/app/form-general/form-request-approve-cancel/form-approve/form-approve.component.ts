import { Component, OnInit } from '@angular/core';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {DynamicDialogConfig} from 'primeng/dynamicdialog';

@Component({
  selector: 'app-form-approve',
  templateUrl: './form-approve.component.html',
  styleUrls: ['./form-approve.component.scss']
})
export class FormApproveComponent implements OnInit {

  constructor(
    public ref: DynamicDialogRef, 
    public configDialog: DynamicDialogConfig
  ) { 

  }

  title: string;
  submitted = false;

  showApproveBy: boolean = false;
  acceptStatus: boolean = true;
  check_approve: boolean;
  dataApprove = {
    id: 0,
    userApproveId: 1,
    approveNote: null,
  }
  ngOnInit(): void {
      this.dataApprove.id = this.configDialog.data.id;
      this.check_approve = true;
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
    this.ref.close({ data: this.dataApprove, accept: this.acceptStatus, checkApprove:this.check_approve});
  }

  validForm(): boolean {
    let validRequired;
    validRequired = this.dataApprove?.approveNote?.trim();
    return validRequired;
  }
  
}
