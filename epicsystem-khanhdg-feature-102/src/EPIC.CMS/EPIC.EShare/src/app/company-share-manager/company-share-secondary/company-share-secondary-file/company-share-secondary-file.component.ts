import { Component, Inject, Injector, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppConsts, ContractTypeConst, FormNotificationConst, SearchConst, StatusCompanyShareInfoFileConst } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { ContractTemplateServiceProxy, CompanyShareSecondaryFileServiceProxy } from '@shared/service-proxies/company-share-manager-service';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies-base';
import { AppUtilsService } from '@shared/services/utils.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from "rxjs/operators";
import { FormNotificationComponent } from 'src/app/form-notification/form-notification.component';
@Component({
    selector: 'app-company-share-secondary-file',
    templateUrl: './company-share-secondary-file.component.html',
    styleUrls: ['./company-share-secondary-file.component.scss'],
    providers: [ConfirmationService, MessageService]
})
export class CompanyShareSecondaryFileComponent extends CrudComponentBase {

    constructor(injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private _contractTemplateService: ContractTemplateServiceProxy,
        private _companyShareSecondaryFile: CompanyShareSecondaryFileServiceProxy,
        private _utilsService: AppUtilsService,
        private routeActive: ActivatedRoute,
        private _dialogService: DialogService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.companyShareSecondaryId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    private baseUrl: string;

    companyShareSecondaryId: string;

    headerTitle: string;

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
    fieldDates = ['expirationDate','effectiveDate' ];
    StatusCompanyShareSecondaryFileConst = StatusCompanyShareInfoFileConst;

    rows: any[] = [];

    policyFile: any = {
        'policyFileId': 0,
        'effectiveDate': null,  // Ngày có hiệu lực
        'expirationDate': null, // Ngày hết hiệu lực
        'name': null,
        'url': null,
        'companyShareSecondaryId': 0,
        'tradingProviderId': 0
    };

    filedDates=['effectiveDate','expirationDate'];
    urlfilePDF:string = '';
    submitted: boolean;

    actions: any[] = [];
    actionsDisplay: any[] = [];
    listAction: any[] = [];

    ngOnInit() {
        console.log('--------------------', this.rows);
        this.actions = [
            // {
            //     label: 'Phê duyệt',
            //     icon: 'pi pi-check',
            //     statusActive: [this.StatusCompanyShareSecondaryFileConst.RESPONSE_FALSE],
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
            //     label: 'Sửa',
            //     icon: 'pi pi-pencil',
            //     statusActive: [this.StatusCompanyShareSecondaryFileConst.RESPONSE_FALSE],
            //     permission: this.isGranted(),
            //     command: () => {
            //         this.edit();
            //     }
            // },
            // {
            //     label: 'Xóa',
            //     icon: 'pi pi-trash',
            //     statusActive: [this.StatusCompanyShareSecondaryFileConst.RESPONSE_FALSE],
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

    genListAction(data = []) {
        this.listAction = data.map(orderItem => {
            const actions = [];
            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_FileChinhSach_Sua])) {
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
            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_BTKH_TTCT_FileChinhSach_Xoa])) {
                actions.push(
                    {
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
            companyShareSecondaryId: +this.companyShareSecondaryId,
        };
    }

    clickDropdown(row) {
        this.actionsDisplay = [];
        this.policyFile = { 
            ...row,
            effectiveDate: row.effectiveDate ? new Date(row.effectiveDate) : null, 
            expirationDate: row.expirationDate ? new Date(row.expirationDate) : null, 
        };
        console.log({ distributionContractPayment: row });
        this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(+row.status) && action.permission);
    }

    create() {
        this.resetContractTemplateObject();
        this.submitted = false;
        this.modalDialog = true;
        this.headerTitle = 'Thêm mới';
    }

    edit(policyFile) {
        
        this.modalDialog = true;
        this.policyFile = {
            ...policyFile
        }
        this.setTimeZoneList(this.fieldDates, this.policyFile);

        this.headerTitle = 'Cập nhật';
    }

    confirmDelete() {
        this.deleteItemDialog = false;
        this._contractTemplateService.delete(this.policyFile.id).subscribe(
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
                    detail: `Không xóa được chính sách ${this.policyFile.displayName}`,
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

        this._companyShareSecondaryFile.getAll(this.page, this.companyShareSecondaryId).subscribe(
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
        this.submitted = true;
        //
        this.setTimeZoneList(this.filedDates, this.policyFile);
        if (this.policyFile.policyFileId) {
            this._companyShareSecondaryFile.update(this.policyFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
                        this.submitted = false;
                        this.setPage({ page: this.page.pageNumber });
                        this.hideDialog();
                    } else {
                        this.submitted = false;
                        this.resetTimeZoneList(this.filedDates, this.policyFile);
                    }
                },
                () => {
                    this.submitted = false;
                    this.resetTimeZoneList(this.filedDates, this.policyFile);
                }
            );
        } else {
            this._companyShareSecondaryFile.create(this.policyFile).subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                        this.submitted = false;
                        this.setPage();
                        this.hideDialog();
                    } else {
                        this.submitted = false;
                        this.resetTimeZoneList(this.filedDates, this.policyFile);
                    }
                },
                () => {
                    this.submitted = false;
                    this.resetTimeZoneList(this.filedDates, this.policyFile);
                }
            );
        }
    }

    validForm(): boolean {
        return this.policyFile?.url?.trim() && this.policyFile?.name?.trim();
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
                        this.policyFile.url = response.data;
                    }
                },
                (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    // delete(policyFile) {
    //     this.confirmationService.confirm({
    //         message: 'Bạn có chắc chắn xóa chính sách này?',
    //         header: 'Thông báo',
    //         acceptLabel: "Đồng ý",
	// 		rejectLabel: "Hủy",
    //         icon: 'pi pi-times-circle',
    //         accept: () => {
    //             this._companyShareSecondaryFile.delete(policyFile.policyFileId).subscribe((response) => {
    //                 if (this.handleResponseInterceptor(response, "Xóa thành công")) {
    //                     // this.messageService.add({ severity: 'success', summary: '', detail: '', life: 1500 });
    //                     this.setPage();
    //                 }
    //             });
    //         },
    //         reject: () => {

    //         },
    //     });
    // }

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
			this._companyShareSecondaryFile.delete(policyFile.policyFileId).subscribe((response) => {
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

    approve() {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn phê duyệt thanh toán này?',
            header: 'Thông báo',
            acceptLabel: "Đồng ý",
			rejectLabel: "Hủy",
            icon: 'pi pi-check-circle',
            accept: () => {
                this._companyShareSecondaryFile.approve(this.policyFile.policyFileId).subscribe((response) => {
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
            icon: 'pi pi-times-circle',
            accept: () => {
                this._companyShareSecondaryFile.cancel(this.policyFile.policyFileId).subscribe((response) => {
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

    onRowEditSave(file) {
        const data = file;
        console.log('data', data)
        this._companyShareSecondaryFile.update(data).subscribe(
            (response) => {
                if (this.handleResponseInterceptor(response, "Cập nhật thành công")) {
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
