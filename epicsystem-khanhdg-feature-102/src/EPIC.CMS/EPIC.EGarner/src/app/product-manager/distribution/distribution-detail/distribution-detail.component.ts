import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DialogService } from 'primeng/dynamicdialog';
import { ActivatedRoute, Router } from '@angular/router';
import { OBJECT_PRODUCT } from '@shared/base-object';
import { DistributionService } from '@shared/services/distribution.service';
import { DistributionConst, FormNotificationConst, ProductConst, YesNoConst } from '@shared/AppConsts';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { FormRequestComponent } from 'src/app/form-general/form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from 'src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component';
import { TabView } from 'primeng/tabview';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-distribution-detail',
  templateUrl: './distribution-detail.component.html',
  styleUrls: ['./distribution-detail.component.scss']
})
export class DistributionDetailComponent extends CrudComponentBase {

  constructor(
		_injector: Injector,
		_messageService: MessageService,
		private _router: Router,
		private _routeActive: ActivatedRoute,
		private _breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
		private _confirmationService: ConfirmationService,
		private _distributionService: DistributionService,
	) {
		super(_injector, _messageService);
		this._breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Phân phối sản phẩm" }
		]);

		this.distributionId = +this.cryptDecode(this._routeActive.snapshot.paramMap.get('id'));
	}

	distributionId: number;
	distributionDetail: any = {
		"openCellDate": null,
		"closeCellDate": null,
  	};

	productDetail: any;

	listBanks: any[] = [];
	ProductConst = ProductConst;
	DistributionConst = DistributionConst;
	FormNotificationConst = FormNotificationConst;
	YesNoConst = YesNoConst;
	activeIndex = 0;
	isEdit: boolean = false;

	fieldDates = ['openCellDate', 'closeCellDate', 'startDate', 'endDate'];

	interfaceShare = {...OBJECT_PRODUCT.GENERAL,...OBJECT_PRODUCT.SHARE};
	distributionUpdate = {
		"id": 0,
		"productId": null,
		"tradingBankAccountCollects" : null,
		"tradingBankAccountPays": null,
		"openCellDate": null,
		"closeCellDate": null,
	}

	ngOnInit() {
		this.isLoadingPage = true;
		forkJoin([this._distributionService.getById(this.distributionId), this._distributionService.getBankList()]).subscribe(([res, resBank]) => {
			this.isLoadingPage = false;
			if(this.handleResponseInterceptor(res) && res.data) {
				this.distributionDetail = {
					...res.data,
					openCellDate: new Date(res.data?.openCellDate),
					closeCellDate: new Date(res.data?.closeCellDate),
					remainAmount: (res.data?.garnerProduct?.invTotalInvestment || (res.data?.garnerProduct?.cpsQuantity * res.data?.garnerProduct?.cpsParValue)) - res.data?.isInvested,
				};
				//
				this.distributionDetail.remainQuantity = this.distributionDetail.remainAmount / res.data?.garnerProduct?.cpsParValue;
				//
				this.productDetail = this.distributionDetail?.garnerProduct;
				this.productDetail = this.formatCalendarDisplay(this.fieldDates, this.productDetail);
				console.log("this.productDetail: ", this.productDetail);
			}
			//
			if(this.handleResponseInterceptor(resBank, '')) {
				if(resBank?.data?.length) {
					this.listBanks = resBank.data.map(bank => {
						bank.labelName = bank?.bankAccNo + ' - ' + bank.bankAccName + ' - ' + bank.bankName;
						return bank;
					});

					console.log('listBank___', this.listBanks);
					
				}
			}
		}, (err) => {
			this.isLoadingPage = false;
			this.messageError('Có lỗi khi lấy dữ liệu');

		});
	}

	tabViewActive = {
		'thongTinChung': true,
		'tongQuan': false,
		'bangGia': false,
		'chinhSach': false,
		'fileChinhSach': false,
		'mauHopDong': false,
		// 'hopDongPhanPhoi': false,
		'mauGiaoNhanHD': false,
	};
	
	@ViewChild(TabView) tabView: TabView;

	labelButtonEdit() {
		return this.isEdit ? 'Lưu lại' : 'Chỉnh sửa';
	}

	getDetail() {
		this.isLoading = true;
		this._distributionService.getById(this.distributionId).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) && res.data) {
				this.distributionDetail = {
					...res.data,
					openCellDate: new Date(res.data?.openCellDate),
					closeCellDate: new Date(res.data?.closeCellDate),
					remainAmount: (res.data?.garnerProduct?.invTotalInvestment || (res.data?.garnerProduct?.cpsQuantity * res.data?.garnerProduct?.cpsParValue)) - res.data?.isInvested,
				};
				//
				console.log('logRemainAmount', res.data?.garnerProduct?.invTotalInvestment, (res.data?.garnerProduct?.cpsQuantity * res.data?.garnerProduct?.cpsParValue), res.data?.isInvested);
				//
				this.productDetail = this.distributionDetail?.garnerProduct;
				this.productDetail = this.formatCalendarDisplay(this.fieldDates, this.productDetail);
			}
		});
	}

	setStatusEdit() {
        this.isEdit = !this.isEdit;
    }

	changeEdit() {
        if (this.isEdit) {
            let body = this.formatCalendar(this.fieldDates, 
				{...this.filterField(this.distributionDetail, this.distributionUpdate)}
			);
            this._distributionService.update(body).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.setStatusEdit();
					this.getDetail();
                } 
            });
        } else {
            this.setStatusEdit();
        }
    }

	approveEpic(distributionId) {

			const ref = this._dialogService.open(
					FormNotificationComponent,
					{
						header: "Thông báo",
						width: '600px',
						contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
						styleClass: 'p-dialog-custom',
						baseZIndex: 10000,
						data: {
							title : "Bạn có chắc chắn EPIC duyệt phân phối sản phẩm này?",
							icon: FormNotificationConst.IMAGE_APPROVE,
						},
					}
				);
			ref.onClose.subscribe((dataCallBack) => {
				if (dataCallBack?.accept) {
				var	body = {
						id : distributionId,
					}
				this._distributionService.approveEpic(body).subscribe((response) => {
				  if (
					this.handleResponseInterceptor(
					  response,
					  "Phê duyệt thành công"
					)
				  ) {
					this.getDetail();
				  }
				});
				} else { 
				}
	
			});
		  
	}

	request(distributionId) {
		const params = {
			id: distributionId,
			summary: ProductConst.getTypeName(this.productDetail?.productType) + ': ' + this.productDetail?.code + ' - ' + this.productDetail?.name + ' ( ID: ' + this.productDetail?.id + ' )',
		}
		const ref = this._dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._distributionService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.getDetail();
					}
				});
			}
		});
	}

	approveSharing(distributionDetail) {
		console.log("investoraaaaaaa",distributionDetail);
		
		const ref = this._dialogService.open(
			FormApproveComponent,
			{
				header: "Phê duyệt",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					id: distributionDetail.id
				},
			}
		);
		console.log("abc", distributionDetail);

		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				const body = {
					approveNote: dataCallBack?.data?.approveNote,
					id: distributionDetail.id,
					cancelNote: dataCallBack?.data?.approveNote,
				}
				if ( dataCallBack?.checkApprove == true) {
					this._distributionService.approve(body).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
							this.getDetail();
						}
					});
				} else if (dataCallBack?.checkApprove == false) {
					this._distributionService.cancel(body).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
							this.getDetail();
						}
					});
				}
			}
		});
		
	}

	changeTab(e) {
		let tabHeader = this.tabView.tabs[e.index].header;
		this.tabViewActive[tabHeader] = true;
	}

}
