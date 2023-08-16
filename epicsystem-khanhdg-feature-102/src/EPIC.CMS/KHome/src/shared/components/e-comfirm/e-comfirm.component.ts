import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { IconConfirm } from '@shared/consts/base.const';
import { IEConfirm } from '@shared/interface/other.interface';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'app-e-comfirm',
  templateUrl: './e-comfirm.component.html',
  styleUrls: ['./e-comfirm.component.scss']
})

export class EConfirmComponent implements OnInit {

  constructor(
      public ref: DynamicDialogRef, 
      public configDialog: DynamicDialogConfig,
      public sanitizer: DomSanitizer,
  ) { }
  
  confirm: IEConfirm = {
      message: null,
  }

  Object = Object;
  IconConfirm = IconConfirm;
  styleIcon = {'border-radius':'8px', 'width': '40px'};

  ngOnInit(): void {
      this.confirm = this.configDialog.data;
  }
  
  sanitizerUr(src): SafeResourceUrl {
      return this.sanitizer.bypassSecurityTrustResourceUrl(src);
  }
  
  accept() {
      this.ref.close(true);
  }

  cancel() {
      this.ref.close(false);
  }
}
