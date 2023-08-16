import { Component, Injector, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AppConsts, MediaConst, NotificationTemplateConst } from '@shared/AppConsts';
import { SimpleModalComponent } from "ngx-simple-modal";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { BroadcastService } from '@shared/service-proxies/broadcast-service';
import { ContractTemplateServiceProxy } from '@shared/service-proxies/bond-manager-service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components/upload-image/upload-image.component';
import { decode } from 'html-entities';
import { CrudComponentBase } from '@shared/crud-component-base';
import { NotificationExtendService } from '@shared/services/notification-extend.service';


@Component({
  selector: 'app-add-news',
  templateUrl: './add-news.component.html',
  styleUrls: ['./add-news.component.scss']
})

export class AddNewsComponent extends CrudComponentBase implements OnInit {

  title: string;
  inputData: any;
  postForm: FormGroup;
  types = []
  statuses = []
  imageFile: string = ""
  baseUrl: string;
  formatVideo:boolean;
  formatImage:boolean;
  caretPos: number = 0;
  disableStatus: boolean = true;
  NotificationTemplateConst = NotificationTemplateConst;
  htmlMarkdownOptions: any = [
    {
      value: 'MARKDOWN',
      name: 'MARKDOWN'
    },
    {
      value: 'HTML',
      name: 'HTML'
    }
  ]
  secondLevelOptions: any = [];

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private broadcastService: BroadcastService,
    protected messageService: MessageService,
    public dialogService: DialogService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private _notificationExtendService: NotificationExtendService,
    private _contractTemplateService: ContractTemplateServiceProxy
  ) {
      super(injector, messageService);
  }

  ngOnInit() {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    if (this.config.data.inputData) {
      this.filteredsecondLevelOptions(); 
      this.inputData = this.config.data.inputData;
      this.postForm = this.fb.group({
        title: [this.inputData.title, Validators.required],
        content: [decode( this.inputData.content), [Validators.required]],
        contentType: [this.inputData.contentType, [Validators.required]],
        type: [this.inputData.type, [Validators.required]],
        displayText: [this.inputData.displayText, []],
        status: [this.inputData.status, []],
        order: [this.inputData.order, []],
        isFeatured: [this.inputData.isFeatured || false, []],
        isNavigation: [this.inputData.isNavigation, []], 
        navigationType: [this.inputData.navigationType, []], 
        navigationLink: [this.inputData.navigationLink, []], 
        levelOneNavigation: [this.inputData.levelOneNavigation, []], 
        secondLevelNavigation: [this.inputData.secondLevelNavigation ,[]], 
      });
      this.imageFile = this.inputData.mainImg;

    } else {
      this.postForm = this.fb.group({
        title: ['', Validators.required],
        content: ['', [Validators.required]],
        contentType: ['HTML', [Validators.required]],
        displayText: ['', []],
        type: ['PURE_NEWS', [Validators.required]],
        status: ['DRAFT', []],
        isFeatured: [false, []],
        order: [0, []],
        isNavigation: [false, []], 
        navigationType: [null, []],
        navigationLink: ["", []],
        levelOneNavigation: [null, []],
        secondLevelNavigation: ["", []],
      });
    }
    this.detectVideo();
    for (let key in MediaConst.newsTypes) {
      this.types.push({ key: key, value: MediaConst.newsTypes[key] });
    }
    for (let key in MediaConst.mediaStatus) {
      this.statuses.push({ key: key, value: MediaConst.mediaStatus[key] });
    }
    // undisable chuc nang select status trong edit
    if (this.config.data.unDisableStatus) {
      this.disableStatus = false;
    }

  }

  onLevelOneNavigationChange(event) {
    this.filteredsecondLevelOptions(event);
    this.postForm.value.secondLevelNavigation = null
  }

  getDistributionInvest() {
    this._notificationExtendService.getDistributionInvest().subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.secondLevelOptions = [];
        res.data.forEach(item => {
          this.secondLevelOptions.push({
            value: '' + item.id,
            secondLevel: item.code + ` - ` + item.name + (item.isSalePartnership ? `( Bán hộ )` : '')
          });
        });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);

    });
  }
  getDistributionGarner() {
    this._notificationExtendService.getDistributionGarner().subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.secondLevelOptions = [];
        res.data.forEach(item => {

          this.secondLevelOptions.push({
            value: '' + item.id,
            secondLevel: item.code + ` - ` + item.name + (item.isSalePartnership ? `( Bán hộ )` : '')
          });

        });

      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);

    });
  }

  getOpenSellRst() {
    this._notificationExtendService.getOpenSellRst().subscribe((res) => {
      this.isLoading = false;
      if (this.handleResponseInterceptor(res, '')) {
        this.secondLevelOptions = [];
        res.data.forEach(item => {
          this.secondLevelOptions.push({
            value: '' + item.id,
            secondLevel: item.code + ` - ` + item.name + (item.isSalePartnership ? `( Bán hộ )` : '')
          });

        });
      }
    }, (err) => {
      this.isLoading = false;
      console.log('Error-------', err);

    });
  }

  filteredsecondLevelOptions(event?) {
    let levelOneNavigation = event?.value || this.config.data.inputData.levelOneNavigation;
    console.log("levelOneNavigation",levelOneNavigation);
    const eventHandlers = {
      [NotificationTemplateConst.DAU_TU_TAI_CHINH]: this.getDistributionInvest,
      [NotificationTemplateConst.DAU_TU_TICH_LUY]: this.getDistributionGarner,
      [NotificationTemplateConst.GIAO_DICH_BDS]: this.getOpenSellRst,
      default: () => {
        let filteredArray = NotificationTemplateConst.levelOneOptions.filter(option => option.value === levelOneNavigation);
        if(levelOneNavigation !== NotificationTemplateConst.VOUCHER) {
          this.secondLevelOptions = filteredArray;
        } else {
          this.secondLevelOptions =  NotificationTemplateConst.voucherOptions
        }
      }
    }
    const handler = eventHandlers[levelOneNavigation] || eventHandlers.default;
    handler.call(this);
  }

  onSwitchNavigationChange(event) {
    this.postForm.value.navigationType = !this.postForm.value.isNavigation ? 
          null : this.postForm.value.navigationType;
  }

  onNavigationTypeChange(event) {
    this.postForm.value.navigationLink = ((this.postForm.value.isNavigation && 
        this.postForm.value.navigationType == NotificationTemplateConst.IN_APP) || 
        (!this.postForm.value.isNavigation)) ? null : this.postForm.value.navigationLink;

    this.postForm.value.levelOneNavigation = ((this.postForm.value.isNavigation && 
      this.postForm.value.navigationType == 
      NotificationTemplateConst.LIEN_KET_KHAC) ||
        (!this.postForm.value.isNavigation)) ? null : this.postForm.value.levelOneNavigation;

    this.postForm.value.secondLevelNavigation = ((this.postForm.value.isNavigation && 
      this.postForm.value.navigationType == NotificationTemplateConst.LIEN_KET_KHAC) ||
        (!this.postForm.value.isNavigation)) ? null : this.postForm.value.secondLevelNavigation;
      console.log("this.postForm.value",this.postForm.value);
      
  }

  get postFormControl() {
    return this.postForm.controls;
  }

  newsMedia: any = {}
  editorConfig: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '15rem',
    minHeight: '15rem',
    maxHeight: 'auto',
    width: 'auto',
    minWidth: '0',
    translate: 'yes',
    enableToolbar: true,
    showToolbar: true,
    placeholder: 'Enter text here...',
    defaultParagraphSeparator: '',
    defaultFontName: '',
    defaultFontSize: '',
   
    fonts: [
      { class: 'arial', name: 'Arial' },
      { class: 'times-new-roman', name: 'Times New Roman' },
      { class: 'calibri', name: 'Calibri' },
      { class: 'comic-sans-ms', name: 'Comic Sans MS' }
    ],
    customClasses: [],
    sanitize: false,
    toolbarPosition: 'top',
    toolbarHiddenButtons: [
      [
        'undo',
        'redo',
        'strikeThrough',
        'subscript',
        'superscript',
        'heading',
        'fontName'
      ],
      [
        'backgroundColor',
        'customClasses',
        'link',
        'unlink',
        'removeFormat',
        'toggleEditorMode'
      ]
    ]
  };
   
  detectVideo() {
    const images = ["jpg", "gif", "png"]
    let videos = ["mp4", "3gp", "ogg", "mkv"]
    for (var i = 0; i < videos.length; i++) {

      let position = this.imageFile.search(videos[i])
      console.log("position ", position);
      
      if (this.imageFile.search(videos[i]) > -1) {
        this.formatVideo = true;
        this.formatImage = false;
        break;
      }
      
      if(this.imageFile.search(images[i]) > -1){
        this.formatImage = true;
        this.formatVideo = false;

        break;
      }
    }
    console.log("kết quả", this.formatVideo);
    
  }

  
  header() {
    return this.title
  }

  selectImg() {
    const ref = this.dialogService.open(UploadImageComponent, {
      data: {
        inputData: [this.imageFile]
      },
      header: 'Tải hình ảnh',
      width: '500px',
      footer: ""
    });
    ref.onClose.subscribe(images => {
      if (images && images.length > 0)
        this.imageFile = images[0].data;
        this.detectVideo();
    });
  }

  onSubmit() {
    if (this.postForm.valid) {
      if (this.inputData) {
        this.postForm.value.mainImg = this.imageFile;
        this.postForm.value.id = this.inputData.id;
        console.log("12423546547567i68o798", this.postForm.value);
        this.broadcastService.saveNews(this.postForm.value).subscribe((result) => {
          if(this)
          this.ref.close(result);
        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi lưu tin tức",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        })
      } else {
        this.postForm.value.mainImg = this.imageFile;
        console.log(this.postForm.value);
        this.broadcastService.createNews(this.postForm.value).subscribe((result) => {
          this.ref.close(result);
        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi tạo tin tức",
            detail: "Vui lòng thử lại",
            life: 3000,
          });
        })
      }
    }
  }
  close() {
    this.ref.close();
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

      let oldContentValue = this.postForm.value.content;
      let a = oldContentValue.slice(0, this.caretPos) + imagesUrl + oldContentValue.slice(this.caretPos); 
      this.postForm.controls['content'].setValue(a);

    })
  }

  getCaretPos(oField) {
    if (oField.selectionStart || oField.selectionStart == '0') {
      this.caretPos = oField.selectionStart;
      console.log("this.caretPos",this.caretPos);
    }
  }

}
