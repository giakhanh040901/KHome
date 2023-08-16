import { Component, EventEmitter, Injector, Output } from "@angular/core";
import { IssuerConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { GeneralContractorServiceProxy } from "@shared/service-proxies/setting-service";
import { MessageService } from "primeng/api";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";

@Component({
	selector: "app-filter-business-customer",
	templateUrl: "./filter-business-customer.component.html",
	styleUrls: ["./filter-business-customer.component.scss"],
})
export class FilterBusinessCustomerComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		private dialogConfig: DynamicDialogConfig,
		private generalContractorService: GeneralContractorServiceProxy,
		private readonly dialogRef: DynamicDialogRef,
		messageService: MessageService,
		) {
		super(injector, messageService);
	}
	
	@Output() onCloseDialog = new EventEmitter<any>();

	rows: any[] = [];

	IssuerConst = IssuerConst;
  
	page = new Page();

	ngOnInit(): void {}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;
	
		this.generalContractorService.getByTaxCode(this.keyword?.trim()).subscribe((res) => {
		  this.isLoading = false;
		  if (this.handleResponseInterceptor(res, '') && res?.data) {
			this.page.totalItems = res.data.totalItems;
			this.rows = [res.data];
		  } else {
			this.messageError('Không tìm thấy dữ liệu');
		  }
		}, () => {
		  this.isLoading = false;
		});
	  }
	
	  isChoose(businessCustomer) {
		this.dialogRef.close(businessCustomer);
	  }
}
