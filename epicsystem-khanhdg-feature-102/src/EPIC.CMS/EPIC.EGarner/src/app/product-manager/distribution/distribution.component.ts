import { Component, Injector, OnInit } from "@angular/core";
import { DistributionConst, ProductConst, ProductPolicyConst, SearchConst, StatusApprove, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ConfirmationService, MessageService } from "primeng/api";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { Router } from "@angular/router";
import { OJBECT_DISTRIBUTION_CONST } from '@shared/base-object';
import { DistributionService } from '@shared/services/distribution.service';
import { CreateDistributionComponent } from './create-distribution/create-distribution.component';
import { FormSetDisplayColumnComponent } from "src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component";
import { FormRequestComponent } from "src/app/form-general/form-request-approve-cancel/form-request/form-request.component";
import { FormApproveComponent } from "src/app/form-general/form-request-approve-cancel/form-approve/form-approve.component";

const { POLICY, POLICY_DETAIL, BASE } = OJBECT_DISTRIBUTION_CONST;

@Component({
	selector: 'app-distribution',
  templateUrl: './distribution.component.html',
  styleUrls: ['./distribution.component.scss'],
})
export class DistributionComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private router: Router,
		private breadcrumbService: BreadcrumbService,
		private dialogService: DialogService,
		//
		private confirmationService: ConfirmationService,
		private _distributionService: DistributionService,

	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ['/home'] },
			{ label: "Phân phối đầu tư" }
		]);
	}

	rows: any[] = [];
	row: any;
	col: any;
	_selectedColumns: any[];

	// ACTION BUTTON
	listAction: any[] = [];

	// Data Init
	ProductPolicyConst = ProductPolicyConst;
	YesNoConst = YesNoConst;
	DistributionConst = DistributionConst;

	///////

	cols: any[];

	fieldDates = ["openCellDate", "closeCellDate"];
	isClose = '';
	
	page = new Page();
	offset = 0;

	statusSearch: any[] = [...this.DistributionConst.statusConst];

	fieldCheckBoxs = [];

	ngOnInit() {
		//
		this.setPage({ page: this.offset });
		//
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
		//
		this.fieldCheckBoxs = ['isCheck', 'isShowApp', 'isClose'];
		this.cols = [
			{ field: 'code', header: 'Mã sản phẩm', width: '15rem', isPin: false, isResize: false, class: '' },
			{ field: 'name', header: 'Tổ chức phát hành / Tổng thầu', width: '25rem', isPin: false, isResize: true, class: '' },
			{ field: 'producTypeDisplay', header: 'Loại tích lũy', width: '10rem', isPin: false, isResize: false, class: ''},
			{ field: 'totalValue', header: 'Tổng giá trị', width: '12rem', isPin: false, isResize: false, class: 'justify-content-end'},
			{ field: 'remainAmount', header: 'Còn lại', width: '12em', isPin: false, isResize: false, class: 'justify-content-end'},
			{ field: 'openCellDate', header: 'Ngày mở bán', width: '10rem', isPin: false, isResize: false, class: 'justify-content-center' },
			{ field: 'closeCellDate', header: 'Ngày kết thúc', width: '10rem', isPin: false, isResize: false, class: 'justify-content-center' },
			{ field: 'isShowApp', header: 'Show app', width: '10rem', isPin: false, isResize: false, class: 'justify-content-center' },
		];
		//
		this.cols = this.cols.map((item,index) => {
			item.position = index + 1;
			return item;
		});
		// this._selectedColumns = this.cols;
		this._selectedColumns = [...(this.getLocalStorage('distributionGan') ?? this.cols)];
		
	}

	setColumn(col, _selectedColumns) {
		const ref = this.dialogService.open(
			FormSetDisplayColumnComponent,
			this.getConfigDialogServiceDisplayTableColumn(col, _selectedColumns)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._selectedColumns = dataCallBack.data.sort(function (a, b) {
					return a.position - b.position;
				});
				this.setLocalStorage(this._selectedColumns, 'distributionGan');
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			// ĐỔI GIÁ TRỊ Y/N SANG TRUE/FALSE
			for(let field of this.fieldCheckBoxs) {
				row[field] = (row[field] == YesNoConst.YES);
			}
			//
			row.code = row?.garnerProduct?.code,
			row.name = row?.garnerProduct?.cpsIssuer?.name || row?.garnerProduct?.invGeneralContractor?.name,
			row.countType = DistributionConst.getCountTypeName(row?.garnerProduct?.countType),

			row.producTypeDisplay = ProductConst.getTypeName(row?.garnerProduct?.productType);
			row.totalValue = this.formatCurrency(row?.garnerProduct?.invTotalInvestment || row?.garnerProduct?.cpsQuantity*row?.garnerProduct?.cpsParValue);
			row.remainAmount = this.formatCurrency((row?.garnerProduct?.invTotalInvestment || row?.garnerProduct?.cpsQuantity*row?.garnerProduct?.cpsParValue) - row.isInvested);
			row.openCellDate = this.formatDate(row.openCellDate);
			row.closeCellDate = this.formatDate(row.closeCellDate);
		}
		console.log('showData', rows);
	}

	getPageCurrentInfo() {
		return {page: this.page.pageNumber, rows: this.page.pageSize};
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		if(this.status == this.DistributionConst.CLOSED){
			this.isClose='Y';
		} else if (this.status == null){
			this.isClose = '';
		} else {
			this.isClose = 'N'
		}
		this.isLoading = true;
		//
		this._distributionService.getAll(this.page, this.status, this.isClose).subscribe((res) => {
			this.isLoading = false;
			if(this.handleResponseInterceptor(res)) {
				this.page.totalItems = res?.data?.totalItems;
				this.rows = res?.data?.items;
				if(this.rows?.length) { 
					this.genListAction(this.rows);
					this.showData(this.rows);
				}
			}
			}, (err) => {
				this.isLoading = false;
				console.log('Error-------', err);
			}
		);
	}

	/* ACTION */
	genListAction(data = []) {
		this.listAction = data.map(distribution => {
			const actions = [];

			if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_ThongTinPPSP])) {
				actions.push({
					data: distribution,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.view($event.item.data?.id);
					}
				});
			}
			//
			if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_TrinhDuyet]) && (distribution.status == DistributionConst.KHOI_TAO)) {
				actions.push({
					data: distribution,
					label: 'Trình duyệt bán',
					icon: 'pi pi-volume-down',
					command: ($event) => {
						this.request($event.item.data);
					}
				});
			}
			//
			if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_BatTatShowApp])) {
				actions.push({
					data: distribution,
					label: distribution.isShowApp == YesNoConst.YES ? 'Tắt show app' : 'Bật show app',
					icon: distribution.isShowApp == YesNoConst.YES ? 'pi pi-eye-slash' : 'pi pi-eye',
					command: ($event) => {
						this.showApp($event.item.data?.id);
					}
				});
			}
			//
			if ( this.isGranted([this.PermissionGarnerConst.GarnerPPSP_DongOrMo]) && (distribution.status == DistributionConst.HOAT_DONG && distribution.isClose == YesNoConst.NO)) {
				actions.push({
					data: distribution,
					label: 'Đóng',
					icon: 'pi pi-lock',
					command: ($event) => {
						this.closeOrOpen($event.item.data);
					}
				})
			}

			if ( this.isGranted([this.PermissionGarnerConst.GarnerPPSP_DongOrMo]) && (distribution.isClose == YesNoConst.YES)) {
				actions.push({
					data: distribution,
					label: 'Mở', 
					icon: 'pi pi-lock-open',
					command: ($event) => {
						this.closeOrOpen($event.item.data);
					}
				})
			}

			return actions;
		});
	}

	// Thêm phân phối đầu tư
	create() {
		const ref = this.dialogService.open(
			CreateDistributionComponent,
			{
				header: 'Thêm mới phân phối sản phẩm',
				width: '1000px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
				baseZIndex: 10000,
			}
		);
		//
		ref.onClose.subscribe((distribution) => {
			console.log('dataCallBack', distribution);
			if (distribution?.id) {
				this.messageService.add({ severity: 'success', summary: '', detail: "Thêm mới thành công", life: 1500 });
				this.view(distribution?.id);
			}
		});
	}

	view(distributionId) {
		this.router.navigate(["product-manager/distribution/detail/" + this.cryptEncode(distributionId)]);
	}

	request(distribution) {
		const params = {
			id: distribution.id,
			summary: 'Phân phối đầu tư bán theo kỳ hạn',
		}
		const ref = this.dialogService.open(
			FormRequestComponent,
			this.getConfigDialogServiceRAC("Trình duyệt", params)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._distributionService.request(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Trình duyệt thành công!")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			}
		});
	}

	// Hủy Duyệt
	cancel(distribution) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Hủy duyệt", { id: distribution?.id })
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._distributionService.cancel(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			}
		});
	}

	// DUYỆT / BỎ DUYỆT
	approve(distribution) {
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC("Phê duyệt", { id: distribution?.id })
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			console.log('dataCallBack', dataCallBack);
			if (dataCallBack?.accept) {
				this._distributionService.approve(dataCallBack.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			}
		});
	}
	
	changeStatus() {
		this.setPage(this.getPageCurrentInfo())
	}

	// Api Epic kiểm tra
	check(distribution) {
		this.confirmationService.confirm({
			message: 'Bạn có chắc chắn phê duyệt phân phối đầu tư phiếu này không?',
			header: 'Thông báo',
			icon: 'pi pi-check-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this._distributionService.check({ id: distribution?.id }).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage(this.getPageCurrentInfo());
					}
				});
			},
			reject: () => {
			},
		});
	}

	// KHOA TAM
	toggleClosed(distributionId) {
		this._distributionService.toggleIsClosed(distributionId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật trạng thái đóng thành công")) {
					this.submitted = false;
					this.setPage(this.getPageCurrentInfo());
				}
			}, (err) => {
				console.log('err--', err);
			}
		);
	}

	// BAT TAT IS SHOW APP
	showApp(distributionId) {
		this._distributionService.showApp(distributionId).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
					this.submitted = false;
					this.setPage(this.getPageCurrentInfo());
				}
			}, (err) => {
				console.log('err---', err);
			}
		);
	}

	closeOrOpen(distribution) {
		this.confirmationService.confirm({
			message: distribution.status == StatusApprove.HOAT_DONG ? 'Bạn có chắc chắn đóng sản phẩm này không?' : 'Bạn có chắc chắn mở sản phẩm này không?',
			header: 'Thông báo',
			icon: 'pi pi-check-circle',
			acceptLabel: 'Đồng ý',
			rejectLabel: 'Hủy bỏ',
			accept: () => {
				this._distributionService.openOrClose(distribution.id).subscribe((response) => {
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


