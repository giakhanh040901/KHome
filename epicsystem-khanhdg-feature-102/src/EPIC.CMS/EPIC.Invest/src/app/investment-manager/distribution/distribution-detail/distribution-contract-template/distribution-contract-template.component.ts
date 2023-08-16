
import { Component, ElementRef, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { ActiveDeactiveConst, AppConsts, DistributionContractTemplateConst, FormNotificationConst, IssuerConst, SampleContractConst, StatusResponseConst, TableConst, YesNoConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { DistributionService } from "@shared/services/distribution.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { CreateDistributionContractComponent } from "./create-distribution-contract/create-distribution-contract.component";
import { ContractTemplateService } from "@shared/services/contract-template.service";
import { IAction, IColumn } from "@shared/interface/p-table.model";
import { DistributionContractFilter } from "@shared/interface/filter.model";
import { CreateMultipleDistributionContractTemplateComponent } from "./create-multiple-distribution-contract-template/create-multiple-distribution-contract-template.component";
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
      	private dialogService: DialogService,
		private _distributionService: DistributionService,
		private _contractTemplateService: ContractTemplateService,

		
	) {
		super(injector, messageService);
		// this.productId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
	}
	@Input() distributionId: number;

	modalDialog: boolean;
	deleteItemDialog: boolean = false;

	// CONST
	DistributionContractTemplateConst = DistributionContractTemplateConst;
	ActiveDeactiveConst = ActiveDeactiveConst;
	SampleContractConst = SampleContractConst;
	YesNoConst = YesNoConst;
	IssuerConst = IssuerConst;

	policies = [];

	list: any = {};
	submitted: boolean;

	rows: any[] = [];
	listAction: IAction[][] = [];
	columns: IColumn[] = [];
	page = new Page();

	dataFilter: DistributionContractFilter = new DistributionContractFilter();

	@Input() contentHeight: number;
	idHeader:string = "distribution-contract-tenplate";

	ngOnInit(): void {
		this.columns = [
            { field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
            { field: 'contractName', header: 'Tên hợp đồng', width: 20, isPin: true },
            { field: 'configContractCodeName', header: 'Cấu trúc mã sử dụng', width: 20, isPin: true },
            { field: 'policyCode', header: 'Chính sách', width: 12 },
            { field: 'contractSource', header: 'Kiểu HĐ', width: 6, type: TableConst.columnTypes.CONVERT_DISPLAY },
            { field: 'typeName', header: 'Hiển thị', width: 14 },
            { field: '', header: '', width: 0, displaySettingColumn: false, isResize: true },	
            { field: 'status', header: 'Trạng thái', displaySettingColumn: false, width: 7, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.STATUS , class: 'b-border-frozen-right'},
            { field: '', header: '', displaySettingColumn: false, width: 3, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
        ];
		//
		this.setPage();    
	}
	
	setPage(event?: any) {
		if(!event) this.page.pageNumber = 0;
		this.isLoading = true;
		forkJoin([
			this._contractTemplateService.getAllContractForm(this.page, this.distributionId, this.dataFilter), 
			this._distributionService.getAllPolicy(this.distributionId)
		]).subscribe(([resContract, resPolicy]) => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(resContract)) {
				this.page.totalItems = resContract?.data?.totalItems;
				this.rows = resContract?.data?.items;
				this.genListAction(this.rows);
				this.setData(this.rows);
			}
			//
			if (resPolicy.status == StatusResponseConst.RESPONSE_TRUE) {
				this.policies = resPolicy?.data.map(item => {
					item.labelName = item.code + ' - ' + item.name;
					return item;
				});
			}
		}, (err) => {
			console.log('err__', err);
			this.isLoading = false;
		});
	}

	setData(rows) {
		this.rows = rows.map(row => {
			row.contractName = row?.contractTemplateName;
			row.configContractCodeName = row?.configContractName;
			row.policyCode = row?.policyName;
			row.contractSourceDisplay = SampleContractConst.getContractSource(row.contractSource, 'name');
			row.typeName = DistributionContractTemplateConst.getDisplayType(row?.displayType);
			row.statusElement = ActiveDeactiveConst.getInfo(row.status);
			return row;
		});
	}

	genListAction(data = []) {
		this.listAction = data.map((item) => {
			const action = [];
			//
			action.push({
				data: item,
				label: "Test Fill HĐ cá nhân",
				icon: "pi pi-download",
				permission: this.isGranted([]),
				command: ($event) => {
					this.downloadContractFileFill($event.item.data, IssuerConst.INVESTOR);
				},
			});
			//
			action.push({
				data: item,
				label: "Test Fill HĐ doanh nghiệp",
				icon: "pi pi-download",
				permission: this.isGranted([]),
				command: ($event) => {
					this.downloadContractFileFill($event.item.data, IssuerConst.BUSINESSCUSTOMER);
				},
			});
			//
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_MauHopDong_CapNhat])) {
				action.push({
					data: item,
					label: item.status == ActiveDeactiveConst.ACTIVE ? "Khóa" : "Kích hoạt",
					icon: item.status == ActiveDeactiveConst.ACTIVE ? "pi pi-times-circle" : "pi pi-check-circle",
					permission: this.isGranted([]),
					command: ($event) => {
						this.changeStatus($event.item.data);
					},
				});
			}
			//
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_MauHopDong_CapNhat])) {
				action.push({
					data: item,
					label: "Chỉnh sửa",
					icon: "pi pi-pencil",
					permission: this.isGranted([]),
					command: ($event) => {
						this.edit($event.item.data);
					},
				});
			}

			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_MauHopDong_Xoa])) {
				action.push({
					data: item,
					label: "Xóa",
					icon: "pi pi-trash",
					permission: this.isGranted([]),
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
			data: {
				title: "Bạn có chắc chắn muốn xóa mẫu hợp đồng này?",
				icon: FormNotificationConst.IMAGE_CLOSE,
			},
		}).onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
				this._contractTemplateService.deleteContractForm(item.id).subscribe((response) => {
					if ( this.handleResponseInterceptor(response, "Xóa thành công")) {
						this.setPage();
					}
				}, (err) => {
					this.messageError(`Không xóa được ${item.name}`);
				});
			} else {
				this.messageError(AppConsts.messageError);
			}
		});
	}

	create() {
		this.dialogService.open(CreateDistributionContractComponent, {
			header: "Thêm mới mẫu hợp đồng",
			width: '800px',
			data: {
				distributionId: this.distributionId
			},
		}).onClose.subscribe((statusResponse) => {
			if(statusResponse) {
				this.setPage();
			}
		});
	}

	createMultiple() {
		this.dialogService.open(CreateMultipleDistributionContractTemplateComponent, {
			header: "Thêm mới mẫu hợp đồng",
			width: '800px',
			data: {
				distributionId: this.distributionId
			},
		}).onClose.subscribe((statusResponse) => {
			if(statusResponse) {
				this.setPage();
			}
		});
	}

	edit(row) {
		console.log("row",row);
		
		this.dialogService.open(CreateMultipleDistributionContractTemplateComponent, {
			header: "Cập nhật mẫu hợp đồng",
			width: '800px',
			data: {
			  distributionId: this.distributionId,
			  contractDistributionTemplate : row,
			},
		}).onClose.subscribe((statusResponse) => {
			if(statusResponse) {
				this.setPage();
			}
		});
	}

	isDownload: boolean = false;
	downloadContractFileFill(contractTemplate, type) {
		this.isDownload = true;
		this._distributionService.downloadContractTemplateFillPdf(contractTemplate.id, type).subscribe((response) => {
			this.isDownload = false;
			if(response?.status === 0) this.handleResponseInterceptor(response);
		}, (err) => {
			this.isDownload = false;
		})
	}

	changeStatus(row) {
		const ref = this.dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '400px',
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
	
	hideDialog() {
		this.modalDialog = false;
		this.submitted = false;
	}
}


