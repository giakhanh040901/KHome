import { FormRequestComponent } from './../../../form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from './../../../form-request-approve-cancel/form-approve/form-approve.component';
import { Component, Injector, Input, ViewChild } from '@angular/core';
import { ProductBondPrimaryConst, ProductBondInfoConst, SearchConst, UnitDateConst, AppConsts } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ContractTemplateServiceProxy, ProductBondInfoServiceProxy, ProductBondPrimaryServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { MenuModule } from 'primeng/menu';
import { ConfirmationService, MessageService } from 'primeng/api';
import { debounceTime } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { Page } from '@shared/model/page';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import * as moment from 'moment';
import { TabView } from 'primeng/tabview';
import { FormBondInfoApproveComponent } from 'src/app/approve-manager/approve/form-bond-info-approve/form-bond-info-approve.component';

@Component({
	selector: 'app-product-bond-info-detail',
	templateUrl: './product-bond-info-detail.component.html',
	styleUrls: ['./product-bond-info-detail.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService, MenuModule],
})
export class ProductBondInfoDetailComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private routeActive: ActivatedRoute,
		private _productBondPrimaryService: ProductBondPrimaryServiceProxy,
		private _contractTemplateService: ContractTemplateServiceProxy,
		private _productBondInfoService: ProductBondInfoServiceProxy,
		private confirmationService: ConfirmationService,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
	) {
		super(injector, messageService);

		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Lô trái phiếu', routerLink: ['/bond-manager/product-bond-info'] },
			{ label: 'Chi tiết lô trái phiếu', },
		]);

		this.productBondInfoId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	isEdit = false;
	fieldErrors = {};
	fieldDates = ['issueDate', 'dueDate'];
	


	productBondInfoId: number;
	productBondInfoDetail: any = {};
	labelButtonEdit = "Chỉnh sửa";

	deleteItemDialog: boolean = false;
	rows: any[] = [];
	actions: any[] = [];  // list button actions
	actionsDisplay: any[] = [];
	showActions = true;
	checkMarkdownType: boolean = true;
	checkHtmlType:boolean = false;

	iconDefault = "assets/layout/images/icon-default-bond-info.svg";

	tabViewActive = {
		'thongTinChung': true,
		'taiSanDamBao': false,
		'hoSoPhapLy': false,
		'thongTinTraiTuc': false,
	  };
	
	@ViewChild(TabView) tabView: TabView;

	ProductBondInfoConst = ProductBondInfoConst;
	ProductBondPrimaryConst = ProductBondPrimaryConst;
	UnitDateConst = UnitDateConst;
	AppConsts = AppConsts;

	ngOnInit() {
		this.actions = [
			{
				label: 'Trình duyệt',
				icon: 'pi pi-arrow-up',
				statusActive: [ProductBondInfoConst.KHOI_TAO],
				permission: this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_TrinhDuyet]),
				command: () => {
					this.request();
				}
			},
			// {
			// 	label: 'Phê duyệt',
			// 	icon: 'pi pi-check',
			// 	statusActive: [ProductBondInfoConst.CHO_DUYET],
			// 	permission: this.isGranted(),
			// 	command: () => {
			// 		this.approve();
			// 	}
			// },
			{
				label: 'Phê duyệt (Epic)',
				icon: 'pi pi-check',
				statusActive: [ProductBondInfoConst.HOAT_DONG],
				permission: this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_EpicXacMinh]),
				command: () => {
					this.check(this.productBondInfoDetail);
				}
			},
			{
				label: 'Đóng trái phiếu',
				icon: 'pi pi-lock',
				statusActive: [ProductBondInfoConst.HOAT_DONG],
				permission:  this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_DongTraiPhieu]),
				command: () => {
					this.close(this.productBondInfoDetail)
				}
			},
			{
				label: 'Mở trái phiếu',
				icon: 'pi pi-key',
				statusActive: [ProductBondInfoConst.DONG],
				permission:  this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_DongTraiPhieu]),
				command: () => {
					this.close(this.productBondInfoDetail)
				}
			},
		];
		this.getDetail();
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
					this._productBondInfoService.approve(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
							setTimeout(() => {
								this.getDetail();
							}, 1000);
							
						}
					});
				}
				else {
					this._productBondInfoService.cancel(dataCallBack.data).subscribe((response) => {
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

	changeTabview(e) {
		let tabHeader = this.tabView.tabs[e.index].header;
		this.tabViewActive[tabHeader] = true;
	}

	getDetail(isLoading = true) {
		this.isLoading = isLoading;
		this._productBondInfoService.get(this.productBondInfoId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, '')) {
					this.productBondInfoDetail = res.data;
					this.productBondInfoDetail.description;
					console.log({ productBondInfoDetail: this.productBondInfoDetail });
					this.productBondInfoDetail = {
						...this.productBondInfoDetail,
						issueDate: this.productBondInfoDetail?.issueDate ? new Date(this.productBondInfoDetail?.issueDate) : null,
						dueDate: this.productBondInfoDetail?.dueDate ? new Date(this.productBondInfoDetail?.dueDate) : null,
						allowSbd: this.productBondInfoDetail?.allowSbd ?? ProductBondInfoConst.QUESTION_NO,
					};
					this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.productBondInfoDetail.status) && action.permission);
					console.log({ productBondInfoDetail: this.productBondInfoDetail });
				}
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			});
	}
	getActionsDisplay() {
		this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.productBondInfoDetail.status) && action.permission);
	}

	request() {
		let title = "Trình duyệt", id = this.productBondInfoDetail.productBondId;
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC(title, id)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('-----------------------');
			if (dataCallBack?.accept) {
				this._productBondInfoService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
						this.getDetail();
					}
				});
			}
		});
	}

	approve() {
		let title = "Phê duyệt", id = this.productBondInfoDetail.productBondId;
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC(title, id)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('-----------------------');
			if (dataCallBack?.accept) {
				this._productBondInfoService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail();
					}
				});
			}
		});
	}
	

	check(productBondInfoDetail) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô trái phiếu này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._productBondInfoService.check({ id: productBondInfoDetail.productBondId }).subscribe((response) => {

					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail();
					}
				});
			},
			reject: () => {
			},
		});
	}

	open(productBondInfoDetail) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn mở trái phiếu?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: productBondInfoDetail?.id,
					closeNote: null,
				}
				this._productBondInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Mở thành công")) {
						this.getDetail();
					}
				});
			},
			reject: () => {
			}
		});
	}

	close(productBondInfoDetail) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn đóng trái phiếu?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: productBondInfoDetail?.id,
					closeNote: null,
				}
				this._productBondInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Đóng thành công")) {
						this.getDetail();
					}
				});
			},
			reject: () => {
			}
		});
	}

	delete() {
		this.deleteItemDialog = true;
	}

	setFieldError() {
		for (const [key, value] of Object.entries(this.productBondInfoDetail)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}

	setStatusEdit() {
		this.isEdit = !this.isEdit;
		this.labelButtonEdit = this.isEdit ? "Lưu lại" : "Chỉnh sửa";
		this.editorConfig.editable = this.isEdit;
	}

	changeInterestRateType(value) {
		if (value == this.ProductBondInfoConst.INTEREST_RATE_TYPE_PERIOD_END) {
			this.productBondInfoDetail.interestPeriod = null;
			this.productBondInfoDetail.interestPeriodUnit = null;
		} else {
			this.productBondInfoDetail.interestPeriodUnit = this.ProductBondInfoConst.UNIT_DATE_YEAR;
		}
	}

	changePeriod(e, field = 'bondPeriod') {
		if (field == 'bondPeriod') this.productBondInfoDetail.bondPeriod = e.value;
		setTimeout(() => {
			this.changeCelldate();
		}, 0);
	}

	changeCelldate() {
		setTimeout(() => {
			let issueDate = this.productBondInfoDetail?.issueDate ? new Date(this.productBondInfoDetail?.issueDate) : null;
			let dueDate = this.productBondInfoDetail?.dueDate ? new Date(this.productBondInfoDetail?.dueDate) : null;

			if (issueDate && this.productBondInfoDetail?.bondPeriod && this.productBondInfoDetail?.bondPeriodUnit) {

				this.productBondInfoDetail.dueDate = new Date(moment(issueDate).add(this.productBondInfoDetail?.bondPeriod, this.UnitDateConst.list[this.productBondInfoDetail?.bondPeriodUnit]).format("YYYY-MM-DD"));
			}

			if (issueDate && dueDate) {
				if (issueDate >= dueDate) {
					this.productBondInfoDetail.dueDate = null;
				}
			}
		}, 100);
	}

	// Update Icon
	myUploader(event) {
		if (event?.files[0]) {
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "icon-bond-info").subscribe((response) => {
				if (this.handleResponseInterceptor(response, "")) {
					this.productBondInfoDetail.icon = response?.data;
				}
			}, (err) => {
				console.log('err-----', err);
				this.messageError("Có sự cố khi upload!");
			}
			);
		}
	}

	changeEdit() {
		console.log(this.productBondInfoDetail);
		if (this.isEdit) {
			let body = this.formatCalendar(this.fieldDates, {...this.productBondInfoDetail});
			this._productBondInfoService.update(body).subscribe((response) => {
				this.callTriggerFiledError(response, this.fieldErrors);
				if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
					this.setStatusEdit();
					this.getDetail(false);
				}
			});
		} else {
			this.setStatusEdit();
		}
	}
	markdownType(){
		this.checkHtmlType = false;
		this.checkMarkdownType = true;

	}
	htmlType() {
		this.checkMarkdownType = false;
		this.checkHtmlType = true;
	}
}
