import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  COMPARE_TYPE,
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeStatus,
  EventOverview,
  FormNotificationConst,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { compareDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IHeaderColumn,
  INotiDataModal,
} from '@shared/interface/InterfaceConst.interface';
import {
  CrudEventDetailInfor,
  EventDetailInforItem,
} from '@shared/interface/event-management/event-overview/EventOverviewDetailInfor.model';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { CrudEventTicketInforComponent } from './crud-event-ticket-infor/crud-event-ticket-infor.component';
import { ReplicateEventTicketInforComponent } from './replicate-event-ticket-infor/replicate-event-ticket-infor.component';

@Component({
  selector: 'crud-event-detail-infor',
  templateUrl: './crud-event-detail-infor.component.html',
  styleUrls: ['./crud-event-detail-infor.component.scss'],
})
export class CrudEventDetailInforComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private router: Router,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private dialogService: DialogService,
    private eventOverviewService: EventOverviewService
  ) {
    super(injector, messageService);
  }
  public crudDTO: CrudEventDetailInfor = new CrudEventDetailInfor();
  public headerColumns: IHeaderColumn[] = [];
  public listAction: IActionTable[][] = [];
  public type: string = EConfigDataModal.VIEW;

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public get listStatus() {
    return EventOverview.listStatusInfor;
  }

  public getStatusSeverity(code: any) {
    return EventOverview.getStatusInfor(code, ETypeStatus.SEVERITY);
  }

  public getStatusName(code: any) {
    return EventOverview.getStatusInfor(code, ETypeStatus.LABEL);
  }

  public get CO() {
    return EventOverview.CO;
  }

  public get listWaitPay() {
    return EventOverview.listYesNo;
  }

  public get listShowRestTicket() {
    return EventOverview.listYesNo;
  }

  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
    }

    this.headerColumns = [
      {
        field: 'id',
        header: '#ID',
        width: '5rem',
        type: ETypeDataTable.INDEX,
      },
      {
        field: 'name',
        header: 'Tên vé',
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'price',
        header: 'Giá vé',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'total',
        header: 'Tổng số vé',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'register',
        header: 'Đã đăng ký',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'pay',
        header: 'Đã thanh toán',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'isFree',
        header: 'Miễn phí',
        width: '8rem',
        type: ETypeDataTable.CHECK_BOX,
        posTextCell: EPositionTextCell.CENTER,
      },
      {
        field: 'isShowApp',
        header: 'Show app',
        width: '8rem',
        type: ETypeDataTable.CHECK_BOX,
        posTextCell: EPositionTextCell.CENTER,
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '8rem',
        type: ETypeDataTable.STATUS,
        funcStyleClassStatus: this.funcStyleClassStatus,
        funcLabelStatus: this.funcLabelStatus,
        posTextCell: EPositionTextCell.LEFT,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
        class: 'b-border-frozen-right',
      },
      {
        field: '',
        header: '',
        width: '3rem',
        type: ETypeDataTable.ACTION,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
        hideBtnSetColumn: true,
      },
    ];

    this.crudDTO.mapData(this.config.data.dataSource);
    this.crudDTO.detailInforItem.length && this.genListAction();
    this.eventOverviewService.eventDetailInforId = this.crudDTO.id;
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  private genListAction() {
    this.listAction = this.crudDTO.detailInforItem.map((data: EventDetailInforItem, index: number) => {
      const actions: IActionTable[] = [];

      actions.push({
        data: data,
        label: 'Thông tin chi tiết',
        icon: 'pi pi-info-circle',
        command: ($event) => {
          this.detail($event.item.data);
        },
      });

      // trạng thái !== HET_VE
      if (data.status !== EventOverview.HET_VE) {
        actions.push({
          data: data,
          label: 'Chỉnh sửa',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.edit($event.item.data);
          },
        });

        // trạng thái === KICH_HOAT
        if (data.status === EventOverview.KICH_HOAT) {
          actions.push({
            data: data,
            label: 'Hủy kích hoạt',
            icon: 'pi pi-times',
            command: ($event) => {
              this.deactive($event.item.data);
            },
          });
        }

        actions.push({
          data: data,
          label: !!data.isShowApp ? 'Tắt ShowApp' : 'Bật ShowApp',
          icon: 'pi pi-pencil',
          command: ($event) => {
            this.showApp($event.item.data, !!data.isShowApp);
          },
        });

        if (data.status === EventOverview.HUY_KICH_HOAT) {
          actions.push({
            data: data,
            label: 'Kích hoạt',
            icon: 'pi pi-times',
            command: ($event) => {
              this.active($event.item.data);
            },
          });
        }
      }
      //
      return actions;
    });
  }

  public detail(data: EventDetailInforItem) {
    if (data.id) {
      this.eventOverviewService.getTicketDetail(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CrudEventTicketInforComponent, {
            header: 'Xem chi tiết',
            width: '1200px',
            baseZIndex: 10000,
            data: {
              dataSource: res.data,
              type: EConfigDataModal.VIEW,
            } as IConfigDataModal,
          });
        }
      });
    }
  }

  public edit(data: EventDetailInforItem) {
    if (data.id) {
      this.eventOverviewService.getTicketDetail(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudEventTicketInforComponent, {
            header: 'Chỉnh sửa',
            width: '1200px',
            baseZIndex: 10000,
            data: {
              dataSource: res.data,
              type: EConfigDataModal.EDIT,
            } as IConfigDataModal,
          });
          ref.onClose.subscribe((response) => {
            if (this.handleResponseInterceptor(response, '')) {
              this.messageService.add({
                severity: 'success',
                summary: '',
                detail: 'Chỉnh sửa thành công',
                life: 1500,
              });
              this.reloadDetailInforItem();
            }
          });
        }
      });
    }
  }

  public showApp(data: EventDetailInforItem, isShowApp: boolean) {
    if (data) {
      const ref = this.dialogService.open(FormNotificationComponent, {
        header: 'Show App',
        width: '600px',
        contentStyle: {
          'max-height': '600px',
          overflow: 'auto',
          'padding-bottom': '50px',
        },
        styleClass: 'p-dialog-custom',
        baseZIndex: 10000,
        data: {
          title: `Xác nhận ${isShowApp ? 'Tắt ShowApp' : 'Bật ShowApp'}?`,
          icon: isShowApp ? FormNotificationConst.IMAGE_CLOSE : FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeShowAppTicket(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Thành công')) {
                this.reloadDetailInforItem();
              }
            },
            (err) => {
              this.messageError(`Thất bại`);
            }
          );
        }
      });
    }
  }

  public deactive(data: EventDetailInforItem) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn hủy kích hoạt?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeStatusTicket(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.reloadDetailInforItem();
              }
            },
            (err) => {
              this.messageError(`Không hủy kích hoạt được voucher`);
            }
          );
        }
      });
    }
  }

  public active(data: EventDetailInforItem) {
    if (data) {
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
          title: 'Bạn có chắc chắn muốn kích hoạt?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeStatusTicket(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.reloadDetailInforItem();
              }
            },
            (err) => {
              this.messageError(`Không hủy kích hoạt được voucher`);
            }
          );
        }
      });
    }
  }

  onSelectStartTime(event) {
    if(!this.crudDTO.endTime) {
      this.crudDTO.endTime = event;
    }
  }

  public create(event: any) {
    if (event) {
      const ref = this.dialogService.open(CrudEventTicketInforComponent, {
        header: 'Thông tin loại vé',
        width: '1400px',
        baseZIndex: 10000,
        data: {
          type: EConfigDataModal.CREATE,
        } as IConfigDataModal,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Thêm mới thành công',
            life: 1500,
          });
          this.reloadDetailInforItem();
        }
      });
    }
  }

  public replicate(event: any) {
    if (event) {
      const ref = this.dialogService.open(ReplicateEventTicketInforComponent, {
        header: 'Nhân bản thông tin loại vé',
        width: '1000px',
        baseZIndex: 10000,
      });
      ref.onClose.subscribe((response) => {
        if (this.handleResponseInterceptor(response, '')) {
          this.messageService.add({
            severity: 'success',
            summary: '',
            detail: 'Nhân bản thành công',
            life: 1500,
          });
          this.reloadDetailInforItem();
        }
      });
    }
  }

  private reloadDetailInforItem() {
    this.eventOverviewService
      .getEventDetailInfor(this.eventOverviewService.eventDetailInforId)
      .subscribe((res: any) => {
        if (this.handleResponseInterceptor(res, '')) {
          this.crudDTO.detailInforItem = this.crudDTO.mapDataDetailInforItem(res.data.tickets);
          this.genListAction();
          this.changeDetectorRef.detectChanges();
          this.changeDetectorRef.markForCheck();
        }
      });
  }

  public close(event: any) {
    if (this.type === EConfigDataModal.CREATE) {
      this.eventOverviewService.deleteEventDetailInfor(this.crudDTO.id).subscribe();
    }
    this.ref.close();
  }

  public save(event?: any) {
    if (event) {
      if (this.crudDTO.isValidData()) {
        this.eventOverviewService.updateEventDetailInfor(this.crudDTO.toObjectSendToAPI()).subscribe(
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

  public onChangeStartTime(event: any) {
    if (event) {
      if (
        this.crudDTO.startTime &&
        this.crudDTO.endTime &&
        compareDate(this.crudDTO.startTime, this.crudDTO.endTime, COMPARE_TYPE.GREATER)
      ) {
        this.crudDTO.endTime = new Date(this.crudDTO.startTime);
      }
    }
  }
}
