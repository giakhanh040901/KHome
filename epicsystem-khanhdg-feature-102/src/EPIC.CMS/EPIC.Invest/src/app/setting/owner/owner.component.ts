import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector } from '@angular/core';
import { Page } from '@shared/model/page';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ContractorConst, BusinessCustomerConst, FormNotificationConst, TableConst } from '@shared/AppConsts';
import { Router } from "@angular/router"
import { DialogService } from 'primeng/dynamicdialog';
import { MessageService } from 'primeng/api';
import { OwnerServiceProxy } from '@shared/services/owner-service';
import { NationalityConst } from '@shared/nationality-list';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { FilterBusinessCustomerComponent } from '../general-contractor/filter-business-customer/filter-business-customer.component';

@Component({
  selector: 'app-owner',
  templateUrl: './owner.component.html',
  styleUrls: ['./owner.component.scss']
})

export class OwnerComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private router: Router,
		private breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
    	private ownerService: OwnerServiceProxy,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Chủ đầu tư' },
		]);
	}

	owner: any = {
		businessCustomerId: null,
		businessTurnover: null,	// Doanh thu
		businessProfit: null,	// Lợi nhuận
		roa: null,
		roe: null,
		fanpage: null,
		hotline: null,
		image: null,
	};

	modalDialog: boolean;
	listAction: any[] = [];

	ContractorConst = ContractorConst;
	BusinessCustomerConst = BusinessCustomerConst;
	NationalityConst = NationalityConst;

	businessCustomer: any = {};

	businessCustomerId: number;

	submitted: boolean;

	page = new Page();

	isRefresh = true;

	rows: any[] = [];
	columns: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();
	
	ngOnInit() {
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left'},
			{ field: 'taxCode', header: 'Mã số thuế', width: 8 },
			{ field: 'name', header: 'Tên doanh nghiệp', width : 25 },
			{ field: 'shortName', header: 'Tên viết tắt', width : 15 },
			{ field: 'email', header: 'Email', width : 20 },
			{ field: 'phone', header: 'SĐT', width : 12, isResize: true},
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
    	];
		//
		this.setPage();
	}

	showData(rows) {
		for (let row of rows) {
			row.taxCode = row?.businessCustomer?.taxCode,
			row.name = row?.businessCustomer?.name,
			row.shortName = row?.businessCustomer?.shortName,
			row.email = row?.businessCustomer?.email,
			row.phone = row?.businessCustomer?.phone
		};
	}

	onSort(event) {
		this.sortData = event;
		this.setPage();
	}

	genListAction(data = []) {
		this.listAction = data.map(ownerItem => {
			const actions = [];

			if(this.isGranted([this.PermissionInvestConst.InvestChuDT_ThongTinChuDauTu])) {
				actions.push({
					data: ownerItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}

			if(this.isGranted([this.PermissionInvestConst.InvestChuDT_Xoa])) {
				actions.push({
					data: ownerItem,
					label: 'Xoá',
					icon: 'pi pi-trash',
					command: ($event) => {
						this.delete($event.item.data);
					}
				})
			}
			return actions;
		});
	}

	create() {
		this.owner = {};
		this.businessCustomer = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	delete(owner) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xóa nhà đâu tư này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this.ownerService.delete(owner?.id).subscribe((response) => {
					if (this.handleResponseInterceptor(response,"Xóa nhà đâu tư thành công")) {
						this.setPage(this.page);
						this.owner = {};
					}
				});
			} 
		});
	  }

	detail(owner) {
		this.router.navigate(['/setting/owner/detail', this.cryptEncode(owner?.id)]);
	}

	showBusinessCustomer() {
		const ref = this.dialogService.open(FilterBusinessCustomerComponent,
			{
				header: 'Tìm kiếm khách hàng doanh nghiệp',
				width: '1000px',
				contentStyle: { "height": "auto", "overflow": "hidden", "padding": "0px" },
				style: { top: "-15%", height: '350px' },
			});

		ref.onClose.subscribe((businessCustomer) => {
			if(businessCustomer) {
				this.owner.businessCustomerId = businessCustomer.businessCustomerId;
				this.businessCustomer = {...businessCustomer};
			}
		});
	}



	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this.ownerService.getAllOwner(this.page).subscribe((res) => {
			this.isLoading = false;
			if(this.callBackData(res) && this.isRefresh) {
				this.isRefresh = false;
				this.setPage();
			} else {
				if (this.handleResponseInterceptor(res, '')) {
					this.page.totalItems = res.data.totalItems;
					this.rows = res.data.items;
					this.genListAction(this.rows);
					this.showData(this.rows);
					console.log({ rows: res.data.items, totalItems: res.data.totalItems });
				}
			}
		}, (err) => {
			console.log('err-------', err);
			this.isLoading = false;
		});
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.submitted = true;
		this.ownerService.create(this.owner).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
					this.submitted = false;
					this.hideDialog();
					this.isLoadingPage = true;
					setTimeout(() => {
						this.isLoadingPage = false;
						this.router.navigate(['/setting/owner/detail', this.cryptEncode(response.data.id)]);
					}, 1000);
				} else {
					this.submitted = false;
				}
			}, () => {
				this.submitted = false;
			}
		);
	}

	validForm(): boolean {
		const validRequired = this.businessCustomer?.code;
		return validRequired;
	  }
}

