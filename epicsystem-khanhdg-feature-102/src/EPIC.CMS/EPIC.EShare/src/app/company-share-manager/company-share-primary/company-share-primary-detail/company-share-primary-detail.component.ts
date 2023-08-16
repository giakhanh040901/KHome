import { FormApproveComponent } from './../../../form-request-approve-cancel/form-approve/form-approve.component';
import { FormRequestComponent } from './../../../form-request-approve-cancel/form-request/form-request.component';
import { Component, Injector, Input } from '@angular/core';
import { CompanySharePrimaryConst, CompanyShareInfoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { CompanySharePrimaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { MenuModule } from 'primeng/menu';
import { FormCancelComponent } from 'src/app/form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormCompanyShareInfoApproveComponent } from 'src/app/approve-manager/approve/form-company-share-info-approve/form-company-share-info-approve.component';

@Component({
  selector: 'app-company-share-primary-detail',
  templateUrl: './company-share-primary-detail.component.html',
  styleUrls: ['./company-share-primary-detail.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService, MenuModule],

})
export class CompanySharePrimaryDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private routeActive: ActivatedRoute,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private _companySharePrimaryService: CompanySharePrimaryServiceProxy,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
  ) {
    super(injector, messageService);
    // this.companySharePrimaryDetail = this.dialogConfig.data.companySharePrimary;

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Phát hành sơ cấp', routerLink: ['/company-share-manager/company-share-primary'] },
      { label: 'Chi tiết phát hành sơ cấp', },
    ]);

    this.companySharePrimaryId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  fieldErrors: any = {};
  fieldDates = ['openCellDate', 'closeCellDate'];

  companySharePrimaryId: number;
  companySharePrimaryDetail: any = {};
  isEdit = false;
  labelButtonEdit = "Chỉnh sửa";

  rows: any[] = [];
  companyShareInfos: any = [];
  tradingProviders: any = [];
  companyShareInfoBanks: any[] = [];
  actions: any[] = [];
  actionsDisplay: any[] = [];
  deleteItemDialog: boolean = false;
  showActions = true;

  CompanyShareInfoConst = CompanyShareInfoConst;
  CompanySharePrimaryConst = CompanySharePrimaryConst;

  ngOnInit() {
    const T = 'Khởi tạo';
    const P = 'Trình duyệt';
    const A = 'Hoạt động';
    const C = 'Đóng';
    console.log("this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy",this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy);
    
    this.actions = [
      {
        label: 'Trình duyệt',
        icon: 'pi pi-arrow-up',
        statusActive: ['T'],
        permission: this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_TrinhDuyet]),
        command: () => {
          this.request();
        }
      },
      {
        label: 'Phê duyệt',
        icon: 'pi pi-check',
        statusActive: ['P'],
        permission: this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy]),
        command: () => {
          this.approve();
        }
      },
      {
        label: 'Hủy duyệt',
        icon: 'pi pi-times',
        statusActive: ['P'],
        permission: this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_PHSC_PheDuyetOrHuy]),
        command: () => {
          this.cancel();
        }
      },
      // {
      //   label: 'Phê duyệt (Epic)',
      //   icon: 'pi pi-check',
      //   statusActive: ['A'],
      //   permission: this.isGranted(),
      //   command: () => {
      //     this.check(this.companySharePrimaryDetail);
      //   }
      // },
      // {
      //   label: 'Đóng cổ phần',
      //   icon: 'pi pi-lock',
      //   statusActive: ['A'],
      //   permission: this.isGranted(),
      //   command: () => {
      //     this.close();
      //   }
      // },
    ];
    this.isLoading = true;

    this._companySharePrimaryService.get(this.companySharePrimaryId).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.companySharePrimaryDetail = res.data;
            this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.companySharePrimaryDetail.status) && action.permission);

            this.companySharePrimaryDetail = {
                ...this.companySharePrimaryDetail,
                openCellDate: this.companySharePrimaryDetail?.openCellDate ? new Date(this.companySharePrimaryDetail?.openCellDate) : null,
                closeCellDate: this.companySharePrimaryDetail?.closeCellDate ? new Date(this.companySharePrimaryDetail?.closeCellDate) : null,
            };

            if (this.companySharePrimaryDetail?.companyShareInfo?.issuer?.businessCustomer?.businessCustomerBanks) {
                let listBanks = this.companySharePrimaryDetail?.companyShareInfo?.issuer?.businessCustomer?.businessCustomerBanks ?? [];
                for (let bank of listBanks) {
                bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
                }
            }

            console.log({ companySharePrimaryDetail: this.companySharePrimaryDetail });
        }
    }, (err) => {
        this.isLoading = false;
        console.log('Error', err);
        
    });

    forkJoin([this._companySharePrimaryService.getAllCompanyShareInfo(this.page), this._companySharePrimaryService.getAllTradingProvider(this.page)]).subscribe(
      ([resCompanyShareInfo, resTradingProvider]) => {
        this.isLoading = false;
        if (this.handleResponseInterceptors([resCompanyShareInfo, resTradingProvider])) {
            this.companyShareInfos = resCompanyShareInfo.data?.items;
            this.tradingProviders = resTradingProvider.data?.items;
            console.log({ 'resCompanyShareInfo': resCompanyShareInfo, 'resTradingProvider': resTradingProvider });
        }
      }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
    });
  }

  approveCompanyShare(companyShareData) {
		console.log("companyShareDataaa",companyShareData);
		
		const ref = this.dialogService.open(
			FormCompanyShareInfoApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: companyShareData
			}
		);

		ref.onClose.subscribe((dataCallBack) => {

			if (dataCallBack?.accept) {
				
				if (dataCallBack.checkApprove == true) {
					this._companySharePrimaryService.approve(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
							setTimeout(() => {
								this.getDetail();
							}, 1000);
							
						}
					});
				}
				else {
					this._companySharePrimaryService.cancel(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Huỷ duyệt thành công!")) {
							setTimeout(() => {
								this.getDetail();
							}, 1000);
						}
					});
				}
			}
		});
	}

  getActionsDisplay() {
    this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.companySharePrimaryDetail.status) && action.permission);
  }

  request() {
    let title = "Trình duyệt", id = this.companySharePrimaryDetail.companySharePrimaryId;
    const ref = this.dialogService.open(
      FormRequestComponent,
      this.getConfigDialogServiceRAC(title, id)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._companySharePrimaryService.request(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
            this.getDetail();
          }
        });
      }
    });
  }

  getDetail() {
    this._companySharePrimaryService.get(this.companySharePrimaryId).subscribe(
      (res) => {
        this.isLoading = false;
        this.companySharePrimaryDetail = res.data;
        this.companySharePrimaryDetail = {
          ...this.companySharePrimaryDetail,
          licenseDate: this.companySharePrimaryDetail?.licenseDate ? new Date(this.companySharePrimaryDetail?.licenseDate) : null,
          decisionDate: this.companySharePrimaryDetail?.decisionDate ? new Date(this.companySharePrimaryDetail?.decisionDate) : null,
          dateModified: this.companySharePrimaryDetail?.dateModified ? new Date(this.companySharePrimaryDetail?.dateModified) : null,
        };

        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.companySharePrimaryDetail.status) && action.permission);
        console.log({ companySharePrimaryDetail: this.companySharePrimaryDetail });
      });
  }

  approve() {
    let title = "Phê duyệt", id = this.companySharePrimaryDetail.companySharePrimaryId;
    const ref = this.dialogService.open(
      FormApproveComponent,
      this.getConfigDialogServiceRAC(title, id)
    );
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._companySharePrimaryService.approve(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.getDetail();
          }
        });
      }
    });
  }

  cancel() {
    let title = "Huỷ duyệt", id = this.companySharePrimaryDetail.companySharePrimaryId;
    const ref = this.dialogService.open(
      FormCancelComponent,
      this.getConfigDialogServiceRAC(title, id)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._companySharePrimaryService.cancel(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
            this.getDetail();
          }
        });
      }
    });
  }

  check(companySharePrimaryDetail) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn phê duyệt phát hành sơ cấp này không?',
      header: 'Thông báo',
      acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
      icon: 'pi pi-check-circle',
      accept: () => {
        this._companySharePrimaryService.check({ id: companySharePrimaryDetail.companySharePrimaryId }).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.getDetail();
          }
        });
      },
      reject: () => {
      },
    });
  }

  // close() {
  //   this.confirmationService.confirm({
  //     message: 'Bạn có chắc chắn đóng phát hành sơ cấp này này?',
  //     header: 'Đóng phát hành sơ cấp!',
  //     icon: 'pi pi-exclamation-triangle',
  //     accept: () => {
  //       this._companySharePrimaryService.close(this.companySharePrimaryDetail.companySharePrimaryId).subscribe((response) => {
  //         if (this.handleResponseInterceptor(response, "Đóng thành công")) {
  //           this.getDetail();
  //         }
  //       });
  //     },
  //     reject: () => {

  //     },
  //   });
  // }

  setFieldError() {
    for (const [key, value] of Object.entries(this.companySharePrimaryDetail)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  changeOpenCellDate(value) {
    if (!this.companySharePrimaryDetail?.contractCode?.trim()) {
      this.companySharePrimaryDetail.contractCode = this.formatDate(value).split("/").join("") + '/MAHĐ/' + new Date().getTime();
    }
  }

  changeEdit() {
    console.log(this.companySharePrimaryDetail);
    if(this.isEdit) {
      this.setTimeZoneList(this.fieldDates, this.companySharePrimaryDetail);
      this._companySharePrimaryService.update(this.companySharePrimaryDetail).subscribe((response) => {
        this.callTriggerFiledError(response, this.fieldErrors);
        if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
          this.isEdit = !this.isEdit;
          this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
        } else {
          this.callTriggerFiledError(response, this.fieldErrors);
          this.resetTimeZoneList(this.fieldDates, this.companySharePrimaryDetail);
        }
      });
    } else {
      this.isEdit = !this.isEdit;
      this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
    }
  }
}

