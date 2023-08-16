import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EConfigDataModal, FormNotificationConst, YES_NO } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { getUrlImage } from '@shared/function-common';
import { IDescriptionContent, IImage, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { CrudTicketInforItem } from '@shared/interface/event-management/event-overview/EventOverviewDetailInfor.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-event-ticket-infor',
  templateUrl: './crud-event-ticket-infor.component.html',
  styleUrls: ['./crud-event-ticket-infor.component.scss'],
})
export class CrudEventTicketInforComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private eventOverviewService: EventOverviewService
  ) {
    super(injector, messageService);
  }
  public crudDTO: CrudTicketInforItem = new CrudTicketInforItem();
  public type: string = EConfigDataModal.CREATE;
  public imgBackground = 'assets/layout/images/add-image-bg.svg';
  public defaultImage: IImage = {
    src: '',
    width: 140,
    height: 140,
  };

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public get YES_NO() {
    return YES_NO;
  }

  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    if (this.type === EConfigDataModal.CREATE) {
    } else {
      this.crudDTO.mapData(this.config.data.dataSource);
      if (this.crudDTO.listImage.length) {
        this.crudDTO.listImage = this.crudDTO.listImage.map((img: IImage) => ({
          ...this.defaultImage,
          src: getUrlImage(img.src),
          id: img.id,
        }));
      }
    }
    this.getListImage();
  }

  public onChangeDescriptionContent(event: IDescriptionContent | undefined) {
    if (event) {
      this.crudDTO.content = event.content;
      this.crudDTO.contentType = event.contentType;
    }
  }

  public save(event?: any) {
    if (event) {
      if (this.crudDTO.isValidData()) {
        this.eventOverviewService
          .createOrEditTicket(
            this.crudDTO.toObjectSendToAPI(this.eventOverviewService.eventDetailInforId, this.imgBackground),
            this.type === EConfigDataModal.EDIT
          )
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        this.messageDataValidator(this.crudDTO.dataValidator);
      }
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public getShowBtnRemove(data: IImage) {
    if (data) {
      return data.src !== this.imgBackground;
    }
    return false;
  }

  public onRemove(event: any, index: number, image: IImage) {
    if (event) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Thông báo',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: 'Bạn có chắc chắn muốn xóa hình ảnh?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.crudDTO.listImage.splice(index, 1);
          this.getListImage();
        }
      });
    }
  }

  public onChangeImage(event: any, index: number) {
    if (event) {
      this.crudDTO.listImage[index].src = event.src;
      this.getListImage();
    }
  }

  public getListImage() {
    if (
      !this.crudDTO.listImage.length ||
      this.crudDTO.listImage[this.crudDTO.listImage.length - 1].src !== this.imgBackground
    ) {
      this.crudDTO.listImage.push({
        ...this.defaultImage,
        src: this.imgBackground,
      } as IImage);
    }
  }

  public onChangeIsFree(event: any) {
    if (event) {
      this.crudDTO.price = 0;
    }
  }
}
