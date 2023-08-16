import { FormApproveComponent } from './../../form-request-approve-cancel/form-approve/form-approve.component';
import { CompanySharePolicyServiceProxy, CompanyShareSecondaryServiceProxy } from "./../../../shared/service-proxies/company-share-manager-service";
import { Component, Injector, OnInit } from "@angular/core";
import { CompanyShareInterestConst, CompanyShareSecondaryConst, ProductPolicyConst, SearchConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DynamicDialogRef, DialogService } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";
import { debounceTime } from "rxjs/operators";
import { Router } from "@angular/router";
import { OJBECT_SECONDARY_CONST } from "@shared/base-object";
import { FormRequestComponent } from "src/app/form-request-approve-cancel/form-request/form-request.component";
import { FormSetDisplayColumnComponent } from 'src/app/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { TokenService } from '@shared/services/token.service';

const { POLICY, POLICY_DETAIL, BASE } = OJBECT_SECONDARY_CONST;

@Component({
	selector: "app-company-share-secondary",
	templateUrl: "./company-share-secondary.component.html",
	styleUrls: ["./company-share-secondary.component.scss"],
	providers: [DialogService],
})
export class CompanyShareSecondaryComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _secondaryService: CompanyShareSecondaryServiceProxy,
		private _policyService: CompanySharePolicyServiceProxy,
		private _router: Router,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Bán theo kỳ hạn" }
		]);
	}
	statusSearch: any[] = [
		{
			name: 'Tất cả',
			code: ''
		  },
		  ...CompanyShareSecondaryConst.statusList
	];

	ref: DynamicDialogRef;

	modalDialog: boolean;
	modalDialogPolicy: boolean;
	modalDialogPolicyDetail: boolean;

	deleteItemDialog: boolean = false;

	deleteItemsDialog: boolean = false;

	rows: any[] = [];
	row: any;
	col: any;
	_selectedColumns: any[];

	// ACTION BUTTON
	listAction: any[] = [];

	// Data Init
	listPrimary: any = [];
	ProductSecondaryConst = CompanyShareSecondaryConst;
	ProductPolicyConst = ProductPolicyConst;
	CompanyShareInterestConst = CompanyShareInterestConst;
	YesNoConst = YesNoConst;

	///////
	expandedRows = {};

	isExpanded: boolean = false;
	isClose = '';
	companyShareSecondary: any = { ...BASE.SECODARY };
	policy: any = { ...BASE.POLICY };
	policyDetail: any = { ...BASE.POLICY_DETAIL };
	/**
	 * Dùng cho search policy và detail base
	 *  */
	search = {
		policy: {
			companySharePolicyDetailTemp: [],
		},
		detail: {},
		listPolicy: [],
		listDetails: [],
	};
	
	submitted: boolean;

	cols: any[];

	statuses: any[];

	fieldDates = ["openCellDate", "closeCellDate"];

	page = new Page();
	offset = 0;

	ngOnInit() {
		console.log('status', this.statusSearch);

		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		this.onChangeIntestType({ value: CompanyShareInterestConst.INTEREST_TYPES.DINH_KY });

		this.cols = [
			{ field: 'code', header: 'Mã CP', width: '5rem', class: 'justify-content-start', cutText: 'b-cut-text-5', isPin: true },
			{ field: 'name', header: 'Tên cổ phần', width: '17rem', class: 'b-cut-text-20', pTooltip: 'Hạn mức đầu tư còn lại', tooltipPosition: 'top', isPin: true },
			{ field: 'hanMucToiDa', header: 'Hạn mức tối đa', width: '14rem', class: 'justify-content-end', pTooltip: 'Hạn mức đầu tư tối đa', tooltipPosition: 'top', isPin: true },
			{ field: 'openCellDate', header: 'Ngày mở bán', width: '18rem', class: 'justify-content-center' },
			{ field: 'closeCellDate', header: 'Ngày kết thúc bán', width: '18rem', class: 'justify-content-center' },
			{ field: 'isCheck', header: 'Kiểm tra', width: '10rem', class: 'justify-content-center' },
		];

		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		})

		this._selectedColumns = this.cols;
		// this._selectedColumns = this.getLocalStorage('companyShareSecondary') ?? this.cols;
	}

	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}
	setLocalStorage(data) {
		return localStorage.setItem('companyShareSecondary', JSON.stringify(data));
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
			row.code = row?.companyShareInfo?.code,
			row.name = row?.name,
			row.soLuongTraiPhieuNamGiu = this.utils.transformMoney(row?.soLuongTraiPhieuNamGiu),
			row.hanMucToiDa = this.utils.transformMoney(row?.hanMucToiDa),
			row.openCellDate = this.formatDate(row?.openCellDate);
			row.closeCellDate = this.formatDate(row?.closeCellDate);
		}
		console.log('showData', rows);
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

		this.page.keyword = this.keyword;
		this.isLoading = true;
		// if(this.status == this.ProductSecondaryConst.STATUS.CLOSED){
		// 	this.status='';
		// 	this.isClose='Y';
		// } else{
		// 	this.isClose = 'N';
		// }
		forkJoin([this._secondaryService.getAllCompanySharePrimary(), this._secondaryService.getAll(this.page, this.status, this.isClose), this._policyService.getAllTemporaryNoPaging()]).subscribe(
			([resCompanySharePrimary, resCompanyShareSecondary, resTempPolicy]) => {
				this.isLoading = false;
				// set type isCheck sang kiểu boolean
				if (this.handleResponseInterceptor(resCompanyShareSecondary, '')){
					if (resCompanyShareSecondary.data?.items) {
						this.rows = resCompanyShareSecondary.data.items.map(row => {
							row.isCheck = (row.isCheck == this.YesNoConst.STATUS_YES);
							return row
						});
					}
				}

				this.listPrimary = resCompanySharePrimary?.data ?? [];
				if(this.rows?.length) { 
					this.genListAction(this.rows);
					this.showData(this.rows);
				}
				
				this.page.totalItems = resCompanyShareSecondary?.data?.totalItems;
				this.search.listPolicy = [...resTempPolicy?.data?.items];

				console.log({ companySharePrimay: resCompanySharePrimary, rows: resCompanyShareSecondary, resCompanySharePrimary: resCompanySharePrimary });
			},
			(err) => {
				this.isLoading = false;
				console.log('Error-------', err);
				
			}
		);
	}



	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(secondaryItem => {
			const actions = [];

			if (this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_ThongTinChiTiet])){
				actions.push({
					data: secondaryItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.view($event.item.data);
					}
				})
			}

			if (secondaryItem.status == this.ProductSecondaryConst.STATUS.NHAP && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_ThongTinChiTiet])) {
				actions.push({
					data: secondaryItem,
					label: 'Trình duyệt',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.request($event.item.data);
					}
				});
			}

			// if (secondaryItem.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted(this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_TrinhDuyet)) {
			// 	actions.push({
			// 		data: secondaryItem,
			// 		label: 'Phê duyệt',
			// 		icon: 'pi pi-check',
			// 		command: ($event) => {
			// 			this.approve($event.item.data);
			// 		}
			// 	});
			// }

			// if (secondaryItem.status == this.ProductSecondaryConst.STATUS.TRINH_DUYET && this.isGranted()) {
			// 	actions.push({
			// 		data: secondaryItem,
			// 		label: 'Hủy duyệt',
			// 		icon: 'pi pi-times',
			// 		command: ($event) => {
			// 			this.cancel($event.item.data);
			// 		}
			// 	});
			// }

			if (secondaryItem.status == this.ProductSecondaryConst.STATUS.HOAT_DONG && secondaryItem.isCheck == false && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_EpicXacMinh])) {
				actions.push({
					data: secondaryItem,
					label: 'Phê duyệt (Epic)',
					icon: 'pi pi-check',
					command: ($event) => {
						this.check($event.item.data);
					}
				});
			}

			if (secondaryItem.isClose == YesNoConst.STATUS_NO && secondaryItem.status == this.ProductSecondaryConst.STATUS.HOAT_DONG && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_DongTam])) {
				actions.push({
					data: secondaryItem,
					label: 'Đóng tạm',
					icon: 'pi pi-lock',
					command: ($event) => {
						this.toggleClosed($event.item.data?.secondaryId, YesNoConst.STATUS_YES);
					}
				});
			}

			if (secondaryItem.isClose == YesNoConst.STATUS_YES && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_DongTam])) {
				actions.push({
					data: secondaryItem,
					label: 'Mở',
					icon: 'pi pi-unlock',
					command: ($event) => {
						this.toggleClosed($event.item.data?.secondaryId, YesNoConst.STATUS_NO);
					}
				});
			}

			if (secondaryItem.isShowApp == YesNoConst.STATUS_NO && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_BatTatShowApp])) {
				actions.push({
					data: secondaryItem,
					label: 'Bật show app',
					icon: 'pi pi-eject',
					command: ($event) => {
						this.toggleIsShowApp($event.item.data?.secondaryId, YesNoConst.STATUS_YES);
					}
				});
			}

			if (secondaryItem.isShowApp == YesNoConst.STATUS_YES && this.isGranted([this.PermissionCompanyShareConst.CompanyShareMenuQLTP_BTKH_BatTatShowApp])) {
				actions.push({
					data: secondaryItem,
					label: 'Tắt show app',
					icon: 'pi pi-eye-slash',
					command: ($event) => {
						this.toggleIsShowApp($event.item.data?.secondaryId, YesNoConst.STATUS_NO);
					}
				});
			}

			return actions;
		});
	}

	// Dialog THEM PHAT HANH THU CAP
	create() {
		// this.expandAll();
		this.companyShareSecondary = { ...BASE.SECODARY, policies: [] };
		this.submitted = false;
		this.modalDialog = true;
	}

	closeModalCreateSecondary() {
		this.hideDialog('modalDialog');
	}

	view(companyShareSecondary) {
		this._router.navigate(["company-share-manager/company-share-secondary/update/" + this.cryptEncode(companyShareSecondary?.secondaryId)]);
	}

	request(companyShareSecondary) {
		const summary = 'Bán theo kỳ hạn: ' + companyShareSecondary?.companyShareInfo?.code + ' - ' + companyShareSecondary?.companyShareInfo?.name + '( ID: ' + companyShareSecondary.secondaryId + ')';
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", companyShareSecondary.secondaryId, summary)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);

			if (dataCallBack?.accept) {
				this._secondaryService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage();
					}
				});
			}
		});
	}

	/**
	 * NHẬN EVENT TẠO MỚI POLICY TỪ MODAL THÊM MỚI SECONDARY
	 */
	createPolicy() {
		this.policy = { ...BASE.POLICY };
		this.policy.tradingProviderId = 1;
		this.submitted = false;
		this.modalDialogPolicy = true;
	}

	/**
	 * NHẬN EVENT CẬP NHẬT POLICY TỪ MODAL THÊM MỚI SECONDARY
	 * @param policy 
	 */
	editPolicy(policy) {
		this.policy = { ...policy };
		this.submitted = false;
		this.modalDialogPolicy = true;
	}

	/**
	 * NHẬN EVENT TẠO POLICY DETAIL TỪ MODAL THÊM MỚI SECONDARY
	 * @param data 
	 */
	createPolicyDetail(data) {
		const { policy, policyIndex } = data;
		this.policyDetail = {
			...BASE.POLICY_DETAIL,
			fakeId: 0,
			policyId: policy.fakeId,
			policyIndex: policyIndex,
			tradingProviderId: this.companyShareSecondary.tradingProdviderId,
		};

		console.log({ policyDetail: this.policyDetail });
		this.submitted = false;
		this.modalDialogPolicyDetail = true;
	}

	/**
	 * NHẬN EVENT SỬA POLICY DETAIL TỪ MODAL THÊM MỚI SECONDARY
	 * @param policyDetail 
	 */
	editPolicyDetail(policyDetail) {
		this.policyDetail = { ...policyDetail };
		this.submitted = false;
		this.modalDialogPolicyDetail = true;
	}

	/**
	 * TẠO MỚI BÁN THEO KỲ HẠN
	 * @param companyShareSecondary 
	 */
	save(companyShareSecondary) {
		this.companyShareSecondary = { ...companyShareSecondary };
		this.submitted = true;
		console.log({ Secondary: this.companyShareSecondary });
		//
		this.setTimeZoneList(this.fieldDates, this.companyShareSecondary);
		this._secondaryService.create(this.companyShareSecondary).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Thêm Phát hành thứ cấp thành công")) {
					const { data } = response;
					this.submitted = false;
					this.hideDialog();
					this.isLoadingPage = true;
					setTimeout(() => {
						this.view(data);
					}, 1000);
				} else {
					this.resetTimeZoneList(this.fieldDates, this.companyShareSecondary);
					this.submitted = false;
				}
			}, () => {
				this.submitted = false;
			}
		);
	}

	hideDialog(modalName = "modalDialog") {
		this[modalName] = false;
		this.submitted = false;
	}

	validForm(): boolean {
		const validRequired = this.companyShareSecondary?.companySharePrimaryId;
		return validRequired;
	}

	validFormPolicy(): boolean {
		const validRequired =
			this.policy?.code && this.policy?.name && this.policy?.type && this.policy?.typeInvestor && this.policy?.minMoneny && this.policy?.numberClosePer && this.policy?.incomeTax;
		return validRequired;
	}
	validFormPolicyDetail(): boolean {
		const validRequired =
			this.policyDetail?.code &&
			this.policyDetail?.name &&
			this.policyDetail?.type &&
			this.policyDetail?.typeInvestor &&
			this.policyDetail?.minMoneny &&
			this.policyDetail?.numberClosePer &&
			this.policyDetail?.incomeTax &&
			this.policyDetail?.interestPeriod &&
			this.policyDetail?.interestPeriodType;
		return validRequired;
	}

	/**
	 * NHẬN EVENT XOÁ POLICY || POLICY DETAIL TỪ MODAL THÊM MỚI SECONDARY
	 * @param data 
	 */
	delete(data) {
		const { item, type } = data;
		this.deleteItemDialog = true;
		if (type == POLICY) {
			this.policyDetail = {};
			this.policy = { ...item };
		}
		if (type == POLICY_DETAIL) {
			this.policy = {};
			this.policyDetail = { ...item };
		}
	}

	confirmDelete(type) {
		this.deleteItemDialog = false;
		if (type == POLICY) {
			if (this.policy) {
				let index = this.companyShareSecondary.policies.findIndex((item) => item.fakeId == this.policy.fakeId);
				this.companyShareSecondary.policies.splice(index, 1);
			}
		} else if (type == POLICY_DETAIL) {
			if (this.policyDetail) {
				for (let policy of this.companyShareSecondary.policies) {
					let index = policy.details.findIndex((item) => item.fakeId == this.policyDetail.fakeId);
					if (index !== -1) {
						policy.details.splice(index, 1);
					}
				}
			}
		}
	}

	expandAll() {
		if (!this.isExpanded) {
			this.companyShareSecondary.policies.forEach((product) => (this.expandedRows[product.fakeId] = true));
		} else {
			this.expandedRows = {};
		}
		this.isExpanded = !this.isExpanded;
	}

	onSavePolicy(data) {
		this.policy = { ...this.policy, ...data };
		this.savePolicy();
	}

	savePolicy() {
		if (this.policy?.fakeId) {
			this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;
			let index = this.companyShareSecondary.policies.findIndex((item) => item.fakeId == this.policy.fakeId);
			this.companyShareSecondary.policies[index] = { ...this.policy };
			console.log({ CompanyShareSecondary: this.companyShareSecondary });
		} else {
			this.policy.action = this.ProductPolicyConst.ACTION_CREATE;
			this.policy.fakeId = new Date().getTime();
			if (!this.companyShareSecondary.policies) {
				this.companyShareSecondary.policies = [];
			}
			this.companyShareSecondary.policies.push(this.policy);
		}
		this.modalDialogPolicy = false;
	}

	/** NHAN SU KIEN TU MODAL POLICY DETAIL */
	onSaveAddPolicyDetail(data) {
		this.policyDetail = {
			...this.policyDetail,
			...data
		};
		console.log(this.policyDetail);
		this.savePolicyDetail();
	}
	/**
	 * SAVE POLICY DETAIL
	 */
	savePolicyDetail() {
		let policyIndex = this.policyDetail.policyIndex;
		if (this.policyDetail.fakeId) {
			this.policy.action = this.ProductPolicyConst.ACTION_UPDATE;
			let index = this.companyShareSecondary.policies[policyIndex].details.findIndex((item) => item.fakeId == this.policyDetail.fakeId);
			this.companyShareSecondary.policies[policyIndex].details[index] = { ...this.policyDetail };
		} else {
			this.policy.action = this.ProductPolicyConst.ACTION_CREATE;
			this.policyDetail.fakeId = new Date().getTime();
			if (!this.companyShareSecondary.policies[policyIndex].details) {
				this.companyShareSecondary.policies[policyIndex].details = [];
			}
			this.companyShareSecondary.policies[policyIndex].details.push(this.policyDetail);
			this.expandedRows[this.companyShareSecondary.policies[policyIndex].fakeId] = true;
			console.log(this.companyShareSecondary.policies[policyIndex].details);
		}

		console.log({ companyShareSecondary: this.companyShareSecondary });

		this.modalDialogPolicyDetail = false;
	}

	// XU LY FILTER POLICY & POLICY DETAIL
	selectPolicy($event) {
		const { value } = $event;
		this.search.listDetails = [...value.companySharePolicyDetailTemp];

		Object.keys(this.policy).forEach((key) => {
			if (key in value) {
				this.policy[key] = value[key];
			}
		});

		console.log(this.search.listDetails);
	}

	selectPolicyDetail($event) {
		const { value } = $event;

		Object.keys(this.policyDetail).forEach((key) => {
			if (key in value) {
				this.policyDetail[key] = value[key];
			}
		});

		console.log({ value }, this.policyDetail);
	}
	// Hủy Duyệt
	cancel(companyShareSecondary) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Hủy duyệt", companyShareSecondary?.secondaryId)
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._secondaryService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.setPage();
					}
				});
				// this.processChangeStatus(secondaryId, CompanyShareSecondaryConst.STATUS.HOAT_DONG, "Duyệt thành công");
			}
		});
	}

	// DUYỆT / BỎ DUYỆT
	approve(companyShareSecondary) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", companyShareSecondary?.secondaryId)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._secondaryService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
				// this.processChangeStatus(secondaryId, CompanyShareSecondaryConst.STATUS.HOAT_DONG, "Duyệt thành công");
			}
		});
	}

	unapprove(secondaryId) {
		this.processChangeStatus(secondaryId, CompanyShareSecondaryConst.STATUS.NHAP, "Đã từ chối duyệt");
	}

	changeStatus() {
		this.setPage({ Page: this.offset })
	}

	processChangeStatus(secondaryId, status, msg) {
		this._secondaryService.changeStatus(secondaryId, status).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, msg)) {
					this.setPage({ page: this.page.pageNumber });
				}
			});
	}

	// TRÌNH DUYỆT
	submit(secondaryId) {
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", secondaryId)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._secondaryService.submit(secondaryId).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Trình duyệt thành công")) {
							this.setPage({ page: this.page.pageNumber });
						}
					}
				);
			}
		});
	}

	// Api Epic kiểm tra
	check(companyShareSecondary) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt lô cổ phần này không?',
			header: 'Thông báo',
			acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
			icon: 'pi pi-check-circle',
			accept: () => {
				this._secondaryService.check({ id: companyShareSecondary?.secondaryId }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			},
			reject: () => {
			},
		});
	}

	// KHOA TAM
	toggleClosed(secondaryId, value) {
		this._secondaryService.toggleIsClosed(secondaryId, value).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật trạng thái đóng thành công")) {
					this.submitted = false;
					this.setPage({ page: this.page.pageNumber });
					this.hideDialog();
				} else {
					this.resetTimeZoneList(this.fieldDates, this.companyShareSecondary);
					this.submitted = false;
				}
			},
			() => {
				this.submitted = false;
			}
		);
	}

	// BAT TAT IS SHOW APP
	toggleIsShowApp(secondaryId, value) {
		this._secondaryService.toggleIsShowApp(secondaryId, value).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật show app thành công")) {
					this.submitted = false;
					this.setPage({ page: this.page.pageNumber });
					this.hideDialog();
				} else {
					this.resetTimeZoneList(this.fieldDates, this.companyShareSecondary);
					this.submitted = false;
				}
			},
			() => {
				this.submitted = false;
			}
		);
	}

	// CHO PHEP GO GIA TRI INPUT CUA SECONDARY
	allowEditSecondary() {
		return true;
	}

	// 
	onChangeIntestType($event) {
		const { value } = $event;
		if (CompanyShareInterestConst.isDinhKy(value)) {
			this.policyDetail.interestPeriodType = CompanyShareInterestConst.interestPeriodTypes[0].code;
		} else {
			this.policyDetail.interestPeriodQuantity = null;
			this.policyDetail.interestPeriodType = null;
		}
	}
}
