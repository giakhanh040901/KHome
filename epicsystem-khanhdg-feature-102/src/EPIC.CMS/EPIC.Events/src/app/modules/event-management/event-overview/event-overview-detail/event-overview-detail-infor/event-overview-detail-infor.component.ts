import { Component, Injector } from '@angular/core';
import {
  EConfigDataModal,
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  ETypeStatus,
  EventOverview,
  FormNotificationConst,
} from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import {
  IActionTable,
  IConfigDataModal,
  IHeaderColumn,
  INotiDataModal
} from '@shared/interface/InterfaceConst.interface';
import { EventOverviewDetailInforModel } from '@shared/interface/event-management/event-overview/EventOverviewDetailInfor.model';
import { Page } from '@shared/model/page';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { CrudEventDetailInforComponent } from './crud-event-detail-infor/crud-event-detail-infor.component';

@Component({
  selector: 'event-overview-detail-infor',
  templateUrl: './event-overview-detail-infor.component.html',
  styleUrls: ['./event-overview-detail-infor.component.scss'],
})
export class EventOverviewDetailInforComponent extends CrudComponentBase {
  public dataSource: EventOverviewDetailInforModel[] = [];
  public page: Page = new Page();
  public headerColumns: IHeaderColumn[] = [];
  public listAction: IActionTable[][] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private eventOverviewService: EventOverviewService
  ) {
    super(injector, messageService);
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

  ngOnInit() {
    this.headerColumns = [
      {
        field: 'id',
        header: '#ID',
        width: '5rem',
        isPin: true,
        type: ETypeDataTable.INDEX,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.LEFT,
      },
      {
        field: 'startDate',
        header: 'Ngày bắt đầu',
        isPin: true,
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'endDate',
        header: 'Ngày kết thúc',
        isPin: true,
        width: '12rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'typeTicketNum',
        header: 'Số loại vé',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'ticketNum',
        header: 'Số lượng vé',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'registerNum',
        header: 'Đăng ký',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'payNum',
        header: 'Thanh toán',
        width: '8rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'settingUser',
        header: 'Người cài đặt',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'status',
        header: 'Trạng thái',
        width: '8rem',
        type: ETypeDataTable.STATUS,
        funcStyleClassStatus: this.funcStyleClassStatus,
        funcLabelStatus: this.funcLabelStatus,
        posTextCell: EPositionTextCell.CENTER,
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
    ].map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.setPage();
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  private genListAction() {
    this.listAction = this.dataSource.map((data: EventOverviewDetailInforModel, index: number) => {
      const actions: IActionTable[] = [];
    
      if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_ChiTiet)) {
        actions.push({
          data: data,
          label: 'Thông tin chi tiết',
          icon: 'pi pi-info-circle',
          command: ($event) => {
            this.detail($event.item.data);
          },
        });
      }
      // trạng thái !== HET_VE
      if (data.status !== EventOverview.HET_VE) {
        if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_CapNhat)) { 
          actions.push({
            data: data,
            label: 'Chỉnh sửa',
            icon: 'pi pi-pencil',
            command: ($event) => {
              this.edit($event.item.data);
            },
          });
        }
        // trạng thái !== KICH_HOAT
        if(this.isGranted(this.PermissionEventConst.TongQuanSuKien_ChiTiet_ThoiGianVaLoaiVe_DoiTrangThai)) { 
          if (data.status !== EventOverview.KICH_HOAT) {
            actions.push({
              data: data,
              label: 'Kích hoạt',
              icon: 'pi pi-pencil',
              command: ($event) => {
                this.active($event.item.data);
              },
            });
          }
          // trạng thái !== HUY_KICH_HOAT
          if (data.status !== EventOverview.HUY_KICH_HOAT) {
            actions.push({
              data: data,
              label: 'Hủy kích hoạt',
              icon: 'pi pi-times',
              command: ($event) => {
                this.deactive($event.item.data);
              },
            });
          }
        }
      }
      //
      return actions;
    });
  }

  public detail(data: EventOverviewDetailInforModel) {
    if (data.id) {
      this.eventOverviewService.getEventDetailInfor(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(CrudEventDetailInforComponent, {
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

  public edit(data: EventOverviewDetailInforModel) {
    if (data.id) {
      this.eventOverviewService.getEventDetailInfor(data.id).subscribe((res: any) => {
        if (res) {
          const ref = this.dialogService.open(CrudEventDetailInforComponent, {
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
              this.setPage();
            }
          });
        }
      });
    }
  }

  public active(data: EventOverviewDetailInforModel) {
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
          title: 'Bạn có chắc chắn muốn kích hoạt khung giờ tổ chức sự kiện?',
          icon: FormNotificationConst.IMAGE_APPROVE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeStatusEventDetailInfor(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Kích hoạt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Kích hoạt không thành công`);
            }
          );
        }
      });
    }
  }

  public deactive(data: EventOverviewDetailInforModel) {
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
          title: 'Bạn có chắc chắn muốn hủy kích hoạt khung giờ tổ chức sự kiện?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.eventOverviewService.changeStatusEventDetailInfor(data.id).subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, 'Hủy kích hoạt thành công')) {
                this.setPage();
              }
            },
            (err) => {
              this.messageError(`Hủy kích hoạt không thành công`);
            }
          );
        }
      });
    }
  }

  public create(event: any) {
    if (event) {
      this.eventOverviewService.createEventDetailInfor().subscribe((res) => {
        if (this.handleResponseInterceptor(res, '')) {
          if (res.data.id) {
            const ref = this.dialogService.open(CrudEventDetailInforComponent, {
              header: 'Thêm mới',
              width: '1200px',
              baseZIndex: 10000,
              data: {
                dataSource: res.data,
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
                this.setPage();
              }
            });
          }
        }
      });
    }
  }

  public changePage(event: any) {
    if (event) {
      this.setPage();
    }
  }

  public setPage() {
    this.isLoading = true;

    this.eventOverviewService.findAllInfor(this.page).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  startDate: item.startDate ? formatDate(item.startDate, ETypeFormatDate.DATE_TIME) : '',
                  endDate: item.endDate ? formatDate(item.endDate, ETypeFormatDate.DATE_TIME) : '',
                  typeTicketNum: item.ticketTypeQuantity,
                  ticketNum: item.ticketQuantity,
                  registerNum: item.registerQuantity,
                  payNum: item.payQuantity,
                  settingUser: item.createdBy,
                  status: item.status,
                } as EventOverviewDetailInforModel)
            );
            this.genListAction();
          }
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );

    this.genListAction();
  }
}
