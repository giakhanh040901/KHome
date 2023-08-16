import { Component, Injector, OnInit } from '@angular/core';
import { AppConsts, ConfigureSystemConst } from '@shared/AppConsts';
import { NotificationService } from '@shared/service-proxies/notification-service';
import { MenuItem, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { UploadImageComponent } from 'src/app/components/upload-image/upload-image.component';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';
import { decode } from 'html-entities';
import { CrudComponentBase } from '@shared/crud-component-base';
import { Observable, forkJoin } from 'rxjs';
import { ConfigureSystemService } from '@shared/services/configure-system.service';
import * as moment from 'moment';

@Component({
  selector: 'app-system-template',
  templateUrl: './system-template.component.html',
  styleUrls: ['./system-template.component.scss']
})
export class SystemTemplateComponent extends CrudComponentBase implements OnInit {

    baseUrl: string;
    systemNotificationTemplate: any;
    items: any[];
    itemSelected: any;
    caretPos: number = 0;
    actionsList: any = [
        { value: 'PUSH_NOTIFICATION', name : 'Đẩy thông báo trên app' }, 
        { value: 'SEND_SMS', name: 'Gửi SMS' },
        { value: 'SEND_EMAIL', name: 'Gửi email'}
    ];
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
    ];

    configKeys: any = {}

    settingTimeNotifySendHappybirthday: any = {};

    keyNotifyDateConst = ConfigureSystemConst.keyNotifyDates;

    
    constructor(
        private notificationService: NotificationService,
        private dialogService: DialogService,
        injector: Injector,
        protected messageService: MessageService,
        private breadcrumbService: BreadcrumbService,
        private configureSystemService: ConfigureSystemService,
        ) { 
        super(injector, messageService)
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
            smsContent: "",
            titleAppContent: ""
        }
        
        forkJoin([this.configureSystemService.getAll(this.page, true), this.notificationService.getSystemNotification('system')]).subscribe(([dataConfig, dataKeyNotify]) => { 
            if(dataKeyNotify && dataKeyNotify.configKeys) {
                var normalizedSettings = new Object;
                if(dataKeyNotify.value.settings) {
                    dataKeyNotify.value.settings.forEach(setting => {
                        normalizedSettings[setting.key] = { ...setting }; 
                    });
                    //Nếu FE chưa có thì bôt sung thêm config vào.
                    dataKeyNotify.configKeys.forEach(config => {
                        if(!normalizedSettings[config.key]) {
                            normalizedSettings[config.key] = { ...config }
                        }
                    })
                }else {
                    dataKeyNotify.configKeys.forEach(config => {
                        normalizedSettings[config.key] = { ...config }
                    })
                }
             
                this.configKeys = normalizedSettings;   
                this.items = dataKeyNotify.configKeys
            }
            //
            if(dataConfig?.data?.length) {
                this.setDataConfigSystemSendNotify(dataConfig.data);
            }
        }, error => {
            this.messageError('Lỗi khi tải cấu hình thông báo');
        }); 
    }

    setDataConfigSystemSendNotify(dataConfig) {
        for(let info of this.keyNotifyDateConst) {
            let configSystem = dataConfig.find(c => c.key === info.code);
            this.settingTimeNotifySendHappybirthday[info.keyNotify] = {
                key: info.code,
                value: configSystem ? new Date(moment().format("YYYY-MM-DD ")+configSystem.value) : new Date(),
                isUpdate: !!configSystem,
            };
        }
    }

    
    notificationChange() {
        this.currentKey = this.itemSelected.key; 
        this.configKeys[this.currentKey].pushAppContent = decode(this.configKeys[this.currentKey].pushAppContent);
        this.configKeys[this.currentKey].emailContent = decode(this.configKeys[this.currentKey].emailContent);
    }

    saveSetting() {
        var settings = Object.keys(this.configKeys).map(key => {
            return {
                key,
                ...this.configKeys[key],
            }
        });
        this.notificationService.createOrUpdateSystemNotification(settings, 'system').subscribe(data => {
            this.messageSuccess('Cập nhật thành công');
            }, error => {
            this.messageError('Lỗi khi cập nhật cấu hình thông báo');
        });
        //\\
        let apiUpdateConfigs: Observable<any>[] = [];
        for( const [key, value] of Object.entries(this.settingTimeNotifySendHappybirthday)) {
            let body = {
                ...this.settingTimeNotifySendHappybirthday[key],
                value: moment(this.settingTimeNotifySendHappybirthday[key].value).format("HH:mm")
            }
            apiUpdateConfigs.push(this.configureSystemService.createOrUpdate(body))
        }

        forkJoin(apiUpdateConfigs).subscribe(() => {
            this.configureSystemService.getAll(this.page, true).subscribe(res => {
                this.setDataConfigSystemSendNotify(res?.data);
            });
        }, (err) => {
            this.messageError('Lỗi cập nhật thời gian gửi thông báo!');
        });
        //
    }

    insertImage(contentName) {
        this.dialogService.open(UploadImageComponent, {
            header: 'Chèn hình ảnh',
            width: '600px',
            footer: "",
            data: {
                inputData: [],
                showOrder: false
            }, 
        }).onClose.subscribe(images => {
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
