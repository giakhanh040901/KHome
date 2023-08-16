import { CrudComponentBase } from '@shared/crud-component-base';
import { Component, Injector } from '@angular/core';
import { Page } from '@shared/model/page';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { ContractorConst, BusinessCustomerConst, FormNotificationConst, TableConst } from '@shared/AppConsts';
import { Router } from "@angular/router"
import { DialogService } from 'primeng/dynamicdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { FilterBusinessCustomerComponent } from './filter-business-customer/filter-business-customer.component';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
import { GeneralContractorService } from '@shared/services/general-contractor.service';
import { DataTableEmit, IColumn } from '@shared/interface/p-table.model';
import { NationalityConst } from '@shared/nationality-list';

@Component({
	selector: 'app-general-contractor',
	templateUrl: './general-contractor.component.html',
	styleUrls: ['./general-contractor.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService]
})
export class GeneralContractorComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private router: Router,
		private generalContractorService: GeneralContractorService,
		private breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: 'Trang chủ', routerLink: ['/home'] },
			{ label: 'Tổng thầu' },
		]);
	}

	status = null;

	modalDialog: boolean;
	listAction: any[] = [];
	
	ContractorConst = ContractorConst;
	BusinessCustomerConst = BusinessCustomerConst;
	NationalityConst = NationalityConst;
	
	businessCustomer: any = {};
	
	contractor: any;
	
	businessCustomerId: number;
	
	submitted: boolean;
	
	page = new Page();
	
	rows: any[] = [];
	columns: IColumn[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();

	ngOnInit() {
		this.columns = [
			{ field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'taxCode', header: 'Mã số thuế', width: 9, isPin: true },
			{ field: 'name', header: 'Tên tổng thầu', width : 25, isPin: true },
			{ field: 'shortName', header: 'Tên viết tắt', width : 15 },
			{ field: 'email', header: 'Email', width : 18 },
			{ field: 'phone', header: 'SĐT', width : 12, isResize: true},
			{ field: '', header: '', width: 4, displaySettingColumn: false, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN, class: 'justify-content-end' },
        ];
		//
		this.setPage();

	}

	genListAction(data = []) {
		this.listAction = data.map(contractorItem => {
			const actions = [];

			if (this.isGranted([this.PermissionInvestConst.InvestTongThau_ThongTinTongThau])) {
				actions.push({
					data: contractorItem,
					label: 'Thông tin chi tiết',
					icon: 'pi pi-info-circle',
					command: ($event) => {
						this.detail($event.item.data);
					}
				})
			}

			if (this.isGranted([this.PermissionInvestConst.InvestTongThau_Xoa])) {
				actions.push({
					data: contractorItem,
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
		this.contractor = {};
		this.businessCustomer = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	onSort(event) {
		this.sortData = event;
		this.setPage();
	}

	delete(contractor) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xóa tổng thầu này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      		console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this.generalContractorService.delete(contractor?.id).subscribe((response) => {
			  if (this.handleResponseInterceptor(response, "Xóa tổng thầu thành công")) {
				this.setPage(this.page);
				this.contractor = {};
			  }
			});
			} 
		});
	  }

	detail(contractor) {
		this.router.navigate(['/setting/general-contractor/detail', this.cryptEncode(contractor?.id)]);
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
				this.businessCustomerId = businessCustomer.businessCustomerId;
				this.businessCustomer = {...businessCustomer};
			}
		});
	}

	showData(rows) {
		for (let row of rows) {
			row.email = row?.businessCustomer?.email,
			row.phone = row?.businessCustomer?.phone
		};
	}

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this.generalContractorService.getAllContractor(this.page).subscribe((res) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.page.totalItems = res.data.totalItems;
				this.rows = res.data.items;
				this.genListAction(this.rows);
				this.showData(this.rows);
				console.log({ rows: res.data.items, totalItems: res.data.totalItems });
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

	save() {
		this.submitted = true;
		this.generalContractorService.create(this.businessCustomer).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, 'Thêm thành công')) {
					this.submitted = false;
					this.hideDialog();
					this.isLoadingPage = true;
					setTimeout(() => {
						this.isLoadingPage = false;
						this.router.navigate(['/setting/general-contractor/detail', this.cryptEncode(response.data.id)]);
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

