import { Component, ElementRef, Inject, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts, AtributionConfirmConst, BusinessTypeConst, DistributionContractTemplateConst, FormNotificationConst, IsSignPdfConst, OrderConst, TableConst, YesNoConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IColumn } from '@shared/interface/p-table.model';
import { Page } from '@shared/model/page';

import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ContractTemplateService } from '@shared/services/contract-template.service';
import { OrderService } from '@shared/services/order.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';
import { FormNotificationComponent } from 'src/app/form-general/form-notification/form-notification.component';

@Component({
    selector: 'app-order-contract',
    templateUrl: './order-contract.component.html',
    styleUrls: ['./order-contract.component.scss'],
    providers: [ConfirmationService, MessageService]
})
export class OrderContractComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private routeActive: ActivatedRoute,
        private _contractTemplateServiceProxy: ContractTemplateService,
        private _orderServiceProxy: OrderService,
		private _dialogService: DialogService,

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
    @Input() contentHeight: number;
    idHeader:string = "page-header-contract";

    ref: DynamicDialogRef;
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

    page = new Page();

    urlfilePDF:string = '';

    isSign: boolean = false

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
			{ field: 'viewFileScan', header: 'Xem HS', width: 6, isPin: true, type: TableConst.columnTypes.ACTION_ICON,icon: 'pi pi-eye'},
        ];
        this.setPage();
    }
    
    setPage(event?: Page) {
        if(!event) this.page.pageNumber = 0;
        this.isLoading = true;
        this.isSign = false;
        forkJoin([this._contractTemplateServiceProxy.getByOrder(this.page, this.orderId),this._orderServiceProxy.get(this.orderId)]).subscribe(([resContract, resOrder]) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(resContract, '')) {
                this.page.totalItems = resContract.data.totalItems;
                this.rows = resContract.data.items.map((item) => {
                    item.isFileUrl = !!item.fileScanUrl;
                    if (item.isSign === YesNoConst.YES) {
                        this.isSign = true;
                    }
                    return item;
                });
                this.setData(this.rows);
            }
            //
            if(this.handleResponseInterceptor(resOrder)){
                this.orderDetail.status = resOrder?.data?.status;
                this.orderDetail.source = resOrder?.data?.source;
            }
            //
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

    updateOrderContract(isMessage = true) {
        this.isLoading = true;
        this._orderServiceProxy.updateOrderContract(this.orderId).subscribe((res) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(res, (isMessage ? 'Update thành công' : ''))) {
                this.setPage();
            } 
        }, () => {
            this.isLoading = false;
        });
    }

    signOrderContract() {
        this.isLoading = true;
        this._orderServiceProxy.signOrderContract(this.orderId).subscribe((res) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(res, 'Ký điện tử thành công')) {
                this.setPage();
            } 
        }, () => {
            this.isLoading = false;
        });
    }

    downloadFile(contractTemplate) {
        this.isLoading = false;
        this._orderServiceProxy.downloadFile(this.orderId, contractTemplate.id).subscribe((res) => {
            this.isLoading = false;
            this.handleResponseInterceptor(res, '');
        }, () => {
            this.isLoading = false;
        });
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
        this.urlfilePDF = '/' + row.fileScanUrl;
        if(!row.fileScanUrl){
            this.messageError("Không có file hồ sơ", "")
        }else{
            if(this.utils.isPdfFile(row.fileScanUrl)){
                this._orderServiceProxy.viewFilePDF(this.urlfilePDF + '&download=true').subscribe();
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", "")
            }
        }
    }

    activeOnline() {
		this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title : "Bạn có chắc chắn muốn đổi sang Chuyển online?",
                    icon: FormNotificationConst.IMAGE_APPROVE,
                },
            }).onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
                this._orderServiceProxy.activeOnline(this.orderDetail.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Chuyển Online thành công")) {
                        this.setPage();
                    }
                });
			}
		});
	}

    approve() {
		this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title : "Bạn có chắc chắn duyệt hồ sơ sổ lệnh này?",
                    icon: FormNotificationConst.IMAGE_APPROVE,
                },
            }).onClose.subscribe((dataCallBack) => {
			if (dataCallBack?.accept) {
                this._orderServiceProxy.approve(this.orderDetail.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Duyệt hồ sơ thành công")) {
                        this.setPage();
                    }
                });
			}
		});
	}

    cancel() {
		this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title : "Bạn có chắc chắn hủy duyệt hồ sơ sổ lệnh này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }).onClose.subscribe((dataCallBack) => {
                if (dataCallBack?.accept) {
                    this._orderServiceProxy.cancel(this.orderDetail.id).subscribe((response) => {
                        if (this.handleResponseInterceptor(response,"Hủy duyệt hồ sơ thành công")) {
                            this.setPage();
                        }
                    });
                }
		    }
        );
	}

    cancelSign() {
        this._dialogService.open(
            FormNotificationComponent,
            {
                header: "Thông báo",
                width: '600px',
                data: {
                    title : "Bạn có chắc chắn hủy ký điện tử hồ sơ sổ lệnh này?",
                    icon: FormNotificationConst.IMAGE_CLOSE,
                },
            }
        ).onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
            this._orderServiceProxy.cancelSign(this.orderDetail.id).subscribe((response) => {
                if (this.handleResponseInterceptor(response,"Hủy ký điện tử hồ sơ thành công")) {
                    this.setPage();
                }
            });
        }
    });
    }

    resentNotify() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn gửi thông báo?',
            ...AtributionConfirmConst,
            accept: () => {
                this._orderServiceProxy.resentNotify(this.orderDetail.id).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                        this.getOrderDetail();
                    };
                }, (err) =>  {
                    this.messageError('Gửi thất bại!', '');
                });
            },
        });
    }

    takeHardContract(){
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn muốn gửi yêu cầu nhận hợp đồng bản cứng?',
            ...AtributionConfirmConst,
            accept: () => {
                this._orderServiceProxy.takeHardContract(this.orderDetail.id).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi yêu cầu thành công')) {
                        this.getOrderDetail();
                    };
                }, (err) =>  {
                    this.messageError('Gửi yêu cầu thất bại!', '');
                });
            },
        });
    }

    getOrderDetail() {
        this._orderServiceProxy.get(this.orderDetail.id).subscribe((resOrder) => {
            if (this.handleResponseInterceptor(resOrder, '')) {
                this.orderDetail = resOrder?.data;
            }
        });
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

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
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
}
