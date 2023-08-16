import { Component, ElementRef, Injector, Input, OnInit, ViewChild } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { AppConsts, FormNotificationConst, TableConst } from "@shared/AppConsts";
import { MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { UploadImageComponent } from "src/app/components/upload-image/upload-image.component";
import { AppUtilsService } from "@shared/services/utils.service";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ActivatedRoute, Router } from "@angular/router";
import { DistributionService } from "@shared/services/distribution.service";
import { ContractTemplateService } from "@shared/services/contract-template.service";
import { IColumn } from "@shared/interface/p-table.model";

@Component({
  selector: "app-distribution-over-view",
  templateUrl: "./distribution-over-view.component.html",
  styleUrls: ["./distribution-over-view.component.scss"],
})
export class DistributionOverViewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    private router: Router,
    private _dialogService: DialogService,
	private routeActive: ActivatedRoute,
    private _utilsService: AppUtilsService,
    protected messageService: MessageService,
    private _distributionService: DistributionService,
    public dialogService: DialogService,
    private _contractTemplateService: ContractTemplateService,
  ) 
  {
    super(injector, messageService);
    this.distributionId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }
    
    distributionId: number;

    title: string;
    nameOrg: string;
    urlOrg: string;
    inputData: any;
    postForm: FormGroup;
    types = [];
    statuses = [];
    baseUrl: string;
    formatVideo: boolean;
    formatImage: boolean;
    caretPos: number = 0;
    disableStatus: boolean = true;
    headerTitle: string = ""; 
    overView = {
        id: 0,
        contentType: "",
        overviewImageUrl: "",
        overviewContent: "",
        projectOverviewOrgs: [],
        projectOverviewFiles: []
    }

    listAction: any[] = [];
    filesAction: any[] = [];
    listFilesAction: any[] = [];

    projectOverviewOrg = {
        id: 0,
        fakeId: 0,
        distributionId: 0,
        name: "",
        orgCode: "", 
        icon: "",
        url: ""
    };

    icon: any;

    projectOverviewFile = {
        id: 0,
        fakeId: 0,
        distributionId: 0,
        title: "",
        url: ""
    };

    htmlMarkdownOptions: any = [
        {
        value: "MARKDOWN",
        name: "MARKDOWN",
        },
        {
        value: "HTML",
        name: "HTML",
        },
    ];

    // EDIT
    isEdit: boolean = false;
    fieldErrors: any = [];
    modalDialog: boolean;
    modalDialogOrgs: boolean;
    submitted: boolean;

    modalDialogPDF: boolean;
    urlfilePDF:string = '';

    content: string;
    contentType: string;

    imageDefault = AppConsts.imageDefault;

    @Input() contentHeight: number = 0;
    scrollHeight: number = 0;

    columnProjectOverviews: IColumn[] = [];
    columnProjectOverviewFiles: IColumn[] = [];

    ngOnInit(): void {   
        this.setColumnTables();
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        this.getOverview(this.distributionId);
        
    }

    @ViewChild('pageEl') pageEl: ElementRef<HTMLElement>
    ngAfterViewInit() {
        let heightPageInit = this.pageEl.nativeElement.offsetHeight;
        this.scrollHeight = this.contentHeight - heightPageInit;
    }

    setColumnTables() {
        this.columnProjectOverviews = [
            { field: 'id', header: '#ID', width: 4},
            { field: 'name', header: 'Tên tổ chức', width: 25, isResize: true},
            { field: 'orgCode', header: 'Mã tổ chức', width: 12},
            {   
                field: 'editOrg', header: 'Cập nhật', width: 7, type: TableConst.columnTypes.ACTION_ICON, 
                icon: 'pi pi-pencil',
                class: 'justify-content-center'
            },
            { 
                field: 'deleteOrg', header: 'Xóa', width: 5, type: TableConst.columnTypes.ACTION_ICON,
                icon: 'pi pi-trash',
                class: 'justify-content-center'
            },
        ];
        //
        this.columnProjectOverviewFiles = [
            { field: '', header: '#', width: 3, type: TableConst.columnTypes.REORDER },
            { field: 'title', header: 'Tiêu đề', width: 25, isResize: true },
            { field: 'dowloadFile', header: 'Tải file', width: 7, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-download' },
            { field: 'viewFile', header: 'Xem file', width: 7, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-eye' },
            { field: 'editFile', header: 'Cập nhật', width: 7, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-pencil' },
            { field: 'deleteFile', header: 'Xóa', width: 5, type: TableConst.columnTypes.ACTION_ICON, icon: 'pi pi-trash' },
        ]
    } 

    setData(overView) {
        this.overView.projectOverviewOrgs = overView.projectOverviewOrgs.map(item => {
            item.editOrg = (item) => this.editOrg(item);
            item.deleteOrg = (item) => this.deleteOrg(item);
            return item;
        });
        //
        this.overView.projectOverviewFiles = overView.projectOverviewFiles.map(item => {
            item.dowloadFile = (item) => this.downloadFile(item);
            item.viewFile = (item) => this.viewFile(item);
            item.editFile = (item) => this.editFile(item);
            item.deleteFile = (item) => this.deleteFile(item);
            return item;
        });
    }

  	// LABEL EDIT BUTTON
	labelButtonEdit() {
		return this.isEdit ? 'Lưu lại' : 'Chỉnh sửa';
	}

    setStatusEdit() {
		this.isEdit = !this.isEdit;
	}

    createFile() {
        this.resetFile();
        this.submitted = false;
        this.modalDialog = true;
        this.headerTitle = "Thêm mới"
    }

    createOrg() {
        this.modalDialogOrgs = true;
        this.projectOverviewOrg = {
            id: 0,
            fakeId: 0,
            distributionId: this.distributionId,
            orgCode: "", 
            name: "",
            icon: "",
            url: ""
        }
        this.headerTitle = "Thêm mới"
    }

    resetFile() {
        this.projectOverviewFile = {
            id: 0,
            fakeId: 0,
            distributionId: 0,
            title: "",
            url: ""
        };
    }

    edit() {
        // this.collateral = { ...collateral };
        this.submitted = false;
        this.modalDialog = true;
    }

    hideDialog() {
        this.modalDialog = false;
        this.submitted = false;
        this.modalDialogPDF = false;
    }

    genListAction(data = []) {
        this.listAction = data.map(bondInfoCollateralItem => {
            const actions = [];
            actions.push({
                data: bondInfoCollateralItem,
                label: 'Sửa',
                icon: 'pi pi-pencil',
                command: ($event) => {
                    this.editFile($event.item.data);
                }
            })

            actions.push({
                data: bondInfoCollateralItem,
                label: 'Xóa',
                icon: 'pi pi-trash',
                command: ($event) => {
                    this.deleteFile($event.item.data);
                }
            })
            return actions;
        });
        this.listFilesAction = data.map(bondInfoCollateralItem => {
            let filesAction = [];
            filesAction = bondInfoCollateralItem.guaranteeFiles.map(item => {
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
            
            return filesAction;
        });
        console.log('------------', this.listFilesAction[0]);
    }

    editFile(row) {
        this.modalDialog = true;
        this.projectOverviewFile = {...row};
        this.headerTitle = 'Cập nhật';
    }

    downloadFile(row) {
        if(row.url != null) {
            const fileUrl = this.baseUrl + "/" + row.url;
            console.log('fileUrl_____: ', fileUrl);
            
            this._utilsService.makeDownload('', fileUrl);
        } else {
            // const fileUrl = this.baseUrl + "/" + url;
            // this._utilsService.makeDownload('', fileUrl);
        }
    }

    editOrg(row) {
        this.modalDialogOrgs = true;
        this.projectOverviewOrg = {...row};
        this.headerTitle = 'Cập nhật';
    }

    saveOgr() {
        let id = this.projectOverviewOrg?.fakeId || this.projectOverviewOrg?.id;
        if(!id) {
            this.overView.projectOverviewOrgs.push({
                fakeId: new Date().getTime(),
                distributionId: this.distributionId,
                name: this.projectOverviewOrg?.name,
                orgCode: this.projectOverviewOrg?.orgCode,  
                icon: this.projectOverviewOrg?.icon,
                url: this.projectOverviewOrg?.url
            });
            this.setData(this.overView);
        } else {
            let item = this.overView.projectOverviewOrgs.find(o => ((o.id == this.projectOverviewOrg?.id) && o?.id) || ((o.fakeId == this.projectOverviewOrg?.fakeId) && o.fakeId));
            item.name = this.projectOverviewOrg?.name;
            item.orgCode = this.projectOverviewOrg?.orgCode;
            item.icon = this.projectOverviewOrg?.icon;
            item.url = this.projectOverviewOrg?.url;
        }
        this.modalDialogOrgs = false;
    }      
    
    saveFile(){
        let id = this.projectOverviewFile?.fakeId || this.projectOverviewFile?.id;
        if(!id) {
            this.overView.projectOverviewFiles.push({
                fakeId: new Date().getTime(),
                distributionId: this.distributionId,
                title: this.projectOverviewFile?.title,
                url: this.projectOverviewFile?.url
            });
            this.setData(this.overView);
        } else {
            let item = this.projectOverviewFile.id
                ? this.overView.projectOverviewFiles.find(o => o.id === this.projectOverviewFile.id)
                : this.overView.projectOverviewFiles.find(o => o.fakeId === this.projectOverviewFile.fakeId);
            if(item) {
                item.title = this.projectOverviewFile?.title;
                item.url = this.projectOverviewFile?.url;
            }
        }
        this.modalDialog = false;
    }

    myUploader(event) {
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "distribution").subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Tải file thành công")) {
                        this.projectOverviewFile.url = response.data;
                    }
                }, (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    selectImg() {
        const ref = this.dialogService.open(UploadImageComponent, {
            header: 'Tải hình ảnh',
            width: '500px',
            data: {
                inputData: [this.overView.overviewImageUrl]
              },
          });
          ref.onClose.subscribe(images => {
            if (images && images.length > 0)
              this.overView.overviewImageUrl = images[0].data;
          });
    }

    selectAvatar() {
        const ref = this.dialogService.open(UploadImageComponent, {
            header: 'Tải hình ảnh',
            width: '500px',
            data: {
                inputData: [this.icon]
            },
        });
        ref.onClose.subscribe(images => {
            if (images && images.length > 0) {
                this.icon = images[0].data;
                this.projectOverviewOrg.icon = this.icon;
            }
        });
    }

    close() {
        this.modalDialogOrgs = false;
        this.modalDialog = false;
    }

    removeElement(array, elem) {
        var index = array.indexOf(elem);
        if (index > -1) {
            array.splice(index, 1);
        }
    }

    deleteFile(row) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xoá file này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this.removeElement(this.overView.projectOverviewFiles, row);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!' });
            }
		});
	}

    deleteOrg(row) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					data: {
						title : "Bạn có chắc chắn xoá tổ chức này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this.removeElement(this.overView.projectOverviewOrgs, row);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!' });
            }
		});
	}
    
    insertImage() {
        const ref = this.dialogService.open(UploadImageComponent, {
            header: 'Chèn hình ảnh',
            width: '600px',
            data: {
                inputData: [],
                showOrder: false
            },
        });
        ref.onClose.subscribe(images => {
            let imagesUrl = "";
            images.forEach(image => {
                imagesUrl += `![](${this.baseUrl}/${image.data}) \n`;
            })
        
            let oldContentValue = this.content;
            let a = oldContentValue.slice(0, this.caretPos) + imagesUrl + oldContentValue.slice(this.caretPos); 
            this.content = a;
        })
    }

    getCaretPos(oField) {
        if (oField.selectionStart || oField.selectionStart == '0') {
            this.caretPos = oField.selectionStart;
        }
    }

    changeEdit() {
        this.setStatusEdit();
        if (!this.isEdit) {
            this.overView.id = this.distributionId;
            this.overView.contentType = this.contentType;
            this.overView.overviewContent = this.content;
            this.overView.projectOverviewFiles.forEach( (item, index) => {
                item.sortOrder = index + 1;
            });
            this._distributionService.updateOverview(this.overView).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.getOverview(this.distributionId, false);
                } 
            }, (err) => {
                console.log('err---', err);
            });
        }
    }

    // GET OVERVIEW
	getOverview(distributionId, isLoading: boolean = true) {
		this.isLoading = isLoading;
		this._distributionService.getOverviewById(distributionId).subscribe(res => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.overView = { ...res.data };
                this.setData(this.overView);
                this.content = this.overView?.overviewContent ?? "";
                this.contentType = this.overView?.contentType  ?? "MARKDOWN";
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

    validForm(): boolean {
		return  this.projectOverviewOrg?.name?.trim() && this.projectOverviewOrg?.orgCode?.trim() && this.projectOverviewOrg?.icon?.trim() && true;
	}

    validFormFile(): boolean {
        return this.projectOverviewFile?.title.trim() && this.projectOverviewFile?.url.trim() && true;
    }

    viewFile(row) {
        let fileUrl = row.url
        this.urlfilePDF = '/' + fileUrl;
        if(!fileUrl){
            this.messageError("Không có file hồ sơ", "")
        }else{
            if(this.utils.isPdfFile(fileUrl)){
                this._distributionService.viewFilePDF(this.urlfilePDF + '&download=true').subscribe();
            } else {
                this.messageError("Hệ thống hiện tại chỉ hỗ trợ xem file PDF", "")
            }
        }
    }
}
