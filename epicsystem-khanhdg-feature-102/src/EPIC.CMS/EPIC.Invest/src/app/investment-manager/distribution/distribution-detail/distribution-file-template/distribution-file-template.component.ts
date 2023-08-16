import { Component, ElementRef, Inject, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts, FormNotificationConst, StatusBondInfoFileConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IAction, IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { DistributionService } from '@shared/services/distribution.service';
import { AppUtilsService } from '@shared/services/utils.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
@Component({
  selector: 'app-distribution-file-template',
  templateUrl: './distribution-file-template.component.html',
  styleUrls: ['./distribution-file-template.component.scss']
})
export class DistributionFileTemplateComponent extends CrudComponentBase {

    constructor(injector: Injector,
        messageService: MessageService,
        private _distributionService: DistributionService,
        private _utilsService: AppUtilsService,
        private routeActive: ActivatedRoute,
        private _dialogService: DialogService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.distributionId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    private baseUrl: string;
    distributionId: string;
    headerTitle: string;

    StatusBondSecondaryFileConst = StatusBondInfoFileConst;
    AppConsts = AppConsts;

    urlfilePDF: string = '';
    
    modalDialog: boolean;
    modalDialogPDF: boolean;

    rows: any[] = [];

    policyFile: any = {
        'id': 0,
        'title': null,
        'fileUrl': null,
        'distributionId': 0,
        'tradingProviderId': 0
    };

    filedDates=['effectiveDate','expirationDate'];

    submitted: boolean;

    listAction: IAction[][] = [];
    columns: IColumn[] = [];
    page = new Page();

    @Input() contentHeight: number;
    idHeader:string = "distribution-file-tenplate";

    ngOnInit() {
        this.columns = [
            { field: 'id', header: '#ID', width: 5, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
            { field: 'title', header: 'Tên hợp đồng', width: 25, isPin: true, isResize: true },
            { field: 'downloadFile', header: 'Tải file', width: 6, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download' },
            { field: 'viewFile', header: 'Xem file ', width: 6, icon: 'pi pi-eye', type: TableConst.columnTypes.ACTION_ICON },
            { field: '', header: ' ', width: 0, displaySettingColumn: false, isResize: true},
            { field: '', header: '', displaySettingColumn: false, width: 3, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
        ];
        //
        this.setPage();
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.expirationDate = this.formatDate(row.expirationDate);
            row.downloadFile = (row) => this.downloadFile(row);
            row.viewFile = (row) => this.viewFile(row);
            return row;
        });
    }

    genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_HopDongPhanPhoi_CapNhat])) {
			    actions.push(
			    {
                    data: orderItem,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                    this.edit($event.item.data);
                    }
				});
		    }
            
            if (this.isGranted([this.PermissionInvestConst.InvestPPDT_HopDongPhanPhoi_Xoa])) {
			    actions.push({
                    data: orderItem,
                    label: 'Xóa',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
                });
		    }
			return actions;
		});
	}

    resetContractTemplateObject() {
        this.policyFile = {
            id: 0,
            title: null,
            fileUrl: null,
            distributionId: +this.distributionId,
        };
    }

    clickDropdown(row) {
        this.policyFile = { 
            ...row,
            effectiveDate: row.effectiveDate ? new Date(row.effectiveDate) : null, 
            expirationDate: row.expirationDate ? new Date(row.expirationDate) : null, 
        };
    }

    create() {
        this.resetContractTemplateObject();
        this.submitted = false;
        this.modalDialog = true;
        this.headerTitle = 'Thêm mới';
    }

    edit(policyFile) {
        this.modalDialog = true;
        this.headerTitle = 'Cập nhật';
        this.policyFile = {
            ...policyFile,
        };
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this._distributionService.getAllFileDistribution(this.page, this.distributionId).subscribe(
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

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
    }

    save() {
        this.submitted = true;
        let body = this.formatCalendar(this.filedDates, {...this.policyFile});
        if (this.policyFile.id) {
            this._distributionService.updateFile(body).subscribe(
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
            this._distributionService.createFileDistribution(body).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
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
      return this.policyFile?.title?.trim() && this.policyFile?.fileUrl?.trim();
    }

    myUploader(event) {
        if (event?.files[0]) {     
            this._distributionService.uploadFileGetUrl(event?.files[0], "distribution").subscribe(
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
                        this.policyFile.fileUrl = response.data;
                    }
                },
                (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    delete(policyFile) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa hợp đồng phân phối này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
                this._distributionService.deleteFileDistribution(policyFile.id).subscribe((response) => {
                    if(this.handleResponseInterceptor(response, "Xóa hợp đồng phân phối thành công")) {
                        this.setPage();
                    }
                });
			}

		});
	}

  
    downloadFile(row) {
        const url = this.baseUrl + "/" + row.url;
        this._utilsService.makeDownload("", url);
    }

    viewFile(row) {
        let fileUrl = row.fileUrl;
        const url = this.AppConsts.redicrectHrefOpenDocs + this.baseUrl + '/' + fileUrl;
        this.urlfilePDF = '/' + fileUrl;
        if(!fileUrl){
            this.messageError("Không có file hồ sơ", "")
        }else{
            if(this.utils.isPdfFile(fileUrl)){
                this._distributionService.viewFilePDF(this.urlfilePDF + '&download=true').subscribe();
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", "")
            }
        }
    }
}
