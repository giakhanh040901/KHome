import { Component, Inject, Injector, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts, BusinessTypeConst, DistributionContractTemplateConst, IsSignPdfConst, OrderConst, TableConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ContractTemplateService } from '@shared/services/contract-template.service';
import { OrderService } from '@shared/services/order.service';
import { MessageService } from 'primeng/api';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-order-contract-view',
  templateUrl: './order-contract-view.component.html',
  styleUrls: ['./order-contract-view.component.scss']
})
export class OrderContractViewComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private routeActive: ActivatedRoute,
        private _contractTemplateServiceProxy: ContractTemplateService,
        private _orderServiceProxy: OrderService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        //
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
        this.orderId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
    }

    private baseUrl: string;

    AppConsts = AppConsts;

    orderId: number;
    @Input() orderDetail: any = {};
    @Input() isPartner: boolean;
    @Input() contentHeight: number = 0;
    modalDialog: boolean;
    modalDialogPDF: boolean;
    rows: any[] = [];

    BusinessTypeConst = BusinessTypeConst;
    IsSignPdfConst = IsSignPdfConst;
    DistributionContractTemplateConst = DistributionContractTemplateConst;
    OrderConst = OrderConst;

    contractTemplate: any = {
        id: 0,
        code: null,
        name: "",
        type: null,
        contractTempUrl: "",
        businessType: BusinessTypeConst.B2B,
        status: null,
        deleted: null,
        orderContractFileId: null,
        fileScanUrl: null,
        fileTempUrl: null,
        fileSignatureUrl: null
    };

    orderContractFile: any = {
        orderContractFileId: 0,
        orderId: 0,
        ContractTempId: 0,
        fileScanUrl: null
    };
    
    actions: any[] = [];
    
    urlfilePDF:string = '';

    isSignAll: boolean = true;
    haveSign: boolean = false

    columns: IColumn[] = [];

    ngOnInit() {
        this.columns = [
            { field: 'orderContractFileId', header: '#ID', width: 5, displaySettingColumn: false, isPin: true, isFrozen: true, alignFrozen: TableConst.alignFrozenColumn.LEFT, class: 'b-border-frozen-left' },
			{ field: 'name', header: 'Tên hợp đồng', width: 20, isPin: true, isResize: true },
			{ field: 'isFileUrl', header: 'Lưu trữ', width: 6, isPin: true, type: TableConst.columnTypes.CHECKBOX_SHOW },
			{ field: 'isSign', header: 'Đã ký', width: 5, isPin: true, type: TableConst.columnTypes.CHECKBOX_SHOW},
			{ field: 'downloadFileWord', header: 'Tải HS word', width: 8, isPin: true, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download' },
			{ field: 'downloadFilePdf', header: 'Tải HS pdf', width: 7.5, isPin: true, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download' },
			{ field: 'downloadFileSign', header: 'tải HS ký điện tử', width: 11, isPin: true, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download' },
			{ field: 'uploadFile', header: 'Upload HS', width: 8, isPin: true, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-upload', isPermission: this.isGranted([this.PermissionInvestConst.InvestHDPP_SoLenh_TTCT_HSKHDangKy_TaiLenHS]) },
			{ field: 'viewFileScan', header: 'Xem HS', width: 6, isPin: true, type: TableConst.columnTypes.ACTION_ICON,icon: 'pi pi-eye' },
        ];
        this.setPage();
    }

    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this.isSignAll = true;
        this.haveSign = false;
        forkJoin([this._contractTemplateServiceProxy.getByOrder(this.page, this.orderId),this._orderServiceProxy.get(this.orderId)]).subscribe(([resContract, resOrder]) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(resOrder)){
                this.orderDetail.status = resOrder?.data?.status;
                this.orderDetail.source = resOrder?.data?.source;
            }
            //
            if(this.handleResponseInterceptor(resContract, '')) {
                this.page.totalItems = resContract.data.totalItems;
                this.rows = resContract.data.items.map((item) => {
                    item.isFileUrl = item.fileScanUrl ? true : false;
                    item.isSign = item.isSign == IsSignPdfConst.YES ? true : false;
                    //
                    if (item.isSign) {
                        this.haveSign = true;
                    }
                    if(!item.isSign && this.orderDetail.source == OrderConst.SOURCE_ONLINE) {
                        this.isSignAll = false;
                    }    // Check xem đã ký điện tử hay chưa
                    //
                    return item;
                });
                this.setData(this.rows);
            }
        }, () => {
            this.isLoading = false;
        });
    }

    setData(rows) {
        this.rows = rows.map(row => {
            row.downloadFileWord = (row) => this.downloadFileTempContract(row);
            row.downloadFilePdf = (row) => this.downloadFileTempPdfContract(row);
            row.downloadFileSign = (row) => this.downloadFileSignatureContract(row);
            row.uploadFile = (row) => this.showModalUpload(row);
            row.viewFileScan = (row) => this.viewFile(row);
            return row;
        })
    }

    showModalUpload(row) {
        this.submitted = false;
        this.modalDialog = true;
        this.contractTemplate = { ...row };
        
        this.orderContractFile = {
            orderContractFileId: this.contractTemplate.orderContractFileId,
            orderId: this.orderId,
            ContractTempId: this.contractTemplate.id,
            fileScanUrl: this.contractTemplate.fileScanUrl
        };
    }

    downloadFileTempContract(contractTemplate) {
        if (contractTemplate.orderContractFileId > 0) {
            this.isLoading = false;
            this._orderServiceProxy.downloadFileTempContract(contractTemplate.orderContractFileId).subscribe((res) => {
                this.isLoading = false;
                this.handleResponseInterceptor(res, '');
            }, () => {
                this.isLoading = false;
            });
        } else {
            this.messageError("Chưa cập nhật hồ sơ!");
        }

    }

    downloadFileTempPdfContract(contractTemplate) {
        if (contractTemplate.orderContractFileId > 0) {
            this.isLoading = false;
            this._orderServiceProxy.downloadFileTempPdfContract(contractTemplate.orderContractFileId).subscribe((res) => {
                this.isLoading = false;
                this.handleResponseInterceptor(res, '');
            }, () => {
                this.isLoading = false;
            });
        } else {
            this.messageError("Chưa cập nhật hồ sơ!");
        }
    }

    downloadFileSignatureContract(contractTemplate) {
        if (contractTemplate.orderContractFileId > 0) {
            this.isLoading = false;
            this._orderServiceProxy.downloadFileSignatureContract(contractTemplate.orderContractFileId).subscribe((res) => {
                this.isLoading = false;
                this.handleResponseInterceptor(res, '');
            }, () => {
                this.isLoading = false;
            });
        } else {
            this.messageError("Chưa cập nhật hồ sơ!");
        }
    }

    downloadFileScanUrl() {
         window.open(this.urlfilePDF, "_blank");        
    }

    viewFile(row) {
        this.urlfilePDF = this.baseUrl + '/' + row.fileScanUrl;
        if(!row.fileScanUrl){
            this.messageError("Không có file hồ sơ", "")
        }else{
            if(this.utils.isPdfFile(row.fileScanUrl)){
                console.log('file truyen', this.urlfilePDF);
                this.modalDialogPDF = true;
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", "")
            }
        }
    }

    getOrderDetail() {
        this._orderServiceProxy.get(this.orderDetail.id).subscribe((resOrder) => {
            if (this.handleResponseInterceptor(resOrder, '')) {
                this.orderDetail = resOrder?.data;
            }
        });
    }

    validForm(): boolean {
        return this.orderContractFile?.fileScanUrl?.trim() !== "";
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateServiceProxy.uploadFileGetUrl(event?.files[0], "order-contract").subscribe((response) => {
                    console.log({response});  
                    if (this.handleResponseInterceptor(response, 'Tải lên thành công!')) {
                        this.orderContractFile.fileScanUrl = response.data;
                    }
                },(err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    hideDialog() {
        this.modalDialog = false;
        this.modalDialogPDF = false;
    }

    saveOrderContractFile() {
        this.submitted = true;
        if (this.contractTemplate.orderContractFileId > 0) {
            this._orderServiceProxy.updateOrderContractFile(this.orderContractFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                        this.hideDialog();
                        this.setPage();
                    } 
                    this.submitted = false;
                },() => {
                    this.submitted = false;
                }
            );
        } else {
            this._orderServiceProxy.createOrderContractFile(this.orderContractFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                        this.hideDialog();
                        this.setPage();
                    }
                    this.submitted = false;
                },() => {
                    this.submitted = false;
                }
            );
        }
    }
}
