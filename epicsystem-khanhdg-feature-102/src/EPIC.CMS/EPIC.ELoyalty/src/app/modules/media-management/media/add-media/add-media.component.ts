import { Component, Injector, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AppConsts, MediaConst, NotificationTemplateConst } from '@shared/AppConsts';
import { SimpleModalComponent } from "ngx-simple-modal";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import {decode} from 'html-entities'
import { CrudComponentBase } from '@shared/crud-component-base';
import { NotificationExtendService } from '@shared/services/notification-extend.service';
import { BroadcastService } from '@shared/services/broadcast.service';
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';
@Component({
  selector: 'app-add-media',
  templateUrl: './add-media.component.html',
  styleUrls: ['./add-media.component.scss'],
})
export class AddMediaComponent extends CrudComponentBase implements OnInit {

  baseUrl: string;
  title: string;
  inputData: any;
  postForm: FormGroup;
  types = [];
  productKeys = [];
  statuses = []
  imageFiles = []
  mainImg: any = "";
  formatVideo:boolean;
  formatImage:boolean;
  caretPos: number = 0;
  disableStatus: boolean = true;
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

  public haveNavigation = ['videos'];
  typePositions: any = {
    uu_dai_cua_toi: {
      // 'banner_popup': "Banner popup",
      'slide_image': 'Slide ảnh',
      // 'popup_birthday': "Popup sinh nhật",
      'banner_top': 'Banner trên top',
      // 'hot_today': 'Hôm nay có gì hot', 
      // 'just_for_you': 'Dành cho bạn',
      // 'videos': 'Video',
      // 'vi_sao_su_dung_epic': "Vì sao sử dụng EPIC",
    },
    kham_pha: {
      
    },
    dau_tu: {
    
    },
    tai_san: {
     
    },
    san_pham: {
     
    },
    mua_bds: {
     
    },
    thue_bds: {
      
    }

  }
  positionsList: any = this.typePositions.uu_dai_cua_toi
  typeList: any = {
    'uu_dai_cua_toi': "Ưu đãi của tôi",
    
  }

  productKeyLists: any = {
    'invest': "Đầu tư tài chính",
    'bond': 'Đầu tư trái phiếu',
    'tich_luy': 'Đầu tư tích lũy',
  }
  secondLevelOptions: any = [];
  NotificationTemplateConst =NotificationTemplateConst;
  positions = [];

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private broadcastService: BroadcastService,
    protected messageService: MessageService,
    public dialogService: DialogService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private _notificationExtendService: NotificationExtendService,
    ) {
      super(injector, messageService);
  }

  changeType() {
    this.positionsList = this.typePositions[this.postForm.value.type]
    this.positions = []
    for (let key in this.positionsList) {
      this.positions.push({ key: key, value: this.positionsList[key] });
    }
  }

  ngOnInit() {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
   
    if (this.config.data.inputData) {
      this.filteredsecondLevelOptions(); 
      this.inputData = this.config.data.inputData;
      this.mainImg = this.inputData.mainImg;
      this.postForm = this.fb.group({
        title: [this.inputData.title, Validators.required],
        content: [decode(this.inputData.content), [Validators.required]],
        type: [this.inputData.type, [Validators.required]],
        productKey: [this.inputData.productKey, []],
        displayText: [this.inputData.displayText, []],
        status: [this.inputData.status, []],
        sort: [this.inputData.sort, []],
        contentType: [this.inputData.contentType, []],
        position: [this.inputData.position, [Validators.required]],
        isNavigation: [this.inputData.isNavigation, []], 
        navigationType: [this.inputData.navigationType, []], 
        navigationLink: [this.inputData.navigationLink, []], 
        levelOneNavigation: [this.inputData.levelOneNavigation, []], 
        secondLevelNavigation: [this.inputData.secondLevelNavigation ,[]], 
        typeModule: ['eLoyalty',[]], 
      });
      this.positionsList = this.typePositions[this.inputData.type];
    
    } else {
      this.postForm = this.fb.group({
        title: ['', Validators.required],
        content: ['', [Validators.required]],
        type: ['uu_dai_cua_toi', [Validators.required]],
        productKey: ['invest', []],
        displayText: ['', []],
        status: ['DRAFT', []], 
        contentType: ['MARKDOWN', []], 
        sort: [1, []], 
        position: ['banner_top', [Validators.required]],
        isNavigation: [false, []], 
        navigationType: [null, []],
        navigationLink: ["", []],
        levelOneNavigation: [null, []],
        secondLevelNavigation: ["", []],
        typeModule: ['eLoyalty',[]], 
      });
    }
    this.detectVideo();
    for (let key in this.positionsList) {
      this.positions.push({ key: key, value: this.positionsList[key] });
      console.log("this.positions",this.positions);
      
    }
    for (let key in this.productKeyLists) {
      this.productKeys.push({ key: key, value: this.productKeyLists[key] });
    }
    for (let key in this.typeList) {
      this.types.push({ key: key, value: this.typeList[key] });
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

  header() {
    return this.title
  }
  
  selectImg() {
    const ref = this.dialogService.open(UploadImageComponent, {
      data: {
        inputData: []
      },
      header: 'Tải hình ảnh',
      width: '500px',
      footer: ""
    });
    ref.onClose.subscribe(images => {
      if (images && images.length > 0) {
        console.log("đầu vào phần tử từ upload ", images[0].data);
        this.mainImg = images[0].data;
        this.detectVideo();
      }
    });
    
  }

  detectVideo() {
    const images = ["jpg", "gif", "png"]
    let videos = ["mp4", "3gp", "ogg", "mkv"]
    for (var i = 0; i < videos.length; i++) {

      let position = this.mainImg.search(videos[i])
      console.log("position ", position);
      
      if (this.mainImg.search(videos[i]) > -1) {
        this.formatVideo = true;
        this.formatImage = false;
        break;
      }
      
      if(this.mainImg.search(images[i]) > -1){
        this.formatImage = true;
        this.formatVideo = false;

        break;
      }
    }
    console.log("kết quả", this.formatVideo);
    
  }


  onSubmit() {
    if (this.postForm.valid) {

      if (this.inputData) {
        console.log("đầu vào nhập của nội dung content", this.postForm.value.content);

        this.postForm.value.mainImg = this.mainImg;
        this.postForm.value.id = this.inputData.id;
        console.log(this.postForm.value);
        this.broadcastService.saveMedia(this.postForm.value).subscribe((result) => {
          this.ref.close(result);
        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi cập nhật hình ảnh",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        })
      } else {
        console.log("đầu vào nhập của nội dung content", this.postForm.value.content);
        this.postForm.value.mainImg = this.mainImg;
        this.broadcastService.createMedia(this.postForm.value).subscribe((result) => {
          this.ref.close(result);
        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi tạo hình ảnh",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
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
    console.log("this.baseUrl inser", this.baseUrl,);

    ref.onClose.subscribe(images => {
      let imagesUrl = "";
      images.forEach(image => {
        imagesUrl +=  `![](${this.baseUrl}/${image.data}) \n`;
        console.log("imagesUrl",imagesUrl);
        
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
