import { Component, ElementRef, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductBondInterestConst } from '@shared/AppConsts';
import { Page } from '@shared/model/page';
import { ProductBondInterestServiceProxy, ProductPolicyServiceProxy } from '@shared/service-proxies/setting-service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-create-or-update-product-bond-interest',
  templateUrl: './create-or-update-product-bond-interest.component.html',
  styleUrls: ['./create-or-update-product-bond-interest.component.scss']
})
export class CreateOrUpdateProductBondInterestComponent extends AppComponentBase {

  constructor(
    injector: Injector,
    private _datePickerLocale: BsLocaleService,
    public bsModalRef: BsModalRef,
    private _productBondInterestService: ProductBondInterestServiceProxy,
    private _productPolicyService: ProductPolicyServiceProxy,
  ) {
    super(injector);
    this._datePickerLocale.use('vi'); // ngôn ngữ hiển thị Datepicker
  }

  @Output() onSave = new EventEmitter<any>();

  // Element Input
  @ViewChild('bondInterestId') bondInterestId: ElementRef<HTMLElement>;
  @ViewChild('bondDetailId') bondDetailId: ElementRef<HTMLElement>;
  @ViewChild('interestRate') interestRate: ElementRef<HTMLElement>;
  @ViewChild('interestPeriod') interestPeriod: ElementRef<HTMLElement>;
  @ViewChild('interestPeriodUnit') interestPeriodUnit: ElementRef<HTMLElement>;

  productBondInterest: any = {
    "bondInterestId": 0,
    "interestRate": null,
    "interestPeriod": null,
    "interestPeriodUnit": null,
    "feeRate": null,
    "description": null,
  };

  types = [];
  title: string;
  interestPeriodUnits = ProductBondInterestConst.periodUnits;

  //DATA
  page = new Page();
  productPolicies:any = [];

  ngOnInit(): void {
    this._productPolicyService.getAll(this.page).subscribe((resProductPolicies) => {
        this.productPolicies = resProductPolicies.data?.items;
    });

    if (this.productBondInterest.bondInterestId) {
      this.isLoading = true
      this._productBondInterestService.get(this.productBondInterest.bondInterestId).subscribe(res => {
        this.productBondInterest = res.data;
        this.isLoading = false;
      });
    }
  }

  save(): void {
    this.saving = true;
    const data = { ...this.productBondInterest };
    if (!this.productBondInterest.bondInterestId) {
      this._productBondInterestService.create(data).subscribe(
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
      this._productBondInterestService.update(data, this.productBondInterest.bondInterestId).subscribe((response) => {
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
