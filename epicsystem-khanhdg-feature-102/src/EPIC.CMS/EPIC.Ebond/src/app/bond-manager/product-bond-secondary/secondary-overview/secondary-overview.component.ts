import { Component, Injector, Input, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AppConsts, FormNotificationConst } from "@shared/AppConsts";
import { CrudComponentBase } from "@shared/crud-component-base";
import { ContractTemplateServiceProxy, ProductBondSecondaryServiceProxy } from "@shared/service-proxies/bond-manager-service";
import { AppUtilsService } from "@shared/services/utils.service";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { UploadImageComponent } from "src/app/components/upload-image/upload-image.component";
import { FormNotificationComponent } from "src/app/form-notification/form-notification.component";

@Component({
  selector: "app-secondary-overview",
  templateUrl: "./secondary-overview.component.html",
  styleUrls: ["./secondary-overview.component.scss"],
})
export class SecondaryOverviewComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private router: Router,
    private _dialogService: DialogService,
    private routeActive: ActivatedRoute,
    private _utilsService: AppUtilsService,
    protected messageService: MessageService,
    private confirmationService: ConfirmationService,
	private _secondaryService: ProductBondSecondaryServiceProxy,
    public dialogService: DialogService,
    private _contractTemplateService: ContractTemplateServiceProxy
  ) {
    super(injector, messageService);
    this.bondSecondaryId = +this.cryptDecode(this.routeActive.snapshot.paramMap.get('id'));
  }

  bondSecondaryId: number;

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
      bondSecondaryId: 0,
	  contentType: "",
	  overviewImageUrl: "",
	  overviewContent: "",
	  bondSecondaryOverviewFiles: [],
	  bondSecondaryOverviewOrgs: []
  }

  listAction: any[] = [];
  filesAction: any[] = [];
  listFilesAction: any[] = [];

  projectOverviewOrg = {
	  id: 0,
	  fakeId: 0,
	  bondSecondaryId: 0,
	  name: "",
	  orgCode: "", 
	  icon: "",
	  url: ""
  };

  icon: any;

  projectOverviewFile = {
	  id: 0,
	  fakeId: 0,
	  bondSecondaryId: 0,
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

  AppConsts = AppConsts;

  @Input() secondary: any;

  ngOnInit(): void {
	this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.postForm = this.fb.group({
		overviewContent: [this.secondary?.overviewContent ?? "", [Validators.required]],
		contentType: [this.secondary?.contentType ?? "HTML", [Validators.required]],
	});
    this.getOverview(this.bondSecondaryId);


	// this.overView.projectOverviewOrgs = this.secondary?.projectOverViewOrgs ?? [];
	// this.overView.projectOverviewFiles = this.secondary?.projectOverViewFiles ?? [];
	// this.overView.overviewImageUrl = this.secondary?.overviewImageUrl?? '';
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
            bondSecondaryId: this.bondSecondaryId,
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
            bondSecondaryId: 0,
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
        this.projectOverviewFile = {...file};

        this.headerTitle = 'Cập nhật';
        console.log('dataEditFile', this.projectOverviewFile );
        // this.overView.projectOverviewOrgs.find()
    }

    downloadFile(file) {
        console.log('File-----', file);
        if(file.url != null) {
            const fileUrl = this.baseUrl + "/" + file.url;
            this._utilsService.makeDownload('', fileUrl);
        } else {
            // const fileUrl = this.baseUrl + "/" + url;
            // this._utilsService.makeDownload('', fileUrl);
        }
    }

	editOrg(org) {
        this.modalDialogOrgs = true;
        this.projectOverviewOrg = {...org};

        this.headerTitle = 'Cập nhật';
        console.log('dataEdit', this.projectOverviewOrg);
        console.log('overView___', this.overView);
        // this.overView.projectOverviewOrgs.find()
    }

    saveOgr(row?: any) {
        console.log('projectOverviewOrg___',this.projectOverviewOrg);
		console.log('overview____', this.overView);
		
        let id = this.projectOverviewOrg?.fakeId || this.projectOverviewOrg?.id;
        if(!id) {
            this.overView.bondSecondaryOverviewOrgs.push({
                fakeId: new Date().getTime(),
                bondSecondaryId: this.bondSecondaryId,
                name: this.projectOverviewOrg?.name,
                orgCode: this.projectOverviewOrg?.orgCode,  
                icon: this.projectOverviewOrg?.icon,
                url: this.projectOverviewOrg?.url
            });
        } else {
            let item = this.overView.bondSecondaryOverviewOrgs.find(o => o.id == this.projectOverviewOrg?.id || o.fakeId == this.projectOverviewOrg?.fakeId);
            item.name = this.projectOverviewOrg?.name;
            item.orgCode = this.projectOverviewOrg?.orgCode;
            item.icon = this.projectOverviewOrg?.icon;
            item.url = this.projectOverviewOrg?.url;
        }
        
        console.log('this.overView: ',this.overView );
        
        this.modalDialogOrgs = false;
    }

	saveFile(){
        let id = this.projectOverviewFile?.fakeId || this.projectOverviewFile?.id;
        if(!id) {
            this.overView.bondSecondaryOverviewFiles.push({
                fakeId: new Date().getTime(),
                bondSecondaryId: this.bondSecondaryId,
                title: this.projectOverviewFile?.title,
                url: this.projectOverviewFile?.url
            });
        } else {
            let item = this.overView.bondSecondaryOverviewFiles.find(o => o.id == this.projectOverviewFile?.id || o.fakeId == this.projectOverviewFile?.fakeId);
            item.title = this.projectOverviewFile?.title;
            item.url = this.projectOverviewFile?.url;
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
            this._contractTemplateService.uploadFileGetUrl(event?.files[0], "secondaryOverview").subscribe(
                (response) => {
                    if (this.handleResponseInterceptor(response, "Thêm thành công")) {
                        this.projectOverviewFile.url = response.data;
                        // this.overView.projectOverviewFiles.push({
                        //     'id': 0,
                        //     'bondSecondaryId': this.bondSecondaryId,
                        //     'title': this.title,
                        //     "url": response.data,
                        // });
                        // this.close()
                        // this.title = null;
                        // this.isUpload = false;
                        // console.log('this.overView.projectOverviewFiles', this.overView);
                        
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
            this.projectOverviewOrg.icon = this.icon;
            console.log(`this.projectOverviewOrg.icon: ${this.baseUrl}/${this.icon}` );
            console.log("projectOverviewOrg...........: ", this.projectOverviewOrg);
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
                this.removeElement(this.overView.bondSecondaryOverviewFiles, file);
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
                this.removeElement(this.overView.bondSecondaryOverviewOrgs, orgs);
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
    
          let oldContentValue = this.postForm.value.overviewContent;
          let a = oldContentValue.slice(0, this.caretPos) + imagesUrl + oldContentValue.slice(this.caretPos); 
          this.postForm.controls['overviewContent'].setValue(a);
    
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
        this.overView.bondSecondaryId = this.bondSecondaryId;
        this.overView.contentType = this.postForm?.value?.contentType;
        this.overView.overviewContent = this.postForm?.value?.overviewContent;
        console.log(' this.overView  this.overView:',  this.overView);
        if (!this.isEdit) {
            this.disableControls();
            this._secondaryService.updateOverview(this.overView).subscribe((response) => {
                if (this.handleResponseInterceptor(response, 'Cập nhật thành công')) {
                    this.router.navigate(["/bond-manager/product-bond-secondary/update/" + this.cryptEncode(this.bondSecondaryId)]);
                } 
            }, (err) => {
                console.log('err---', err);
            });
        }
         else {
            this.enableControls();
        }        
    }

        // GET OVERVIEW
	getOverview(bondSecondaryId) {
		this.isLoading = true;
		this._secondaryService.getOverviewById(bondSecondaryId).subscribe(res => {
			this.isLoading = false;
			if (this.handleResponseInterceptor(res, '')) {
				this.overView = {
					...res.data,			
				};
                this.postForm = this.fb.group({
                    // overviewContent: [this.overView?.overviewContent ?? "", [Validators.required]],
                    overviewContent: [{ value: this.overView?.overviewContent ?? "", disabled: true }],
                    contentType: [this.overView?.contentType ?? "HTML", [Validators.required]],
                }); 

                console.log('overView vao day', this.overView);
                
			}
		}, (err) => {
			this.isLoading = false;
			console.log('Error-------', err);
		});
	}    

    disableControls() {
        this.postForm.get('overviewContent').disable();
    }

    enableControls() {
        this.postForm.get('overviewContent').enable();
    }

	validForm(): boolean {
		return  this.projectOverviewOrg?.name?.trim() && this.projectOverviewOrg?.orgCode?.trim() && this.projectOverviewOrg?.icon?.trim() && true;
	}

    validFormFile(): boolean {
        return this.projectOverviewFile?.title.trim() && this.projectOverviewFile?.url.trim() && true;
    }
}
