import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductBondTypeServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ObjectUnsubscribedError } from 'rxjs';
import { Page } from '@shared/model/page';
import { IssuerConst } from '@shared/AppConsts';

@Component({
  selector: 'app-create-or-update-product-bond-type',
  templateUrl: './create-or-update-product-bond-type.component.html',
  styleUrls: ['./create-or-update-product-bond-type.component.scss']
})
export class CreateOrUpdateProductBondTypeComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _productBondTypeService: ProductBondTypeServiceProxy,
    private _datePickerLocale: BsLocaleService,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  productBondType: any = {
    "id": 0,
    "code": "",
    "name": "",
    "description":"",
    "icon":"",
    "status":"",
  }

  title: string;
  page = new Page();
  productBondInfos: any = [];
  ProductBondTypeConst=IssuerConst.status;
  @Output() onSave = new EventEmitter<any>();
  // Element Input
  @ViewChild('id') id: ElementRef<HTMLElement>;
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('name') name: ElementRef<HTMLElement>;
  @ViewChild('description') description: ElementRef<HTMLElement>;
  @ViewChild('icon') icon: ElementRef<HTMLElement>;
  @ViewChild('status') status: ElementRef<HTMLElement>;
    
  ngOnInit(): void {

    if(this.productBondType.id) { 
      
      this._productBondTypeService.get(this.productBondType.id).subscribe((res) => {  
        this.productBondType = res.data;
        
        // Format date output
        console.log({ item: res.data });
      });
    }
    this._productBondTypeService.getAll(this.page).subscribe((res) => {
      this.productBondInfos = res.data.items;
    });
  }

  triggerFieldError(name: string) {
    let el: HTMLElement = this[name].nativeElement;
    el.focus(); // focus
    el.classList.add('is-invalid');
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if(this.productBondType[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  save(): void {
    this.saving = true;
  
    const data = { ...this.productBondType };
    if (!this.productBondType.id) {
      this._productBondTypeService.create(data).subscribe(
          (response) => {
              if(this.handleResponseInterceptor(response, 'Thêm thành công')) {
                this.bsModalRef.hide();
                this.onSave.emit();
              } else {
                this.saving = false;
                this.callTriggerFieldError(response);
              }
          },() => {
              this.saving = false;
          }
      );
    }
    else {
      this._productBondTypeService.update(data, this.productBondType.id).subscribe((response) => {
            if(this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
              this.bsModalRef.hide();
              this.onSave.emit();
            } else {
              this.saving = false;
              this.callTriggerFieldError(response);
            }
          },() => {
              this.saving = false;
          }
      );
    }
  }
}
