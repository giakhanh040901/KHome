import { Component, Inject, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts, BusinessTypeConst, FormNotificationConst, IsSignPdfConst, OrderConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';

import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { ContractTemplateServiceProxy, OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
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
        private router: Router,
        private routeActive: ActivatedRoute,
        private _contractTemplateServiceProxy: ContractTemplateServiceProxy,
        private _orderServiceProxy: OrderServiceProxy,
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
    @Input() isJustView: any;
    @Input() isPartner: boolean;

    ref: DynamicDialogRef;
    modalDialog: boolean;
    modalDialogPDF: boolean;
    rows: any[] = [];
    
    IsSignPdfConst = IsSignPdfConst;
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
        FileScanUrl: null,
        fileTempUrl: null,
        fileSignatureUrl: null
    };

    orderContractFile: any = {
        orderContractFileId: 0,
        orderId: 0,
        ContractTempId: 0,
        FileScanUrl: null
    };

    page = new Page();
    offset = 0;
    actionsDisplay: any[] = [];
    actions: any[] = [];
    uploadedFiles: any[] = [];
    submitted: boolean;
    urlfilePDF:string = '';

    isSignAll: boolean = true;

    ngOnInit() {
        this.setPage();
        console.log({ orderDetailContract: this.orderDetail });
    }

    saveFile(row) {
        this.submitted = false;
        this.modalDialog = true;
        this.contractTemplate = { ...row };
        
        this.orderContractFile = {
            orderContractFileId: this.contractTemplate.orderContractFileId,
            orderId: this.orderId,
            ContractTempId: this.contractTemplate.id,
            FileScanUrl: this.contractTemplate.FileScanUrl
        };
    }

    updateOrderContract() {
        this.isLoading = true;
        this._orderServiceProxy.updateOrderContract(this.orderId).subscribe((res) => {
            console.log('res', res);
            this.isLoading = false;
            if(this.handleResponseInterceptor(res, 'Update thành công')) {
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
        this._orderServiceProxy.downloadFile(this.orderId, contractTemplate.id).subscribe((res) => {
            this.handleResponseInterceptor(res);
        });
    }

    downloadFileWord(contractTemplate) {
        if (contractTemplate.id > 0) {
            this._orderServiceProxy.downloadFileTempContract(contractTemplate.orderContractFileId).subscribe((res) => {
                this.handleResponseInterceptor(res);
            });
        } else {
            this.messageError("Chưa cập nhật hồ sơ!");
        }

    }

    downloadFilePdf(contractTemplate) {
        if (contractTemplate.orderContractFileId > 0) {
            this._orderServiceProxy.downloadFileTempPdfContract(contractTemplate.orderContractFileId).subscribe((res) => {
                this.handleResponseInterceptor(res);
            });
        } else {
            this.messageError("Chưa cập nhật hồ sơ!");
        }

    }

    downloadFileSignature(contractTemplate) {
        if (contractTemplate.orderContractFileId > 0) {
            this._orderServiceProxy.downloadFileSignatureContract(contractTemplate.orderContractFileId).subscribe((res) => {
                this.handleResponseInterceptor(res);
            });
        } else {
            this.messageError("Chưa cập nhật hồ sơ!");
        }
    }

    downloadFileScanUrl(){
         window.open(this.urlfilePDF, "_blank");        
    }

    viewFile(fileUrl) {
        this.urlfilePDF = this.baseUrl + '/' + fileUrl;
        if(!fileUrl){
            this.messageError("Không có file hồ sơ")
        }else{
            if(this.utils.isPdfFile(fileUrl)){
                console.log('file truyen', this.urlfilePDF);
                this.modalDialogPDF = true;
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF")
            }
        }
    }

    activeOnline() {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn muốn đổi sang Chuyển online?",
						icon: FormNotificationConst.IMAGE_APPROVE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
        console.log({ dataCallBack });
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
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn duyệt hồ sơ sổ lệnh này?",
						icon: FormNotificationConst.IMAGE_APPROVE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
        console.log({ dataCallBack });
			if (dataCallBack?.accept) {
                this._orderServiceProxy.approve(this.orderDetail.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Duyệt hồ sơ thành công")) {
                        this.setPage({ page: this.offset });
                    }
                });
			}
		});
	}

    cancel() {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn hủy duyệt hồ sơ sổ lệnh này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		    ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
			if (dataCallBack?.accept) {
                this._orderServiceProxy.cancel(this.orderDetail.id).subscribe((response) => {
                    if (this.handleResponseInterceptor(response,"Hủy duyệt hồ sơ thành công")) {
                        this.setPage({ page: this.offset });
                    }
                });
			}
		});
	}

    resentNotify() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn gửi thông báo?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-question-circle',
            accept: () => {
                this._orderServiceProxy.resentNotify(this.orderDetail.id).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                        this.getOrderDetail();
                    };
                }, (err) =>  {
                    this.messageError('Gửi thất bại!');
                });
            },
            reject: () => {

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

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        this.page.keyword = this.keyword;
        this.isLoading = true;
        this.isSignAll = true;
        forkJoin([this._contractTemplateServiceProxy.getByOrder(this.page, this.orderId),this._orderServiceProxy.get(this.orderId)]).subscribe(([resContract, resOrder]) => {
            this.isLoading = false;
            if(this.handleResponseInterceptor(resOrder)){
                this.orderDetail.status = resOrder?.data?.status;
                this.orderDetail.source = resOrder?.data?.source;
            }
            //
            if(this.handleResponseInterceptor(resContract, '')) {
                this.page.totalItems = resContract.data.totalItems;
                this.rows = resContract.data.map((item) => {
                    item.isFileUrl = item.FileScanUrl ? true : false;
                    item.isSign = item.isSign == IsSignPdfConst.YES ? true : false;
                    //
                    if(!item.isSign && this.orderDetail.source == OrderConst.SOURCE_ONLINE) {
                        this.isSignAll = false;
                    }    // Check xem đã ký điện tử hay chưa
                    //
                    return item;
                });
                console.log('row hop dong', this.rows);
            }
        }, () => {
            this.isLoading = false;
        });
    }

    changeKeyword() {
        if (this.keyword === '') {
            this.setPage({ page: this.offset });
        }
    }

    clickDropdown(row) {
        this.contractTemplate = { ...row };

        this.orderContractFile = {
            secondaryContractFileId: this.contractTemplate.secondaryContractFileId,
            orderId: this.orderId,
            ContractTempId: this.contractTemplate.id,
            FileScanUrl: this.contractTemplate.FileScanUrl
        };

        this.actionsDisplay = this.actions.filter(action => action.permission);
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
                        this.submitted = false;
                        // this.setPage({ page: this.page.pageNumber });
                        // this.updateOrderContract();
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
            this._orderServiceProxy.createOrderContractFile(this.orderContractFile).subscribe(
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

    takeHardContract(){
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn muốn gửi yêu cầu nhận hợp đồng bản cứng?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-question-circle',
            accept: () => {
                this._orderServiceProxy.takeHardContract(this.orderDetail.id).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi yêu cầu thành công')) {
                        this.getOrderDetail();
                    };
                }, (err) =>  {
                    this.messageError('Gửi yêu cầu thất bại!');
                });
            },
            reject: () => {

            },
        });
    }

    validForm(): boolean {
        return this.orderContractFile?.FileScanUrl?.trim() !== "";
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateServiceProxy.uploadFileGetUrl(event?.files[0], "order-contract").subscribe((response) => {
                    console.log({response});  
                    if (this.handleResponseInterceptor(response,'')) {
                        this.orderContractFile.FileScanUrl = response.data;
                    }
                },(err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }
}
