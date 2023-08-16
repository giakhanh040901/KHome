import { Component, ElementRef, EventEmitter, Injector, Input, OnInit, Output, Renderer2, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { IssuerServiceProxy } from '@shared/service-proxies/setting-service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import * as moment from 'moment';
import { IssuerConst } from '@shared/AppConsts';
import { ObjectUnsubscribedError } from 'rxjs';

@Component({
  selector: 'app-create-or-update-issuer',
  templateUrl: './create-or-update-issuer.component.html',
  styleUrls: ['./create-or-update-issuer.component.scss']
})
export class CreateOrUpdateIssuerComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _issuerService: IssuerServiceProxy,
    private _datePickerLocale: BsLocaleService,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  @Output() onSave = new EventEmitter<any>();

  // Element Input
  @ViewChild('name') name: ElementRef<HTMLElement>;
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('nameEn') nameEn: ElementRef<HTMLElement>;
  @ViewChild('shortName') shortName: ElementRef<HTMLElement>;
  @ViewChild('type') type: ElementRef<HTMLElement>;
  @ViewChild('address') address: ElementRef<HTMLElement>;
  @ViewChild('phone') phone: ElementRef<HTMLElement>;
  @ViewChild('fax') fax: ElementRef<HTMLElement>;
  @ViewChild('mobile') mobile: ElementRef<HTMLElement>;
  @ViewChild('email') email: ElementRef<HTMLElement>;
  @ViewChild('taxCode') taxCode: ElementRef<HTMLElement>;
  @ViewChild('bankAccNo') bankAccNo: ElementRef<HTMLElement>;
  @ViewChild('bankName') bankName: ElementRef<HTMLElement>;
  @ViewChild('bankBranchName') bankBranchName: ElementRef<HTMLElement>;
  @ViewChild('foundationNo') foundationNo: ElementRef<HTMLElement>;
  @ViewChild('foundationDate') foundationDate: ElementRef<HTMLElement>;
  @ViewChild('foundationIssuer') foundationIssuer: ElementRef<HTMLElement>;
  @ViewChild('licenseNo') licenseNo: ElementRef<HTMLElement>;
  @ViewChild('licenseDate') licenseDate: ElementRef<HTMLElement>;
  @ViewChild('licenseIssuer') licenseIssuer: ElementRef<HTMLElement>;
  @ViewChild('capital') capital: ElementRef<HTMLElement>;
  @ViewChild('description') description: ElementRef<HTMLElement>;
  @ViewChild('repName') repName: ElementRef<HTMLElement>;
  @ViewChild('business') business: ElementRef<HTMLElement>;
  @ViewChild('repPosition') repPosition: ElementRef<HTMLElement>;
  @ViewChild('repIdNo') repIdNo: ElementRef<HTMLElement>;
  @ViewChild('repIdDate') repIdDate: ElementRef<HTMLElement>;
  @ViewChild('repIdIssuer') repIdIssuer: ElementRef<HTMLElement>;
  @ViewChild('content') content: ElementRef<HTMLElement>;
  @ViewChild('links') links: ElementRef<HTMLElement>;
  @ViewChild('status') status: ElementRef<HTMLElement>;

  // Tab
  @ViewChild('tab1') tab1:ElementRef<HTMLElement>;
  @ViewChild('tab2') tab2:ElementRef<HTMLElement>;
  @ViewChild('contentTab1') contentTab1:ElementRef<HTMLElement>;
  @ViewChild('contentTab2') contentTab2:ElementRef<HTMLElement>;
  //
  idTabElements = ['contentTab1', 'contentTab2'];
  tabElements = ['tab1', 'tab2'];

  tabInfo = [
    {
      tab: 'tab1',
      idTab: 'contentTab1',
      inputName: ['name', 'code','email','nameEn','phone','mobile'],
    },
    {
      tab: 'tab2',
      idTab: 'contentTab2',
      inputName: [],
    }
  ]

  issuer: any = {
    "issuerId": 0,
    "code": "",
    "name": "",
    "nameEn": "",
    "shortName": "",
    "type": 1,
    "address": "",
    "phone": null,
    "fax": "",
    "mobile": null,
    "email": 'example@gmail.com',
    "taxCode": "",
    "bankAccNo": "",
    "bankName": "",
    "bankBranchName": "",
    "foundationNo": "",
    "foundationDate": null,
    "foundationIssuer": "",
    "licenseNo": "",
    "licenseDate": null,
    "licenseIssuer": "",
    "capital": null,
    "business": "",
    "description": "",
    "repName": "",
    "repPosition": "",
    "repIdNo": "",
    "repIdDate": null,
    "repIdIssuer": "",
    "content": "",
    "links": "",
    "status": null,
  };

  issuerFieldTypeDates = ['foundationDate','licenseDate', 'repIdDate'];
  issuerFieldDateDisplays = {
    'foundationDate': null,
    'licenseDate': null,
    'repIdDate' : null
  }
  issuerFieldCurrencyDisplays = {
    'capital': null,
  }

  types = [];
  title: string;
  statusConst = IssuerConst.status;
  typeConst = IssuerConst.types;

  ngOnInit(): void {

    console.log()
    if(this.issuer.issuerId) {
      this.isLoading = true
      this._issuerService.get(this.issuer.issuerId).subscribe(res => {
        this.issuer = res.data;
        // Format date output
        this.formatDateOutput(this.issuerFieldTypeDates, this.issuer, this.issuerFieldDateDisplays);
        this.formatCurrencyOutput(this.issuerFieldCurrencyDisplays, this.issuer );
        this.isLoading = false;
        console.log({ itemIssuer: res.data , display: this.issuerFieldCurrencyDisplays});
      });
    }
  }

  changeDatePicker(field, value) {
    console.log(value)
    if(value == 'Invalid Date') {
      this.triggerFieldError(field);
    } else {
      this.issuer[field] = value;
    }
  }

  changeCurrency(field, value) {
     this.issuerFieldCurrencyDisplays[field] = this.formatCurrency(value);
  }

  showTab(tab:string, idTab:string ) {
    for(let tab of this.tabElements) {
      let elTab: HTMLElement = this[tab].nativeElement;
      elTab.classList.remove('active');
    }
    let elTab: HTMLElement = this[tab].nativeElement;
    elTab.classList.add('active');
    //
    for(let idTab of this.idTabElements) {
      let elContentTab: HTMLElement = this[idTab].nativeElement;
      elContentTab.classList.remove('active');
    }
    let elContentTab: HTMLElement = this[idTab].nativeElement;
    elContentTab.classList.add('active');
  }

  triggerFieldError(name: string) {
      //
      let tabTrigger = this.tabInfo.find(item => item.inputName.find(field => field == name));
      if(tabTrigger) {
        this.showTab(tabTrigger.tab, tabTrigger.idTab);
      }

      let el: HTMLElement = this[name].nativeElement;
      el.focus(); // focus
      el.classList.add('is-invalid');
  }

  callTriggerFieldError(response) {
    let keyFirstNameError = this.getKeyFirstNameError(response);
    if(this.issuer[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  save(): void {
      this.saving = true;
      // Fomat date Input
      this.formatDateInput(this.issuerFieldTypeDates, this.issuer);
      this.formatCurrencyInput(this.issuerFieldCurrencyDisplays, this.issuer);
      const data = { ...this.issuer };
      if (!this.issuer.issuerId) {
        this._issuerService.create(data).subscribe(
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
        this._issuerService.update(data, this.issuer.issuerId).subscribe((response) => {
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
