import { FormApproveComponent } from './../../../form-request-approve-cancel/form-approve/form-approve.component';
import { FormRequestComponent } from './../../../form-request-approve-cancel/form-request/form-request.component';
import { Component, Injector, Input } from '@angular/core';
import { ProductBondPrimaryConst, ProductBondInfoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ProductBondPrimaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { Router } from "@angular/router"
import { ActivatedRoute } from '@angular/router';
import { DialogService } from 'primeng/dynamicdialog';
import { MenuModule } from 'primeng/menu';
import { FormCancelComponent } from 'src/app/form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormBondInfoApproveComponent } from 'src/app/approve-manager/approve/form-bond-info-approve/form-bond-info-approve.component';

@Component({
  selector: 'app-product-bond-primary-detail',
  templateUrl: './product-bond-primary-detail.component.html',
  styleUrls: ['./product-bond-primary-detail.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService, MenuModule],

})
export class ProductBondPrimaryDetailComponent extends CrudComponentBase {

  constructor(
    injector: Injector,
    private routeActive: ActivatedRoute,
    private dialogService: DialogService,
    private confirmationService: ConfirmationService,
    private _productBondPrimaryService: ProductBondPrimaryServiceProxy,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
  ) {
    super(injector, messageService);
    // this.bondPrimaryDetail = this.dialogConfig.data.productBondPrimary;

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Phát hành sơ cấp', routerLink: ['/bond-manager/product-bond-primary'] },
      { label: 'Chi tiết phát hành sơ cấp', },
    ]);

    this.bondPrimaryId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  fieldErrors: any = {};
  fieldDates = ['openCellDate', 'closeCellDate'];

  bondPrimaryId: number;
  bondPrimaryDetail: any = {};
  isEdit = false;
  labelButtonEdit = "Chỉnh sửa";

  rows: any[] = [];
  bondInfos: any = [];
  tradingProviders: any = [];
  bondInfoBanks: any[] = [];
  actions: any[] = [];
  actionsDisplay: any[] = [];
  deleteItemDialog: boolean = false;
  showActions = true;

  ProductBondInfoConst = ProductBondInfoConst;
  ProductBondPrimaryConst = ProductBondPrimaryConst;

  ngOnInit() {
    const T = 'Khởi tạo';
    const P = 'Trình duyệt';
    const A = 'Hoạt động';
    const C = 'Đóng';
    console.log("this.PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy",this.PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy);
    
    this.actions = [
      {
        label: 'Trình duyệt',
        icon: 'pi pi-arrow-up',
        statusActive: ['T'],
        permission: this.isGranted([this.PermissionBondConst.BondMenuQLTP_PHSC_TrinhDuyet]),
        command: () => {
          this.request();
        }
      },
      {
        label: 'Phê duyệt',
        icon: 'pi pi-check',
        statusActive: ['P'],
        permission: this.isGranted([this.PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy]),
        command: () => {
          this.approve();
        }
      },
      {
        label: 'Hủy duyệt',
        icon: 'pi pi-times',
        statusActive: ['P'],
        permission: this.isGranted([this.PermissionBondConst.BondMenuQLTP_PHSC_PheDuyetOrHuy]),
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
      //     this.check(this.bondPrimaryDetail);
      //   }
      // },
      // {
      //   label: 'Đóng trái phiếu',
      //   icon: 'pi pi-lock',
      //   statusActive: ['A'],
      //   permission: this.isGranted(),
      //   command: () => {
      //     this.close();
      //   }
      // },
    ];
    this.isLoading = true;

    this._productBondPrimaryService.get(this.bondPrimaryId).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
            this.bondPrimaryDetail = res.data;
            this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.bondPrimaryDetail.status) && action.permission);

            this.bondPrimaryDetail = {
                ...this.bondPrimaryDetail,
                openCellDate: this.bondPrimaryDetail?.openCellDate ? new Date(this.bondPrimaryDetail?.openCellDate) : null,
                closeCellDate: this.bondPrimaryDetail?.closeCellDate ? new Date(this.bondPrimaryDetail?.closeCellDate) : null,
            };

            if (this.bondPrimaryDetail?.productBondInfo?.issuer?.businessCustomer?.businessCustomerBanks) {
                let listBanks = this.bondPrimaryDetail?.productBondInfo?.issuer?.businessCustomer?.businessCustomerBanks ?? [];
                for (let bank of listBanks) {
                bank.labelName = bank.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
                }
            }

            console.log({ bondPrimaryDetail: this.bondPrimaryDetail });
        }
    }, (err) => {
        this.isLoading = false;
        console.log('Error', err);
        
    });

    forkJoin([this._productBondPrimaryService.getAllBondInfo(this.page), this._productBondPrimaryService.getAllTradingProvider(this.page)]).subscribe(
      ([resBondInfo, resTradingProvider]) => {
        this.isLoading = false;
        if (this.handleResponseInterceptors([resBondInfo, resTradingProvider])) {
            this.bondInfos = resBondInfo.data?.items;
            this.tradingProviders = resTradingProvider.data?.items;
            console.log({ 'resBondInfo': resBondInfo, 'resTradingProvider': resTradingProvider });
        }
      }, (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
        
    });
  }

  approveBond(bondData) {
		console.log("bondDataaa",bondData);
		
		const ref = this.dialogService.open(
			FormBondInfoApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: bondData
			}
		);

		ref.onClose.subscribe((dataCallBack) => {

			if (dataCallBack?.accept) {
				
				if (dataCallBack.checkApprove == true) {
					this._productBondPrimaryService.approve(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
							setTimeout(() => {
								this.getDetail();
							}, 1000);
							
						}
					});
				}
				else {
					this._productBondPrimaryService.cancel(dataCallBack.data).subscribe((response) => {
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
    this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.bondPrimaryDetail.status) && action.permission);
  }

  request() {
    let title = "Trình duyệt", id = this.bondPrimaryDetail.bondPrimaryId;
    const ref = this.dialogService.open(
      FormRequestComponent,
      this.getConfigDialogServiceRAC(title, id)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._productBondPrimaryService.request(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
            this.getDetail();
          }
        });
      }
    });
  }

  getDetail() {
    this._productBondPrimaryService.get(this.bondPrimaryId).subscribe(
      (res) => {
        this.isLoading = false;
        this.bondPrimaryDetail = res.data;
        this.bondPrimaryDetail = {
          ...this.bondPrimaryDetail,
          licenseDate: this.bondPrimaryDetail?.licenseDate ? new Date(this.bondPrimaryDetail?.licenseDate) : null,
          decisionDate: this.bondPrimaryDetail?.decisionDate ? new Date(this.bondPrimaryDetail?.decisionDate) : null,
          dateModified: this.bondPrimaryDetail?.dateModified ? new Date(this.bondPrimaryDetail?.dateModified) : null,
        };

        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.bondPrimaryDetail.status) && action.permission);
        console.log({ bondPrimaryDetail: this.bondPrimaryDetail });
      });
  }

  approve() {
    let title = "Phê duyệt", id = this.bondPrimaryDetail.bondPrimaryId;
    const ref = this.dialogService.open(
      FormApproveComponent,
      this.getConfigDialogServiceRAC(title, id)
    );
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._productBondPrimaryService.approve(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
            this.getDetail();
          }
        });
      }
    });
  }

  cancel() {
    let title = "Huỷ duyệt", id = this.bondPrimaryDetail.bondPrimaryId;
    const ref = this.dialogService.open(
      FormCancelComponent,
      this.getConfigDialogServiceRAC(title, id)
    );
    //
    ref.onClose.subscribe((dataCallBack) => {
      if (dataCallBack?.accept) {
        this._productBondPrimaryService.cancel(dataCallBack.data).subscribe((response) => {
          if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
            this.getDetail();
          }
        });
      }
    });
  }

  check(bondPrimaryDetail) {
    this.confirmationService.confirm({
      message: 'Bạn có chắc chắn phê duyệt phát hành sơ cấp này không?',
      header: 'Thông báo',
      acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
      icon: 'pi pi-check-circle',
      accept: () => {
        this._productBondPrimaryService.check({ id: bondPrimaryDetail.bondPrimaryId }).subscribe((response) => {
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
  //       this._productBondPrimaryService.close(this.bondPrimaryDetail.bondPrimaryId).subscribe((response) => {
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
    for (const [key, value] of Object.entries(this.bondPrimaryDetail)) {
      this.fieldErrors[key] = false;
    }
    console.log({ filedError: this.fieldErrors });
  }

  resetValid(field) {
    this.fieldErrors[field] = false;
  }

  changeOpenCellDate(value) {
    if (!this.bondPrimaryDetail?.contractCode?.trim()) {
      this.bondPrimaryDetail.contractCode = this.formatDate(value).split("/").join("") + '/MAHĐ/' + new Date().getTime();
    }
  }

  changeEdit() {
    console.log(this.bondPrimaryDetail);
    if(this.isEdit) {
      let body = this.formatCalendar(this.fieldDates, {...this.bondPrimaryDetail});
      this._productBondPrimaryService.update(body).subscribe((response) => {
        this.callTriggerFiledError(response, this.fieldErrors);
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

