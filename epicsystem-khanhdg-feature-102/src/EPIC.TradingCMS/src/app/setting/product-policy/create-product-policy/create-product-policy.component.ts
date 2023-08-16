import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import * as moment from 'moment';
import { YesNoConst, ProductPolicyConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ProductPolicyServiceProxy } from '@shared/service-proxies/setting-service';
import { NgSelectComponent } from '@ng-select/ng-select';

@Component({
  selector: 'app-create-product-policy',
  templateUrl: './create-product-policy.component.html',
  styleUrls: ['./create-product-policy.component.scss']
})
export class CreateProductPolicyComponent extends AppComponentBase {


  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _productPolicyService: ProductPolicyServiceProxy,
    private _datePickerLocale: BsLocaleService,
    private _render: Renderer2,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  productPolicy: any = {
    "bondPolicyId": 0,
    "policyCode": null,
    "policyName": null,
    "policyType": null,
    "market": null,
    "allowTransfer": null,
    "transferTaxRate": null,
    "custType": null,
    "callDay": null,
    "minValue": null,
    "status": ProductPolicyConst.statusList[0].code,
    "policyDesc": null,
  }

  fieldCurrencyDisplays = {
    "transferTaxRate": null,
    "minValue": null,
  }

  elementSelect = ['policyType','market','custType','status'];

  title: string;
  page = new Page();

  // AppConsts
  ProductPolicyConst = ProductPolicyConst;
  YesNoConst = YesNoConst;

  @Output() onSave = new EventEmitter<any>();
  // Element Input
  @ViewChild('policyCode') policyCode: ElementRef<HTMLElement>;
  @ViewChild('policyType') policyType: NgSelectComponent;
  @ViewChild('policyName') policyName: ElementRef<HTMLElement>;
  @ViewChild('policyDesc') policyDesc: ElementRef<HTMLElement>;
  @ViewChild('market') market: NgSelectComponent;
  @ViewChild('allowTransfer') allowTransfer: NgSelectComponent;
  @ViewChild('transferTaxRate') transferTaxRate: ElementRef<HTMLElement>;
  @ViewChild('custType') custType: NgSelectComponent;
  @ViewChild('callDay') callDay: ElementRef<HTMLElement>;
  @ViewChild('minValue') minValue: ElementRef<HTMLElement>;
  @ViewChild('status') status: NgSelectComponent;

  ngOnInit(): void {

    if(this.productPolicy.bondPolicyId) {
      this.isLoading = true
      this._productPolicyService.get(this.productPolicy.bondPolicyId).subscribe((res) => {
        this.productPolicy = res.data;
        // Format date output
        this.formatCurrencyOutput(this.fieldCurrencyDisplays, this.productPolicy);
        this.isLoading = false;
        console.log({ itemBondDetail: res.data , display: this.fieldCurrencyDisplays});
      });
    }
  }

  disableWarningSelect(nameElement: string) {
    let elSelect = this[nameElement].element.querySelector('div.ng-select-container');
    this._render.setStyle(elSelect, 'border', '1px solid #d9d9d9', );
    // elSelect.ownerDocument.body.style.setProperty('border','1px solid #d9d9d9', '!important');
  }

  triggerFieldError(name: string) {
    if(this.elementSelect.includes(name)) {
      let elSelect = this[name].element.querySelector('div.ng-select-container');
      // this[name].focus();
      this._render.setStyle(elSelect, 'border', '1px solid red');
    } else {
      let el: HTMLElement = this[name].nativeElement;
      el.focus(); // focus
      el.classList.add('is-invalid');
    }
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if(this.productPolicy[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  changeDatePicker(field, value) {
    console.log(value)
    if(value == 'Invalid Date') {
      this.triggerFieldError(field);
    } else {
      this.productPolicy[field] = value;
    }
  }

  changeCurrency(field, value) {
    this.fieldCurrencyDisplays[field] = this.formatCurrency(value);
  }

  save(formValid:boolean): void {
    if(formValid) {
      this.saving = true;
      // Fomat date Input
      this.formatCurrencyInput(this.fieldCurrencyDisplays, this.productPolicy);
      const data = { ...this.productPolicy };
      if (!this.productPolicy.bondPolicyId) {
        this._productPolicyService.create(data).subscribe(
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
        this._productPolicyService.update(data, this.productPolicy.bondPolicyId).subscribe((response) => {
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
    } else {
      this.notify.error('Vui lòng nhập dữ liệu cho các mục có dấu (*)');
    }
  }
}
