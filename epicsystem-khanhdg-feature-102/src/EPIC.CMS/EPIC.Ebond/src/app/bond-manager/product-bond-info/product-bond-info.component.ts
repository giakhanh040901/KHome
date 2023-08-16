import { FormRequestComponent } from './../../form-request-approve-cancel/form-request/form-request.component';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ProductBondInfoConst, ProductBondDetailConst, SearchConst, KeyFilter, UnitDateConst, YesNoConst, ProductBondPolicyDetailTemplateConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ProductBondInfoServiceProxy } from '@shared/service-proxies/bond-manager-service';
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
	selector: 'app-product-bond-info',
	templateUrl: './product-bond-info.component.html',
	styleUrls: ['./product-bond-info.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class ProductBondInfoComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private _productBondInfoService: ProductBondInfoServiceProxy,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private router: Router,
		private confirmationService: ConfirmationService,

	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Lô trái phiếu' },
		]);
	}
	showplaceholderIssueDate:any = false;

	statusSearch: any[] = [
		{
			name: 'Tất cả ',
			code: ''
		},
		...ProductBondInfoConst.statusConst
	];

	isCheckSearch: any[] = [
		{
			name: 'Tất cả',
			code: ''
		},
		...ProductBondInfoConst.isCheckConst
	];

	ProductBondInfoConst = ProductBondInfoConst;
	ProductBondPolicyDetailTemplateConst = ProductBondPolicyDetailTemplateConst;
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
	productBondInfo: any = {};

	fieldErrors = {};

	fieldDates = ['issueDate', 'dueDate'];

	submitted: boolean;
	cols: any[];
	productBondInfoConst = ProductBondInfoConst;
	page = new Page();
	offset = 0;
	issuers: any = [];
	depositProviders: any = [];
	bondTypes: any = [];
	bondInfos: any = [];

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
		forkJoin([this._productBondInfoService.getAllIssuer(this.page), this._productBondInfoService.getAllDepositProvider(this.page)]).subscribe(
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
			{ field: 'bondCode', header: 'Mã TP', width: '10rem', cutText: 'b-cut-text-10', isPin: true },
			{ field: 'nameIssuer', header: 'TCPH', width: '16rem', cutText: 'b-cut-text-16', isPin: true },
			{ field: 'nameDeposit', header: 'ĐLLK', width: '16rem', cutText: 'b-cut-text-16' },
			{ field: 'bondPeriod', header: 'Kỳ hạn', width: '10rem', cutText: 'b-cut-text-10' },
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

		this._selectedColumns = this.getLocalStorage('bondInfo') ?? this.cols;
	}
	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('bondInfo', JSON.stringify(data));
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
			row.interestPeriod = (row?.interestPeriod && row?.interestPeriodUnit) ? (row?.interestPeriod + ' ' + this.ProductBondInfoConst.getUnitDates(row?.interestPeriodUnit)) : null,
			row.bondPeriod = (row?.bondPeriod && row?.bondPeriodUnit) ? (row?.bondPeriod + ' ' + this.ProductBondInfoConst.getUnitDates(row?.bondPeriodUnit)) : null,
			row.tongGiaTri = this.utils.transformMoney(row?.quantity * row?.parValue),
			row.quantity = this.utils.transformMoney(row?.quantity),
			row.soLuongConLai = this.utils.transformMoney(row?.soLuongConLai),
			row.issueDate = this.formatDate(row?.issueDate);
			row.dueDate = this.formatDate(row?.dueDate);
		}
		console.log('row', rows);
	}

	genListAction(data = []) {
		this.listAction = data.map(bondInfoItem => {
			const actions = [];

			if (this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_TTCT])){
				actions.push({
					data: bondInfoItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}
			
			if (bondInfoItem.status == this.productBondInfoConst.KHOI_TAO && this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_Xoa])) {
				actions.push({
					data: bondInfoItem,
					label: 'Xoá',
					icon: 'pi pi-trash',
					command: ($event) => {
						this.delete($event.item.data);
					}
				});
			}

			if (bondInfoItem.status == this.productBondInfoConst.KHOI_TAO && this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_TrinhDuyet])) {
				actions.push({
					data: bondInfoItem,
					label: 'Trình duyệt',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.request($event.item.data);
					}
				});
			}

			if (bondInfoItem.status == this.productBondInfoConst.HOAT_DONG && bondInfoItem.isCheck == false && this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_EpicXacMinh])) {
				actions.push({
					data: bondInfoItem,
					label: 'Phê duyệt (Epic)',
					icon: 'pi pi-check',
					command: ($event) => {
						this.check($event.item.data);
					}
				});
			}

			if (bondInfoItem.status == this.productBondInfoConst.HOAT_DONG && this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_DongTraiPhieu])) {
				actions.push({
					data: bondInfoItem,
					label: 'Đóng trái phiếu',
					icon: 'pi pi-lock',
					command: ($event) => {
						this.close($event.item.data);
					}
				});
			}

			if (bondInfoItem.status == this.productBondInfoConst.DONG && this.isGranted([this.PermissionBondConst.BondMenuQLTP_LTP_DongTraiPhieu])) {
				actions.push({
					data: bondInfoItem,
					label: 'Mở trái phiếu',
					icon: 'pi pi-key',
					command: ($event) => {
						this.open($event.item.data);
					}
				});
			}

			return actions;
		});
	}

	detail(productBondInfo) {
		this.router.navigate(['/bond-manager/product-bond-info/detail/' + this.cryptEncode(productBondInfo?.productBondId)]);
	}

	request(productBondInfo) {
		const summary = 'Lô TP: ' + productBondInfo?.bondCode + ' - ' + productBondInfo?.bondName + ' ( ID: ' + productBondInfo.productBondId + ' )';
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", productBondInfo.productBondId, summary)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);

			if (dataCallBack?.accept) {
				this._productBondInfoService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage();
					}
				});
			}
		});
	}

	approve(productBondInfo) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", productBondInfo?.productBondId)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._productBondInfoService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	cancel(productBondInfo) {
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Hủy phê duyệt", productBondInfo?.productBondId)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._productBondInfoService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	close(productBondInfo) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn đóng trái phiếu?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: productBondInfo?.productBondId,
					closeNote: null,
				}
				this._productBondInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
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

	open(productBondInfo) {
		this.confirmationService.confirm({
			message: 'Bạn có muốn mở trái phiếu?',
			header: 'Thông báo',
			icon: 'pi pi-question-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Huỷ',
			accept: () => {
				let dataClose = {
					id: productBondInfo?.productBondId,
					closeNote: null,
				}
				this._productBondInfoService.closeOrOpen(dataClose.id).subscribe((response) => {
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
	check(productBondInfo) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô trái phiếu này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._productBondInfoService.check({ id: productBondInfo?.productBondId }).subscribe((response) => {
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
		for (const [key, value] of Object.entries(this.productBondInfo)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}
	create() {
		this.productBondInfo = {};
		this.productBondInfo.allowSbd = this.productBondInfoConst.QUESTION_NO;
		this.productBondInfo.interestPeriodUnit = this.productBondInfoConst.UNIT_DATE_YEAR;
		this.submitted = false;
		this.modalDialog = true;
	}

	changeInterestRateType(value) {
		if (value == this.productBondInfoConst.INTEREST_RATE_TYPE_PERIOD_END) {
			this.productBondInfo.interestPeriod = null;
			this.productBondInfo.interestPeriodUnit = null;
		} else {
			this.productBondInfo.interestPeriodUnit = this.productBondInfoConst.UNIT_DATE_YEAR;
		}
	}

	deleteSelectedItems() {
		this.deleteItemsDialog = true;
	}

	edit() {
		this.productBondInfo = {
			...this.productBondInfo,
			issueDate: this.productBondInfo?.issueDate ? new Date(this.productBondInfo?.issueDate) : null,
			dueDate: this.productBondInfo?.dueDate ? new Date(this.productBondInfo?.dueDate) : null,
			allowSbd: this.productBondInfo?.allowSbd ?? ProductBondInfoConst.QUESTION_NO,
		};
		this.modalDialog = true;
	}

	delete(productBondInfo) {
		this.confirmationService.confirm({
			message: `Bạn có chắc chắn xóa lô TP : ${productBondInfo?.bondName} này?`,
			header: 'Thông báo',
			icon: 'pi pi-times-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy',
			accept: () => {
				this._productBondInfoService.delete(productBondInfo?.productBondId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, 'Xóa thành công')) {
							this.setPage({ page: this.page.pageNumber });
							this.productBondInfo = {};
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

		if (this.productBondInfo[field]) {
			this.productBondInfo.totalValue = value * this.productBondInfo[field];
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
		this._productBondInfoService.getAllBondInfo(this.page, this.status, this.isCheck,issueDate,dueDate ).subscribe((res) => {
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
		console.log('hello----1', this.productBondInfo.issueDate);
		let body = this.formatCalendar(this.fieldDates, {...this.productBondInfo});
		if (this.productBondInfo.productBondId) {
			this._productBondInfoService.update(body).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
						this.submitted = false;
						this.hideDialog();

					} else {
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
				}
			);
		} else {
			console.log("hello test", body.issueDate);
			this._productBondInfoService.create(body).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
						this.submitted = false;
						this.hideDialog();
						console.log(response);
						this.isLoadingPage = true;
						setTimeout(() => {
							this.router.navigate(['/bond-manager/product-bond-info/detail', this.cryptEncode(response.data.productBondId)]);
						}, 1000);
					} else {
						this.submitted = false;
					}
				}, () => {
					this.submitted = false;
				}
			);
		}
	}

	changePeriod(e, field = 'bondPeriod') {
		if (field == 'bondPeriod') this.productBondInfo.bondPeriod = e.value;
		setTimeout(() => {
			this.changeCelldate();
		}, 0);
	}

	changeCelldate() {
		setTimeout(() => {
			let issueDate = this.productBondInfo?.issueDate ? new Date(this.productBondInfo?.issueDate) : null;
			let dueDate = this.productBondInfo?.dueDate ? new Date(this.productBondInfo?.dueDate) : null;

			if (issueDate && this.productBondInfo?.bondPeriod && this.productBondInfo?.bondPeriodUnit) {
				this.productBondInfo.dueDate = new Date(moment(issueDate).add(this.productBondInfo?.bondPeriod, this.UnitDateConst.list[this.productBondInfo?.bondPeriodUnit]).format("YYYY-MM-DD"));
			}

			if (issueDate && dueDate) {
				if (issueDate >= dueDate) {
					this.productBondInfo.dueDate = null;
				}
			}
		}, 100);
	}

	validForm(): boolean {
		const validRequired = this.productBondInfo?.issuerId
			&& this.productBondInfo?.depositProviderId
			&& this.productBondInfo?.bondName?.trim()
			&& this.productBondInfo?.bondCode?.trim()
			&& this.productBondInfo?.bondPeriod
			&& this.productBondInfo?.bondPeriodUnit
			&& this.productBondInfo?.interestRate
			&& this.productBondInfo?.parValue
			&& this.productBondInfo?.quantity
			&& this.productBondInfo?.issueDate
			&& (this.productBondInfo?.interestRateType == ProductBondPolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END || 
				(this.productBondInfo?.interestRateType !== ProductBondPolicyDetailTemplateConst.INTEREST_RATE_TYPE_PERIOD_END && this.productBondInfo.interestPeriod));
		return validRequired;
	}

	header(): string {
		return this.productBondInfo?.productBondId > 0 ? 'Sửa ' : 'Thêm lô trái phiếu';
	}
}
