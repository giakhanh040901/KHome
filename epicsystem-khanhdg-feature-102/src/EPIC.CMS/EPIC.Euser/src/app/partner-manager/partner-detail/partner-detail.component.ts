import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PartnerConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { NationalityConst } from '@shared/nationality-list';
import { PartnerServiceProxy } from '@shared/service-proxies/partner-service';
import { Location } from '@angular/common'

import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'app-partner-detail',
  templateUrl: './partner-detail.component.html',
  styleUrls: ['./partner-detail.component.scss']
})
export class PartnerDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    messageService: MessageService,
    private location: Location,
    private routeActive: ActivatedRoute,
    private partnerService: PartnerServiceProxy,
    private breadcrumbService: BreadcrumbService
  ) { 
    super(injector, messageService);

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Danh sách tài khoản', routerLink: ['/user/list']  },
      { label: 'Thông tin tài khoản',},
    ]);

    this.partnerId = +this.routeActive.snapshot.paramMap.get('id');
  }

  fieldErrors = {};
  fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];

  partnerId: number;
  partnerDetail: any = {
    capital: 0
  };
  isEdit = false;
  labelButtonEdit = "Chỉnh sửa";

  PartnerConst=PartnerConst;
  NationalityConst = NationalityConst;

  ngOnInit(): void {
    this.isLoading = true;
    this.partnerService.get(this.partnerId).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.partnerDetail = res.data;
            this.partnerDetail = {
              ...this.partnerDetail,
              licenseDate: this.partnerDetail?.licenseDate ? new Date(this.partnerDetail?.licenseDate): null,
              decisionDate: this.partnerDetail?.decisionDate ? new Date(this.partnerDetail?.decisionDate): null,
              dateModified: this.partnerDetail?.dateModified ? new Date(this.partnerDetail?.dateModified): null,
            };
        }
    }, (err) => {
        this.isLoading = false;
    });
  }

  setFieldError() {
    for (const [key, value] of Object.entries(this.partnerDetail)) {
      this.fieldErrors[key] = false;
    }
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  changeEdit() {
    if(this.isEdit) {
      this.setTimeZoneList(this.fieldDates, this.partnerDetail);
      this.partnerService.update(this.partnerDetail).subscribe((response) => {
        this.callTriggerFiledError(response, this.fieldErrors);
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.isEdit = !this.isEdit;
          this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        } else {
          this.callTriggerFiledError(response, this.fieldErrors);
          this.resetTimeZoneList(this.fieldDates, this.partnerDetail);
        }
      });
    } else {
      this.isEdit = !this.isEdit;
      this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
    }
  }
}
