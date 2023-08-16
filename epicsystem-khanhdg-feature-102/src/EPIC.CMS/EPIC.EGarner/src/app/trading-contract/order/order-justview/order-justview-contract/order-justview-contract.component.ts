import { Component, Inject, Injector, Input, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { AppConsts, BusinessTypeConst, IsSignPdfConst, OrderConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { Page } from "@shared/model/page";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import {
	ContractTemplateServiceProxy,
	OrderServiceProxy,
} from "@shared/service-proxies/trading-contract-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogRef } from "primeng/dynamicdialog";
import { forkJoin } from "rxjs";

@Component({
	selector: "app-order-justview-contract",
	templateUrl: "./order-justview-contract.component.html",
	styleUrls: ["./order-justview-contract.component.scss"],
})
export class OrderJustviewContractComponent extends CrudComponentBase {
	constructor(
		injector: Injector,
		messageService: MessageService,
		private confirmationService: ConfirmationService,
		private routeActive: ActivatedRoute,
		private _contractTemplateServiceProxy: ContractTemplateServiceProxy,
		private _orderServiceProxy: OrderServiceProxy,

		@Inject(API_BASE_URL) baseUrl?: string
	) {
		super(injector, messageService);
		//
		this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
		this.orderId = +this.cryptDecode(
		this.routeActive.snapshot.paramMap.get("id")
		);
	}

 	private baseUrl: string;

  	AppConsts = AppConsts;

  	orderId: number;
  	@Input() orderDetail: any = {};

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

  	ngOnInit(): void {
		this.setPage();
        console.log({ orderDetailContract: this.orderDetail });
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

	hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
        this.modalDialogPDF = false;
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
}
