import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { AppConsts, MediaConst } from '@shared/AppConsts';
import { SimpleModalComponent } from "ngx-simple-modal";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { MediaService } from '@shared/service-proxies/setting-service';
import {decode} from 'html-entities'
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';
@Component({
  selector: 'app-add-media',
  templateUrl: './add-media.component.html',
  styleUrls: ['./add-media.component.scss'], 
})
export class AddMediaComponent implements OnInit {
  
  baseUrl: string;
  title: string;
  inputData: any;
  postForm: FormGroup;
  types = []
  statuses = []
  imageFiles = []
  mainImg: any = ""
  formatImage:boolean;
  formatVideo:boolean;
  caretPos: number = 0;
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

  // positionsList: any = {
  //   'banner_popup': "Banner popup",
  //   'slide_image': 'Slide ảnh',
  //   'banner_top': 'Banner trên top',
  //   'hot_today': 'Hôm nay có gì hot',
  //   'just_for_you': 'Dành cho bạn',
  //   'videos': 'Video'
  // }
  // typeList: any = {
  //   'kham_pha': "Trang khám phá",
  //   'dau_tu': 'Trang đầu tư',
  //   'tai_san': 'Trang tài sản',
  //   'san_pham': 'Sản phẩm',
  //   'san_pham_dau_tu': 'Sản phẩm đầu tư',
  //   'mua_bds': 'Trang mua BĐS',
  //   'thue_bds': 'Trang thuê BĐS'
  // }

  // typePositions: any = {
  //   kham_pha: {
  //     'banner_popup': "Banner popup",
  //     'slide_image': 'Slide ảnh',
  //     'banner_top': 'Banner trên top',
  //     'hot_today': 'Hôm nay có gì hot',
  //     'just_for_you': 'Dành cho bạn',
  //     'videos': 'Video'
  //   },
  //   dau_tu: {
  //     'banner_popup': "Banner popup",
  //     'slide_image': 'Slide ảnh',
  //     'banner_top': 'Banner trên top'
  //   },
  //   tai_san: {
  //     'banner_popup': "Banner popup",
  //     'slide_image': 'Slide ảnh',
  //     'banner_top': 'Banner trên top',
  //   },
  //   san_pham: {
  //     'banner_popup': "Banner popup",
  //     'slide_image': 'Slide ảnh',
  //     'banner_top': 'Banner trên top'
  //   },
  //   mua_bds: {
  //     'banner_popup': "Banner popup",
  //     'slide_image': 'Slide ảnh',
  //     'banner_top': 'Banner trên top'
  //   },
  //   thue_bds: {
  //     'banner_popup': "Banner popup",
  //     'slide_image': 'Slide ảnh',
  //     'banner_top': 'Banner trên top'
  //   }

  // }
  positions = [];
  

  constructor( private fb: FormBuilder,
    private broadcastService: MediaService,
    protected messageService: MessageService,
    public dialogService: DialogService,
    public ref: DynamicDialogRef, 
    public config: DynamicDialogConfig,
  ) {  
  }

  // changeType() {
  //   this.positionsList = this.typePositions[this.postForm.value.type]
  //   this.positions = []
  //   for (let key in this.positionsList) {
  //     this.positions.push({ key: key, value: this.positionsList[key] });
  //   }
  // }
   ngOnInit() {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl; 

    if (this.config.data.inputData) {
      this.inputData = this.config.data.inputData;
      console.log("this.inputData",this.inputData);
      this.mainImg = this.inputData.mainImg; 
      this.postForm = this.fb.group({
        title: [this.inputData.title, Validators.required],
        content: [decode(this.inputData.content), [Validators.required]],
        // type: [this.inputData.type, [Validators.required]],
        type: "garner",
        displayText: [this.inputData.displayText, []],
        // productId: [this.inputData.productId, []],
        isFeatured: [this.inputData.isFeatured, []],
        sort: [this.inputData.sort, []],
        contentType: [this.inputData.contentType, []],
        // position: [this.inputData.position, [Validators.required]]
      });
      // this.positionsList = this.typePositions[this.inputData.type]
      this.positions = []
      // for (let key in this.positionsList) {
      //   this.positions.push({ key: key, value: this.positionsList[key] });
      // }
    } else {
      this.postForm = this.fb.group({
        title: ['', Validators.required],
        content: ['', [Validators.required]],
        // type: ['kham_pha', [Validators.required]],
        type: ['garner', []], 
        displayText: ['', []], 
        // productId: ['', []],
        isFeatured: [false, []], 
        status: ['DRAFT', []], 
        contentType: ['MARKDOWN', []], 
        sort: [1, []], 
        // position: ['banner_popup', [Validators.required]]
      });
    }
    this.detectVideo();

    // for (let key in this.positionsList) {
    //   this.positions.push({ key: key, value: this.positionsList[key] });
    // }
    // for (let key in this.typeList) {
    //   this.types.push({ key: key, value: this.typeList[key] });
    // }
    for (let key in MediaConst.mediaStatus) {
      this.statuses.push({ key: key, value: MediaConst.mediaStatus[key] });
    }
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
      if(images && images.length > 0) {
        this.mainImg = images[0].data;
        this.detectVideo();
      }
    });
  }
   
  detectVideo() {
    const images = ["jpg", "gif", "png"]
    let videos = ["mp4", "3gp", "ogg", "mkv"]
    for (var i = 0; i < videos.length; i++) {

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
        this.postForm.value.mainImg = this.mainImg;
        this.postForm.value.id = this.inputData.id;
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
        console.log("đầu vào nhập của nội dung content", );
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
    console.log("this.baseUrl inser",this.baseUrl,  );
    
    ref.onClose.subscribe(images => {
      let imagesUrl = "";
      images.forEach(image => {
        // imagesUrl +=  "![](http://118.70.215.61:8089/"+image.data+") \n";
        // `${this.investorEndpoint}/ekyc
        imagesUrl +=  `![](${this.baseUrl}/${image.data}) \n`;
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
