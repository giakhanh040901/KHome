import { Component, Injector, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AppConsts, NotificationTemplateConst } from '@shared/AppConsts';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { decode } from 'html-entities';
import { NotificationExtendService } from '@shared/services/notification-extend.service';
import { CrudComponentBase } from '@shared/crud-component-base';
import { NotificationService } from '@shared/services/notification.service';
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';
@Component({
  selector: 'app-add-notification-template',
  templateUrl: './add-notification-template.component.html',
  styleUrls: ['./add-notification-template.component.scss'],
  providers: [DialogService, MessageService],
})
export class AddNotificationTemplateComponent extends CrudComponentBase implements OnInit {

  selectedHtmlMarkdown: any;
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
  selectedTopic: any;
  topicTest: any;
  title: string;
  inputData: any;
  postForm: FormGroup;
  actions = []
  statuses = []
  topicList = []
  mainImg: any;
  baseUrl: string;
  caretPos: number = 0;
  typeList: any = {
    'kham_pha': "Trang khám phá",
    'dau_tu': 'Trang đầu tư',
    'tai_san': 'Trang tài sản'
  }
  positions = [];
  secondLevelOptions: any = [];
  NotificationTemplateConst = NotificationTemplateConst;
  actionsList: any = {
    PUSH_NOTIFICATION: 'Đẩy thông báo trên app',
    SEND_SMS: 'Gửi SMS',
    SEND_EMAIL: 'Gửi email'
  }

  constructor(injector: Injector,
    private fb: FormBuilder,
    private notificationService: NotificationService,
    private _notificationExtendService: NotificationExtendService,
    protected messageService: MessageService,
    public dialogService: DialogService,
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,

  ) {
    super(injector, messageService);
    this.statuses = [
      {
        value: 'ACTIVE',
        name: 'Hoạt động'
      },
      {
        value: 'INACTIVE',
        name: 'Không hoạt động'
      }
    ];

    this.topicList = [
      {
        value: 'Thông báo ngày lễ',
        name: 'Thông báo ngày lễ'
      },
      {
        value: 'Thông báo chung',
        name: 'Thông báo chung'
      },
      {
        value: 'Thông báo chương trình bán hàng',
        name: 'Thông báo chương trình bán hàng'
      },
    ];
  }

  filteredsecondLevelOptions(event?) {
    let levelOneNavigation = event?.value || this.config.data.inputData.levelOneNavigation;
    console.log("levelOneNavigation", levelOneNavigation);
    const eventHandlers = {
      [NotificationTemplateConst.DAU_TU_TAI_CHINH]: this.getDistributionInvest,
      [NotificationTemplateConst.DAU_TU_TICH_LUY]: this.getDistributionGarner,
      [NotificationTemplateConst.GIAO_DICH_BDS]: this.getOpenSellRst,
      default: () => {
        let filteredArray = NotificationTemplateConst.levelOneOptions.filter(option => option.value === levelOneNavigation);
        if (levelOneNavigation !== NotificationTemplateConst.VOUCHER) {
          this.secondLevelOptions = filteredArray;
        } else {
          this.secondLevelOptions = NotificationTemplateConst.voucherOptions
        }
      }
    }
    const handler = eventHandlers[levelOneNavigation] || eventHandlers.default;
    handler.call(this);
  }

  ngOnInit() {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    if (this.config.data.inputData) {
      this.filteredsecondLevelOptions();
      this.inputData = this.config.data.inputData;
      this.mainImg = this.inputData.mainImg;
      this.postForm = this.fb.group({
        title: [this.inputData.title, Validators.required],
        code: [this.inputData.code, []],
        actions: [this.inputData.actions, [Validators.required]],
        description: [this.inputData.description, []],
        isFeatured: [this.inputData.isFeatured, []],
        appNotificationDesc: [this.inputData.appNotificationDesc, [Validators.required]],
        notificationContent: [decode(this.inputData.notificationContent), []],
        smsContent: [decode(this.inputData.smsContent), []],
        emailContent: [decode(this.inputData.emailContent), []],
        type: [this.inputData.type, []],
        status: [this.inputData.status, []],
        actionView: [this.inputData.actionView, []],
        contentType: [this.inputData.contentType, []],
        externalEvent: [this.inputData.externalEvent, []],
        externalParams: [this.inputData.externalParams, []],
        isNavigation: [this.inputData.isNavigation, []],
        navigationType: [this.inputData.navigationType, []],
        navigationLink: [this.inputData.navigationLink, []],
        levelOneNavigation: [this.inputData.levelOneNavigation, []],
        secondLevelNavigation: [this.inputData.secondLevelNavigation, []],
        typeModule: ['eLoyalty',[]],
      });
      console.log("___", this.config.data.inputData);

    } else {
      this.postForm = this.fb.group({
        title: ['', Validators.required],
        code: ['', []],
        actions: [['PUSH_NOTIFICATION'], []],
        description: ['', []],
        isFeatured: [false, []],
        appNotificationDesc: ['', [Validators.required]],
        notificationContent: ['', []],
        smsContent: ['', []],
        emailContent: ['', []],
        type: ['HE_THONG', []],
        status: ['ACTIVE', []],
        actionView: ["", []],
        contentType: ["MARKDOWN", []],
        externalEvent: ["", []],
        externalParams: ["", []],
        isNavigation: [false, []],
        navigationType: [null, []],
        navigationLink: ["", []],
        levelOneNavigation: [null, []],
        secondLevelNavigation: ["", []],
        typeModule: ['eLoyalty',[]],
      });


    }
    this.selectedTopic = this.inputData?.description;
    for (let key in this.actionsList) {
      this.actions.push({ key: key, value: this.actionsList[key] });
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
        this.mainImg = images[0].data;
      }
    });
  }
  removeFile(file) {
    this.mainImg = null;
  }

  onNavigationTypeChange(event) {
    if (event.value == NotificationTemplateConst.IN_APP) {
      this.getDistributionInvest();
    }

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
    console.log("this.postForm.value", this.postForm.value);

  }

  onSwitchNavigationChange(event) {
    this.postForm.value.navigationType = !this.postForm.value.isNavigation ?
      null : this.postForm.value.navigationType;
  }

  onSubmit() {
    if (this.postForm.valid) {
      if (this.inputData) {

        this.postForm.value.mainImg = this.mainImg;
        this.postForm.value.id = this.inputData.id;

        this.postForm.value.mainImg = this.mainImg;
        this.notificationService.saveNotificationTemplate(this.postForm.value).subscribe((result) => {
          this.ref.close(result);

        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi cập nhật mẫu thông báo",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        })
      } else {
        this.postForm.value.mainImg = this.mainImg;
        this.notificationService.createNotificationTemplate(this.postForm.value).subscribe((result) => {
          this.ref.close(result);
        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi tạo mẫu thông báo",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        });
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
      console.log(imagesUrl)
      let oldEmailContentValue = this.postForm.value.emailContent;
      let a = oldEmailContentValue.slice(0, this.caretPos) + imagesUrl + oldEmailContentValue.slice(this.caretPos);
      this.postForm.controls['emailContent'].setValue(a);
    })
  }

  insertImageApp() {
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
      console.log(imagesUrl)
      let oldAppContentValue = this.postForm.value.notificationContent;
      let a = oldAppContentValue.slice(0, this.caretPos) + imagesUrl + oldAppContentValue.slice(this.caretPos);
      this.postForm.controls['notificationContent'].setValue(a);
    })
  }

  getCaretPos(oField) {
    if (oField.selectionStart || oField.selectionStart == '0') {
      this.caretPos = oField.selectionStart;

    }
  }

}
