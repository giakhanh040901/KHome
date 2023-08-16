import { Component, Injector, OnInit } from "@angular/core";
import { AppConsts, AtributionConfirmConst, ConfigureSystemConst } from "@shared/AppConsts";
import { ConfirmationService, MessageService } from "primeng/api";
import { DialogService } from "primeng/dynamicdialog";
import { BreadcrumbService } from "src/app/layout/breadcrumb/breadcrumb.service";
import { decode } from "html-entities";
import { CrudComponentBase } from "@shared/crud-component-base";
import { UploadImageComponent } from "@shared/components/upload-image/upload-image.component";
import { NotificationService } from "@shared/services/notification.service";
import * as moment from "moment";
import { Observable, forkJoin } from "rxjs";
import { ConfigureSystemService } from "@shared/services/configure-system.service";

@Component({
  selector: 'system-notification-template',
  templateUrl: './system-notification-template.component.html',
  styleUrls: ['./system-notification-template.component.scss'],
	providers: [DialogService, ConfirmationService, MessageService],
})
export class SystemNotificationTemplateComponent
	extends CrudComponentBase
	implements OnInit
{
	baseUrl: string;
	systemNotificationTemplate: any;
	items: any[];
	itemsAdmin: any[];
	itemSelected: any;
	itemSelectedAdmin: any;
	caretPos: number = 0;
	actionsList: any = [
		{ value: "PUSH_NOTIFICATION", name: "Đẩy thông báo trên app" },
		{ value: "SEND_SMS", name: "Gửi SMS" },
		{ value: "SEND_EMAIL", name: "Gửi email" },
	];
	currentKey: any;
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
	configKeys: any = {};
	settingTimeNotifySend: any = [];

    keyNotifyDateConst = ConfigureSystemConst.keyNotifyDates;

	constructor(
		private notificationService: NotificationService,
		private dialogService: DialogService,
		injector: Injector,
		protected messageService: MessageService,
		private breadcrumbService: BreadcrumbService,
		public confirmationService: ConfirmationService,
		private configureSystemService: ConfigureSystemService,
	) {
		super(injector, messageService);

		this.breadcrumbService.setItems([
			{ label: "Trang chủ", routerLink: ["/home"] },
			{ label: "Cài đặt" },
			{ label: "Cấu hình thông báo từ hệ thống" },
		]);
	}

	ngOnInit(): void {
		this.baseUrl = AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
		this.systemNotificationTemplate = {
			key: "",
			notificationTemplateName: "",
			description: "",
			actions: [],
			emailContentType: "MARKDOWN",
			pushAppContentType: "MARKDOWN",
			smsContentType: "MARKDOWN",
			emailContent: "",
			pushAppContent: "",
			smsContent: "",
			titleAppContent: "",
		};
		
		forkJoin([this.configureSystemService.getAll(this.page, true),this.notificationService.getSystemNotification("event")]).subscribe(
			([dataConfig,data]) => {
				if (data && data.configKeys) {
					var normalizedSettings = new Object();
					if (data.value.settings) {
						data.value.settings.forEach((setting) => {
							normalizedSettings[setting.key] = { ...setting };
						});
						//Nếu FE chưa có thì bôt sung thêm config vào.
						data.configKeys.forEach((config) => {
							if (
								normalizedSettings[config.key]?.hasOwnProperty("isAdmin") &&
								config?.isAdmin
							) {
								normalizedSettings[config.key].isAdmin = true;
								normalizedSettings[config.key].adminEmailDisplay =
									normalizedSettings[config.key].adminEmail.map((email) => ({
										email,
									}));
							}
							if (!normalizedSettings[config.key]) {
								normalizedSettings[config.key] = config?.isAdmin
									? { ...config, adminEmailDisplay: [] }
									: { ...config };
							}
						});
					} else {
						data.configKeys.forEach((config) => {
							normalizedSettings[config.key] = config?.isAdmin
								? { ...config, adminEmailDisplay: [] }
								: { ...config };
						});
					}
					this.configKeys = normalizedSettings;
					this.itemsAdmin = data?.configKeys
						.filter((item) => item.isAdmin)
						.map((item) => ({
							...item,
							notificationTemplateName:
								item.notificationTemplateName + " [Admin]",
						}));
					this.items = data?.configKeys?.filter((item) => !item.isAdmin);
				}
				if(dataConfig?.data?.length) {
					this.setDataConfigSystemSendNotify(dataConfig.data);
				}
			},
			(error) => {
				return this.messageError('Lỗi khi tải cấu hình thông báo');
			}
		);
	}

	setDataConfigSystemSendNotify(dataConfig) {
		this.settingTimeNotifySend = this.keyNotifyDateConst.reduce((result, info) => {
		  let configSystem = dataConfig.find(c => c.key === info.code);
		  let obj;
		  if (configSystem && configSystem.key === "EventTimeSendNotiEventUpComingForCustomer") {
			obj = {
			  key: info.code,
			  value: new Date(moment().format("YYYY-MM-DD ") + configSystem.value),
			  isUpdate: !!configSystem,
			};
		  } else {
			obj = {
			  key: info.code,
			  value: configSystem ? configSystem.value : 0,
			  isUpdate: !!configSystem,
			};
		  }
		  result[info.keyNotify] = result[info.keyNotify] || [];
		  result[info.keyNotify].push(obj);
		  return result;
		}, {});
	}
	  
	addvalue(item) {
		this.configKeys[this.currentKey].adminEmailDisplay.push({ email: null });
	}

	removeElement(index) {
		this.confirmationService.confirm({
			message: "Xóa email này?",
			...AtributionConfirmConst,
			accept: () => {
				this.configKeys[this.currentKey].adminEmailDisplay.splice(index, 1);
			},
		});
	}

	notificationChange(item, isAdmin) {
		isAdmin ? (this.itemSelected = null) : (this.itemSelectedAdmin = null);
		this.currentKey = item.key;
		this.configKeys[this.currentKey].pushAppContent = decode(
			this.configKeys[this.currentKey].pushAppContent
		);
		this.configKeys[this.currentKey].emailContent = decode(
			this.configKeys[this.currentKey].emailContent
		);
	}

	saveSetting() {
		if (this.configKeys[this.currentKey]?.adminEmailDisplay?.length) {
			this.configKeys[this.currentKey].adminEmail = this.configKeys[
				this.currentKey
			].adminEmailDisplay.map((obj) => obj.email);
		}
		var settings = Object.keys(this.configKeys).map((key) => {
			return {
				key,
				...this.configKeys[key],
			};
		});
		this.notificationService
			.createOrUpdateSystemNotification(settings, "event")
			.subscribe(
				(data) => {
					return this.messageSuccess("Cập nhật thành công");
				},
				(error) => {
					return this.messageError("Lỗi khi cập nhật cấu hình thông báo"); 
				}
			);
			
			let apiUpdateConfigs: Observable<any>[] = [];

			for (const [key, value] of Object.entries(this.settingTimeNotifySend)) {
				
				let body = this.settingTimeNotifySend[key].map(obj => {
					if (obj.value instanceof Date) {
						obj.value = moment(obj.value).format("HH:mm");
					}
					return obj;
					});
				apiUpdateConfigs.push(this.configureSystemService.createOrUpdate(body));
			}

			forkJoin(apiUpdateConfigs).subscribe(
				() => {
					this.configureSystemService.getAll(this.page, true).subscribe(res => {
						this.setDataConfigSystemSendNotify(res?.data);
					});
				},
				(err) => {
					this.messageError('Lỗi cập nhật thời gian gửi thông báo!');
				}
			);
	}

	insertImage(contentName) {
		const ref = this.dialogService.open(UploadImageComponent, {
			data: {
				inputData: [],
				showOrder: false,
			},
			header: "Chèn hình ảnh",
			width: "600px",
		});
		ref.onClose.subscribe((images) => {
			let imagesUrl = "";
			images.forEach((image) => {
				imagesUrl += `![](${this.baseUrl}/${image.data}) \n`;
			});

			let oldEmailContentValue = this.configKeys[this.currentKey][contentName];
			let a =
				oldEmailContentValue.slice(0, this.caretPos) +
				imagesUrl +
				oldEmailContentValue.slice(this.caretPos);
			this.configKeys[this.currentKey][contentName] = a;
		});
	}

	getCaretPos(oField) {
		if (oField.selectionStart || oField.selectionStart == "0") {
			this.caretPos = oField.selectionStart;
		}
	}
}

