import { FormCancelComponent } from './../../../form-request-approve-cancel/form-cancel/form-cancel.component';
import { FormApproveComponent } from './../../../form-request-approve-cancel/form-approve/form-approve.component';
import { Component, Inject, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { BusinessTypeConst, ContractTypeConst, DistributionContractConst, FormNotificationConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ContractTemplateServiceProxy, DistributionContractFileServiceProxy } from "@shared/service-proxies/company-share-manager-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { debounceTime } from "rxjs/operators";
import { DialogService } from 'primeng/dynamicdialog';
import { AppUtilsService } from '@shared/services/utils.service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';

@Component({
	selector: 'app-distribution-contract-file',
	templateUrl: './distribution-contract-file.component.html',
	styleUrls: ['./distribution-contract-file.component.scss'],
	providers: [ConfirmationService, MessageService]
})
export class DistributionContractFileComponent extends CrudComponentBase {

	constructor(
		injector: Injector,
		messageService: MessageService,
		private dialogService: DialogService,
		private confirmationService: ConfirmationService,
		private _contractTemplateService: ContractTemplateServiceProxy,
		private _distributionContractFile: DistributionContractFileServiceProxy,
		private routeActive: ActivatedRoute,
		private _utilsService: AppUtilsService,
		private _dialogService: DialogService,
		@Inject(API_BASE_URL) baseUrl?: string,
	) {
		super(injector, messageService);
		this.distributionContractId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
		this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
	}

	private baseUrl: string;

	distributionContractId: string;

	uploadedFiles: any[] = [];
	contractTypeId: number = -1;
	contractTypes: any[] = [...ContractTypeConst.list];
	contractTypesSearch: any[] = [
		{
			code: -1,
			name: "Chọn tất cả",
		},
		...ContractTypeConst.list,
	];
	modalDialog: boolean;

	deleteItemDialog: boolean = false;

	DistributionContractConst = DistributionContractConst;

	rows: any[] = [];

	contractFile: any = {
		'fileId': 0,
		'title': null,
		'fileUrl': null,
		'distributionContractId': 0,
	};

	submitted: boolean;

	actions: any[] = [];
	actionsDisplay: any[] = [];

	listAction: any[] = [];

	ngOnInit() {
		// this.actions = [
		// 	{
		// 		label: 'Phê duyệt',
		// 		icon: 'pi pi-check',
		// 		statusActive: [this.DistributionContractConst.FILE_PENDING],
		// 		permission: this.isGranted(),
		// 		command: () => {
		// 			this.approve();
		// 		}
		// 	},
		// 	{
		// 		label: 'Huỷ duyệt',
		// 		icon: 'pi pi-times',
		// 		statusActive: [this.DistributionContractConst.FILE_APPROVE],
		// 		permission: this.isGranted(),
		// 		command: () => {
		// 			this.cancel();
		// 		}
		// 	},

		// 	{
		// 		label: 'Xóa',
		// 		icon: 'pi pi-trash',
		// 		statusActive: [this.DistributionContractConst.FILE_PENDING],
		// 		permission: this.isGranted(),
		// 		command: () => {
		// 			this.delete();
		// 		}
		// 	},
		// ];
		this.setPage({ page: this.offset });
		this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
			if (this.keyword === "") {
				this.setPage({ page: this.offset });
			} else {
				this.setPage();
			}
		});
	}

	genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
	
			if (orderItem.status == this.DistributionContractConst.PENDING) {
				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_HDPP_DMHSKHK_PheDuyet])){
					actions.push({
						data: orderItem,
						label: 'Phê duyệt',
						icon: 'pi pi-check',
						command: ($event) => {
						  this.approve($event.item.data);
						}
					})
				}

				if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_HDPP_DMHSKHK_PheDuyet])){
					actions.push({
						data: orderItem,
						label: 'Xóa',
						icon: 'pi pi-trash',
						command: ($event) => {
						  this.delete($event.item.data);
						}
					})
				}
		  }
		  
	
			if (orderItem.status == this.DistributionContractConst.SUCCESS && this.isGranted([this.PermissionCompanyShareConst.CompanyShare_HDPP_DMHSKHK_HuyPheDuyet])) {
			  actions.push(
			  {
				data: orderItem,
				label: 'Hủy duyệt',
				icon: 'pi pi-times',
				command: ($event) => {
				  this.cancel($event.item.data);
				}
				});
		  }
		  
	
	
		  console.log("orderItem.status",this.actions);
		  
			return actions;
		});
	}

	resetContractTemplateObject() {
		this.contractFile = {
			id: 0,
			title: null,
			fileUrl: null,
			distributionContractId: +this.distributionContractId,
		};
	}

	clickDropdown(row) {
		this.actionsDisplay = [];
		this.contractFile = { ...row };
		console.log({ distributionContractPayment: row });
		this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(+row.status) && action.permission);
	}

	create() {
		this.resetContractTemplateObject();
		this.submitted = false;
		this.modalDialog = true;
	}

	confirmDelete(contractFile) {
		this.deleteItemDialog = false;
		this._contractTemplateService.delete(this.contractFile.id).subscribe(
			(response) => {
				if (this.handleResponseInterceptor(response, "Xóa thành công")) {
					this.setPage({ offset: this.page.pageNumber });
					this.resetContractTemplateObject();
				}
			},
			() => {
				this.messageService.add({
					severity: "error",
					summary: "",
					detail: `Không xóa được mẫu hợp đồng ${this.contractFile.displayName}`,
					life: 3000,
				});
			}
		);
	}

	changeContractType(value) {
		if (value) {
			this.setPage({ page: this.offset });
		}
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;

		this._distributionContractFile.getAll(this.page, this.distributionContractId).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					this.page.totalItems = res.data.totalItems;
					this.rows = res.data.items;
					this.genListAction(this.rows);
					console.log({ rowsFile: res.data.items, totalItems: res.data.totalItems });
				}
			},
			() => {
				this.isLoading = false;
			}
		);
	}

	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.submitted = true;
		//
		if (this.contractFile.fieldId) {
			this._distributionContractFile.update(this.contractFile).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
						this.submitted = false;
						this.setPage({ page: this.page.pageNumber });
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				},
				() => {
					this.submitted = false;
				}
			);
		} else {
			this._distributionContractFile.create(this.contractFile).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, "Thêm thành công")) {
						this.submitted = false;
						this.setPage();
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				},
				() => {
					this.submitted = false;
				}
			);
		}
	}

	validForm(): boolean {
		return this.contractFile?.contractUrl?.trim() !== "" && this.contractFile?.title?.trim();
	}

	myUploader(event) {
		if (event?.files[0]) {
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "companyShare").subscribe(
				(response) => {
					console.log({
						response,
					});
					if (response?.code === 0) {
						switch (response?.status) {
							case 200:
								break;
							case 0:
								this.messageError(response?.message || "");
								break;
							default:
								this.messageError("Có sự cố khi upload!");
								break;
						}
					} else if (response?.code === 200) {
						this.contractFile.fileUrl = response.data;
					}
				},
				(err) => {
					this.messageError("Có sự cố khi upload!");
				}
			);
		}
		console.log("Event",event);
		
	}

	// delete(contractFile) {
	// 	this.confirmationService.confirm({
	// 		message: 'Bạn có chắc chắn xóa hồ sơ này?',
	// 		header: 'Xóa hồ sơ',
	// 		acceptLabel: "Đồng ý",
	// 		rejectLabel: "Hủy",
	// 		icon: 'pi pi-exclamation-triangle',
	// 		accept: () => {
	// 			this._distributionContractFile.delete(contractFile.fileId).subscribe((response) => {
	// 				if (this.handleResponseInterceptor(response, "Xóa thành công")) {
	// 					this.setPage();
	// 				}
	// 			});
	// 		},
	// 		reject: () => {

	// 		},
	// 	});
	// }

	delete(contractFile) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa hồ sơ này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._distributionContractFile.delete(contractFile.fileId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa hồ sơ thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

	approve(contractFile) {
		let title = "Phê duyệt", id = contractFile.fileId;
		const ref = this.dialogService.open(
			FormApproveComponent,
			this.getConfigDialogServiceRAC(title, id)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionContractFile.approve(contractFile.fileId).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	cancel(contractFile) {
		let title = "Hủy duyệt", id = contractFile.fileId;
		const ref = this.dialogService.open(
			FormCancelComponent,
			this.getConfigDialogServiceRAC(title, id)
		);
		//
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._distributionContractFile.cancel(dataCallBack?.data).subscribe((response) => {
					if (this.handleResponseInterceptor(response, "Hủy duyệt thành công")) {
						this.setPage();
					}
				});
			}
		});
	}

	downloadFile(fileUrl) {
		const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }
}
