import { Injectable } from "@angular/core";
import { DomSanitizer, SafeHtml, SafeUrl } from "@angular/platform-browser";
import { StatusResponseConst } from "@shared/AppConsts";
import { EConfirmComponent } from "@shared/components/e-comfirm/e-comfirm.component";
import { EUploadComponent } from "@shared/components/e-upload/e-upload.component";
import { EViewComponent } from "@shared/components/e-view/e-view.component";
import { ContentTypeEView, IconConfirm } from "@shared/consts/base.const";
import { IDialogUploadFileConfig, IParamHandleDTO } from "@shared/interface/other.interface";
import { IResponseItem } from "@shared/interface/response.interface";
import { MessageService } from "primeng/api";
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";

@Injectable()
export class HelpersService {
    constructor(
        protected messageService: MessageService,
        protected sanitizer: DomSanitizer,
        public dialogService: DialogService,
    ) {
    }

    handleResponseInterceptor(response: IResponseItem<any>, message?: string): boolean {
        if (response?.status === StatusResponseConst.RESPONSE_TRUE) {
            if (message) {
				this.messageService.add({ severity: 'success', summary: '', detail: message, life: 1000 });
            } else if(response?.successFE && message === undefined) {
                // MESSAGE CHUNG CHO CREATE | UPDATE | DELETE KHI KHÔNG TRUYỀN VÀO PARAM MESSAGE 
                this.messageService.add({ severity: 'success', summary: '', detail: response.successFE, life: 1000 });
            }
            return true;
        } else {
            let dataMessage = response?.data;
            if (dataMessage) {
				this.messageService.add({ severity: 'error', summary: '', detail: dataMessage[Object.keys(dataMessage)[0]], life: 3000 });
            } else {
                let message = response?.message;
                if(response?.code > 1 && response?.code < 1000) {
                    message = "Có lỗi xảy ra vui lòng thử lại sau!";
                } 
				this.messageService.add({ severity: 'error', summary: '', detail: response?.message, life: 3000 });
            }
            return false;
        }
    }

    messageError(msg = '', summary = '', life = 3500) {
		this.messageService.add({ severity: 'error', summary, detail: msg, life: life });
	}

	messageSuccess(msg = '', summary = '', life = 2000) {
		this.messageService.add({ severity: 'success', summary, detail: msg, life: life });
	}

    messageWarn(msg = '', life = 3000) {
		this.messageService.add({ severity: 'warn', summary: '', detail: msg, life: life, icon: 'pi-bell' });
	}

     // LỌC PROPERTY CHỈ LẤY CÁC PROPERTY CỦA MODEL ĐỂ PUSH VÀO API
     mapDTO(data: Object, model: Object, paramHanldes?: IParamHandleDTO) {
        paramHanldes = {
            idName: paramHanldes?.idName || 'id',
            isCheckNull: !!paramHanldes?.isCheckNull,
        }
        //
        let newData: any = {};
        for (const key of Object.keys(model)) {
            // NẾU ID NULL THÌ KHÔNG ĐẨY LÊN, NẾU CHECK NULL THÌ CÁC TRƯỜNG NULL KHÔNG ĐẨY LÊN
            if((key === paramHanldes?.idName || paramHanldes.isCheckNull) && !data[key]) continue;
            newData[key] = data[key];
        }
        return newData;
    }

    getAtributionConfirmDialog(message: string, icon: IconConfirm = IconConfirm.WARNING) {
        return {
            header: "Thông báo",
            with: 'auto',
            style: {'min-width': '350px'},
            data: {
                message : message,
                icon: icon,
            },
        } as DynamicDialogConfig;
    }

    getBlobUrlImage(image: File): SafeUrl {
        return this.sanitizer.bypassSecurityTrustUrl(image['objectURL']['changingThisBreaksApplicationSecurity']);
    }

    getContentHtml(content): SafeHtml {
        return this.sanitizer.bypassSecurityTrustHtml(content);
    }
    
    checkValidForm(isValid: boolean, ) {
        if(!isValid) this.messageError('Vui lòng nhập đủ thông tin');
    }

    dialogConfirmRef(message: string, icon?: IconConfirm): DynamicDialogRef {
        return this.dialogService.open(
            EConfirmComponent,
            this.getAtributionConfirmDialog(message, icon)
        );
    }

    // dialogRequestRef(id: number, summary: string): DynamicDialogRef {
    //     return this.dialogService.open(
    //         ERequestComponent,
    //         {
    //             header: "Trình duyệt",
    //             width: '600px',
    //             data: {
    //                 id: id,
    //                 summary: summary,
    //             },
    //         }
    //     );
    // }

    // dialogApproveRef(id: number): DynamicDialogRef {
    //     return this.dialogService.open(
    //         EApproveComponent,
    //         {
    //             header: "Phê duyệt",
    //             width: '600px',
    //             data: {
    //                 id: id
    //             },
    //         }
    //     );
    // }

    dialogUploadImagesRef(folderUpload: string, params?: IDialogUploadFileConfig): DynamicDialogRef {
        return this.dialogService.open(
            EUploadComponent,
            {
                header: params?.header || 'Preview Image',
                width: params?.width || '650px',
                data: {
                    ...(params || {}),
                    folderUpload: folderUpload,
                },
            }
        )
    }

    dialogViewerRef(content: any, type: ContentTypeEView, params?: DynamicDialogConfig) {
        params = {
            ...(params || new DynamicDialogConfig()),
            contentStyle: params?.contentStyle || {},
        }
        //
        if(type === ContentTypeEView.FILE) {
            params.contentStyle['padding-top'] = 0;
            params.contentStyle['padding-bottom'] = 0;
        } else {
            if(!params.contentStyle['padding-bottom']) {
                params.contentStyle['padding-bottom'] = '10px';
            }
        }
        //
        params.header = params.header || 'Preview';
        if(type === ContentTypeEView.IMAGE) params.header = '';
        //
        return this.dialogService.open(EViewComponent, {
            header: params.header,
            width: params.width || '100%',
            style: {...(params.style || {}), 'border-radius': 0 },
            contentStyle: {...(params.contentStyle || {}) }, 
            styleClass: params.styleClass + ' height-100 ' + (type === ContentTypeEView.IMAGE && ' no-background '),
            data: {
                content: content,
                type: type,
            },
        });
    }

}
