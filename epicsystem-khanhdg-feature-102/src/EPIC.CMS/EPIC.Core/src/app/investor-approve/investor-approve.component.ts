import { Component, Injector } from "@angular/core";
import { Router } from "@angular/router";
import { CrudComponentBase } from "@shared/crud-component-base";
import { InvestorServiceProxy } from "@shared/service-proxies/investor-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "../layout/breadcrumb/breadcrumb.service";
import { ProductBondInfoConst, InvestorConst, ApproveConst, SearchConst, BusinessCustomerApproveConst, PermissionCoreConst, TypeFormatDateConst } from "@shared/AppConsts";
import { Page } from "@shared/model/page";
import { DialogService } from "primeng/dynamicdialog";
import { FormRequestComponent } from "../form-request-approve-cancel/form-request/form-request.component";
import { FormApproveComponent } from "../form-request-approve-cancel/form-approve/form-approve.component";
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from "../form-set-display-column/form-set-display-column.component";
import { Subject } from "rxjs";

@Component({
	selector: "app-investor-approve",
	templateUrl: "./investor-approve.component.html",
	styleUrls: ["./investor-approve.component.scss"],
	providers: [DialogService, ConfirmationService],
})
export class InvestorApproveComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private _investorService: InvestorServiceProxy,
		private dialogService: DialogService,
		private breadcrumbService: BreadcrumbService,
		private router: Router,
		private _dialogService: DialogService
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Thêm mới khách hàng cá nhân' },
		]);
	}

	InvestorConst = InvestorConst;
	BusinessCustomerApproveConst = BusinessCustomerApproveConst;
	PermissionCoreConst = PermissionCoreConst;

	statusSearch: any[] = [
		{
			name: "Tất cả",
			code: ''
		},
		...InvestorConst.statusListApprove
	];

	modalDialog: boolean;
	modalDialogUpdate: boolean;
	deleteItemDialog: boolean = false;
	deleteItemsDialog: boolean = false;

	dataFilter = {
		fieldSearch: null,
		status: '',
	}

	rows: any[] = [];
	row: any;
	col: any;

	cols: any[];
	_selectedColumns: any[];

	selectedInvestor: any = {
		phone: "",
		email: "",
		address: "",
		idType: InvestorConst.ID_TYPES.CMND,
		idNo: "",
		fullname: "",
		dateOfBirth: null,
		nationality: "",
		personalIdentification: "",
		idIssuer: "",
		idDate: null,
		idExpiredDate: null,
		placeOfOrigin: "",
		placeOfResidence: "",
		sex: "",
		idFrontImageUrl: "",
		idBackImageUrl: "",
		idExtraImageUrl: "",
		faceImageUrl: "",
		faceVideoUrl: "",
		bankAccount: "",
		bankName: "",
	};

	investor: any = {};

	fieldErrors = {};

	fieldDates = ["issueDate", "dueDate"];

	submitted: boolean;
	page = new Page();
	offset = 0;
	issuers: any = [];
	depositProviders: any = [];
	bondTypes: any = [];
	bondInfos: any = [];
	listInvestor: any = [];
	//
	issuer = {};
	depositProvider = {};

	sortData: any[] = []
	listAction: any[] = [];

	ngOnInit() {
		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		this.subject.isSetPage.subscribe(() => {
			this.setPage();
		  })
		this.cols = [
			{ field: 'cifCode', header: 'Mã KH', width: '12rem', class: 'justify-content-left', isPin: true, isSort: true },
			{ field: 'defaultIdentification.fullname', header: 'Họ tên', width: '16rem', cutText: 'b-cut-text-16',class: 'justify-content-left', isPin: true, isSort: true },
			{ field: 'defaultIdentification.dateOfBirth', header: 'Ngày sinh',class: 'justify-content-center', width: '12rem', isSort: true },
			{ field: 'defaultIdentification.sex', header: 'Giới tính', class: 'justify-content-left',width: '10rem', isSort: true },
			{ field: 'phone', header: 'Điện thoại',class: 'justify-content-left', width: '12rem', isSort: true },
			{ field: 'email', header: 'Email', width: '18rem', class: 'justify-content-left', cutText: 'b-cut-text-18', isSort: true },
			{ field: 'source', header: 'Nguồn', width: '6rem', isSort: true },
			{ header: '', type:'hidden' },
		];
		// Sắp xếp vị trị hiển thị trên table từ trên xuống dưới
		this.cols = this.cols.map((item, index) => {
			item.position = index + 1;
			return item;
		});
		// this._selectedColumns = this.cols;
		this._selectedColumns = this.getLocalStorage('investorApproveCore') ?? this.cols;  
	}

	edit(investor) {
		this.router.navigate([`/customer/investor/${this.cryptEncode(investor?.investorId)}/temp/1`]);
	}

	getLocalStorage(key) {
		return JSON.parse(localStorage.getItem(key))
	}

	setLocalStorage(data) {
		return localStorage.setItem('investorApproveCore', JSON.stringify(data));
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn("Sửa cột hiển thị", col, _selectedColumns)
		);

		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns)
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row['defaultIdentification.sex'] = this.InvestorConst.getSexName(row.defaultIdentification?.sex),
			row['defaultIdentification.dateOfBirth'] = this.formatDate(row.defaultIdentification?.dateOfBirth,TypeFormatDateConst.DMY);
			row['defaultIdentification.fullname'] = row.defaultIdentification?.fullname;
		};
	}

	/**
	 * XEM THÔNG TIN CHI TIẾT INVESTOR TẠM
	 * @param investor
	 */
	view(investor) {
		this.router.navigate([`/customer/investor/${this.cryptEncode(investor?.investorId)}/temp/1`]);
	}

	/* ACTION BUTTON */
	genListAction(data = []) {
		this.listAction = data.map((item) => {
			const actions = [];

			if (this.isGranted([PermissionCoreConst.CoreDuyetKHCN_ThongTinKhachHang])) {
				actions.push({
					data: item,
					label: "Thông tin chi tiết",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.view($event.item.data);
					},
				})
			}

			return actions;
		});

		console.log(this.listAction);
	}

	approve(investor) {
		const ref = this._dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", investor?.investorId)
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this.isLoading = true;
				const body = {
					investorId: investor.investorId,
					investorGroupId: investor?.investorGroupId,
					notice: dataCallBack?.data?.notice,
				}
				this._investorService.approve(body).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.isLoading = false;
						this.viewInvestor();
					}
				}, (error) => {
					console.error("Lỗi xảy ra khi phê duyệt:", error);
					this.isLoading = false;
				  });
			}
		});
	}

	viewInvestor() {
		this.router.navigate(["/customer/investor"]);
	}

	changeStatus() {
		console.log("status");
		
		this.setPage({ page: this.offset });
	}

	changeFieldSearch(){
		if(this.keyword) {
			this.setPage();
		}
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;

		this.isLoading = true;
		this._investorService.getInvestorTemporary(this.page, this.dataFilter,this.sortData).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, "")) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				this.investor = res.data;
				if (res.data?.items?.length) {
					this.genListAction(this.rows);
					this.showData(this.rows);
				}
			}
		},(err) => {
				this.isLoading = false;
				console.log('Error-------', err);		
			}
		);
	}

	/**
	 * MỞ EKYC MODAL
	 */
	openEkycModal() {
		this.submitted = false;
		this.modalDialog = true;
	}

	deleteSelectedItems() {
		this.deleteItemsDialog = true;
	}

	delete() {
		this.deleteItemDialog = true;
	}

	confirmDelete() {
		this.deleteItemDialog = false;
	}


	/**
	 * CALLBACK SAU KHI THEM MOI INVESTOR THANH CONG
	 */
	onSaveInvestor() {
		this.setPage(0);
	}

	header(): string {
		return this.investor?.investorId > 0 ? 'Sửa thông tin khách hàng ' : 'Thêm thông tin khách hàng ';
	}
}

