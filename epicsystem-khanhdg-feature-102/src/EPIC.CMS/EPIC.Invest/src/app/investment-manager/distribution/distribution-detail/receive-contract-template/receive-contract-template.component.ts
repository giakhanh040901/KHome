import { Component, ElementRef, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { ContractTemplateConst, FormNotificationConst, TableConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { IAction, IColumn } from "@shared/interface/p-table.model";
import { Page } from "@shared/model/page";
import { DistributionService } from "@shared/services/distribution.service";
import { MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";

@Component({
  selector: 'app-receive-contract-template',
  templateUrl: './receive-contract-template.component.html',
  styleUrls: ['./receive-contract-template.component.scss']
})
export class ReceiveContractTemplateComponent extends CrudComponentBase implements OnInit {

	constructor(
		injector: Injector, 
		messageService: MessageService,  
		private _contractTemplateService: DistributionService, 
		private _dialogService: DialogService, 
		) {
		super(injector, messageService);
	}

	ContractTemplateConst = ContractTemplateConst;

	@Input() distributionId: number;

	uploadedFiles: any[] = [];
	classifyId: number;
	
	modalDialog: boolean;

	deleteItemDialog: boolean = false;
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

	columns: IColumn[] = [];

	listAction: IAction[][] = [];

	@Input() contentHeight: number;
	idHeader:string = "receive-contract-tenplate";

	ngOnInit() {
		this.columns = [
            { field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
            { field: 'name', header: 'Tên mẫu giao nhận', width: 25, isPin: true, isResize: true },
            { field: 'code', header: 'Mã mẫu giao nhận', width: 25, isPin: true, isResize: true },
            { field: '', header: ' ', width: 0, displaySettingColumn: false, isResize: true},
            { field: '', header: '', displaySettingColumn: false, width: 3, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
        ];
		//
		this.setPage();
	}
	
	setPage(event?: Page) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		this._contractTemplateService.getAllReceiveContractTemplate(this.page, {distributionId: this.distributionId}).subscribe(
			(res) => {
				this.isLoading = false;
				if (this.handleResponseInterceptor(res, "")) {
					this.page.totalItems = res.data.totalItems;
					this.rows = res.data.items;
					this.genListAction(this.rows);
					this.setData(this.rows);
				}
			},() => {
				this.isLoading = false;
			}
		);
	}

	setData(rows) {
        this.rows = rows.map(row => {
            row.statusElement = ContractTemplateConst.getInfo(row.status);
            return row;
        });
    }

	genListAction(data = []) {
		this.listAction = data.map(contractTemplate => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_CapNhat])) {
				actions.push({
					data: contractTemplate,
					label: 'Sửa',
					icon: 'pi pi-pencil',
					command: ($event) => {
						this.edit($event.item.data);
					}
				});
			}
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_CapNhat])) {
				actions.push({
					data: contractTemplate,
					label: 'Kiểm tra file mẫu',
					icon: 'pi pi-download',
					command: ($event) => {
						this.downloadFilePdf($event.item.data);
					}
				})
			}
			
			if (contractTemplate?.status === ContractTemplateConst.DEACTIVE && this.isGranted([this.PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_KichHoat])) {
				actions.push({
					data: contractTemplate,
					label: "Kích hoạt",
					icon: "pi pi-check-circle",
					command: ($event) => {
					this.changeStatus($event.item.data);
				},
				});
			}

			if (contractTemplate?.status === ContractTemplateConst.DEACTIVE && this.isGranted([this.PermissionInvestConst.InvestPPDT_MauGiaoNhanHD_Xoa])) {
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

	downloadFilePdf(contractTemplate) {
		this.isLoading = true;
        this._contractTemplateService.downloadContractTemplatePdf(contractTemplate.tradingProviderId, contractTemplate.id).subscribe((res) => {
            this.isLoading = false;
            this.handleResponseInterceptor(res); 
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

	changeStatus(row) {
		const ref = this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '400px',
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
                        this.setPage(this.page);
                    }
                }, (err) => {
                    console.log('err____', err);
                });
            } 
        });
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
		this.modalDialog = true;
		this.contractTemplate = { ...contractTemplate }
	}

	delete(contractTemplate) {
		this.contractTemplate = { ...contractTemplate }
		const ref = this._dialogService.open(
			FormNotificationComponent,
			{
				header: "Thông báo",
				width: '600px',
				data: {
					title: "Bạn có chắc chắn xóa mẫu giao nhận hợp đồng?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._contractTemplateService.deleteRecceiveContractTemp(this.contractTemplate.id).subscribe(
					(response) => {
						if (this.handleResponseInterceptor(response, "Xóa thành công")) {
							this.setPage();
							this.resetContractTemplateObject();
						}
					},() => {
						this.messageError(`Không xóa được mẫu hợp đồng ${this.contractTemplate.displayName}`);
					}
				);
			}
		});
	}
	
	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}

	save() {
		this.contractTemplate.distributionId = +this.distributionId;
		this.submitted = true;
		if (this.contractTemplate.id > 0) {
			this._contractTemplateService.updateReceiveContractTemplate(this.contractTemplate).subscribe(
				(response) => {
					if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
						this.submitted = false;
						this.setPage(this.page);
						this.hideDialog();
					} else {
						this.submitted = false;
					}
				},() => {
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
				},() => {
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
			this._contractTemplateService.uploadFileGetUrl(event?.files[0], "receive-contract-template").subscribe(
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


