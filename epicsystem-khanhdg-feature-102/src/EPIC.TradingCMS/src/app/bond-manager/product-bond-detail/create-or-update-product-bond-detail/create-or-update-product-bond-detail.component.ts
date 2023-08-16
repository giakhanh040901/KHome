import { ProductPolicyServiceProxy } from '@shared/service-proxies/setting-service';
import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductBondDetailServiceProxy, ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import * as moment from 'moment';
import { ProductBondDetailConst } from '@shared/AppConsts';
import { forkJoin, ObjectUnsubscribedError } from 'rxjs';
import { Page } from '@shared/model/page';

@Component({
  selector: 'app-create-or-update-product-bond-detail',
  templateUrl: './create-or-update-product-bond-detail.component.html',
  styleUrls: ['./create-or-update-product-bond-detail.component.scss']
})
export class CreateOrUpdateProductBondDetailComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _datePickerLocale: BsLocaleService,
    private _productBondDetailService: ProductBondDetailServiceProxy,
    private _productBondInfoService: ProductBondInfoServiceProxy,
    private _productPolicyService: ProductPolicyServiceProxy,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  productBondDetail: any = {
    "bondDetailId": 0,
    "productBondId": null,
    "depositProviderId": null,
    "code": "",
    "name": "",
    "period": null,
    "periodUnit": null,
    "interestRate": null,
    "interestPeriod": null,
    "interestPeriodUnit": null,
    "issueDate": null,
    "dueDate": null,
    "lastOrderDate": null,
    "parValue": null,
    "totalValue": null,
    "allowOnlineTrading": 'Y',
    "market": null,
    "policyIds":[],
  }

  bondDetailFieldTypeDates = ['issueDate','dueDate', 'lastOrderDate'];
  bondDetailDateDisplays = {
    'issueDate': null,
    'dueDate': null,
    'lastOrderDate' : null
  };
  bondDetailFieldCurrencyDisplays = {
    'totalValue': null,
    "parValue": null,
  }

  title: string;
  page = new Page();
  productBondInfos: any = [];
  depositProviders: any = [];
  productPolicies: any [];

  unitDates = ProductBondDetailConst.unitDates;
  allowOnlineTradings = ProductBondDetailConst.allowOnlineTradings;
  marketConst = ProductBondDetailConst;

  @Output() onSave = new EventEmitter<any>();
  // Element Input
  @ViewChild('productBondId') productBondId: ElementRef<HTMLElement>;
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('name') name: ElementRef<HTMLElement>;
  @ViewChild('depositProviderId') depositProviderId: ElementRef<HTMLElement>;
  @ViewChild('period') period: ElementRef<HTMLElement>;
  @ViewChild('periodUnit') periodUnit: ElementRef<HTMLElement>;
  @ViewChild('interestRate') interestRate: ElementRef<HTMLElement>;
  @ViewChild('interestPeriod') interestPeriod: ElementRef<HTMLElement>;
  @ViewChild('interestPeriodUnit') interestPeriodUnit: ElementRef<HTMLElement>;
  @ViewChild('issueDate') issueDate: ElementRef<HTMLElement>;
  @ViewChild('dueDate') dueDate: ElementRef<HTMLElement>;
  @ViewChild('lastOrderDate') lastOrderDate: ElementRef<HTMLElement>;
  @ViewChild('parValue') parValue: ElementRef<HTMLElement>;
  @ViewChild('totalValue') totalValue: ElementRef<HTMLElement>;
  @ViewChild('allowOnlineTrading') allowOnlineTrading: ElementRef<HTMLElement>;

  ngOnInit(): void {
    if(this.productBondDetail.bondDetailId) {
      this.isLoading = true
      this._productBondDetailService.get(this.productBondDetail.bondDetailId).subscribe((res) => {
        this.productBondDetail = res.data;
        if(res) {
          this.productBondDetail.policyIds = res.data.policies.map(item => item.bondPolicyId);
        }
        // Format date output
        this.formatDateOutput(this.bondDetailFieldTypeDates, this.productBondDetail, this.bondDetailDateDisplays);
        this.formatCurrencyOutput(this.bondDetailFieldCurrencyDisplays, this.productBondDetail);
        this.isLoading = false;
        console.log({ itemBondDetail: res.data});
      });
    }

    forkJoin([this._productBondDetailService.getAllBondInfo(this.page), this._productBondInfoService.getAllDepositProvider(this.page), this._productPolicyService.getAll(this.page)]).subscribe(
      ([resProductBondInfo, resDepositProvider, resproductPolicy]) => {
        this.productBondInfos = resProductBondInfo.data?.items;
        this.depositProviders = resDepositProvider.data?.items;
        this.productPolicies = resproductPolicy.data?.items;
        console.log({ resproductPolicy: resproductPolicy });
    });
  }

  triggerFieldError(name: string) {
    let el: HTMLElement = this[name].nativeElement;
    el.focus(); // focus
    el.classList.add('is-invalid');
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if(this.productBondDetail[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  changeDatePicker(field, value) {
    if(value == 'Invalid Date') {
      this.triggerFieldError(field);
    } else {
      this.productBondDetail[field] = value;
    }
  }

  changeCurrency(field, value) {;
    this.bondDetailFieldCurrencyDisplays[field] = this.formatCurrency(value);
  }

  save(): void {
    this.saving = true;
    // Fomat date Input
    this.formatDateInput(this.bondDetailFieldTypeDates, this.productBondDetail);
    this.formatCurrencyInput(this.bondDetailFieldCurrencyDisplays, this.productBondDetail);
    const data = { ...this.productBondDetail };
    if (!this.productBondDetail.bondDetailId) {
      this._productBondDetailService.create(data).subscribe(
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
      this._productBondDetailService.update(data, this.productBondDetail.bondDetailId).subscribe((response) => {
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
