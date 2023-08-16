import { Component, ElementRef, Inject, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts, FormNotificationConst, StatusBondInfoFileConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { DataTableEmit, IAction, IColumn } from '@shared/interface/p-table.model';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { DistributionService } from '@shared/services/distribution.service';
import { AppUtilsService } from '@shared/services/utils.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';
@Component({
    selector: 'app-distribution-file',
    templateUrl: './distribution-file.component.html',
    styleUrls: ['./distribution-file.component.scss'],
    providers: [ConfirmationService, MessageService]
})
export class DistributionFileComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
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

    AppConsts = AppConsts;

    uploadedFiles: any[] = [];
    contractTypeId: number = -1;

    modalDialog: boolean;
    modalDialogPDF: boolean;

    deleteItemDialog: boolean = false;

    StatusBondSecondaryFileConst = StatusBondInfoFileConst;

    rows: any[] = [];

    policyFile: any = {
        'id': 0,
        'effectiveDate': null,  // Ngày có hiệu lực
        'expirationDate': null, // Ngày hết hiệu lực
        'name': null,
        'url': null,
        'distributionId': 0,
        'tradingProviderId': 0
    };

    filedDates=['effectiveDate','expirationDate'];

    submitted: boolean;
    urlfilePDF:string = '';

    listAction: IAction[][] = [];
    columns: IColumn[] = [];
    dataTableEmit: DataTableEmit = new DataTableEmit();

    @Input() contentHeight: number;
    idHeader: string = "distribution-file";

    ngOnInit() {
        this.columns = [
            { field: 'id', header: '#ID', width: 5, isPin: true },
            { field: 'name', header: 'Tên chính sách', width: 20, isPin: true },
            { field: 'effectiveDate', header: 'Ngày có hiệu lực', width: 12, isPin: true },
            { field: 'expirationDate', header: 'Ngày hết hiệu lực', width: 12 },
            { field: 'downloadFile', header: 'Tải file', width: 6, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download' },
            { field: 'viewFile', header: 'Xem file ', width: 6, icon: 'pi pi-eye', type: TableConst.columnTypes.ACTION_ICON },
            { field: '', header: ' ', width: 0, displaySettingColumn: false, isResize: true},
            { field: '', header: '', displaySettingColumn: false, width: 3, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.RIGHT, type: TableConst.columnTypes.ACTION_DROPDOWN },
        ];
        //
        this.setPage();
    }
    
    setPage(event?: any) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this._distributionService.getAllFile(this.page, this.distributionId).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.rows = res.data.items;
                    this.genListAction(this.rows);
                    this.setData(this.rows);
                    console.log({ rowsFile: res.data.items, totalItems: res.data.totalItems });
                }
            },() => {
                this.isLoading = false;
            }
        );
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.effectiveDate = this.formatDate(row.effectiveDate);
            row.expirationDate = this.formatDate(row.expirationDate);
            row.downloadFile = (row) => this.downloadFile(row);
            row.viewFile = (row) => this.viewFile(row);
            return row;
        });
    }

    genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
			if (this.isGranted([this.PermissionInvestConst.InvestPPDT_FileChinhSach_CapNhat])) {
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

            if (this.isGranted([this.PermissionInvestConst.InvestPPDT_FileChinhSach_Xoa])) {
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
            name: null,
            url: null,
            distributionId: +this.distributionId,
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
            effectiveDate: policyFile.effectiveDate ? new Date(policyFile.effectiveDate) : null, 
            expirationDate: policyFile.expirationDate ? new Date(policyFile.expirationDate) : null, 
        };
    }

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
        this.modalDialogPDF = false;
    }

    save() {
        this.submitted = true;
        let body = this.formatCalendar(this.filedDates, {...this.policyFile});
        if (this.policyFile.id) {
            this._distributionService.updateFile(body).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                        this.submitted = false;
                        this.setPage({ page: this.page.pageNumber });
                        this.hideDialog();
                    } else {
                        this.submitted = false;
                    }
                },() => {
                    this.submitted = false;
                }
            );
        } else {
            this._distributionService.createFile(body).subscribe(
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
      return this.policyFile?.name?.trim() && this.policyFile?.url?.trim();
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
                        this.policyFile.url = response.data;
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
					data: {
						title : "Bạn có chắc chắn xóa chính sách này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
                this._distributionService.deleteFile(policyFile.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, "Xóa chính sách thành công")) {
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
        let fileUrl = row.url;
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
