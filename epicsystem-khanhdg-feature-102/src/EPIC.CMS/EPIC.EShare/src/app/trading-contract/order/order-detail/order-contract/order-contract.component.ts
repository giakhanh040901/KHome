import { Component, Inject, Injector, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts, BusinessTypeConst, ContractTemplateConst, ContractTypeConst, IsSignPdfConst, OrderConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { OrderServiceProxy } from '@shared/service-proxies/trading-contract-service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { forkJoin } from 'rxjs';

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

    ref: DynamicDialogRef;
    modalDialog: boolean;
    modalDialogPDF: boolean;
    rows: any[] = [];

    BusinessTypeConst = BusinessTypeConst;
    ContractTemplateConst = ContractTemplateConst;
    OrderConst = OrderConst;
    IsSignPdfConst = IsSignPdfConst;

    contractTemplate: any = {
        id: 0,
        code: null,
        name: "",
        contractType: null,
        contractTempUrl: "",
        businessType: BusinessTypeConst.B2B,
        status: null,
        deleted: null,
        secondaryContractFileId: null,
        fileScanUrl: null,
        fileTempUrl: null,
        fileSignatureUrl: null
    };

    orderContractFile: any = {
        secondaryContractFileId: 0,
        orderId: 0,
        ContractTempId: 0,
        fileScanUrl: null
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
            secondaryContractFileId: this.contractTemplate.secondaryContractFileId,
            orderId: this.orderId,
            ContractTempId: this.contractTemplate.id,
            fileScanUrl: this.contractTemplate.fileScanUrl
        };
    }

    updateOrderContract() {
        this.isLoading = true;
        this._orderServiceProxy.updateOrderContract(this.orderId).subscribe((res) => {
            console.log('res', res);
            if(this.handleResponseInterceptor(res, 'Update thành công')) {
                this.setPage();
            } else {
            this.isLoading = false;
            }
        }, () => {
            this.isLoading = false;
        });
    }

    signOrderContract() {
        this.isLoading = true;
        this._orderServiceProxy.signOrderContract(this.orderId).subscribe((res) => {
            console.log('res', res);
            if(this.handleResponseInterceptor(res, 'Ký điện tử thành công')) {
                this.setPage();
            } else {
            this.isLoading = false;
            }
        }, () => {
            this.isLoading = false;
        });
    }
    
    downloadFile(contractTemplate) {
        this._orderServiceProxy.downloadFile(this.orderId, contractTemplate.id).subscribe((res) => {
            console.log(res);
            this.isLoading = false;
            if (this.handleResponseInterceptor(res, '')) {
                console.log({ "Tải xuống": res });
            }
        }, () => {
            this.isLoading = false;
        });
    }

    downloadFileScanContract(contractTemplate) {
        if (contractTemplate.secondaryContractFileId > 0) {
            this._orderServiceProxy.downloadFileScanContract(this.orderId, contractTemplate.id, contractTemplate.secondaryContractFileId).subscribe((res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, '')) {
                    console.log({ "Tải xuống": res });
                }
            }, () => {
                this.isLoading = false;
            });
        } else {
            this.messageError("Bạn chưa tải lên file scan!");
        }

    }

    downloadFileTempContract(contractTemplate) {
        if (contractTemplate.secondaryContractFileId > 0) {
            this._orderServiceProxy.downloadFileTempContract(this.orderId, contractTemplate.id, contractTemplate.secondaryContractFileId).subscribe((res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, '')) {
                    console.log({ "Tải xuống": res });
                }
            }, () => {
                this.isLoading = false;
            });
        } else {
            this.messageError("File mẫu không tồn tại!");
        }

    }

    downloadFileSignatureContract(contractTemplate) {
        if (contractTemplate.secondaryContractFileId > 0) {
            this._orderServiceProxy.downloadFileSignatureContract(this.orderId, contractTemplate.id, contractTemplate.secondaryContractFileId).subscribe((res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, '')) {
                    console.log({ "Tải xuống": res });
                }
            }, () => {
                this.isLoading = false;
            });
        } else {
            this.messageError("File chữ ký điện tử không tồn tại!");
        }

    }

    downloadFileScanUrl(){
        window.open(this.urlfilePDF, "_blank");        
   }

    viewFile(fileUrl) {
        this.urlfilePDF = this.baseUrl + '/' + fileUrl;
        if(!fileUrl){
            this.messageError("Không có file hồ sơ", "")
        }else{
            if(this.utils.isPdfFile(fileUrl)){
                console.log('file truyen', this.urlfilePDF);
                this.modalDialogPDF = true;
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", "")
            }
        }
    }

    getOrderDetail() {
        this._orderServiceProxy.get(this.orderDetail.orderId).subscribe((resOrder) => {
            if (this.handleResponseInterceptor(resOrder, '')) {
                this.orderDetail.status = resOrder?.data?.status;
                this.orderDetail.source = resOrder?.data?.source;
            }
        });
    }

    activeOnline() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn muốn đổi sang Chuyển online?',
            header: 'Chuyển Online',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-question-circle',
            accept: () => {
                this._orderServiceProxy.activeOnline(this.orderDetail.orderId).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Chuyển Online thành công')) {
                        this.setPage();
                    };
                });
            },
            reject: () => {

            },
        });
    }

    approve() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn duyệt hồ sơ?',
            header: 'Duyệt hồ sơ',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-question-circle',
            accept: () => {
                this._orderServiceProxy.approve(this.orderDetail.orderId).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Duyệt hồ sơ thành công')) {
                        this.setPage();
                    };
                });
            },
            reject: () => {

            },
        });
    }

    cancel() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn hủy duyệt hồ sơ?',
            header: 'Hủy duyệt hồ sơ',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-question-circle',
            accept: () => {
                this._orderServiceProxy.cancel(this.orderDetail.orderId).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Hủy duyệt thành công')) {
                        this.setPage();
                    };
                });
            },
            reject: () => {

            },
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
                this._orderServiceProxy.resentNotify(this.orderDetail.orderId).subscribe((res) => {
                    if (this.handleResponseInterceptor(res, 'Gửi thành công')) {
                        this.getOrderDetail();
                    };
                }, (err) =>  {
                    this.messageError('Gửi thất bại!', '');
                });
            },
            reject: () => {

            },
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
                this.rows = resContract.data.items.map((item) => {
                    item.isFileUrl = item.fileScanUrl ? true : false;
                    item.isSign = item.isSign == IsSignPdfConst.YES ? true : false;
                    // Check xem đã ký điện tử hay chưa
                    if(!item.isSign && this.orderDetail.source == OrderConst.SOURCE_ONLINE) {
                        this.isSignAll = false;
                    }    
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
            fileScanUrl: this.contractTemplate.fileScanUrl
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
        console.log("aaa", this.orderContractFile);
        if (this.contractTemplate.secondaryContractFileId > 0) {
            this._orderServiceProxy.updateSecondaryContractFileScan(this.orderContractFile).subscribe(
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
            this._orderServiceProxy.createSecondaryContractFileScan(this.orderContractFile).subscribe(
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
        return this.orderContractFile?.fileScanUrl?.trim() !== "";
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateServiceProxy.uploadFileGetUrl(event?.files[0], "companyShare").subscribe(
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
                        this.orderContractFile.fileScanUrl = response.data;
                    }
                },
                (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }
}
