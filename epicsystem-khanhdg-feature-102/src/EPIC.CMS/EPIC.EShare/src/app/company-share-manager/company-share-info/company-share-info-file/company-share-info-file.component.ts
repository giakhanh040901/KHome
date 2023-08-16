import { Component, Inject, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { StatusCompanyShareInfoFileConst, ContractTypeConst, SearchConst, AppConsts } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ContractTemplateServiceProxy, DistributionContractFileServiceProxy, CompanyShareInfoFileServiceProxy } from "@shared/service-proxies/company-share-manager-service";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { AppUtilsService } from "@shared/services/utils.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { Subject } from "rxjs";
import { debounceTime } from "rxjs/operators";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";

@Component({
    selector: 'app-company-share-info-file',
    templateUrl: './company-share-info-file.component.html',
    styleUrls: ['./company-share-info-file.component.scss'],
    providers: [ConfirmationService, MessageService]
})
export class CompanyShareInfoFileComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private _contractTemplateService: ContractTemplateServiceProxy,
        private _companyShareInfoFile: CompanyShareInfoFileServiceProxy,
        private _utilsService: AppUtilsService,
        private routeActive: ActivatedRoute,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.companyShareId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    private baseUrl: string;

    companyShareId: string;

    AppConsts = AppConsts;

    uploadedFiles: any[] = [];
    contractTypeId: number = -1;
    contractTypes: any[] = [...ContractTypeConst.list];
    contractTypesSearch: any[] = [
        {
            code: -1,
            name: "Chọn tất cả",
        },
        ...ContractTypeConst.list,
    ];
    modalDialog: boolean;
    modalDialogPDF: boolean;
    
    deleteItemDialog: boolean = false;

    StatusCompanyShareInfoFileConst = StatusCompanyShareInfoFileConst;

    rows: any[] = [];

    contractFile: any = {
        'juridicalFileId': 0,
        'name': null,
        'url': null,
        'companyShareId': 0,
    };

    submitted: boolean;

    actions: any[] = [];
    actionsDisplay: any[] = [];
    urlfilePDF:string = '';

    
    ngOnInit() {
        this.actions = [
            // {
            //     label: 'Phê duyệt',
            //     icon: 'pi pi-check',
            //     statusActive: [this.StatusCompanyShareInfoFileConst.RESPONSE_FALSE],
            //     permission: this.isGranted(),
            //     command: () => {
            //         this.approve();
            //     }
            // },
            // {
            // 	label: 'Từ chối',
            // 	icon: 'pi pi-times',
            // 	statusActive: [this.StatusCompanyShareInfoFileConst.FILE_APPROVE],
            // 	permission: this.isGranted(),
            // 	command: () => {
            // 		this.cancel();
            // 	}
            // },
            // {
            //     label: 'Xóa',
            //     icon: 'pi pi-trash',
            //     statusActive: [this.StatusCompanyShareInfoFileConst.RESPONSE_FALSE],
            //     permission: this.isGranted(),
            //     command: () => {
            //         this.delete();
            //     }
            // },
        ];
        this.setPage({ page: this.offset });
        this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
            if (this.keyword === "") {
                this.setPage({ page: this.offset });
            } else {
                this.setPage();
            }
        });
    }

    resetContractTemplateObject() {
        this.contractFile = {
            id: 0,
            name: null,
            url: null,
            companyShareId: +this.companyShareId,
        };
    }

    clickDropdown(row) {
        this.actionsDisplay = [];
        this.contractFile = { ...row };
        console.log({ distributionContractPayment: row });
        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(+row.status) && action.permission);
    }

    create() {
        this.resetContractTemplateObject();
        this.submitted = false;
        this.modalDialog = true;
    }

    confirmDelete() {
        this.deleteItemDialog = false;
        this._contractTemplateService.delete(this.contractFile.id).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                    this.setPage({ offset: this.page.pageNumber });
                    this.resetContractTemplateObject();
                }
            },
            () => {
                this.messageService.add({
                    severity: "error",
                    summary: "",
                    detail: `Không xóa được hồ sơ ${this.contractFile.displayName}`,
                    life: 3000,
                });
            }
        );
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

        this._companyShareInfoFile.getAll(this.page, this.companyShareId).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.rows = res.data.items;
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
        this.submitted = true;
        //
        if (this.contractFile.fieldId) {
            this._companyShareInfoFile.update(this.contractFile).subscribe(
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
            this._companyShareInfoFile.create(this.contractFile).subscribe(
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
        return this.contractFile?.url?.trim() && this.contractFile?.name?.trim();
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "companyShare").subscribe(
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
                        this.contractFile.url = response.data;
                    }
                },
                (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    delete(file) {
        const data = file;
        console.log('vafo chua')
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn xóa hồ sơ này?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-times-circle',
            accept: () => {
                this._companyShareInfoFile.delete(data.juridicalFileId).subscribe((response) => {
                    console.log('----', this.contractFile.juridicalFileId)
                    if (this.handleResponseInterceptor(response, "Xóa thành công")) {
                        // this.messageService.add({ severity: 'success', summary: '', detail: '', life: 1500 });
                        this.setPage();
                    }
                });
            },
            reject: () => {

            },
        });
    }

    approve() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn phê duyệt thanh toán này?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-check-circle',
            accept: () => {
                this._companyShareInfoFile.approve(this.contractFile.juridicalFileId).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, "Phê duyệt thành công")) {
                        this.messageService.add({ severity: 'success', summary: '', detail: 'Thành công!', life: 1500 });
                        this.setPage();
                    }
                });
            },
            reject: () => {

            },
        });
    }

    cancel() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn hủy phê duyệt hồ sơ này?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-check-circle',
            accept: () => {
                this._companyShareInfoFile.cancel(this.contractFile.juridicalFileId).subscribe((response) => {
                    if (this.handleResponseInterceptor(response, "Hủy phê duyệt thành công")) {
                        this.messageService.add({ severity: 'success', summary: '', detail: 'Thành công!', life: 1500 });
                        this.setPage();
                    }
                });
            },
            reject: () => {

            },
        });
    }

    downloadFile(fileUrl) {
        const url = this.baseUrl + "/" + fileUrl;
        this._utilsService.makeDownload("", url);
    }

    viewFile(fileUrl) {
        const url = this.AppConsts.redicrectHrefOpenDocs + this.baseUrl + '/' + fileUrl;

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
}
