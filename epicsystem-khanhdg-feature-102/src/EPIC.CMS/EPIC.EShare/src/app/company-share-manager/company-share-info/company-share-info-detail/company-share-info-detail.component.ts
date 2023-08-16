import { FormRequestComponent } from './../../../form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from './../../../form-request-approve-cancel/form-approve/form-approve.component';
import { Component, Injector, Input, ViewChild } from '@angular/core';
import { CompanySharePrimaryConst, CompanyShareInfoConst, SearchConst, UnitDateConst, AppConsts } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ContractTemplateServiceProxy, CompanyShareInfoServiceProxy, CompanySharePrimaryServiceProxy } from '@shared/service-proxies/company-share-manager-service';
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
import { FormCompanyShareInfoApproveComponent } from 'src/app/approve-manager/approve/form-company-share-info-approve/form-company-share-info-approve.component';

@Component({
	selector: 'app-company-share-info-detail',
	templateUrl: './company-share-info-detail.component.html',
	styleUrls: ['./company-share-info-detail.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService, MenuModule],
})
export class CompanyShareInfoDetailComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private routeActive: ActivatedRoute,
		private _companySharePrimaryService: CompanySharePrimaryServiceProxy,
		private _contractTemplateService: ContractTemplateServiceProxy,
		private _companyShareInfoService: CompanyShareInfoServiceProxy,
		private confirmationService: ConfirmationService,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
	) {
		super(injector, messageService);

		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Lô cổ phần', routerLink: ['/company-share-manager/company-share-info'] },
			{ label: 'Chi tiết lô cổ phần', },
		]);

		this.companyShareInfoId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	isEdit = false;
	fieldErrors = {};
	fieldDates = ['issueDate', 'dueDate'];
	


	companyShareInfoId: number;
	companyShareInfoDetail: any = {};
	labelButtonEdit = "Chỉnh sửa";

	deleteItemDialog: boolean = false;
	rows: any[] = [];
	actions: any[] = [];  // list button actions
	actionsDisplay: any[] = [];
	showActions = true;
	checkMarkdownType: boolean = true;
	checkHtmlType:boolean = false;

	iconDefault = "assets/layout/images/icon-default-company-share-info.svg";

	tabViewActive = {
		'thongTinChung': true,
		'taiSanDamBao': false,
		'hoSoPhapLy': false,
		'thongTinTraiTuc': false,
	  };
	
	@ViewChild(TabView) tabView: TabView;

	CompanyShareInfoConst = CompanyShareInfoConst;
	CompanySharePrimaryConst = CompanySharePrimaryConst;
	UnitDateConst = UnitDateConst;
	AppConsts = AppConsts;

	ngOnInit() {
		this.actions = [
			{
				label: 'Trình duyệt',
				icon: 'pi pi-arrow-up',
				statusActive: [CompanyShareInfoConst.KHOI_TAO],
				permission: this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_TrinhDuyet]),
				command: () => {
					this.request();
				}
			},
			// {
			// 	label: 'Phê duyệt',
			// 	icon: 'pi pi-check',
			// 	statusActive: [CompanyShareInfoConst.CHO_DUYET],
			// 	permission: this.isGranted(),
			// 	command: () => {
			// 		this.approve();
			// 	}
			// },
			{
				label: 'Phê duyệt (Epic)',
				icon: 'pi pi-check',
				statusActive: [CompanyShareInfoConst.HOAT_DONG],
				permission: this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_EpicXacMinh]),
				command: () => {
					this.check(this.companyShareInfoDetail);
				}
			},
			{
				label: 'Đóng cổ phần',
				icon: 'pi pi-lock',
				statusActive: [CompanyShareInfoConst.HOAT_DONG],
				permission:  this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_DongTraiPhieu]),
				command: () => {
					this.close(this.companyShareInfoDetail)
				}
			},
			{
				label: 'Mở cổ phần',
				icon: 'pi pi-key',
				statusActive: [CompanyShareInfoConst.DONG],
				permission:  this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_DongTraiPhieu]),
				command: () => {
					this.close(this.companyShareInfoDetail)
				}
			},
		];
		this.getDetail();
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
					this._companyShareInfoService.approve(dataCallBack.data).subscribe((response) => {
						if (this.handleResponseInterceptor(response, "Phê duyệt thành công!")) {
							setTimeout(() => {
								this.getDetail();
							}, 1000);
							
						}
					});
				}
				else {
					this._companyShareInfoService.cancel(dataCallBack.data).subscribe((response) => {
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
		this._companyShareInfoService.get(this.companyShareInfoId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, '')) {
					this.companyShareInfoDetail = res.data;
					this.companyShareInfoDetail.description;
					console.log({ companyShareInfoDetail: this.companyShareInfoDetail });
					this.companyShareInfoDetail = {
						...this.companyShareInfoDetail,
						issueDate: this.companyShareInfoDetail?.issueDate ? new Date(this.companyShareInfoDetail?.issueDate) : null,
						dueDate: this.companyShareInfoDetail?.dueDate ? new Date(this.companyShareInfoDetail?.dueDate) : null,
						isAllowSbd: this.companyShareInfoDetail?.isAllowSbd ?? CompanyShareInfoConst.QUESTION_NO,
					};
					this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.companyShareInfoDetail.status) && action.permission);
					console.log({ companyShareInfoDetail: this.companyShareInfoDetail });
				}
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			});
	}
	getActionsDisplay() {
		this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(this.companyShareInfoDetail.status) && action.permission);
	}

	request() {
		let title = "Trình duyệt", id = this.companyShareInfoDetail.companyShareId;
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC(title, id)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('-----------------------');
			if (dataCallBack?.accept) {
				this._companyShareInfoService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
						this.getDetail();
					}
				});
			}
		});
	}

	approve() {
		let title = "Phê duyệt", id = this.companyShareInfoDetail.companyShareId;
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC(title, id)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('-----------------------');
			if (dataCallBack?.accept) {
				this._companyShareInfoService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail();
					}
				});
			}
		});
	}
	

	check(companyShareInfoDetail) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô cổ phần này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._companyShareInfoService.check({ id: companyShareInfoDetail.companyShareId }).subscribe((response) => {

					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.getDetail();
					}
				});
			},
			reject: () => {
			},
		});
	}

	open(companyShareInfoDetail) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn mở cổ phần?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: companyShareInfoDetail?.id,
					closeNote: null,
				}
				this._companyShareInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Mở thành công")) {
						this.getDetail();
					}
				});
			},
			reject: () => {
			}
		});
	}

	close(companyShareInfoDetail) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn đóng cổ phần?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: companyShareInfoDetail?.id,
					closeNote: null,
				}
				this._companyShareInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
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
		for (const [key, value] of Object.entries(this.companyShareInfoDetail)) {
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
		if (value == this.CompanyShareInfoConst.INTEREST_RATE_TYPE_PERIOD_END) {
			this.companyShareInfoDetail.interestPeriod = null;
			this.companyShareInfoDetail.interestPeriodUnit = null;
		} else {
			this.companyShareInfoDetail.interestPeriodUnit = this.CompanyShareInfoConst.UNIT_DATE_YEAR;
		}
	}

	changePeriod(e, field = 'period') {
		if (field == 'period') this.companyShareInfoDetail.period = e.value;
		setTimeout(() => {
			this.changeCelldate();
		}, 0);
	}

	changeCelldate() {
		setTimeout(() => {
			let issueDate = this.companyShareInfoDetail?.issueDate ? new Date(this.companyShareInfoDetail?.issueDate) : null;
			let dueDate = this.companyShareInfoDetail?.dueDate ? new Date(this.companyShareInfoDetail?.dueDate) : null;

			if (issueDate && this.companyShareInfoDetail?.period && this.companyShareInfoDetail?.companySharePeriodUnit) {

				this.companyShareInfoDetail.dueDate = new Date(moment(issueDate).add(this.companyShareInfoDetail?.period, this.UnitDateConst.list[this.companyShareInfoDetail?.companySharePeriodUnit]).format("YYYY-MM-DD"));
			}

			if (issueDate && dueDate) {
				if (issueDate >= dueDate) {
					this.companyShareInfoDetail.dueDate = null;
				}
			}
		}, 100);
	}

	// Update Icon
	myUploader(event) {
		if (event?.files[0]) {
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "icon-company-share-info").subscribe((response) => {
				if (this.handleResponseInterceptor(response, "")) {
					this.companyShareInfoDetail.icon = response?.data;
				}
			}, (err) => {
				console.log('err-----', err);
				this.messageError("Có sự cố khi upload!");
			}
			);
		}
	}

	changeEdit() {
		console.log(this.companyShareInfoDetail);
		if (this.isEdit) {
			this.setTimeZoneList(this.fieldDates, this.companyShareInfoDetail);

			this._companyShareInfoService.update(this.companyShareInfoDetail).subscribe((response) => {
				this.callTriggerFiledError(response, this.fieldErrors);
				if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
					this.setStatusEdit();
					this.getDetail(false);
				} else {
					this.callTriggerFiledError(response, this.fieldErrors);
					this.resetTimeZoneList(this.fieldDates, this.companyShareInfoDetail);
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
