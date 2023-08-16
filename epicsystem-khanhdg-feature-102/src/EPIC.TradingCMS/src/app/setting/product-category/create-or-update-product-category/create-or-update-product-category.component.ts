import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductCategoryServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-update-product-category',
  templateUrl: './create-or-update-product-category.component.html',
  styleUrls: ['./create-or-update-product-category.component.scss']
})
export class CreateOrUpdateProductCategoryComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _productCategoryService: ProductCategoryServiceProxy,
  ) {
    super(injector);
  }

  saving = false;
  productCategory: any = {
    "productCategoryId": 0,
    "code": "",
    "name": "",
    "description": "",
    "icon": "",
    "content": "",
    "appRouter": "",
  }

  types = [];
  title: string;

  @Output() onSave = new EventEmitter<any>();
  // Element Input
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('name') name: ElementRef<HTMLElement>;
  @ViewChild('description') description: ElementRef<HTMLElement>;
  @ViewChild('icon') icon: ElementRef<HTMLElement>;
  @ViewChild('content') content: ElementRef<HTMLElement>;
  @ViewChild('appRouter') appRouter: ElementRef<HTMLElement>;


  ngOnInit(): void {
    if (this.productCategory.productCategoryId) {
      this.isLoading = true;
      this._productCategoryService.get(this.productCategory.productCategoryId).subscribe((res) => {
        this.productCategory = res.data;
        this.isLoading = false;
        console.log({ itemProductCategory: res.data });
        });
    }
  }

  triggerFieldError(name: string) {
      let el: HTMLElement = this[name].nativeElement;
      el.focus(); // focus
      el.classList.add('is-invalid');
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if(this.productCategory[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  save(): void {
    this.saving = true;
    const data = { ...this.productCategory };
    console.log(data);
    if (!this.productCategory.productCategoryId) {
      this._productCategoryService.create(data).subscribe(
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
  } else {
      this._productCategoryService.update(data, this.productCategory.productCategoryId).subscribe((response) => {
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
