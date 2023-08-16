import { Component, Inject, Injector, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ContractTypeConst, DistributionContractConst, FormNotificationConst, SearchConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ContractTemplateServiceProxy, DistributionContractFileServiceProxy, GuaranteeAssetServiceProxy } from "@shared/service-proxies/company-share-manager-service";
import { ConfirmationService, MessageService } from "primeng/api";
import { debounceTime, map } from "rxjs/operators";
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AppUtilsService } from "@shared/services/utils.service";
import { computeSegEndResizable } from "@fullcalendar/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies-base";
import { DialogService } from "primeng/dynamicdialog";
import { FormNotificationComponent } from "src/app/form-notification/form-notification.component";

@Component({
    selector: 'app-company-share-info-collateral',
    templateUrl: './company-share-info-collateral.component.html',
    styleUrls: ['./company-share-info-collateral.component.scss'],
    providers: [ConfirmationService, MessageService]
})
export class CompanyShareInfoCollateralComponent extends CrudComponentBase {

    constructor(
        injector: Injector,
        messageService: MessageService,
        private confirmationService: ConfirmationService,
        private _contractTemplateService: ContractTemplateServiceProxy,
        private _distributionContractFile: DistributionContractFileServiceProxy,
        private _guaranteeAssetService: GuaranteeAssetServiceProxy,
        private _utilsService: AppUtilsService,
        private routeActive: ActivatedRoute,
        private _dialogService: DialogService,
        @Inject(API_BASE_URL) baseUrl?: string,
    ) {
        super(injector, messageService);
        this.companyShareId = this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
        this.baseUrl = baseUrl !== undefined && baseUrl !== null ? baseUrl : "";
    }

    private baseUrl: string;
    companyShareId: string;

    title: string;
    headerTitle: string;
    collateral = {
        'guaranteeAssetId': 0,
        'companyShareId': 0,
        'code': null,
        'assetValue': null,
        'descriptionAsset': null,
        'guaranteeFiles': []
    }
    modalDialog: boolean;
    
    rows: any[] = [];

    submitted: boolean;

    actions: any[] = [];
    actionsDisplay: any[] = [];

    listAction: any[] = [];
    filesAction: any[] = [];
    listFilesAction: any[] = [];

    guaranteeFiles: any[] = [];
    guaranteeFilesDisplay: any[] = [];

    isUpload = false;

    ngOnInit() {
        this.actions = [
            // {
            // 	label: 'Phê duyệt',
            // 	icon: 'pi pi-check',
            // 	statusActive: [],
            //   permission: this.isGranted(),
            // 	command: () => {
            // 		this.approve();
            // 	}
            // },
            {
                label: 'Sửa',
                icon: 'pi pi-pencil',
                statusActive: [],
                permission: this.isGranted(),
                command: ($event) => {
                    // this.edit($event.item.data);
                }
            },
            {
                label: 'Xóa',
                icon: 'pi pi-trash',
                statusActive: [],
                permission: this.isGranted(),
                command: () => {
                    // this.delete();
                }
            },
        ];

        this.guaranteeFiles = [
            {
                command: () => {
                    // this._guaranteeAssetService.getAll();
                }
            },
        ];
        this.setPage({ page: this.offset });
    }

    genListAction(data = []) {
        this.listAction = data.map(companyShareInfoCollateralItem => {
            const actions = [];

            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_LTP_TSDB_Sua])){
				actions.push({
                    data: companyShareInfoCollateralItem,
                    label: 'Sửa',
                    icon: 'pi pi-pencil',
                    command: ($event) => {
                        this.edit($event.item.data);
                    }
				})
			}

            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_LTP_TSDB_Xoa])){
				actions.push({
                    data: companyShareInfoCollateralItem,
                    label: 'Xóa',
                    icon: 'pi pi-trash',
                    command: ($event) => {
                        this.delete($event.item.data);
                    }
				})
			}

            return actions;
        });
        this.listFilesAction = data.map(companyShareInfoCollateralItem => {
            let filesAction = [];
            if (this.isGranted([this.PermissionCompanyShareConst.CompanyShare_LTP_TSDB_TaiXuong])){
                filesAction = companyShareInfoCollateralItem.guaranteeFiles.map(item => {
                    let file = {
                        dataUrl: item.fileUrl,
                        label: item.title,
                        icon: 'pi pi-download',
                        command: ($event) => {
                            this.downloadFile($event.item.dataUrl);
                        }
                    }
                    return file;
                });
            }
            return filesAction;
        });
        console.log('------------', this.listFilesAction[0]);
    }

    resetCollateral() {
        this.collateral = {
            'guaranteeAssetId': 0,
            'companyShareId': 0,
            'code': null,
            'assetValue': null,
            'descriptionAsset': null,
            'guaranteeFiles': []
        }
    }

    changeTitleFile(title) {
        this.isUpload = false;
        if (title?.trim()) this.isUpload = true;
    }

    clickDropdown(row) {
        this.actionsDisplay = [];
        this.collateral = { ...row };
        // this.actionsDisplay = this.actions.filter(action => action.statusActive.includes(+row.status) && action.permission);
        this.actionsDisplay = [...this.actions];
    }

    clickDropdownFile(row) {
        this.guaranteeFilesDisplay = [];
        this.collateral = { ...row };
        this.guaranteeFilesDisplay = [...this.guaranteeFiles];
        console.log('--------', this.guaranteeFilesDisplay);
    }

    create() {
        this.resetCollateral();
        this.collateral.companyShareId = +this.companyShareId;
        this.submitted = false;
        this.modalDialog = true;
    }

    edit(collateral) {
        this.collateral = { ...collateral };
        this.submitted = false;
        this.modalDialog = true;
    }

    setPage(pageInfo?: any) {
        this.page.pageNumber = pageInfo?.page ?? this.offset;
        this.page.keyword = this.keyword;
        this.isLoading = true;

        this._guaranteeAssetService.getAll(this.page, this.companyShareId).subscribe(
            (res) => {
                this.isLoading = false;
                if (this.handleResponseInterceptor(res, "")) {
                    this.page.totalItems = res.data.totalItems;
                    this.rows = res.data.items;
                    console.log({ rowsFile: res.data.items, totalItems: res.data.totalItems });
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
    }

    save() {
        this.submitted = true;
        //
        if (this.collateral.guaranteeAssetId) {
            console.log('Hello: -----------------', this.collateral);
            this._guaranteeAssetService.update(this.collateral).subscribe(
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
            this._guaranteeAssetService.create(this.collateral).subscribe(
                (response) => {
                    console.log(this.collateral);
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
        const validRequired = this.collateral?.code
                && this.collateral?.assetValue
                && this.collateral?.descriptionAsset;
        return validRequired;
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "collateral").subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                        this.collateral.guaranteeFiles.push({
                            'title': this.title,
                            'fileUrl': response.data,
                        });
                        this.title = null;
                        this.isUpload = false;
                        console.log(this.collateral);
                    }
                }, (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    customTitle() {
        console.log(this.collateral.guaranteeFiles)
    }

    onRowEditSave(fileInfo) {
        console.log(this.collateral.guaranteeFiles);
        this.messageService.add({ severity: 'success', summary: '', detail: 'Cập nhật thành công' });
    }

    deleteFile(index) {
        this.confirmationService.confirm({
            message: 'Bạn có chắc chắn xóa file này?',
            header: 'Cảnh báo!',
            icon: 'pi pi-exclamation-triangle',
            acceptLabel: 'Có',
            rejectLabel: 'Không',
            accept: () => {
                this.collateral.guaranteeFiles.splice(index, 1);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công' });
            },
            reject: (type) => {

            }
        });
    }

    // delete(collateral) {
    //     console.log('----', collateral);
    //     this.confirmationService.confirm({
    //         message: 'Bạn có chắc chắn xoá tài sản đảm bảo này?',
    //         header: 'Cảnh báo!',
    //         icon: 'pi pi-exclamation-triangle',
    //         acceptLabel: 'Có',
    //         rejectLabel: 'Không',
    //         accept: () => {
    //             this._guaranteeAssetService.delete(collateral?.guaranteeAssetId).subscribe((response) => {
    //                 if (this.handleResponseInterceptor(response, "")) {
    //                     this.messageService.add({ severity: 'success', summary: '', detail: 'Xoá thành công!', life: 1500 });
    //                     this.setPage();
    //                 }
    //             });
    //         },
    //         reject: () => {

    //         },
    //     });
    // }

    delete(collateral) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xoá tài sản đảm bảo này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
		ref.onClose.subscribe((dataCallBack) => {
			console.log({ dataCallBack });
			if (dataCallBack?.accept) {
			this._guaranteeAssetService.delete(collateral?.guaranteeAssetId).subscribe((response) => {
			  if (
				this.handleResponseInterceptor(
				  response,
				  "Xoá tài sản đảm bảo thành công"
				)
			  ) {
				this.setPage();
			  }
			});
			}
		});
	  }

    downloadFile(url) {
        console.log('File-----', url);
        if(url.fileUrl != null) {
            const fileUrl = this.baseUrl + "/" + url.fileUrl;
            this._utilsService.makeDownload('', fileUrl);
        } else {
            const fileUrl = this.baseUrl + "/" + url;
            this._utilsService.makeDownload('', fileUrl);
        }
    }
}
