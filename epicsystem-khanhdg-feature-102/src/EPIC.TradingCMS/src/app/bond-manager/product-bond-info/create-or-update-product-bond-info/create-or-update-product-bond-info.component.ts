import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ProductBondInfoConst } from '@shared/AppConsts';
import { forkJoin } from 'rxjs';
import { Page } from '@shared/model/page';

@Component({
  selector: 'app-create-or-update-product-bond-info',
  templateUrl: './create-or-update-product-bond-info.component.html',
  styleUrls: ['./create-or-update-product-bond-info.component.scss']
})
export class CreateOrUpdateProductBondInfoComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _productBondInfoService: ProductBondInfoServiceProxy,
    private _datePickerLocale: BsLocaleService,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  @Output() onSave = new EventEmitter<any>();
  //Input Element
  @ViewChild('productBondId') productBondId: ElementRef<HTMLElement>;
  @ViewChild('issuerId') issuerId: ElementRef<HTMLElement>;
  @ViewChild('tradingProviderId') tradingProviderId: ElementRef<HTMLElement>;
  // @ViewChild('depositProviderId') depositProviderId: ElementRef<HTMLElement>;
  @ViewChild('bondTypeId') bondTypeId: ElementRef<HTMLElement>;
  @ViewChild('bondCode') bondCode: ElementRef<HTMLElement>;
  @ViewChild('bondName') bondName: ElementRef<HTMLElement>;
  @ViewChild('description') description: ElementRef<HTMLElement>;
  @ViewChild('content') content: ElementRef<HTMLElement>;
  @ViewChild('issueDate') issueDate: ElementRef<HTMLElement>;
  @ViewChild('dueDate') dueDate: ElementRef<HTMLElement>;
  @ViewChild('parValue') parValue: ElementRef<HTMLElement>;
  @ViewChild('totalValue') totalValue: ElementRef<HTMLElement>;
  @ViewChild('bondPeriod') bondPeriod: ElementRef<HTMLElement>;
  @ViewChild('bondPeriodUnit') bondbondPeriodUnitName: ElementRef<HTMLElement>;
  @ViewChild('interestRate') interestRate: ElementRef<HTMLElement>;
  @ViewChild('interestPeriod') interestPeriod: ElementRef<HTMLElement>;
  @ViewChild('interestPeriodUnit') interestPeriodUnit: ElementRef<HTMLElement>;
  @ViewChild('interestType') interestType: ElementRef<HTMLElement>;
  @ViewChild('interestRateType') interestRateType: ElementRef<HTMLElement>;
  @ViewChild('interestCouponType') interestCouponType: ElementRef<HTMLElement>;
  @ViewChild('couponBondType') couponBondType: ElementRef<HTMLElement>;
  @ViewChild('isPaymentGuarantee') isPaymentGuarantee: ElementRef<HTMLElement>;
  @ViewChild('allowSbd') allowSbd: ElementRef<HTMLElement>;
  @ViewChild('allowSbdMonth') allowSbdMonth: ElementRef<HTMLElement>;

  // Tab
  @ViewChild('tab1') tab1: ElementRef<HTMLElement>;
  @ViewChild('tab2') tab2: ElementRef<HTMLElement>;
  @ViewChild('contentTab1') contentTab1: ElementRef<HTMLElement>;
  @ViewChild('contentTab2') contentTab2: ElementRef<HTMLElement>;
  //
  idTabElements = ['contentTab1', 'contentTab2'];
  tabElements = ['tab1', 'tab2'];

  tabInfo = [
    {
      tab: 'tab1',
      idTab: 'contentTab1',
      inputName: ['name', 'code', 'email', 'nameEn', 'phone', 'mobile'],
    },
    {
      tab: 'tab2',
      idTab: 'contentTab2',
      inputName: [],
    }
  ]


  productBondInfo: any = {
    "productBondId": 0,
    "issuerId": null,
    "tradingProviderId": null,
    // "depositProviderId": null,
    "bondTypeId": null,
    "bondCode": "",
    "bondName": "",
    "description": "",
    "content": "",
    "issueDate": null,
    "dueDate": null,
    "parValue": null,
    "totalValue": null,
    "bondPeriod": null,
    "bondPeriodUnit": null,
    "interestRate": null,
    "interestPeriod": null,
    "interestPeriodUnit": null,
    "interestType": null,
    "interestRateType": null,
    "interestCouponType": null,
    "couponBondType": null,
    "isPaymentGuarantee": null,
    "allowSbd": null,
    "allowSbdMonth": null,
  };

  productBondInfoFieldTypeDates = ['issueDate', 'dueDate'];
  productBondInfoDateDisplays = {
    'issueDate': null,
    'dueDate': null,
  }
  productBondInfoCurrentcyDisplays = {
    'parValue': null,
    'totalValue': null,
  }

  types = [];
  title: string;
  bondPeriodUnits = ProductBondInfoConst.periodUnits;
  interestPeriodUnits = ProductBondInfoConst.periodUnits;
  interestTypes = ProductBondInfoConst.couponTypes;
  interestRateTypes = ProductBondInfoConst.interestRateTypes;
  interestCouponTypes = ProductBondInfoConst.interestCouponTypes;
  couponBondTypes = ProductBondInfoConst.couponTypes;
  isPaymentGuarantees = ProductBondInfoConst.boolean;
  allowSbds = ProductBondInfoConst.boolean;

  //DATA
  page = new Page();
  issuers: any = [];
  tradingProviders: any = [];
  depositProviders: any = [];
  bondTypes: any = [];

  ngOnInit(): void {
    forkJoin([this._productBondInfoService.getAllIssuer(this.page),
    this._productBondInfoService.getAllTradingProvider(this.page),
    this._productBondInfoService.getAllDepositProvider(this.page),
    this._productBondInfoService.getAllBondType()])
      .subscribe(([resIssuer, resTradingProvider, resDepositProvider, resBondType]) => {
        this.issuers = resIssuer.data?.items;
        this.tradingProviders = resTradingProvider.data?.items;
        this.depositProviders = resDepositProvider.data?.items;
        this.bondTypes = resBondType.data?.items;
        console.log({ 'resIssuer': resIssuer, 'resTradingProvider': resTradingProvider, 'resDepositProvider': resDepositProvider, 'resBondType': resBondType });
      });

    if (this.productBondInfo.productBondId) {
      this.isLoading = true
      this._productBondInfoService.get(this.productBondInfo.productBondId).subscribe(res => {
        this.productBondInfo = res.data;
        // Format date output
        this.formatDateOutput(this.productBondInfoFieldTypeDates, this.productBondInfo, this.productBondInfoDateDisplays);
        this.formatCurrencyOutput(this.productBondInfoCurrentcyDisplays, this.productBondInfo);
        this.isLoading = false;
        console.log({ itemProductInfo: res.data });
      });
    }
  }

  changeDatePicker(field, value) {
    console.log(value)
    if (value == 'Invalid Date') {
      this.triggerFieldError(field);
    } else {
      this.productBondInfo[field] = value;
    }
  }

  changeCurrentcy(field, value) {
    this.productBondInfoCurrentcyDisplays[field] = this.formatCurrency(value);
  }

  showTab(tab: string, idTab: string) {
    for (let tab of this.tabElements) {
      let elTab: HTMLElement = this[tab].nativeElement;
      elTab.classList.remove('active');
    }
    let elTab: HTMLElement = this[tab].nativeElement;
    elTab.classList.add('active');
    //
    for (let idTab of this.idTabElements) {
      let elContentTab: HTMLElement = this[idTab].nativeElement;
      elContentTab.classList.remove('active');
    }
    let elContentTab: HTMLElement = this[idTab].nativeElement;
    elContentTab.classList.add('active');
  }

  triggerFieldError(name: string) {
    //
    let tabTrigger = this.tabInfo.find(item => item.inputName.find(field => field == name));
    if (tabTrigger) {
      this.showTab(tabTrigger.tab, tabTrigger.idTab);
    }

    let el: HTMLElement = this[name].nativeElement;
    el.focus(); // focus
    el.classList.add('is-invalid');
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if (this.productBondInfo[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  save(): void {
    this.saving = true;
    this.formatDateInput(this.productBondInfoFieldTypeDates, this.productBondInfo);
    this.formatCurrencyInput(this.productBondInfoCurrentcyDisplays, this.productBondInfo);
    const data = { ...this.productBondInfo };
    if (!this.productBondInfo.productBondId) {
      this._productBondInfoService.create(data).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
            this.bsModalRef.hide();
            this.onSave.emit('done');
          } else {
            this.saving = false;
            this.callTriggerFieldError(response);
          }
        }, () => {
          this.saving = false;
        }
      );
    } else {
      this._productBondInfoService.update(data, this.productBondInfo.productBondId).subscribe((response) => {
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.bsModalRef.hide();
          this.onSave.emit();
        } else {
          this.saving = false;
          this.callTriggerFieldError(response);
        }
      }, () => {
        this.saving = false;
      }
      );
    }
  }
}
