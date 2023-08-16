import { ProductService } from '@shared/services/product.service';
import { CreateInvestComponent } from './create-product/create-invest/create-invest.component';
import { CreateShareComponent } from './create-product/create-share/create-share.component';
import { Component, Inject, Injector, ViewChild } from "@angular/core";
import { AppConsts, ProductConst, SearchConst, StatusApprove, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { Router } from "@angular/router";
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { FormRequestComponent } from 'src/app/form-general/form-request-approve-cancel/form-request/form-request.component';
import { FormApproveComponent } from 'src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component';
import { FormCancelComponent } from 'src/app/form-general/form-request-approve-cancel/form-cancel/form-cancel.component';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent extends CrudComponentBase {

  constructor(
		_injector: Injector,
		_messageService: MessageService,
		private _router: Router,
		private _breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
		private _confirmationService: ConfirmationService,
		//
		private productService: ProductService,
	) {
		super(_injector, _messageService);
		this._breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Sản phẩm tích lũy" }
		]);
		//
	}

	// CONST
	ProductConst = ProductConst;
	StatusApprove = StatusApprove;
	YesNoConst = YesNoConst;
	statusSearch: any[] = [
	{
		name: 'Tất cả',
		code: ''
	}
	,...this.StatusApprove.list];
	issuers: any[] = [];
	productTypes: any[] = [
		{
			name: 'Tất cả',
			code: ''
		},
		{
            name:'Cổ phần',
            code: ProductConst.SHARE,
        },
        {
            name:'Bất động sản',
            code: ProductConst.INVEST
        }
	]
	rows: any[] = [];
	listAction: any[] = [];

	// Thông tin ẩn hiện cột
	cols: any[];
	_selectedColumns: any[];

	fieldDates = ["openCellDate", "closeCellDate"];
	//
	page = new Page();
	offset = 0;

	modalDialog: boolean = false;
	fieldFilters = {
        status: null,
		issuerId: null,
		productType: null
    }

	configDynamicDialogGeneral = {
		header: 'Thêm mới thông tin cổ phần ưu đãi',
		width: '1000px',
		contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
		baseZIndex: 10000,
	}

	ngOnInit() {
		//
		this.init();
		this.setPage({ page: this.offset });

		this.setupSearchDepounce();
		//
		this.cols = [
			{ field: 'code', header: 'Mã sản phẩm', width: '15rem', isPin: true },
			{ field: 'companyName', header: 'Tổ chức phát hành', width: '25rem', isPin: true, isResize: true },
			{ field: 'productTypeName', header: 'Loại tích lũy', width: '12rem', isPin: true },
			{ field: 'totalValue', header: 'Tổng giá trị', width: '10rem', isPin: true },
			{ field: 'startDateDisplay', header: 'Ngày bắt đầu', width: '11rem', class: 'justify-content-center'},
			{ field: 'endDateDisplay', header: 'Ngày kết thúc', width: '11rem', class: 'justify-content-center' },
			{ field: 'isCheck', header: 'Kiểm tra', width: '6rem', class: 'justify-content-center' },
		];

		this.cols = this.cols.map((item,index) => {
			item.position = index + 1;
			return item;
		});
		//
		this._selectedColumns = this.getLocalStorage('productGan') ?? this.cols;
	}

	init(){
		this.isLoading = true;
		this.productService.getAllIssuer().subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) && res.data) {
				this.issuers = [...res.data];
			}
			console.log({ res__: res });
			} , (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
			}
		);
	}

	setupSearchDepounce() {
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
	}

	setColumn(col, _selectedColumns) {
		//
		const ref = this._dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns, 'productGan');
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.companyName = row?.invOwner?.name || row?.cpsIssuer?.name;
			row.startDateDisplay = this.formatDate(row.startDate);
			row.endDateDisplay = this.formatDate(row.endDate);
			row.productTypeName = ProductConst.getTypeName(row?.productType);
			row.totalValue = this.formatCurrency(row.invTotalInvestment || row?.cpsQuantity*row?.cpsParValue);
		}
	}
		
	getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		//
		this.productService.getAll(this.page,this.fieldFilters).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res) && res.data) {
				this.handleData(res);
			}
			console.log({ res__: res });
      } , (err) => {
        this.isLoading = false;
        console.log('Error-------', err);
      }
		);
	}

	handleData(res) {
		this.rows = res.data.items.map(row => {
			row.isCheck = (row.isCheck == this.YesNoConst.YES);
			return row
		});
		this.genListAction(this.rows);
		this.showData(this.rows);
		this.page.totalItems = res.data?.totalItems;
	}

	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(product => {
			const actions = [];
			if (this.isGranted([this.PermissionGarnerConst.GarnerSPTL_ThongTinSPTL])) {
				actions.push({
					data: product,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data?.id);
					}
				});
			};
			
			if (this.isGranted([this.PermissionGarnerConst.GarnerSPTL_TrinhDuyet]) && product.status == StatusApprove.KHOI_TAO) {
				actions.push({
					data: product,
					label: 'Trình duyệt',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.request($event.item.data);
					}
				})
			}

			if (this.isGranted([this.PermissionGarnerConst.GarnerPDSPTL_PheDuyetOrHuy]) && product.status == StatusApprove.TRINH_DUYET) {
				actions.push({
					data: product,
					label: 'Phê duyệt',
					icon: 'pi pi-check-circle',
					command: ($event) => {
						this.approve($event.item.data);
					}
				});

				actions.push({
					data: product,
					label: 'Hủy duyệt',
					icon: 'pi pi-times',
					command: ($event) => {
						this.cancel($event.item.data);
					}
				});
			}

			if (this.isGranted([this.PermissionGarnerConst.GarnerSPTL_EpicXacMinh]) && product.status == StatusApprove.HOAT_DONG) {
				actions.push({
					data: product,
					label: 'Kiểm tra',
					icon: 'pi pi-arrow-up',
					command: ($event) => {
						this.check($event.item.data);
					}
				})
			}

			if ( this.isGranted([this.PermissionGarnerConst.GarnerSPTL_DongOrMo]) && (product.status == StatusApprove.HOAT_DONG || product.status == StatusApprove.DONG)) {
				actions.push({
					data: product,
					label: product.status == StatusApprove.HOAT_DONG ? 'Đóng' : 'Mở',
					icon: product.status == StatusApprove.HOAT_DONG ? 'pi pi-lock' : 'pi pi-lock-open',
					command: ($event) => {
						this.closeOrOpen($event.item.data);
					}
				})
			}

			return actions;
		});
	}

	// MỞ POPUP CHỌN SẢN PHẨM ĐẦU TƯ
	showProductType() {
		this.modalDialog = true;
	}

	hideModalProductType() {
		this.modalDialog = false;
	}

	createShare() {
		const ref = this._dialogService.open(CreateShareComponent, this.configDynamicDialogGeneral);
		//
		ref.onClose.subscribe((response) => {
			console.log('dataCallBack', response);
			this.reloadPage(response);
		});
	}

	createInvest() {
		const ref = this._dialogService.open(CreateInvestComponent,this.configDynamicDialogGeneral);
		//
		ref.onClose.subscribe((response) => {
			console.log('dataCallBack', response);
			this.reloadPage(response);
		});
	}

	reloadPage(response) {
		if(this.handleResponseInterceptor(response, "Thêm thành công")) {
			this.hideModalProductType();
			this.setPage();
		}
	}

	detail(productId) {
		this._router.navigate(["/product-manager/detail/" + this.cryptEncode(productId)]);
		// let url = this.localBaseUrl + "/product-manager/detail/" + escape(this.cryptEncode(productId));
		// window.open(url);
	}
	
	changeStatus() {
		this.setPage(this.getPageCurrentInfo());
	}

	// TRÌNH DUYỆT
	request(product) {
		const params = {
			id: product.id,
			summary: ProductConst.getTypeName(product?.productType) + ': ' + product?.code + ' - ' + product?.name + ' ( ID: ' + product.id + ' )',
		}
		const ref = this._dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this.productService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			}
		});
	}

	approve(product) {
		const ref = this._dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", { id: product?.id })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this.productService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			}
		});
	}

	cancel(product) {
		const ref = this._dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", { id: product?.id })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this.productService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			}
		});
	}

	check(project) {
		this._confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt sản phẩm này không?',
			header: 'Thông báo',
			icon: 'pi pi-check-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this.productService.rootCheck({ id: project?.id }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			},
			reject: () => {
			},
		});
	}

	closeOrOpen(project) {
		this._confirmationService.confirm({
			message: project.status == StatusApprove.HOAT_DONG ? 'Bạn có chắc chắn đóng sản phẩm này không?' : 'Bạn có chắc chắn mở sản phẩm này không?',
			header: 'Thông báo',
			icon: 'pi pi-check-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this.productService.changeStatus(project.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Thao tác thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			},
			reject: () => {
			},
		});
	}

}
