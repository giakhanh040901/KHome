import { Component, Injector, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { KeyFilter, ProductConst, SearchConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ProductService } from "@shared/services/product.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
@Component({
  selector: 'app-product-history',
  templateUrl: './product-history.component.html',
  styleUrls: ['./product-history.component.scss'],
	providers: [ConfirmationService, MessageService],
})
export class ProductHistoryComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private routeActive: ActivatedRoute,
    	private _investorService: ProductService,
	) {
		super(injector, messageService);
		this.productId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	productId: number;
	ref: DynamicDialogRef;

	modalDialog: boolean;
	deleteItemDialog: boolean = false;

	confirmRequestDialog: boolean = false;
	rows: any[] = [];
	list: any = {};

	YesNoConst = YesNoConst;
	KeyFilter = KeyFilter;
  	ProductConst = ProductConst;

	investorBank: any = {};
	banks: any = {};
	fieldErrors = {};
	submitted: boolean;

	isDetail = false;
	actionsDisplay: any[] = [];
	actions: any[] = [];
	bankFullName: any = {};

	page = new Page();
	offset = 0;

	ngOnInit(): void {
		this.setPage();    
	}

	genListAction(data = []) {
		this.actions = data.map((item) => {
			const action = [
				
			];
			return action;
		});
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._investorService.getHistoryProduct(this.productId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					// this.page.totalItems = res.data.totalItems;
					this.rows = res?.data;
					this.genListAction(this.rows);
					setTimeout(() => {
					}, 2000);
					console.log("1111",{ rows: res.data.items, totalItems: res.data.totalItems });
					
				}
			},
			() => {
				this.isLoading = false;
			}
		);
	}

	clickDropdown(row) {
		this.investorBank = { ...row };
		console.log({ investorBank: row });
		this.actionsDisplay = this.actions.filter((action) => action.statusActive.includes(+row.status) && action.permission);
	}

	setFieldError() {
		for (const [key, value] of Object.entries(this.investorBank)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}

	header(): string {
		return !this.investorBank?.productId ? "Thêm mới" : "Sửa đại lý";
	}

	createTradingProvider() {
		this.investorBank = {};
		this.submitted = false;
		this.modalDialog = true;
	}

	edit(row) {
		this.investorBank = {
			...row,
		};
		console.log({ investorBank: this.investorBank });
		this.modalDialog = true;
	}

	changeKeyword() {
		if (this.keyword === "") {
			this.setPage({ page: this.offset });
		}
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	resetValid(field) {
		this.fieldErrors[field] = false;
	}
}

