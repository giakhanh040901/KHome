import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ContractTypeServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-update-contract-type',
  templateUrl: './create-or-update-contract-type.component.html',
  styleUrls: ['./create-or-update-contract-type.component.scss']
})
export class CreateOrUpdateContractTypeComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _contractTypeService: ContractTypeServiceProxy,
    private _datePickerLocale: BsLocaleService,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  @Output() onSave = new EventEmitter<any>();

  // Element Input
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('name') name: ElementRef<HTMLElement>;
  @ViewChild('description') description: ElementRef<HTMLElement>;

  contractType: any = {
    "id": 0,
    "code": "",
    "name": "",
    "description": "",
  }

  types = [];
  title: string;

  ngOnInit(): void {
    if (this.contractType.id) {
      this.isLoading = true
      this._contractTypeService.get(this.contractType.id).subscribe(res => {
        this.contractType = res.data;
        this.isLoading = false;
      });
    }
  }

  save(): void {
    this.saving = true;
    const data = { ...this.contractType };
    if (!this.contractType.id) {
      this._contractTypeService.create(data).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.bsModalRef.hide();
            this.onSave.emit();
          } else {
            this.saving = false;
            // this.callTriggerFieldError(response);
          }
        }, () => {
          this.saving = false;
        }
      );
    } else {
      this._contractTypeService.update(data, this.contractType.id).subscribe((response) => {
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.bsModalRef.hide();
          this.onSave.emit();
        } else {
          this.saving = false;
          // this.callTriggerFieldError(response);
        }
      }, () => {
        this.saving = false;
      }
      );
    }
  }

}
