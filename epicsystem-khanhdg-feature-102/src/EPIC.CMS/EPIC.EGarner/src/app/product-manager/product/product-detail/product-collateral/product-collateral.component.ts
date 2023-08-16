import { Component, Inject, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { SearchConst, AppConsts, ProductFileConst, FormNotificationConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
// import { ProjectServiceProxy } from "@shared/service-proxies/project-manager-service";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { DistributionService } from "@shared/services/distribution.service";
import { ProductCollateralService } from "@shared/services/product-collateral.service";
import { AppUtilsService } from "@shared/services/utils.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { Subject } from "rxjs";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: 'app-product-collateral',
  templateUrl: './product-collateral.component.html',
  styleUrls: ['./product-collateral.component.scss']
})
export class ProductCollateralComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private _contractTemplateService: DistributionService,
        private _productCollateralService: ProductCollateralService,
        private _utilsService: AppUtilsService,
        private routeActive: ActivatedRoute,
        private _dialogService: DialogService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.productFile.productId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    private baseUrl: string;

    productId: number;

    AppConsts = AppConsts;
    ProductFileConst = ProductFileConst;
    uploadedFiles: any[] = [];
 
    modalDialog: boolean;
    modalDialogPDF: boolean;

    deleteItemDialog: boolean = false;
    rows: any[] = [];

    productFile: any = {
        'id': 0,
        'title': null,
        'url': null,
        'productId': 0,
        'documentType': ProductFileConst.TAI_SAN_DAM_BAO,
        'description': null,
        // 'totalValue': null,
    };
    listAction: any[] = [];

    submitted: boolean;

    actions: any[] = [];
    actionsDisplay: any[] = [];
    urlfilePDF:string = '';
    headerTitle: string;

    ngOnInit() {
        this.actions = [];
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });
    }

    genListAction(data = []) {
		this.listAction = data.map(orderItem => {
			const actions = [];
        if (this.isGranted([this.PermissionGarnerConst.GarnerSPDT_TSDB_DownloadFile])) {
            actions.push(
            {
                data: orderItem,
                label: 'Tải file',
                icon: 'pi pi-download',
                command: ($event) => {
                this.downloadFile($event.item.data.url);
                }
            });
        }

        if (this.utils.isPdfFile(orderItem.url) && this.isGranted([this.PermissionGarnerConst.GarnerSPDT_TSDB_Preview])) {
            actions.push(
            {
                data: orderItem,
                label: 'Xem file',
                icon: 'pi pi-eye',
                command: ($event) => {
                this.viewFile($event.item.data.url);
                }
            });
        }

        if (this.isGranted([this.PermissionGarnerConst.GarnerSPDT_TSDB_CapNhat])) {
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

        if (this.isGranted([this.PermissionGarnerConst.GarnerSPDT_TSDB_DeleteFile])) {
			    actions.push({
                    data: orderItem,
                    label: 'Xóa',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
				});
		    }
			console.log(
                "actions",actions
            );
            
			return actions;
		});
	}

    edit(item) {
        this.modalDialog = true;
        this.headerTitle = 'Cập nhật';
        this.productFile = {
            ...item,
        };
    }

    resetContractTemplateObject() {
        this.productFile = {
            id: 0,
            title: null,
            url: null,
            productId: this.productFile.productId,
            documentType: ProductFileConst.TAI_SAN_DAM_BAO,
            description: null,
            // totalValue: 0,
        };
    }

    clickDropdown(row) {
        this.actionsDisplay = [];
        this.productFile = { ...row };
        console.log({ distributionContractPayment: row });
        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(+row.status) && action.permission);
    }

    create() {
        this.resetContractTemplateObject();
        this.submitted = false;
        this.modalDialog = true;
        this.headerTitle = 'Thêm tài sản đảm bảo';
    }

    changeContractType(value) {
        if (value) {
            this.setPage({ page: this.offset });
        }
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        this.page.keyword = this.keyword;
        this.isLoading = true;
    
        this._productCollateralService.getAll(this.page, this.productFile).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    // this.page.totalItems = res.data.totalItems;
                    this.rows = res.data
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
        this.modalDialogPDF = false;
    }

    save() {
        this.submitted = true;
        //
        if (this.productFile.id) {
            this._productCollateralService.updateProductFile(this.productFile).subscribe(
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
            this._productCollateralService.addProductFile(this.productFile).subscribe(
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
        return this.productFile?.url?.trim() && this.productFile?.title?.trim();
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "product-collateral").subscribe(
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
                        this.productFile.url = response.data;
                    }
                },
                (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    delete(file) {
        const ref = this._dialogService.open(
			FormNotificationComponent,
			{
				header: "Thông báo",
				width: '600px',
				contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
				styleClass: 'p-dialog-custom',
				baseZIndex: 10000,
				data: {
					title: "Bạn có chắc chắn xóa file này không ?",
					icon: FormNotificationConst.IMAGE_CLOSE,
				},
			}
		);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
      if (dataCallBack?.accept) {
        this._productCollateralService.delete(file.id).subscribe((response) => {
            console.log('----', this.productFile.id)
            if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                this.setPage();
            }
        });

		}
			  });
        
    }

    downloadFile(fileUrl) {
        const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }

    viewFile(fileUrl) {
        this.urlfilePDF = this.baseUrl + '/' + fileUrl;
        if(!fileUrl){
            this.messageError("Không có file hồ sơ", 1000)
        }else{
            if(this.utils.isPdfFile(fileUrl)){
                console.log('file truyen', this.urlfilePDF);
                this.modalDialogPDF = true;
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", 1000)
            }
        }
    }

}


