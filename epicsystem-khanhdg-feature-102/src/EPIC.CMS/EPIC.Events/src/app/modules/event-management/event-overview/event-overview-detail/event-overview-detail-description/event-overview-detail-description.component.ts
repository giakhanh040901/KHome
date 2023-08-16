import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { FormNotificationConst, MARKDOWN_OPTIONS } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IDescriptionContent, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { EventOverviewDetailGeneralModel } from '@shared/interface/event-management/event-overview/EventOverviewDetailGeneral.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'event-overview-detail-description',
  templateUrl: './event-overview-detail-description.component.html',
  styleUrls: ['./event-overview-detail-description.component.scss'],
})
export class EventOverviewDetailDescriptionComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private eventOverviewService: EventOverviewService,
    private dialogService: DialogService
  ) {
    super(injector, messageService);
  }
  public dto: EventOverviewDetailGeneralModel = new EventOverviewDetailGeneralModel();
  public isEdit: boolean = false;

  public get isDisable() {
    return !this.isEdit;
  }

  ngOnInit() {
    this.dto.id = this.eventOverviewService.eventId;
    this.dto.contentType = MARKDOWN_OPTIONS.MARKDOWN;
    this.getEventDetail();
  }

  private getEventDetail() {
    this.eventOverviewService.getEventDetail().subscribe((res: any) => {
      if (res.data) {
        this.dto.mapDTODescription(res.data);
        this.dto.contentType =
          this.dto.contentType && this.dto.contentType.length ? this.dto.contentType : MARKDOWN_OPTIONS.MARKDOWN;
      }
    });
  }

  public onChangeDescriptionContent(event: IDescriptionContent | undefined) {
    if (event) {
      this.dto.content = event.content;
      this.dto.contentType = event.contentType;
    }
  }

  public edit(event: any) {
    if (event) {
      this.isEdit = true;
    }
  }

  public save(event: any) {
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
          title: 'Bạn có chắc chắn lưu dữ liệu?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService
            .saveContent({
              id: this.dto.id,
              contentType: this.dto.contentType,
              overviewContent: this.dto.content,
            })
            .subscribe(
              (response) => {
                if (this.handleResponseInterceptor(response, 'Lưu thành công')) {
                  this.isEdit = false;
                  this.getEventDetail();
                }
              },
              (err) => {
                this.messageError(`Không Lưu được`);
              }
            );
        }
      });
    }
  }
}
