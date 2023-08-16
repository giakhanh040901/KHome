import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppConsts, IssuerConst, SearchConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { IssuerServiceProxy } from '@shared/service-proxies/setting-service';
import { MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { NationalityConst } from '@shared/nationality-list';
@Component({
  selector: 'app-issuer-detail',
  templateUrl: './issuer-detail.component.html',
  styleUrls: ['./issuer-detail.component.scss']
})
export class IssuerDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,

    messageService: MessageService,
    private routeActive: ActivatedRoute,
    private issuerService: IssuerServiceProxy,
    private _contractTemplateService: ContractTemplateServiceProxy,
    private breadcrumbService: BreadcrumbService,
  ) {
    super(injector, messageService);

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Tổ chức phát hành', routerLink: ['/setting/issuer'] },
      { label: 'Chi tiết tổ chức phát hành', },
    ]);

    this.issuerId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  fieldErrors = {};
  fieldDates = ['licenseDate', 'decisionDate', 'dateModified'];

  issuerId: number;
  issuerDetail: any = {};
  isEdit = false;
  labelButtonEdit = "Chỉnh sửa";

  imageDefault = 'assets/layout/images/image-bg-default.jpg';

  IssuerConst = IssuerConst;
  NationalityConst = NationalityConst;
  AppConsts = AppConsts;

  ngOnInit(): void {
    this.isLoading = true;
    this.issuerService.getIssuer(this.issuerId).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.issuerDetail = res.data;
          this.issuerDetail = {
            ...this.issuerDetail,
            licenseDate: this.issuerDetail?.licenseDate ? new Date(this.issuerDetail?.licenseDate) : null,
            decisionDate: this.issuerDetail?.decisionDate ? new Date(this.issuerDetail?.decisionDate) : null,
            dateModified: this.issuerDetail?.dateModified ? new Date(this.issuerDetail?.dateModified) : null,
          };
          console.log({ issuerDetail: this.issuerDetail });
        }
      }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
      });
  }

  setFieldError() {
    for (const [key, value] of Object.entries(this.issuerDetail)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  // Update ảnh đại diện
  myUploader(event) {
    console.log(event);
    
    if (event?.files[0]) {

      this._contractTemplateService.uploadFileGetUrl(event?.files[0], "issuer").subscribe((response) => {
        if (this.handleResponseInterceptor(response, "")) {
          this.issuerDetail.businessCustomer.avatarImageUrl = response?.data;
        }
      }, (err) => {
        console.log('err-----', err);
        this.messageError("Có sự cố khi upload!");
      }
      );
    }
  }


  changeEdit() {
    console.log(this.issuerDetail);
    if (this.isEdit) {
      let body = this.formatCalendar(this.fieldDates, {...this.issuerDetail});
      this.issuerService.update(body).subscribe((response) => {
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.isEdit = !this.isEdit;
          this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        } 
      });
    } else {
      this.isEdit = !this.isEdit;
      this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
    }
  }
}
