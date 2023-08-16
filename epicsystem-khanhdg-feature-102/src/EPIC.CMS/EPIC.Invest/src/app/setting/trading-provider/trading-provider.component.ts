import { Component, Injector } from "@angular/core";
import {
	AtributionConfirmConst,
	BusinessCustomerConst,
	TableConst,
} from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { Router } from "@angular/router";
import { DialogService } from "primeng/dynamicdialog";
import { ConfirmationService, MessageService } from "primeng/api";
import { NationalityConst } from "@shared/nationality-list";
import { TradingProviderService } from "@shared/services/trading-provider.service";
import { DataTableEmit, IColumn } from "@shared/interface/p-table.model";
import { FilterBusinessCustomerComponent } from "../general-contractor/filter-business-customer/filter-business-customer.component";

@Component({
	selector: "app-trading-provider",
	templateUrl: "./trading-provider.component.html",
	styleUrls: ["./trading-provider.component.scss"],
	providers: [DialogService, ConfirmationService, MessageService],
})
export class TradingProviderComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private confirmationService: ConfirmationService,
		private router: Router,
		private dialogService: DialogService,
		private tradingProviderService: TradingProviderService,
		private breadcrumbService: BreadcrumbService
	) {
		super(injector, messageService);
		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Đại lý" },
		]);
	}

	modalDialog: boolean;

	BusinessCustomerConst = BusinessCustomerConst;
	NationalityConst = NationalityConst;

	businessCustomer: any = {};

	tradingProvider: any = {
		businessCustomerId: null,
		aliasName: null,
	};

	businessCustomerId: number;

	listAction: any[] = [];
	submitted: boolean;

	page = new Page();

	rows: any[] = [];
	dataTableEmit: DataTableEmit = new DataTableEmit();
	columns: IColumn[] = [];

	ngOnInit(): void {
		this.columns = [
			{ field: "tradingProviderId", header: "#ID", width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: "taxCode", header: "Mã số thuế", width: 10, isPin: true },
			{ field: "name", header: "Tên đại lý", width: 30, isPin: true },
			{ field: "shortName", header: "Tên viết tắt", width: 15 },
			{ field: "email", header: "Email", width: 18 },
			{ field: "phone", header: "SĐT", width: 10, isResize: true },
			{
				field: "",
				header: "",
				width: 4,
				displaySettingColumn: false,
				isFrozen: true,
				alignFrozen: TableConst.alignFrozenColumn.RIGHT,
				type: TableConst.columnTypes.ACTION_DROPDOWN,
				class: "justify-content-end b-table-actions",
			},
		];
		//
		this.setPage();
	}

	genListAction(data = []) {
		this.listAction = data.map((tradingProviderItem) => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestDaiLy_ThongTinDaiLy])) {
				actions.push({
					data: tradingProviderItem,
					label: "Thông tin chi tiết",
					icon: "pi pi-info-circle",
					command: ($event) => {
						this.detail($event.item.data);
					},
				});
			}

			if (this.isGranted([this.PermissionInvestConst.InvestDaiLy_ThongTinDaiLy])) {
				actions.push({
					data: tradingProviderItem,
					label: "Xoá",
					icon: "pi pi-trash",
					command: ($event) => {
						this.delete($event.item.data);
					},
				});
			}

			return actions;
		});
	}

	create() {
		this.tradingProvider = {};
		this.businessCustomer = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	detail(tradingProvider) {
		this.router.navigate([
			"/setting/trading-provider/detail",
			this.cryptEncode(tradingProvider?.tradingProviderId),
		]);
	}

	showBusinessCustomer() {
		const ref = this.dialogService.open(FilterBusinessCustomerComponent, {
			header: "Tìm kiếm khách hàng doanh nghiệp",
			width: "1000px",
			contentStyle: { height: "auto", overflow: "hidden", padding: "0px" },
			style: { top: "-15%", height: "350px" },
		});
		//
		ref.onClose.subscribe((businessCustomer) => {
			if (businessCustomer) {
				this.tradingProvider.businessCustomerId =
					businessCustomer.businessCustomerId;
				this.businessCustomer = { ...businessCustomer };
			}
		});
	}

	onSort(event) {
		this.sortData = event;
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

	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this.tradingProviderService.getAllTradingProvider(this.page).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					this.page.totalItems = res?.data?.totalItems;
					this.rows = res?.data?.items;
					this.genListAction(this.rows);
					this.showData(this.rows);
					console.log({
						rows: res.data.items,
						totalItems: res.data.totalItems,
					});
				}
			},
			(err) => {
				this.isLoading = false;
				console.log("Error-------", err);
			}
		);
		// fix show dropdown options bị ẩn dướ
	}

	delete(tradingProvider) {
		this.confirmationService.confirm({
			message: `Bạn có chắc chắn xóa ${tradingProvider?.name} này?`,
			...AtributionConfirmConst,
			accept: () => {
				this.tradingProviderService
					.delete(tradingProvider?.tradingProviderId)
					.subscribe(
						(response) => {
							if (this.handleResponseInterceptor(response, "Xóa thành công")) {
								this.setPage(this.page);
								this.tradingProvider = {};
							}
						},
						() => {
							this.messageError(`Không xóa được ${tradingProvider?.name}`);
						}
					);
			},
			reject: () => {},
		});
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.submitted = true;
		this.tradingProviderService.create(this.tradingProvider).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Thêm thành công")) {
					this.submitted = false;
					this.hideDialog();
					this.isLoadingPage = true;
					setTimeout(() => {
						this.isLoadingPage = false;
						this.router.navigate([
							"/setting/trading-provider/detail",
							this.cryptEncode(response.data.tradingProviderId),
						]);
					}, 1000);
				}
				this.submitted = false;
			},
			() => {
				this.submitted = false;
				this.messageError('Không lấy được dữ liệu');
			}
		);
	}

	validForm(): boolean {
		const validRequired = this.businessCustomer?.code;
		return validRequired;
	}
}
