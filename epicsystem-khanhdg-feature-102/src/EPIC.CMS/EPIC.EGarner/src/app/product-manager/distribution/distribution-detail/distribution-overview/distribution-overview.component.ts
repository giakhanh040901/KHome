import { Component, Injector, Input, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AppConsts, FormNotificationConst, OverviewConst } from "@shared/AppConsts";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService, DynamicDialogConfig } from "primeng/dynamicdialog";
import { decode } from "html-entities";
import { AppUtilsService } from "@shared/services/utils.service";
import { ContractTemplateServiceProxy } from "@shared/service-proxies/trading-contract-service";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ActivatedRoute, Router } from "@angular/router";
import { DistributionService } from "@shared/services/distribution.service";
import { FormNotificationComponent } from "src/app/form-general/form-notification/form-notification.component";
import { UploadImageComponent } from "src/app/components-general/upload-image/upload-image.component";

@Component({
  selector: 'app-distribution-overview',
  templateUrl: './distribution-overview.component.html',
  styleUrls: ['./distribution-overview.component.scss']
})

export class DistributionOverviewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private router: Router,
    private _dialogService: DialogService,
	private routeActive: ActivatedRoute,
    private _utilsService: AppUtilsService,
    protected messageService: MessageService,
    private confirmationService: ConfirmationService,        
    private _distributionService: DistributionService,
    public dialogService: DialogService,
    private _contractTemplateService: ContractTemplateServiceProxy,
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
    types = [];
    statuses = [];
    baseUrl: string;
    formatVideo: boolean;
    formatImage: boolean;
    caretPos: number = 0;
    disableStatus: boolean = true;
    headerTitle: string = ""; 

    overView: any = {
        id: 0,
        contentType: "",
        overviewImageUrl: "",
        overviewContent: "",
        productOverviewOrgs: [],
        productOverviewFiles: [],
    };
    content: string;
    contentType: string;

    listAction: any[] = [];
    filesAction: any[] = [];
    listFilesAction: any[] = [];

    productOverviewOrg = {
        id: 0,
        fakeId: 0,
        distributionId: 0,
        name: "",
        code: "", 
        icon: "",
        url: "",
        role: "",
    };

    icon: any;

    productOverviewFile = {
        id: 0,
        fakeId: 0,
        distributionId: 0,
        title: "",
        url: "",
        documentType: "",
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

    OverviewConst = OverviewConst;

    ngOnInit(): void {        
        this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
        this.getOverview(this.distributionId);
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
        this.productOverviewOrg = {
            id: 0,
            fakeId: 0,
            distributionId: this.distributionId,
            code: "", 
            name: "",
            icon: "",
            url: "",
            role: "",
        }
        this.headerTitle = "Thêm mới"
    }

    resetFile() {
        this.productOverviewFile = {
            id: 0,
            fakeId: 0,
            distributionId: 0,
            title: "",
            url: "",
            documentType: "",
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

    editFile(file) {
        this.modalDialog = true;
        this.productOverviewFile = {...file};

        this.headerTitle = 'Cập nhật';
        console.log('dataEditFile', this.productOverviewFile );
        // this.overView.productOverviewOrgs.find()
    }

    downloadFile(file) {
        console.log('File-----', file);
        if(file.url != null) {
            const fileUrl = this.baseUrl + "/" + file.url;
            console.log('fileUrl_____: ', fileUrl);
            
            this._utilsService.makeDownload('', fileUrl);
        } else {
            // const fileUrl = this.baseUrl + "/" + url;
            // this._utilsService.makeDownload('', fileUrl);
        }
    }

    editOrg(org) {
        this.modalDialogOrgs = true;
        this.productOverviewOrg = {...org};

        this.headerTitle = 'Cập nhật';
        console.log('dataEdit', this.productOverviewOrg);
        console.log('overView___', this.overView);
        // this.overView.productOverviewOrgs.find()
    }

    saveOgr(row?: any) {
        console.log('productOverviewOrg___',this.productOverviewOrg);
        let id = this.productOverviewOrg?.fakeId || this.productOverviewOrg?.id;
        if(!id) {
            console.log('this.overView.productOverviewOrgs', this.overView.productOverviewOrgs);
            this.overView.productOverviewOrgs.push({
                fakeId: new Date().getTime(),
                distributionId: this.distributionId,
                name: this.productOverviewOrg?.name,
                code: this.productOverviewOrg?.code,  
                icon: this.productOverviewOrg?.icon,
                url: this.productOverviewOrg?.url,
                role: this.productOverviewOrg?.role,
            });
        } else {
            let item = this.overView.productOverviewOrgs.find(o => ((o.id == this.productOverviewOrg?.id) && o?.id) || ((o.fakeId == this.productOverviewOrg?.fakeId) && o.fakeId));
            item.name = this.productOverviewOrg?.name;
            item.code = this.productOverviewOrg?.code;
            item.icon = this.productOverviewOrg?.icon;
            item.url = this.productOverviewOrg?.url;
            item.role = this.productOverviewOrg?.role
        }
        
        console.log('this.overView: ',this.overView );
        
        this.modalDialogOrgs = false;
    }      
    
    saveFile(){
        let id = this.productOverviewFile?.fakeId || this.productOverviewFile?.id;
        if(!id) {
            this.overView.productOverviewFiles.push({
                fakeId: new Date().getTime(),
                distributionId: this.distributionId,
                title: this.productOverviewFile?.title,
                url: this.productOverviewFile?.url,
                documentType: this.productOverviewFile?.documentType,
            });
        } else {
            let item = this.productOverviewFile.id
            ? this.overView.productOverviewFiles.find(o => o.id === this.productOverviewFile.id)
            : this.overView.productOverviewFiles.find(o => o.fakeId === this.productOverviewFile.fakeId)
            
           if(item) {
               item.title = this.productOverviewFile?.title;
               item.url = this.productOverviewFile?.url;
               item.documentType = this.productOverviewFile?.documentType;
           }
        }

        console.log('this.overView: ',this.overView );
        
        this.modalDialog = false;
    }

    onRowEditSave(file) {
        console.log(file);
        this.messageService.add({ severity: 'success', summary: '', detail: 'Cập nhật thành công' });
    }

    myUploader(event) {
        console.log('event', event);
        
        if (event?.files[0]) {
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "distribution").subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                        this.productOverviewFile.url = response.data;
                        // this.overView.productOverviewFiles.push({
                        //     'id': 0,
                        //     'distributionId': this.distributionId,
                        //     'title': this.title,
                        //     "url": response.data,
                        // });
                        // this.close()
                        // this.title = null;
                        // this.isUpload = false;
                        // console.log('this.overView.productOverviewFiles', this.overView);
                        
                    }
                }, (err) => {
                    this.messageError("Có sự cố khi upload!");
                }
            );
        }
    }

    selectImg() {
        const ref = this.dialogService.open(UploadImageComponent, {
            data: {
              inputData: [this.overView.overviewImageUrl]
            },
            header: 'Tải hình ảnh',
            width: '500px',
            footer: ""
          });
          ref.onClose.subscribe(images => {
            if (images && images.length > 0)
              this.overView.overviewImageUrl = images[0].data;
              console.log(`this.overviewImageUrl: ${this.baseUrl}/${this.overView.overviewImageUrl}` );
              console.log("overview...........: ", this.overView);
          });
    }

    selectAvatar() {
        const ref = this.dialogService.open(UploadImageComponent, {
          data: {
            inputData: [this.icon]
          },
          header: 'Tải hình ảnh',
          width: '500px',
          footer: ""
        });
        ref.onClose.subscribe(images => {
          if (images && images.length > 0)
            this.icon = images[0].data;
            this.productOverviewOrg.icon = this.icon;
            console.log(`this.productOverviewOrg.icon: ${this.baseUrl}/${this.icon}` );
            console.log("productOverviewOrg...........: ", this.productOverviewOrg);
        });
    }
    
    // changeTitleFile(title) {
    //     this.isUpload = false;
    //     if (title?.trim()){
    //         this.title = title;
    //         this.isUpload = true;
    //     }   
    // }

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

    deleteFile(file) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',  
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xoá file này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this.removeElement(this.overView.productOverviewFiles, file);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!' });
            }
		});
	}

    deleteOrg(orgs) {
		const ref = this._dialogService.open(
				FormNotificationComponent,
				{
					header: "Thông báo",
					width: '600px',
					contentStyle: { "max-height": "600px", "overflow": "auto", "padding-bottom": "50px" },
					styleClass: 'p-dialog-custom',  
					baseZIndex: 10000,
					data: {
						title : "Bạn có chắc chắn xoá tổ chức này?",
						icon: FormNotificationConst.IMAGE_CLOSE,
					},
				}
			);
        ref.onClose.subscribe((dataCallBack) => {
            console.log({ dataCallBack });
            if (dataCallBack?.accept) {
                this.removeElement(this.overView.productOverviewOrgs, orgs);
                this.messageService.add({ severity: 'success', summary: '', detail: 'Xóa thành công!' });
            }
		});
	}
    
    insertImage() {
        const ref = this.dialogService.open(UploadImageComponent, {
          data: {
            inputData: [],
            showOrder: false
          },
          header: 'Chèn hình ảnh',
          width: '600px',
          footer: ""
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
          console.log("this.caretPos",this.caretPos);
        }
    }

    changeEdit() {
        this.setStatusEdit();

        console.log(' this.overView  this.overView:',  this.overView);
        if (!this.isEdit) {
            this.overView.id = this.distributionId;
            this.overView.contentType = this.contentType;
            this.overView.overviewContent = this.content;
            this._distributionService.updateOverview(this.overView).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.getOverview(this.distributionId);
                } 
            }, (err) => {
                console.log('err---', err);
            });
        }
         else {
        }        
    }

    // GET OVERVIEW
	getOverview(distributionId) {
		this.isLoading = true;
		this._distributionService.getOverviewById(distributionId).subscribe(res => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.overView = {
                    ...this.overView,
					...res.data,			
				};

                this.content = this.overView?.overviewContent ?? "";
                this.contentType = this.overView?.contentType?? "MARKDOWN";

                console.log('overView vao day', this.overView);
                
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}

    validForm(): boolean {
		return  this.productOverviewOrg?.role?.trim() && this.productOverviewOrg?.name?.trim() && this.productOverviewOrg?.code?.trim() && this.productOverviewOrg?.icon?.trim() && true;
	}

    validFormFile(): boolean {
        return this.productOverviewFile?.title.trim() && this.productOverviewFile?.url.trim() && true;
    }
}


