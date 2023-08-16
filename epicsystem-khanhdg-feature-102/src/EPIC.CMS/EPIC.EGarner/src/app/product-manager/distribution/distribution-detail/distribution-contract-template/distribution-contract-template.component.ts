import { Component, Injector, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ActiveDeactiveConst, AppConsts, ContractFormConst, DistributionContractTemplateConst, FormNotificationConst, KeyFilter, PermissionGarnerConst, ProductConst, SampleContractConst, SearchConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ContractFormService } from "@shared/services/contract-form.service";
import { DistributionContractService } from "@shared/services/distribution-contract.service";
import { DistributionService } from "@shared/services/distribution.service";
import { ProductService } from "@shared/services/product.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { CreateDistributionContractComponent } from "./create-distribution-contract/create-distribution-contract.component";

@Component({
  selector: 'app-distribution-contract-template',
  templateUrl: './distribution-contract-template.component.html',
  styleUrls: ['./distribution-contract-template.component.scss'],
	providers: [ConfirmationService, MessageService],
})
export class DistributionContractTemplateComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private routeActive: ActivatedRoute,
		private _distributionContractService: DistributionContractService,
      	private dialogService: DialogService,
		private _distributionService: DistributionService,
		
	) {
		super(injector, messageService);
		// this.productId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}
	@Input() distributionId: number;
	// productId: number;
	ref: DynamicDialogRef;

	modalDialog: boolean;
	deleteItemDialog: boolean = false;

	confirmRequestDialog: boolean = false;
	rows: any[] = [];
	policyList: any;
	list: any = {};

	YesNoConst = YesNoConst;
	KeyFilter = KeyFilter;
	ContractFormConst = ContractFormConst;
	DistributionContractTemplateConst = DistributionContractTemplateConst;
	SampleContractConst = SampleContractConst;
	ActiveDeactiveConst = ActiveDeactiveConst;

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

	fieldFilters = {
		policyId: null,
		status: null,
		contractSource: null,
	}

	ngOnInit(): void {
		this.setPage();    
	}

	genListAction(data = []) {
		this.actions = data.map((item) => {
			const action = [];

			if (this.isGranted([PermissionGarnerConst.GarnerPPSP_MauHopDong_CapNhat])) {
				action.push({
					data: item,
					label: item.status == ActiveDeactiveConst.ACTIVE ? "Khóa" : "Kích hoạt",
					icon: item.status == ActiveDeactiveConst.ACTIVE ? "pi pi-times-circle" : "pi pi-check-circle",
					command: ($event) => {
					this.changeStatus($event.item.data);
				},
				});
			}

			if (this.isGranted([PermissionGarnerConst.GarnerPPSP_MauHopDong_CapNhat])) {
				action.push({
					data: item,
					label: "Chỉnh sửa",
					icon: "pi pi-pencil",
					command: ($event) => {
						this.edit($event.item.data);
					},
				});
			}

			if (this.isGranted([PermissionGarnerConst.GarnerPPSP_MauHopDong_Xoa])) {
				action.push({
					data: item,
					label: "Xóa",
					icon: "pi pi-trash",
					command: ($event) => {
						this.delete($event.item.data);
					},
				});
			}
			return action;
		});
	}

	delete(item) {
		const ref = this.dialogService.open(FormNotificationComponent, {
		  header: "Thông báo",
		  width: "600px",
		  contentStyle: { "max-height": "600px", overflow: "auto", "padding-bottom": "50px", },
		  styleClass: "p-dialog-custom",
		  baseZIndex: 10000,
		  data: {
			title: "Bạn có chắc chắn muốn xóa mẫu hợp đồng này?",
			icon: FormNotificationConst.IMAGE_CLOSE,
		  },
		});
		ref.onClose.subscribe((dataCallBack) => {
		  if (dataCallBack?.accept) {
			this._distributionContractService.deleteContractForm(item.id).subscribe((response) => {
				if ( this.handleResponseInterceptor(response, "Xóa thành công")) {
				  this.setPage();
				}
			  }, (err) => {
				console.log('err____', err);
				this.messageError(`Không xóa được ${item.name}`);
			  });
		  } 
		});
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
		this.page.keyword = this.keyword;
		this.isLoading = true;
		this._distributionContractService.getAllContractForm(this.page, this.distributionId, this.fieldFilters).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					this.page.totalItems = res.data.totalItems;
					this.rows = res?.data?.items;
					this.genListAction(this.rows);
				}
			},() => {
				this.isLoading = false;
			}
		);
	}

	genContractCodeStructure(configContractCodeDetails = []) {
        let contractCodeStructure = '';
        if(configContractCodeDetails?.length) {
            configContractCodeDetails.forEach(element => {
                contractCodeStructure += '<'+ (element?.value ? element.value : element?.key) +'>';
            });
        }
        //
        return contractCodeStructure;
    }

	setFieldError() {
		for (const [key, value] of Object.entries(this.investorBank)) {
			this.fieldErrors[key] = false;
		}
		console.log({ filedError: this.fieldErrors });
	}

	create() {
		const ref = this.dialogService.open(CreateDistributionContractComponent, {
			header: "Thêm mới mẫu hợp đồng",
			width: '800px',
			contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
			baseZIndex: 10000,
			data: {
				distributionId: this.distributionId
			},
		});
		//
		ref.onClose.subscribe((res) => {
			this.setPage();
		});
	}

	edit(row) {
		const ref = this.dialogService.open(CreateDistributionContractComponent, {
			header: "Cập nhật mẫu hợp đồng",
			width: '800px',
			contentStyle: { "max-height": "600px", overflow: "auto", "margin-bottom": "60px", },
			baseZIndex: 10000,
			data: {
			  distributionId: this.distributionId,
			  contractDistributionTemplate : row,
			},
		  });
		  //
		  ref.onClose.subscribe((res) => {
			  this.setPage();
		  });
	}

	changeStatus(row) {
		const ref = this.dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '400px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {	
                    title: row.status == ActiveDeactiveConst.ACTIVE ? 'Khóa hợp đồng' : 'Kích hoạt hợp đồng',
                    icon: row.status == ActiveDeactiveConst.ACTIVE ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._distributionService.changeStatusContractTemplate(row.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Cập nhật thành công")) {
                        this.setPage({ page: this.page.pageNumber });
                    }
                }, (err) => {
                    console.log('err____', err);
                });
            } 
        });
	}

	changePolicy() {
		this.setPage({ page: this.offset });
	}

	changeContractSource() {
		this.setPage({ page: this.offset });
	}

	changeStatusFilter() {
		this.setPage({ page: this.offset });
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



