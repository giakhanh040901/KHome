import { FormRequestComponent } from './../../form-request-approve-cancel/form-request/form-request.component';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { CompanyShareInfoConst, CompanyShareDetailConst, SearchConst, KeyFilter, UnitDateConst, YesNoConst, CompanySharePolicyDetailTemplateConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { CompanyShareInfoServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { MessageService, ConfirmationService, ConfirmEventType } from 'primeng/api';
import { forkJoin } from 'rxjs';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { DialogService } from 'primeng/dynamicdialog';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs/operators';
import * as moment from 'moment';
import { FormApproveComponent } from 'src/app/form-request-approve-cancel/form-approve/form-approve.component';
import { FormCancelComponent } from 'src/app/form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormCloseComponent } from 'src/app/form-request-approve-cancel/form-close/form-close.component';
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { Calendar } from 'primeng/calendar';
import { ProgressSpinner } from 'primeng/progressspinner';
import {InputTextModule} from 'primeng/inputtext';
@Component({
	selector: 'app-company-share-info',
	templateUrl: './company-share-info.component.html',
	styleUrls: ['./company-share-info.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class CompanyShareInfoComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _companyShareInfoService: CompanyShareInfoServiceProxy,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private router: Router,
		private confirmationService: ConfirmationService,

	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Lô cổ phần' },
		]);
	}
	showplaceholderIssueDate:any = false;

	statusSearch: any[] = [
		{
			name: 'Tất cả ',
			code: ''
		},
		...CompanyShareInfoConst.statusConst
	];

	isCheckSearch: any[] = [
		{
			name: 'Tất cả',
			code: ''
		},
		...CompanyShareInfoConst.isCheckConst
	];

	CompanyShareInfoConst = CompanyShareInfoConst;
	CompanySharePolicyDetailTemplateConst = CompanySharePolicyDetailTemplateConst;
	UnitDateConst = UnitDateConst;
	YesNoConst = YesNoConst;
	isCheck:string;
	KeyFilter = KeyFilter;
	expandedRows = {};

	modalDialog: boolean;

	deleteItemDialog: boolean = false;

	deleteItemsDialog: boolean = false;
	rows: any[] = [];

	issueDate:any = null;
	dueDate:any = null;
	companyShareInfo: any = {};

	fieldErrors = {};

	fieldDates = ['issueDate', 'dueDate'];

	submitted: boolean;
	cols: any[];
	companyShareInfoConst = CompanyShareInfoConst;
	page = new Page();
	offset = 0;
	issuers: any = [];
	depositProviders: any = [];
	companyShareTypes: any = [];
	companyShareInfos: any = [];

	isInit = false;
	dueDateInput :any;
	//
	issuer = {};
	depositProvider = {};
	listAction: any[] = [];

	// Menu otions thao 
	actions: any[] = [];
	actionsDisplay: any[] = [];

	row: any;
	col: any;
	_selectedColumns: any[];
	ngOnInit() {
		this.status = '';
		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		//
		this.isLoading = true;
		forkJoin([this._companyShareInfoService.getAllIssuer(this.page), this._companyShareInfoService.getAllDepositProvider(this.page)]).subscribe(
			([resIssuer, resDepositProvider]) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(resIssuer, '')) {
					this.issuers = resIssuer?.data?.items ?? [];
				}
				if (this.handleResponseInterceptor(resDepositProvider, '')) {
					this.depositProviders = resDepositProvider?.data?.items ?? [];
				}
				this.setPage({ page: this.offset });
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
			});

		this.cols = [
			{ field: 'cpsCode', header: 'Mã CP', width: '10rem', cutText: 'b-cut-text-10', isPin: true },
			{ field: 'nameIssuer', header: 'TCPH', width: '16rem', cutText: 'b-cut-text-16', isPin: true },
			// { field: 'nameDeposit', header: 'ĐLLK', width: '16rem', cutText: 'b-cut-text-16' },
			{ field: 'period', header: 'Kỳ hạn', width: '10rem', cutText: 'b-cut-text-10' },
			{ field: 'interestPeriod', header: 'Kỳ trả', width: '10rem', cutText: 'b-cut-text-10' },
			{ field: 'tongGiaTri', header: 'Tổng giá trị', width: '12rem', class: 'text-right justify-content-end', cutText: 'b-cut-text-12' },
			{ field: 'quantity', header: 'SL phát hành', width: '12rem', class: 'text-right justify-content-end', cutText: 'b-cut-text-12' },
			{ field: 'soLuongConLai', header: 'SL còn lại', width: '10rem', class: 'text-right justify-content-end', cutText: 'b-cut-text-10' },
			{ field: 'issueDate', header: 'Ngày phát hành', width: '10rem', cutText: 'b-cut-text-10' },
			{ field: 'dueDate', header: 'Ngày đáo hạn', width: '10rem', cutText: 'b-cut-text-10' },
			{ field: 'isCheck', header: 'Kiểm tra', width: '6rem', class: 'justify-content-center', cutText: 'b-cut-text-10' },
		];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		})

		this._selectedColumns = this.getLocalStorage('companyShareInfo') ?? this.cols;
	}
	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('companyShareInfo', JSON.stringify(data));
	}


	setColumn(col, _selectedColumns) {
		console.log('cols:', col);

		console.log('_selectedColumns', _selectedColumns);

		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns);
				console.log('Luu o local', this._selectedColumns);
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.nameIssuer = row?.issuer?.businessCustomer?.shortName,
			row.nameDeposit = row?.depositProvider?.businessCustomer?.shortName,
			row.interestPeriod = (row?.interestPeriod && row?.interestPeriodUnit) ? (row?.interestPeriod + ' ' + this.CompanyShareInfoConst.getUnitDates(row?.interestPeriodUnit)) : null,
			row.period = (row?.period && row?.periodUnit) ? (row?.period + ' ' + this.CompanyShareInfoConst.getUnitDates(row?.periodUnit)) : null,
			row.tongGiaTri = this.utils.transformMoney(row?.quantity * row?.parValue),
			row.quantity = this.utils.transformMoney(row?.quantity),
			row.soLuongConLai = this.utils.transformMoney(row?.soLuongConLai),
			row.issueDate = this.formatDate(row?.issueDate);
			row.dueDate = this.formatDate(row?.dueDate);
		}
		console.log('row', rows);
	}

	genListAction(data = []) {
		this.listAction = data.map(companyShareInfoItem => {
			const actions = [];

			if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_TTCT])){
				actions.push({
					data: companyShareInfoItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}
			
			if (companyShareInfoItem.status == this.companyShareInfoConst.KHOI_TAO && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_Xoa])) {
				actions.push({
					data: companyShareInfoItem,
					label: 'Xoá',
					icon: 'pi pi-trash',
					command: ($event) => {
						this.delete($event.item.data);
					}
				});
			}

			if (companyShareInfoItem.status == this.companyShareInfoConst.KHOI_TAO && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_TrinhDuyet])) {
				actions.push({
					data: companyShareInfoItem,
					label: 'Trình duyệt',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.request($event.item.data);
					}
				});
			}

			if (companyShareInfoItem.status == this.companyShareInfoConst.HOAT_DONG && companyShareInfoItem.isCheck == false && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_EpicXacMinh])) {
				actions.push({
					data: companyShareInfoItem,
					label: 'Phê duyệt (Epic)',
					icon: 'pi pi-check',
					command: ($event) => {
						this.check($event.item.data);
					}
				});
			}

			if (companyShareInfoItem.status == this.companyShareInfoConst.HOAT_DONG && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_DongTraiPhieu])) {
				actions.push({
					data: companyShareInfoItem,
					label: 'Đóng cổ phần',
					icon: 'pi pi-lock',
					command: ($event) => {
						this.close($event.item.data);
					}
				});
			}

			if (companyShareInfoItem.status == this.companyShareInfoConst.DONG && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_LTP_DongTraiPhieu])) {
				actions.push({
					data: companyShareInfoItem,
					label: 'Mở cổ phần',
					icon: 'pi pi-key',
					command: ($event) => {
						this.open($event.item.data);
					}
				});
			}

			return actions;
		});
	}

	detail(companyShareInfo) {
		console.log('companyShareInfo: ', companyShareInfo);
		
		this.router.navigate(['/company-share-manager/company-share-info/detail/' + this.cryptEncode(companyShareInfo?.id)]);
	}

	request(companyShareInfo) {
		const summary = 'Lô TP: ' + companyShareInfo?.companyShareCode + ' - ' + companyShareInfo?.cpsName + ' ( ID: ' + companyShareInfo.companyShareId + ' )';
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", companyShareInfo.companyShareId, summary)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);

			if (dataCallBack?.accept) {
				this._companyShareInfoService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage();
					}
				});
			}
		});
	}

	approve(companyShareInfo) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", companyShareInfo?.companyShareId)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._companyShareInfoService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	cancel(companyShareInfo) {
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Hủy phê duyệt", companyShareInfo?.companyShareId)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._companyShareInfoService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	close(companyShareInfo) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn đóng cổ phần?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: companyShareInfo?.companyShareId,
					closeNote: null,
				}
				this._companyShareInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Đóng thành công")) {
						this.isLoading = true;
						this.setPage();
					}
				});
			},
			reject: () => {

			}
		});
	}

	open(companyShareInfo) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn mở cổ phần?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: companyShareInfo?.companyShareId,
					closeNote: null,
				}
				this._companyShareInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Mở thành công")) {
						this.isLoading = true;
						this.setPage();
					}
				});
			},
			reject: () => {

			}
		});
	}

	// Api Epic kiểm tra
	check(companyShareInfo) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô cổ phần này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._companyShareInfoService.check({ id: companyShareInfo?.companyShareId }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			},
			reject: () => {
			},
		});
	}

	changeIssuer(issuerId) {
		this.issuer = this.issuers.find(item => item.issuerId == issuerId);
		console.log({ issuer: this.issuer, value: issuerId });
	}

	changeDepositProvider(depositProviderId) {
		this.depositProvider = this.depositProviders.find(item => item.depositProviderId == depositProviderId);
		console.log({ issuer: this.depositProvider, value: depositProviderId });
	}

	setFieldError() {
		for (const [key, value] of Object.entries(this.companyShareInfo)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}
	create() {
		this.companyShareInfo = {};
		this.companyShareInfo.isAllowSbd = this.companyShareInfoConst.QUESTION_NO;
		this.companyShareInfo.interestPeriodUnit = this.companyShareInfoConst.UNIT_DATE_YEAR;
		this.submitted = false;
		this.modalDialog = true;
	}

	changeInterestRateType(value) {
		if (value == this.companyShareInfoConst.INTEREST_RATE_TYPE_PERIOD_END) {
			this.companyShareInfo.interestPeriod = null;
			this.companyShareInfo.interestPeriodUnit = null;
		} else {
			this.companyShareInfo.interestPeriodUnit = this.companyShareInfoConst.UNIT_DATE_YEAR;
		}
	}

	deleteSelectedItems() {
		this.deleteItemsDialog = true;
	}

	edit() {
		this.companyShareInfo = {
			...this.companyShareInfo,
			issueDate: this.companyShareInfo?.issueDate ? new Date(this.companyShareInfo?.issueDate) : null,
			dueDate: this.companyShareInfo?.dueDate ? new Date(this.companyShareInfo?.dueDate) : null,
			isAllowSbd: this.companyShareInfo?.isAllowSbd ?? CompanyShareInfoConst.QUESTION_NO,
		};
		this.modalDialog = true;
	}

	delete(companyShareInfo) {
		this.confirmationService.confirm({
			message: `Bạn có chắc chắn xóa lô TP : ${companyShareInfo?.cpsName} này?`,
			header: 'Thông báo',
			icon: 'pi pi-times-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy',
			accept: () => {
				this._companyShareInfoService.delete(companyShareInfo?.id).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
							this.setPage({ page: this.page.pageNumber });
							this.companyShareInfo = {};
						}
					}, () => {
						this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!', life: 1500 });
					}
				);
			},
			reject: () => {

			},
		});
	}

	changePriceOrQuantity(value, field) {

		if (this.companyShareInfo[field]) {
			this.companyShareInfo.totalValue = value * this.companyShareInfo[field];
		}
	}
	showPlaceHolderIssueDate(){
		this.showplaceholderIssueDate =true;
		
	}
	changeIssueDate(){
		this.showplaceholderIssueDate = true;
		this.isLoading = true;
		this.setPage({ Page: this.offset })
		
	}
	disablePlaceHolderIssueDate(){
		this.showplaceholderIssueDate = false;
	}
	changeDueDate(){

		this.showplaceholderIssueDate = true;
		this.isLoading = true;
		this.setPage({ Page: this.offset })
	}

	changeStatus() {
		this.setPage({ Page: this.offset })
	}
	changeInput(value){
		console.log("giá trị nhập vào ", value);
		
	}
	changeIsCheck() {
		this.setPage({ Page: this.offset })
	}
	
	changeFilter(){
		
		
		this.setPage({ Page: this.offset })
	}

	setPage(pageInfo?: any) {
		if(this.issueDate){
			var issueDate = moment(this.issueDate).format('YYYY-MM-DD');
		}
		if(this.dueDate){
			var dueDate= moment(this.dueDate).format('YYYY-MM-DD');
		}
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

		this.page.keyword = this.keyword;
		this._companyShareInfoService.getAllCompanyShareInfo(this.page, this.status, this.isCheck,issueDate,dueDate ).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				console.log("helllo lo trai phieu ", res.data);
				if (res.data?.items) {
					this.rows = res.data.items.map(row => {
						row.isCheck = (row.isCheck == this.YesNoConst.STATUS_YES);
						return row;
					});
				}
				if(this.rows?.length) {
					this.genListAction(this.rows);
					this.showData(this.rows)
				}
				console.log("hello", this.rows);
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			
		});
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}


	save() {
		// this.submitted = true;
		console.log('hello----1', this.companyShareInfo.issueDate);
		this.setTimeZoneList(this.fieldDates, this.companyShareInfo);
		if (this.companyShareInfo.companyShareId) {
			this._companyShareInfoService.update(this.companyShareInfo).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
						this.submitted = false;
						this.hideDialog();

					} else {
						this.callTriggerFiledError(response, this.fieldErrors);
						this.resetTimeZoneList(this.fieldDates, this.companyShareInfo);
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
				}
			);
		} else {
			console.log("hello test", this.companyShareInfo.issueDate);
			this._companyShareInfoService.create(this.companyShareInfo).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
						this.submitted = false;
						this.hideDialog();
						console.log(response);
						this.isLoadingPage = true;
						setTimeout(() => {
							this.router.navigate(['/company-share-manager/company-share-info/detail', this.cryptEncode(response.data.id)]);
						}, 1000);
					} else {
						this.callTriggerFiledError(response, this.fieldErrors);
						this.resetTimeZoneList(this.fieldDates, this.companyShareInfo);
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
				}
			);
		}
	}

	changePeriod(e, field = 'period') {
		if (field == 'period') this.companyShareInfo.period = e.value;
		setTimeout(() => {
			this.changeCelldate();
		}, 0);
	}

	changeCelldate() {
		setTimeout(() => {
			let issueDate = this.companyShareInfo?.issueDate ? new Date(this.companyShareInfo?.issueDate) : null;
			let dueDate = this.companyShareInfo?.dueDate ? new Date(this.companyShareInfo?.dueDate) : null;

			if (issueDate && this.companyShareInfo?.period && this.companyShareInfo?.periodUnit) {
				this.companyShareInfo.dueDate = new Date(moment(issueDate).add(this.companyShareInfo?.period, this.UnitDateConst.list[this.companyShareInfo?.periodUnit]).format("YYYY-MM-DD"));
			}

			if (issueDate && dueDate) {
				if (issueDate >= dueDate) {
					this.companyShareInfo.dueDate = null;
				}
			}
		}, 100);
	}

	validForm(): boolean {
		const validRequired = this.companyShareInfo?.issuerId
			&& this.companyShareInfo?.cpsName?.trim()
			&& this.companyShareInfo?.companyShareCode?.trim()
			&& this.companyShareInfo?.period
			&& this.companyShareInfo?.periodUnit
			&& this.companyShareInfo?.interestRate
			&& this.companyShareInfo?.parValue
			&& this.companyShareInfo?.quantity
			&& this.companyShareInfo?.issueDate
			&& (this.companyShareInfo?.interestRateType == CompanySharePolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END || 
				(this.companyShareInfo?.interestRateType !== CompanySharePolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END && this.companyShareInfo.interestPeriod));
		return validRequired;
	}

	header(): string {
		return this.companyShareInfo?.companyShareId > 0 ? 'Sửa ' : 'Thêm lô cổ phần';
	}
}
