import { Component, Inject, Injector, Input, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { AppConsts, ContractTemplateConst, FormNotificationConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { DistributionService } from "@shared/services/distribution.service";
import { PolicyFileService } from "@shared/services/policy-file.service";
import { AppUtilsService } from "@shared/services/utils.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
  selector: 'app-distribution-policy-file',
  templateUrl: './distribution-policy-file.component.html',
  styleUrls: ['./distribution-policy-file.component.scss'],
    providers: [ConfirmationService, MessageService]
})
export class DistributionPolicyFileComponent extends CrudComponentBase {

    constructor(injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private _distributionService: DistributionService,
        private _policyFileService: PolicyFileService,
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
  
    modalDialog: boolean;
    modalDialogPDF: boolean;

    deleteItemDialog: boolean = false;
    rows: any[] = [];

    policyFile: any = {
        'id': 0,
        'effectiveDate': null,  // Ngày có hiệu lực
        'expirationDate': null, // Ngày hết hiệu lực
        'title': null,
        'url': null,
        'distributionId': 0,
        'tradingProviderId': 0,
        'documentType': 4,
        'description': null,
    };

    filedDates=['effectiveDate','expirationDate'];
    submitted: boolean;
    urlfilePDF:string = '';

    actions: any[] = [];
    actionsDisplay: any[] = [];
    listAction: any[] = [];

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
			if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_FileChinhSach_CapNhat])) {
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
      if (this.isGranted([this.PermissionGarnerConst.GarnerPPSP_FileChinhSach_Xoa])) {
			    actions.push({
                    data: orderItem,
                    label: 'Xóa',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
				});
		    }
	
		  console.log("orderItem.status",this.actions);
		  
			return actions;
		});
	}


    resetContractTemplateObject() {
        this.policyFile = {
            id: 0,
            title: null,
            url: null,
            distributionId: +this.distributionId,
            documentType: 4,
            description: null,
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

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        this.page.keyword = this.keyword;
        this.isLoading = true;

        this._policyFileService.getAll(this.page, this.distributionId).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.rows = res.data.items;
                    this.genListAction(this.rows);
                    console.log({ rowsFile: res.data.items, totalItems: res.data.totalItems });
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
        console.log("this.policyFile save",this.policyFile);
        
        this.submitted = true;
        let body = this.formatCalendar(this.filedDates, {...this.policyFile});
        if (this.policyFile.id) {
            this._policyFileService.update(body).subscribe(
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
            this._policyFileService.create(body).subscribe(
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
      //  && this.policyFile?.url?.trim()
      return this.policyFile?.title?.trim() && this.policyFile?.url?.trim();
    }

    myUploader(event) {
        if (event?.files[0]) {     
            this._distributionService.uploadFileGetUrl(event?.files[0], "distribution/policy-file").subscribe(
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
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xóa chính sách này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
      console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._distributionService.deleteFile(policyFile.id).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xóa chính sách thành công"
				)
			  ) {
				  this.setPage();
			  }
			});
			}

		});
	  }

    downloadFile(fileUrl) {
        console.log('File-----', fileUrl);
        const url = this.baseUrl + "/" + fileUrl;
        console.log('---------', url)
        this._utilsService.makeDownload("", url);
    }

    viewFile(fileUrl) {
        const url = this.AppConsts.redicrectHrefOpenDocs + this.baseUrl + '/' + fileUrl;
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
