import { Component, Injector, Input, OnInit } from "@angular/core";
import { BusinessTypeConst, ContractTemplateConst, ContractTypeConst, FormNotificationConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { ReceiveContractTemplateServiceProxy } from "@shared/service-proxies/bond-manager-service";
import { UserServiceProxy } from "@shared/service-proxies/service-proxies";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { Subject } from "rxjs";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-notification/form-notification.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: 'app-receive-contract-template',
  templateUrl: './receive-contract-template.component.html',
  styleUrls: ['./receive-contract-template.component.scss']
})
export class ReceiveContractTemplateComponent extends CrudComponentBase implements OnInit {

	constructor(injector: Injector, private _contractTemplateService: ReceiveContractTemplateServiceProxy, messageService: MessageService,  private _dialogService: DialogService, private breadcrumbService: BreadcrumbService) {
		super(injector, messageService);
	}

	@Input() bondSecondaryId: number;
	uploadedFiles: any[] = [];
	classifyId: number;
	classifys: any[] = ContractTemplateConst.classify;
	contractTypes: any[] = ContractTemplateConst.contractType;
	classifysSearch: any[] = [
		{
			code: null,
			name: "Chọn tất cả",
		},
		...ContractTemplateConst.classify,
	];
	modalDialog: boolean;

	deleteItemDialog: boolean = false;
	ContractTemplateConst = ContractTemplateConst;
	deleteItemsDialog: boolean = false;

	rows: any = [];
  rowsCheck: any;

	contractTemplate: any = {
		id: 0,
		bondSecondaryId: 0,
		code: null,
		name: "",
		fileUrl: "",
		status: null,
	};

	submitted: boolean;

	cols: any[];

	statuses: any[];
	actions: any[] = [];  // list button actions
	actionsDisplay: any[] = [];
	listAction: any[] = [];

	ngOnInit() {
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
		this.listAction = data.map(contractTemplate => {
			const actions = [
				// {
				// 	data: contractTemplate,
				// 	label: 'Sửa',
				// 	icon: 'pi pi-pencil',
				// 	command: ($event) => {
				// 		this.edit($event.item.data);
				// 	}
				// },
				// {
				// 	data: contractTemplate,
				// 	label: 'Xoá',
				// 	icon: 'pi pi-trash',
				// 	command: ($event) => {
				// 		this.delete($event.item.data);
				// 	}
				// },
			];

			if (contractTemplate?.id != null && this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD_CapNhat])) {
				actions.push({
					data: contractTemplate,
					label: 'Sửa',
					icon: 'pi pi-pencil',
					command: ($event) => {
						this.edit($event.item.data);
					}
				});
			}
			if (contractTemplate?.id != null && this.isGranted([this.PermissionBondConst.Bond_BTKH_TTCT_MauGiaoNhanHD_CapNhat])) {
				actions.push({
					data: contractTemplate,
					label: 'Kiểm tra file mẫu',
					icon: 'pi pi-download',
					command: ($event) => {
						this.downloadFilePdf($event.item.data);
					}
				})
			}
			// if (contractTemplate.status == this.ContractTemplateConst.status.DEACTIVE && this.isGranted()) {
			// 	actions.push({
			// 		data: contractTemplate,
			// 		label: 'Kích hoạt',
			// 		icon: 'pi pi-check',
			// 		command: ($event) => {
			// 			this.changeStatus($event.item.data);
			// 		}
			// 	});
			// }
			// if (contractTemplate.status == this.ContractTemplateConst.status.ACTIVE && this.isGranted()) {
			// 	actions.push({
			// 		data: contractTemplate,
			// 		label: 'Huỷ kích hoạt',
			// 		icon: 'pi pi-times',
			// 		command: ($event) => {
			// 			this.changeStatus($event.item.data);
			// 		}
			// 	});
			// }

			return actions;
		});
	}
	downloadFilePdf(contractTemplate) {
        this._contractTemplateService.downloadContractTemplatePdf(contractTemplate.tradingProviderId, contractTemplate.id).subscribe((res) => {
            console.log(res);
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                console.log({ "Tải xuống": res });
            }
        }, () => {
            this.isLoading = false;
        });
    }
	resetContractTemplateObject() {
		this.contractTemplate = {
			id: 0,
			code: null,
			name: "",
			fileUrl: "",
		};
	}

	changeStatus(contractTemplate) {
		this._contractTemplateService.changeStatus(contractTemplate?.id).subscribe(
			(response) => {
				var message = "";
				if (this.contractTemplate.status == ContractTemplateConst.status.ACTIVE) {
					message = "Hủy kích hoạt thành công";
				} else {
					message = "Kích hoạt thành công";
				}
				if (this.handleResponseInterceptor(response, message)) {
					this.setPage({ page: this.page.pageNumber });
				}
			}, () => {
				this.messageService.add({
					severity: 'error',
					summary: '',
					detail: `Không thay đổi được trạng thái của mẫu hợp đồng ${contractTemplate?.name}`,
					life: 3000,
				});
			}
		);
	}

	create() {
		this.resetContractTemplateObject();
		this.submitted = false;
		this.modalDialog = true;
	}

	deleteSelectedItems() {
		this.deleteItemsDialog = true;
	}

	edit(contractTemplate) {
    console.log("contractTemplate",contractTemplate);
    
		this.modalDialog = true;
		this.contractTemplate = { ...contractTemplate }
	}

	delete(contractTemplate) {
		// this.deleteItemDialog = true;
		this.contractTemplate = { ...contractTemplate }
		const ref = this._dialogService.open(
			FormNotificationComponent,
			{
				header: "Thông báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title: "Bạn có chắc chắn xóa thanh toán?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
				  if (dataCallBack?.accept) {
					  this._contractTemplateService.delete(this.contractTemplate.id).subscribe(
						  (response) => {
							  if (this.handleResponseInterceptor(response, "Xóa thành công")) {
								  this.setPage();
								  this.resetContractTemplateObject();
							  }
						  },
						  () => {
							  this.messageService.add({
								  severity: "error",
								  summary: "",
								  detail: `Không xóa được mẫu hợp đồng ${this.contractTemplate.displayName}`,
								  life: 3000,
							  });
						  }
					  );
				  // this._distributionContractPaymentService.delete(distributionContractPayment.paymentId).subscribe((response) => {
				  //   if (
				  // 	this.handleResponseInterceptor(
				  // 	  response,
				  // 	  "Xóa thanh toán thành công"
				  // 	)
				  //   ) {
				  // 	this.setPage();
				  //   }
				  // });
				  }
			  });
	}

	confirmDelete() {
		this.deleteItemDialog = false;
		this._contractTemplateService.delete(this.contractTemplate.id).subscribe(
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
					detail: `Không xóa được mẫu hợp đồng ${this.contractTemplate.displayName}`,
					life: 3000,
				});
			}
		);
	}

	changeClassify() {
		this.setPage({ page: this.offset });
	}

	clickDropdown(row) {
		this.contractTemplate = { ...row };
		this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(row.status) && action.permission);
	}

	setPage(pageInfo?: any) {
		this.page.pageNumber = pageInfo?.page ?? this.offset;
		this.page.keyword = this.keyword;
		this.isLoading = true;

		this._contractTemplateService.getAll(this.page, {classifyId: this.classifyId, bondSecondaryId: this.bondSecondaryId}).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) 
        {
					// this.page.totalItems = res.data.totalItems;
          console.log("res.data------",res.data );
					this.rows = [res.data];
          if(res.data == null) {
            this.rowsCheck = true;
          } else {
            this.rowsCheck = false;
          }
           
          console.log("rows------",	this.rowsCheck );
					this.genListAction(this.rows);
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
		this.contractTemplate.bondSecondaryId = +this.bondSecondaryId;
		//
		this.submitted = true;
		if (this.contractTemplate.id > 0) {
			this._contractTemplateService.update(this.contractTemplate).subscribe(
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
			this._contractTemplateService.create(this.contractTemplate).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, "Thêm thành công")) {
						this.classifyId = this.contractTemplate.classify;

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
		return this.contractTemplate?.fileUrl?.trim() !== "" && this.contractTemplate?.code?.trim() && this.contractTemplate?.name?.trim();
	}

	myUploader(event) {
		if (event?.files[0]) {
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "bond").subscribe(
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
						this.contractTemplate.fileUrl = response.data;
					}
				},
				(err) => {
					this.messageError("Có sự cố khi upload!");
				}
			);
		}
	}

  header(): string {
    return !this.contractTemplate.id ? 'Thêm mẫu giao nhận' : 'Sửa mẫu giao nhận';
  }
}

