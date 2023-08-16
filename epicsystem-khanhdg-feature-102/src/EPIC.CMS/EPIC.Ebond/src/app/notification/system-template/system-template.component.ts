import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, PermissionBondConst } from '@shared/AppConsts';
import { NotificationService } from '@shared/service-proxies/notification-service';
import { ConfirmationService, MenuItem, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components/upload-image/upload-image.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { decode } from 'html-entities';
import { CrudComponentBase } from '@shared/crud-component-base';

@Component({
  selector: 'app-system-template',
  templateUrl: './system-template.component.html',
  styleUrls: ['./system-template.component.scss'],
  providers: [DialogService, ConfirmationService, MessageService],
})
export class SystemTemplateComponent extends CrudComponentBase implements OnInit {

  baseUrl: string;
  systemNotificationTemplate: any;
  items: any[];
  itemSelected: any;
  caretPos: number = 0;
  actionsList: any = [
    {value: 'PUSH_NOTIFICATION', name : 'Đẩy thông báo trên app'}, 
    {value: 'SEND_SMS', name: 'Gửi SMS' },
    { value: 'SEND_EMAIL', name: 'Gửi email'}
  ]
  currentKey: any;
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
  configKeys: any = {}
  
  constructor(
		injector: Injector,
    private notificationService: NotificationService,
    private dialogService: DialogService,
    messageService: MessageService,
    private breadcrumbService: BreadcrumbService,
    public confirmationService: ConfirmationService,
    ) { 
		super(injector, messageService);
      this.breadcrumbService.setItems([
        { label: 'Trang chủ', routerLink:['/home'] },
        { label: 'Cấu hình thông báo từ hệ thống'}
      ]);
    }

  ngOnInit(): void {
    this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl; 
    this.systemNotificationTemplate = {
      key: "",
      notificationTemplateName: "",
      description: "",
      actions: [],
      emailContentType: 'MARKDOWN',
      pushAppContentType: 'MARKDOWN',
      smsContentType: 'MARKDOWN',
      emailContent: "",
      pushAppContent: "",
      smsContent: ""
    }
    
    this.notificationService.getSystemNotification('bond').subscribe(data => { 
      if(data && data.configKeys) {
        var normalizedSettings = new Object;
        if(data.value.settings) {
          data.value.settings.forEach(setting => {
            normalizedSettings[setting.key] = { ...setting };
          });
        }else {
          data.configKeys.forEach(config => {
           normalizedSettings[config.key] = config?.isAdmin
            ? { ...config, adminEmailDisplay: [] }
            : { ...config };
          })
          //Nếu FE chưa có thì bôt sung thêm config vào.
          data.configKeys.forEach(config => {
            if (normalizedSettings[config.key]?.hasOwnProperty('isAdmin') && config?.isAdmin) {
              normalizedSettings[config.key].isAdmin = true;
              normalizedSettings[config.key].adminEmailDisplay = normalizedSettings[config.key].adminEmail.map(email => ({email}));
            }
            if(!normalizedSettings[config.key]) {
              normalizedSettings[config.key] = config?.isAdmin
                ? { ...config, adminEmailDisplay: [] }
                : { ...config };
            }
          })
        } 
        this.configKeys = normalizedSettings;   
        this.items = data?.configKeys?.map((item) => {
          if (item?.isAdmin) {
            item.notificationTemplateName += ` [Admin]`
          }
          return item;
        });

      }
    }, error => {
      return this.messageService.add({
        severity: 'error',
        summary: "Lỗi",
        detail: "Lỗi khi tải cấu hình thông báo",
        life: 3000,
      })
    }) 
  }

  addvalue(item) {
    this.configKeys[this.currentKey].adminEmailDisplay.push({ email: null}); 
  }

  removeElement(index) {
    this.confirmationService.confirm({
      message: 'Xóa email này?',
      acceptLabel: 'Đồng ý',
      rejectLabel: 'Hủy',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.configKeys[this.currentKey].adminEmailDisplay.splice(index, 1);
      }
    });
  }
   
  notificationChange() {
    this.currentKey = this.itemSelected.key; 
    this.configKeys[this.currentKey].pushAppContent = decode(this.configKeys[this.currentKey].pushAppContent);
    this.configKeys[this.currentKey].emailContent = decode(this.configKeys[this.currentKey].emailContent);
  }

  saveSetting() {
    this.configKeys[this.currentKey].adminEmail  = this.configKeys[this.currentKey].adminEmailDisplay.map(obj => obj.email);
    var settings = Object.keys(this.configKeys).map(key => {
      return {
        key,
        ...this.configKeys[key]
      }
    });
    this.notificationService.createOrUpdateSystemNotification(settings, 'bond').subscribe(data => {
      return this.messageService.add({
        severity: 'success',
        summary: "Cập nhật thành công",
        detail: "Cấu hình thông báo đã được cập nhật thành công",
        life: 3000,
      })
    }, error => {
      return this.messageService.add({
        severity: 'error',
        summary: "Lỗi",
        detail: "Lỗi khi cập nhật cấu hình thông báo",
        life: 3000,
      })
    }) 
  }

  insertImage(contentName) {
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
        imagesUrl +=  `![](${this.baseUrl}/${image.data}) \n`;
      })
      let oldEmailContentValue = this.configKeys[this.currentKey][contentName];
      let a = oldEmailContentValue.slice(0, this.caretPos) + imagesUrl + oldEmailContentValue.slice(this.caretPos); 
      this.configKeys[this.currentKey][contentName] = a;
    })
  }

  getCaretPos(oField) {
    if (oField.selectionStart || oField.selectionStart == '0') {
      this.caretPos = oField.selectionStart;
    }
  }
}
