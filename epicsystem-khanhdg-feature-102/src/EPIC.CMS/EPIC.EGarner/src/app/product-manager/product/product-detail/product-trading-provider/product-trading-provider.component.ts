
import { Component, Injector, Input } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ProductService } from "@shared/services/product.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { CreateTradingProviderComponent } from "./create-trading-provider/create-trading-provider.component";
@Component({
  selector: 'app-product-trading-provider',
  templateUrl: './product-trading-provider.component.html',
  styleUrls: ['./product-trading-provider.component.scss'],
	providers: [ConfirmationService, MessageService],
})
export class ProductTradingProviderComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private confirmationService: ConfirmationService,
		private routeActive: ActivatedRoute,
		private router: Router,
		private breadcrumbService: BreadcrumbService,
		private _dialogService: DialogService,
    	private _productService: ProductService,
	) {
		super(injector, messageService);
		this.productId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}

	@Input() investorDetail: any = {};
	ref: DynamicDialogRef;


	rows: any[] = [];

	actionsDisplay: any[] = [];
	actions: any[] = [];

	page = new Page();
	offset = 0;
	productId: any;

	ngOnInit(): void {
		this.setPage();
    	console.log("this.investorDetail",this.investorDetail);
	}

	genListAction(data = []) {
		this.actions = data.map((item) => {
			const action = [
				{
					data: item,
					label: "Xem chi tiết",
					icon: "pi pi-eye",
					// statusActive: [1, 2, 3, 4],
					command: ($event) => {
						this.getDetail($event.item.data);
					},
				},
				{
					data: item,
					label: "Chỉnh sửa",
					icon: "pi pi-pencil", 
					// statusActive: [1, 2, 3, 4],
					command: ($event) => {
						this.edit($event.item.data);
					},
				},
				
			];
			return action;
		});
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._productService.getAllTradingProvider(this.productId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					this.page.totalItems = res.data.totalItems;
					this.rows = res?.data;
					this.genListAction(this.rows);
					
					console.log("1111",{ rows: res.data.items, totalItems: res.data.totalItems });
				}
			},(err) => {
				console.log('err__',err);
				this.isLoading = false;
			}
		);
	}

	createTradingProvider() {
		const ref = this._dialogService.open(
			CreateTradingProviderComponent,
			{
			header: "Thêm đại lý phân phối",
			width: '1000px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			baseZIndex: 10000,
			data: {	
				productId: this.productId,
			},
      		}
		);

		ref.onClose.subscribe((statusResponse) => {
			if(statusResponse) {
				this.setPage();
			}
		});
	}

	getDetail(row) {
		console.log("row",row);
		
		const ref = this._dialogService.open(
			CreateTradingProviderComponent,
			{
			header: "Chi tiết phân phối",
			width: '1000px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			baseZIndex: 10000,
			data: {	
				productId: this.productId,
				tradingProviderDetail: row,
				isDetail: true,
			},
      		}
		);
	}

	edit(row) {
		const ref = this._dialogService.open(
			CreateTradingProviderComponent,
			{
			header: "Cập nhật hạn mức",
			width: '1000px',
			contentStyle: { "max-height": "600px", "overflow": "auto", "margin-bottom": "60px" },
			baseZIndex: 10000,
			data: {	
				productId: this.productId,
				tradingProviderDetail: row,
			},
      		}
		);
		ref.onClose.subscribe((statusResponse) => {
			if(statusResponse) {
				this.setPage();
			}
		});
	}
}

