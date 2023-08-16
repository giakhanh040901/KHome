import { Component, Injector, Input, OnInit } from "@angular/core";
import { ContractTemplateConst, FormNotificationConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { DistributionService } from "@shared/services/distribution.service";
import { MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: "app-receive-contract-template",
  templateUrl: "./receive-contract-template.component.html",
  styleUrls: ["./receive-contract-template.component.scss"],
})
export class ReceiveContractTemplateComponent
  extends CrudComponentBase
  implements OnInit
{
  constructor(
    injector: Injector,
    private _contractTemplateService: DistributionService,
    messageService: MessageService,
    private _dialogService: DialogService,
    private breadcrumbService: BreadcrumbService
  ) {
    super(injector, messageService);
  }

  @Input() distributionId: number;
  uploadedFiles: any[] = [];
  classifyId: number;
  classifys: any[] = ContractTemplateConst.list;
  contractTypes: any[] = ContractTemplateConst.contractType;
  classifysSearch: any[] = [
    {
      code: null,
      name: "Chọn tất cả",
    },
    ...ContractTemplateConst.list,
  ];
  modalDialog: boolean;

  deleteItemDialog: boolean = false;
  ContractTemplateConst = ContractTemplateConst;
  deleteItemsDialog: boolean = false;

  rows: any = [];
  rowsCheck: any;

  contractTemplate: any = {
    id: 0,
    distributionId: 0,
    code: null,
    name: "",
    fileUrl: "",
    status: null,
  };

  submitted: boolean;

  cols: any[];

  statuses: any[];
  actions: any[] = []; // list button actions
  actionsDisplay: any[] = [];
  listAction: any[] = [];

  ngOnInit(): void {
    this.setPage({ page: this.offset });
    this.subject.keyword
      .pipe(debounceTime(SearchConst.DEBOUNCE_TIME))
      .subscribe(() => {
        if (this.keyword === "") {
          this.setPage({ page: this.offset });
        } else {
          this.setPage();
        }
      });

    console.log("distributionId", this.distributionId);
  }

  genListAction(data = []) {
    this.listAction = data.map((contractTemplate) => {
      const actions = [];
      console.log("contractTemplate", contractTemplate);

      if ( this.isGranted([ this.PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_CapNhat])) {
        actions.push({
          data: contractTemplate,
          label: "Sửa",
          icon: "pi pi-pencil",
          command: ($event) => {
            this.edit($event.item.data);
          },
        });
      }
      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_CapNhat])) {
        actions.push({
          data: contractTemplate,
          label: "Kiểm tra file mẫu",
          icon: "pi pi-download",
          command: ($event) => {
            this.downloadFilePdf($event.item.data);
          },
        });
      }
      if (contractTemplate?.status === ContractTemplateConst.DEACTIVE && this.isGranted([this.PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_KichHoat])) {
				actions.push({
					data: contractTemplate,
					label: "Kích hoạt",
					icon: "pi pi-check-circle",
					command: ($event) => {
					this.changeStatus($event.item.data);
				},
				});
			}

      if (contractTemplate?.status === ContractTemplateConst.DEACTIVE && this.isGranted([this.PermissionGarnerConst.GarnerPPSP_MauGiaoNhanHD_Xoa])) {
        actions.push({
          data: contractTemplate,
          label: "Xóa",
          icon: "pi pi-trash",
					command: ($event) => {
						this.delete($event.item.data);
					},
        });
      }

      return actions;
    });
  }

  edit(contractTemplate) {
    console.log("contractTemplate", contractTemplate);

    this.modalDialog = true;
    this.contractTemplate = { ...contractTemplate };
  }

  
  downloadFilePdf(contractTemplate) {
    this._contractTemplateService.downloadContractTemplatePdf(contractTemplate.tradingProviderId, contractTemplate.id)
      .subscribe( (res) => {
          this.handleResponseInterceptor(res);
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

  create() {
    this.resetContractTemplateObject();
    this.submitted = false;
    this.modalDialog = true;
  }

  deleteSelectedItems() {
    this.deleteItemsDialog = true;
  }

  delete(contractTemplate) {
    // this.deleteItemDialog = true;
    this.contractTemplate = { ...contractTemplate };
    const ref = this._dialogService.open(FormNotificationComponent, {
      header: "Thông báo",
      width: "600px",
      contentStyle: {
        "max-height": "600px",
        overflow: "auto",
        "padding-bottom": "50px",
      },
      styleClass: "p-dialog-custom",
      baseZIndex: 10000,
      data: {
        title: "Bạn có chắc chắn xóa mẫu giao nhận hợp đồng?",
        icon: FormNotificationConst.IMAGE_CLOSE,
      },
    });
    ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
      if (dataCallBack?.accept) {
        this._contractTemplateService
          .delete(this.contractTemplate.id)
          .subscribe(
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
      }
    });
  }

  changeStatus(row) {
		const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '400px',
                contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
                styleClass: 'p-dialog-custom',
                baseZIndex: 10000,
                data: {	
                    title: 'Kích hoạt hợp đồng',
                    icon: FormNotificationConst.IMAGE_APPROVE,
                },
            }
        );
        ref.onClose.subscribe((dataCallBack) => {
            if (dataCallBack?.accept) {
                this._contractTemplateService.changeReceiveContractStatus(row.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Kích hoạt thành công thành công")) {
                        this.setPage({ page: this.page.pageNumber });
                    }
                }, (err) => {
                    console.log('err____', err);
                });
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
    this.actionsDisplay = this.actions.filter(
      (action) => action.statusActive.includes(row.status) && action.permission
    );
  }

  setPage(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
		if(pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.keyword;
    this.isLoading = true;

    this._contractTemplateService.getAllReceiveContractTemplate(this.page, {distributionId: this.distributionId,}).subscribe((res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, "")) {
          this.page.totalItems = res.data.totalItems;
          this.rows = res.data.items;
          console.log("rows------", this.rowsCheck);
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
		this.contractTemplate.distributionId = +this.distributionId;
		//
		this.submitted = true;
		if (this.contractTemplate.id > 0) {
			this._contractTemplateService.updateReceiveContractTemplate(this.contractTemplate).subscribe(
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
			this._contractTemplateService.createReceiveContractTemplate(this.contractTemplate).subscribe(
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
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "receive-contract-tempate").subscribe(
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
