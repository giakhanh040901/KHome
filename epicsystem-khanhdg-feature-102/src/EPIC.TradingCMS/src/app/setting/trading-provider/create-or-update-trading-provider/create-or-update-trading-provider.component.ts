import {
  Component,
  ElementRef,
  EventEmitter,
  Injector,
  Input,
  OnInit,
  Output,
  ViewChild,
} from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { IssuerConst } from "@shared/AppConsts";
import { TradingProviderServiceProxy } from "@shared/service-proxies/setting-service";
import { BsLocaleService } from "ngx-bootstrap/datepicker";
import { BsModalRef } from "ngx-bootstrap/modal";

@Component({
  selector: "app-create-or-update-trading-provider",
  templateUrl: "./create-or-update-trading-provider.component.html",
  styleUrls: ["./create-or-update-trading-provider.component.scss"],
})
export class CreateOrUpdateTradingProviderComponent extends AppComponentBase {
  constructor(
    injector: Injector,
    public bsModalRef: BsModalRef,
    private _tradingProviderService: TradingProviderServiceProxy,
    private _datePickerLocale: BsLocaleService
  ) {
    super(injector);
    this._datePickerLocale.use("vi"); // ngôn ngữ hiển thị Datepicker
  }

  saving = false;
  tradingProvider: any = {
    "tradingProviderId": 0,
    "code": "",
    "name": "",
    "nameEn": "",
    "shortName": "",
    "type": null,
    "address": "",
    "phone": null,
    "fax": "",
    "mobile": null,
    "email": "example@gmail.com",
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
    "capital": 0,
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

  tradingProviderFieldTypeDates = ['foundationDate','licenseDate', 'repIdDate'];
  tradingProviderDateDisplays = {
    'foundationDate': null,
    'licenseDate': null,
    'repIdDate' : null
  }

  tradingProviderCurrencyDisplays = {
    'capital': null,
  }

  types = [];
  title: string;
  statusConst = IssuerConst.status;
  depositProviderTypes = IssuerConst.types;


  @Output() onSave = new EventEmitter<any>();
  // Element Input
  @ViewChild('code') code: ElementRef<HTMLElement>;
  @ViewChild('name') name: ElementRef<HTMLElement>;
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
  @ViewChild('business') business: ElementRef<HTMLElement>;
  @ViewChild('description') description: ElementRef<HTMLElement>;
  @ViewChild('repName') repName: ElementRef<HTMLElement>;
  @ViewChild('repPosition') repPosition: ElementRef<HTMLElement>;
  @ViewChild('repIdNo') repIdNo: ElementRef<HTMLElement>;
  @ViewChild('repIdDate') repIdDate: ElementRef<HTMLElement>;
  @ViewChild('repIdIssuer') repIdIssuer: ElementRef<HTMLElement>;
  @ViewChild('content') content: ElementRef<HTMLElement>;
  @ViewChild('links') links: ElementRef<HTMLElement>;
  @ViewChild('status') status: ElementRef<HTMLElement>;


  //
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
      inputName: ['phone', 'mobile', 'email'],
    },
    {
      tab: 'tab2',
      idTab: 'contentTab2',
      inputName: [],
    }
  ]

  ngOnInit(): void {
    if (this.tradingProvider.tradingProviderId) {
      this.isLoading = true;
      this._tradingProviderService.get(this.tradingProvider.tradingProviderId).subscribe((res) => {
          this.tradingProvider = res.data;
          // Format date output
        this.formatDateOutput(this.tradingProviderFieldTypeDates, this.tradingProvider,this.tradingProviderDateDisplays);
        this.formatCurrencyOutput(this.tradingProviderCurrencyDisplays, this.tradingProvider );
        this.isLoading = false;
        console.log({ itemTradingProvider: res.data, display: this.tradingProviderFieldTypeDates });
        });
    }
  }

  changeDatePicker(field, value) {
    if (value == 'Invalid Date') {
      this.triggerFieldError(field);
    } else {
      this.tradingProvider[field] = value;
    }
  }

  changeCurrency(field, value) {
    this.tradingProviderCurrencyDisplays[field] = this.formatCurrency(value);
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
    if(this.tradingProvider[keyFirstNameError] !== undefined) {
      this.triggerFieldError(keyFirstNameError);
    }
  }

  save(): void {
    this.saving = true;
    // Fomat date Input
    this.formatDateInput(this.tradingProviderFieldTypeDates, this.tradingProvider);
    this.formatCurrencyInput(this.tradingProviderCurrencyDisplays, this.tradingProvider);
    const data = { ...this.tradingProvider };
    console.log(data);
    if (!this.tradingProvider.tradingProviderId) {
      this._tradingProviderService.create(data).subscribe(
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
      this._tradingProviderService.update(data, this.tradingProvider.tradingProviderId).subscribe((response) => {
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
