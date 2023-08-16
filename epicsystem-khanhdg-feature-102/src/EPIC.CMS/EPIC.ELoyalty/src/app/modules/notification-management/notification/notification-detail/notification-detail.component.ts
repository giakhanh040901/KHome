import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppConsts, NotificationTemplateConst, TypeFormatDateConst, SearchConst, NotifyManagerConst, TabView } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { decode } from 'html-entities';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NotificationExtendService } from '@shared/services/notification-extend.service';
import { style } from '@angular/animations';
import { debounceTime } from 'rxjs/operators';
import { NotificationService } from '@shared/services/notification.service';
import { UploadImageComponent } from 'src/app/components-general/upload-image/upload-image.component';
import { AddPersonListComponent } from './add-person-list/add-person-list.component';

@Component({
  selector: 'app-notification-detail',
  templateUrl: './notification-detail.component.html',
  styleUrls: ['./notification-detail.component.scss']
})
export class NotificationDetailComponent extends CrudComponentBase implements OnInit {

  currentNotificationId: any
  currentNotification: any;
  page = new Page()
  personListPage = new Page()

  isLoadingPersonList: boolean = false;
  isLoading: boolean = false;
  initLoading: boolean = false;
  navigationTypes = []
  secondLevelOptions: any = [];
  selectedCustomers: any[] = [];
  filters: {
    pushAppStatus: any,
    sendEmailStatus: any,
    sendSMSStatus: any,
    sendNotifySize: any,
  }
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
  ];

  pushSMSStatus: any = [
    // {
    //   value: 'DRAFT',
    //   name: 'Nháp'
    // },
    {
      value: 'DRAFT',
      name: 'Chờ gửi'
    },
    {
      value: 'SENT',
      name: 'Đã gửi'
    },
    {
      value: 'SEND_ERROR',
      name: 'Gửi lỗi'
    }
  ];
  NotificationTemplateConst = NotificationTemplateConst;
  NotifyManagerConst = NotifyManagerConst;

  selectedTopic: any;
  title: string;
  inputData: any;
  postForm: FormGroup;
  actions = [];
  statuses = [];
  topicList = [];
  mainImg: any;
  types: any[];
  checkDropdown: any;
  deleteArr: any;
  caretPos: number = 0;
  defaultDate: any;
  listOfReciever: any[] = [];
  baseUrl: string;
  notificationTemplates: any[] = [];
  typeList: any = {
    'kham_pha': "Trang khám phá",
    'dau_tu': 'Trang đầu tư',
    'tai_san': 'Trang tài sản'
  }

  actionsList: any = {
    PUSH_NOTIFICATION: 'Đẩy thông báo trên app',
    SEND_SMS: 'Gửi SMS',
    SEND_EMAIL: 'Gửi email'
  }
  activeIndex: number;
  notificationAdd: any;
  dataFilter: any = {
    field: null,
    status: null,
  }

  constructor(private fb: FormBuilder,
    private notificationService: NotificationService,
    protected messageService: MessageService,
    public dialogService: DialogService,
    private router: Router,
    private breadcrumbService: BreadcrumbService,
    injector: Injector,
    private route: ActivatedRoute,
    private _notificationExtendService: NotificationExtendService,

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

    this.navigationTypes = [
      {
        value: 'IN_APP',
        name: 'In App'
      },
      {
        value: 'LIEN_KET_KHAC',
        name: 'Liên kết khác'
      }
    ];

    this.filters = {
      pushAppStatus: null,
      sendEmailStatus: null,
      sendSMSStatus: null,
      sendNotifySize: NotifyManagerConst.TYPE_SELECTED_PAGE,
    }
    // this.approveId = +this.route.snapshot.paramMap.get('id');
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
    let levelOneNavigation = event
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
    this.initNotificationTemplate({ page: this.offset });
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
    this.selectedCustomers = []

    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Thông báo', routerLink: ['/notification-management/notification'] },
      { label: 'Chi tiết thông báo' }
    ]);

    this.types = [
      {
        type: 'HE_THONG',
        name: "Hệ thống"
      },
      {
        type: 'UU_DAI',
        name: "Ưu đãi"
      },
      {
        type: 'CHINH_SACH',
        name: "Khuyến mại"
      },
      {
        type: 'GIAO_DICH',
        name: "Giao dịch"
      },
    ];

    this.route.queryParamMap.subscribe((dataParams) => {
      if ((<any>dataParams).params.id) {
        this.initLoading = true;
        this.isLoadingPersonList = true;
        this.currentNotificationId = (<any>dataParams).params.id;

        this.notificationService.getNotificationDetail(this.currentNotificationId).subscribe(notification => {
          this.selectedTopic = notification.description;
          this.currentNotification = notification;
          this.mainImg = notification.mainImg;
          this.filteredsecondLevelOptions(notification?.levelOneNavigation)
          this.postForm = this.fb.group({
            title: [notification.title, Validators.required],
            actions: [notification.actions, [Validators.required]],
            description: [notification.description, [Validators.required]],
            isFeatured: [notification.isFeatured, []],
            appNotificationDesc: [decode(notification.appNotificationDesc), [Validators.required]],
            notificationContent: [decode(notification.notificationContent), []],
            smsContent: [decode(notification.smsContent), []],
            emailContent: [decode(notification.emailContent), []],
            type: [notification.type, []],
            status: [notification.status, []],
            actionView: [notification.actionView, []],
            emailContentType: [notification.emailContentType, []],
            externalEvent: [notification.externalEvent, []],
            externalParams: [notification.externalParams, []],
            appNotifContentType: [notification.appNotifContentType, []],
            isNavigation: [notification.isNavigation, []],
            navigationType: [notification.navigationType, []],
            navigationLink: [notification.navigationLink, []],
            levelOneNavigation: [notification.levelOneNavigation, []],
            secondLevelNavigation: [notification.secondLevelNavigation, []],
            isTimer: [notification.isTimer, []],
            appointment: [notification.appointment ? new Date(notification.appointment) : null, []],
            notificationTemplateId: [notification.notificationTemplateId, []],
            typeModule: ['eLoyalty',[]], 
          });
        }, err => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi lấy thông tin thông báo",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        })

        this.setPersonList(this.personListPage);
      } else {
        this.defaultDate = new Date();
        this.defaultDate.setHours(0);
        this.defaultDate.setMinutes(0);
        this.defaultDate.setSeconds(0);
        this.currentNotificationId == null;
        this.checkDropdown == null;
        this.postForm = this.fb.group({
          title: ["", Validators.required],
          actions: ["", [Validators.required]],
          description: ["", [Validators.required]],
          isFeatured: [false, []],
          appNotificationDesc: ['', [Validators.required]],
          notificationContent: ['', []],
          smsContent: ['', []],
          emailContent: ['', []],
          type: ['HE_THONG', []],
          status: ['ACTIVE', []],
          actionView: ["", []],
          emailContentType: ["MARKDOWN", []],
          externalEvent: ["", []],
          externalParams: ["", []],
          appNotifContentType: ["HTML", []],
          isNavigation: [false, []],
          navigationType: [null, []],
          navigationLink: ["", []],
          levelOneNavigation: [null, []],
          secondLevelNavigation: ["", []],
          isTimer: [false, []],
          appointment: ["", []],
          notificationTemplateId: ["", []],
          typeModule: ['eLoyalty',[]], 
        });
      }
    });
    for (let key in this.actionsList) {
      this.actions.push({ key: key, value: this.actionsList[key] });
    }
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword === "") {
        this.setPersonList();
      } else {
        this.setPersonList();
      }
    });
  }

  errorNotify(err) {
    const errorMessages = {
      '{"error":{"code":"messaging/invalid-payload","message":"Exactly one of topic, token or condition is required"}}': 'Thông tin không hợp lệ',
      '{"error":"messaging/registration-token-not-registered"}': 'Mã thông báo đăng ký chưa được đăng ký',
      'Input invalid': 'Đầu vào không hợp lệ'
    };
    
    let result = errorMessages[err] || err;
    return result;
  }

  intervalId: any;
  changeActiveIndex(activeIndex) {
    if (activeIndex == TabView.SECOND) {
      if (!this.intervalId) {
        this.intervalId = setInterval(() => {
          this.setPersonList();
        }, 3000);
      }
    } else {
          clearInterval(this.intervalId);
          this.intervalId = undefined; 
    }

  }

  onLevelOneNavigationChange(event) {
    this.filteredsecondLevelOptions(event?.value);
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

    this.postForm.value.navigationType = !this.postForm.value.isNavigation ?
      null : this.postForm.value.navigationType;
  }

  onSwitchNavigationChange(event) {
    this.postForm.value.navigationType = !this.postForm.value.isNavigation ?
      null : this.postForm.value.navigationType;
  }

  onSwitchTimerChange(event) {
    this.postForm.value.appointment = !this.postForm.value.istimer ?
      null : this.postForm.value.appointment;
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

  initNotificationTemplate(pageInfo?: any) {
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    this.page.keyword = this.keyword;
    this.isLoading = true;
    this.notificationService.getAllNotificationTemplate(this.page).subscribe((res) => {
      this.page.totalItems = res.totalResults;
      this.notificationTemplates = res?.results;
      const listNotificationTemplates = this.notificationTemplates
      if (listNotificationTemplates?.length) {
        this.notificationTemplates = listNotificationTemplates.map(notification => {
          notification.id = notification.id;
          notification.labelName = (notification.type ? notification.type : '') + (notification.title ? (' - ' + notification.title) : '');
          return notification;
        });
      }
      this.isLoading = false;
    }, () => {
      this.isLoading = false;
    });
  }

  changeNotification(notificationTemplateId) {
    const notification = this.notificationTemplates.find(notificationTemplate => notificationTemplate.id == notificationTemplateId);
    this.filteredsecondLevelOptions(notification?.levelOneNavigation);

    this.mainImg = notification.mainImg;
    this.postForm = this.fb.group({
      title: [notification.title, [Validators.required]],
      actions: [notification.actions, [Validators.required]],
      description: [notification.description, [Validators.required]],
      isFeatured: [notification.isFeatured, []],
      appNotificationDesc: [decode(notification.appNotificationDesc), [Validators.required]],
      notificationContent: [decode(notification.notificationContent), []],
      smsContent: [decode(notification.smsContent), []],
      emailContent: [decode(notification.emailContent), []],
      type: [notification.type, []],
      status: [notification.status, []],
      actionView: [notification.actionView, []],
      emailContentType: [notification.contentType, []],
      externalEvent: [notification.externalEvent, []],
      externalParams: [notification.externalParams, []],
      appNotifContentType: ["HTML", []],
      isNavigation: [notification.isNavigation, []],
      navigationType: [notification.navigationType, []],
      navigationLink: [notification.navigationLink, []],
      levelOneNavigation: [notification.levelOneNavigation, []],
      secondLevelNavigation: [notification.secondLevelNavigation, []],
      isTimer: [false, []],
      appointment: ["", []],
      notificationTemplateId: [notification.id, []],
      typeModule: ['eLoyalty',[]], 
    });
  }

  get postFormControl() {
    return this.postForm?.controls;
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

  onSubmit() {
    if (this.postForm.valid) {
      if (this.currentNotification) {
        this.postForm.value.id = this.currentNotification.id;
        this.postForm.value.mainImg = this.mainImg;

        this.notificationService.saveNotification(this.postForm.value).subscribe((result) => {
          this.messageService.add({
            severity: 'success',
            summary: "Cập nhật thông báo thành công",
            life: 2000,
          })
        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi cập nhật thông báo",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        })
      } else {
        this.postForm.value.mainImg = this.mainImg;
        this.notificationService.createNotification(this.postForm.value).subscribe((result) => {
          this.currentNotification = result;
          this.currentNotificationId = result.id;
          let id = this.currentNotificationId;
          this.messageService.add({
            severity: 'success',
            summary: "Tạo mới thông báo thành công",
            life: 3000,
          })
          setTimeout(() => {
            this.router.navigate(['/notification-management/notification-message/detail'], { queryParams: { id } });
          }, 300);

        }, () => {
          this.messageService.add({
            severity: 'error',
            summary: "Lỗi khi tạo thông báo",
            detail: "Vui lòng thử lại",
            life: 3000,
          })
        });
      }
      this.initNotificationTemplate();
    }
  }

  close() {
    if (confirm("Bạn có muốn huỷ thay đổi")) {
      this.router.navigate(['/notification-management/notification-message']);
    }
  }

  insertImageEmailContent() {
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
      let oldEmailContentValue = this.postForm.value.emailContent;
      let a = oldEmailContentValue.slice(0, this.caretPos) + imagesUrl + oldEmailContentValue.slice(this.caretPos);
      this.postForm.controls['emailContent'].setValue(a);

    })
  }

  insertImageNotificationContent() {
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
      let oldNotificationContent = this.postForm.value.notificationContent;
      let b = oldNotificationContent.slice(0, this.caretPos) + imagesUrl + oldNotificationContent.slice(this.caretPos);
      this.postForm.controls['notificationContent'].setValue(b);
    })
  }

  getCaretPos(oField) {
    if (oField.selectionStart || oField.selectionStart == '0') {
      this.caretPos = oField.selectionStart;
    }
  }

  resetSendNotifySize() {
    this.filters.sendNotifySize = NotifyManagerConst.TYPE_SELECTED_PAGE;
  }

  changeFieldSearch() {
    if(this.keyword) {
      this.setPersonList();
    }
  }

  setPersonList(pageInfo?: any) {
    this.personListPage.pageNumber = pageInfo?.page ?? this.offset;
    this.personListPage.keyword = this.keyword;
    if (pageInfo?.rows) this.personListPage.pageSizeNotify = pageInfo?.rows;

    this.isLoadingPersonList = false;
    let filters = {
      'sendSMSStatus': this.filters.sendSMSStatus?.map(status => { return status.value }),
      'pushAppStatus': this.filters.pushAppStatus?.map(status => { return status.value }),
      'sendEmailStatus': this.filters.sendEmailStatus?.map(status => { return status.value }),
    }
    //
    if(this.currentNotificationId) {
      console.log("this.dataFilter",this.dataFilter);
      
      this.notificationService.getPersonList(this.currentNotificationId, this.personListPage, this.dataFilter, filters ).subscribe((res) => {
        this.personListPage.totalItems = res.totalResults;
        this.isLoadingPersonList = false;
        this.initLoading = false;
        this.listOfReciever = res.results;
      }, () => {
        this.isLoadingPersonList = false;
      });
    }
  }

  getStatusName(status) {
    let statusesWithLabel = {
      'DRAFT': "Chờ gửi",
      'NOT_AVAILABLE': "Không gửi",
      'PENDING': "Đang chờ gửi",
      'SENT': "Đã gửi",
      'SENDING': "Đang gửi",
      'SEND_ERROR': "Gửi lỗi",
    }
    return statusesWithLabel[status];
  }

  getStatusSeverity(status) {
    let statusesWithLabel = {
      'DRAFT': "info",
      'NOT_AVAILABLE': "secondary",
      'PENDING': "info",
      'SENT': "success",
      'SENDING': "info",
      'SEND_ERROR': "danger",
    }
    return statusesWithLabel[status];
  }

  applyFilter() {
    this.setPersonList({ page: this.offset })
  }


  addPeopleToSendingList() {
    if (!this.currentNotification) {
      return this.messageService.add({
        severity: 'error',
        summary: "Vui lòng thử lại",
        detail: "Bạn cần phải lưu thông báo trước khi thiết lập danh sách gửi",
        life: 3000,
      })
    }

    this.dialogService.open(AddPersonListComponent, {
      data: {
        inputData: this.currentNotification
      },
      header: 'Cài đặt danh sách thông báo',
      width: '80%',
      height: '85%',
      style: { 'max-height': '100%', 'border-radius': '10px', "overflow": "auto" },
      contentStyle: { "overflow": "auto" },
      footer: ""
    }).onClose.subscribe(result => {
      this.offset = 0;
      this.setPersonList({ page: this.offset });
    })
  }

  sendNotification() {
    if (this.selectedCustomers.length > 0) {
      this.isLoading = true;
      this.notificationService.pushNotification({
        notificationId: this.currentNotificationId,
        receivers: this.selectedCustomers
      }).subscribe(result => {
        this.isLoading = false;
        this.resetSendNotifySize();
        this.selectedCustomers = [];
        this.messageService.add({
          severity: 'success',
          summary: "Thiết lập gửi thông báo thành công.",
          detail: "Thông báo đã được đưa vào danh sách gửi.",
          life: 3000,
        });
        //
        this.peopleToSendingList();
        //
      }, err => {
        this.messageService.add({
          severity: 'error',
          summary: "Lỗi khi gửi thông báo",
          life: 3000,
        })
        this.isLoading = false;
      });
      //
      this.initNotificationTemplate();
      this.setPersonList();
    }
  }

  changeSendNotifySize(type) {
    let filters = {
      'sendSMSStatus': this.filters.sendSMSStatus?.map(status => { return status.value }),
      'pushAppStatus': this.filters.pushAppStatus?.map(status => { return status.value }),
      'sendEmailStatus': this.filters.sendEmailStatus?.map(status => { return status.value }),
    }
    //
    if (type == NotifyManagerConst.TYPE_SELECTED_FULL) {
      this.isLoadingPersonList = true;
      this.notificationService.getPersonList(this.currentNotificationId, this.personListPage, this.dataFilter,filters, true).subscribe((res) => {
        this.isLoadingPersonList = false;
        this.selectedCustomers = [...res.results];
      }, () => {
        this.isLoadingPersonList = false;
      });
    } else {
      this.resetSendNotifySize();
      this.selectedCustomers = [];
    }
  }

  peopleToSendingList() {
    let customers = this.selectedCustomers.map(customer => {
      return {
        fullName: customer.name,
        personCode: customer?.defaultIdentification?.id,
        phoneNumber: customer.phone,
        email: customer.email,
        notification: this.inputData?.id
      }
    });
    //
    if (this.inputData?.id) {
      this.isLoadingPersonList = false;
      this.notificationService.addPeopleToNotification({ sendingList: customers }, this.inputData.id).subscribe(results => {
        this.isLoadingPersonList = false;
        this.messageService.add({ key: 'tst', severity: 'success', summary: 'Thành công', detail: 'Cập nhật danh sách người nhận thành công' });
        // this.ref.close(null); 
      }, error => {
        this.isLoadingPersonList = false;
        this.messageService.add({ key: 'tst', severity: 'error', summary: 'Lỗi', detail: 'Có lỗi xảy ra. Vui lòng thử lại!' });
      });
      //
      this.initNotificationTemplate();
    }
  }

  deleteKH() {
    if (this.selectedCustomers.length > 0) {
      this.isLoading = true;
      const temp = [];
      Object.keys(this.selectedCustomers).forEach((key) => {
      })
      this.selectedCustomers.forEach((element1) => {
        if (element1.id) {
          temp.push(element1.id);
        }
      })
      this.deleteArr = temp.toString();

      this.notificationService.deleteKH(this.deleteArr).subscribe(result => {
        this.isLoading = false;
        this.selectedCustomers = [];
        this.messageService.add({
          severity: 'success',
          summary: "Xóa danh sách gửi thông báo thành công.",
          detail: "Thông báo đã được đưa vào danh sách hủy.",
          life: 3000,
        });
        //
        this.peopleToSendingList();
        //
        this.initNotificationTemplate();
        this.setPersonList();
      }, err => {
        this.messageError('Không thể xóa KH. Vui lòng thử lại sau!');
        this.isLoading = false;
      });
    }
  }

  moveToUserList() {
    let element = document.querySelector('#p-tabpanel-1-label');
    if (element != undefined) {
      (<HTMLElement>element).click();
    }
  }

  out() {
    window.location.href = 'notification-management/notification-message';
  }

}
