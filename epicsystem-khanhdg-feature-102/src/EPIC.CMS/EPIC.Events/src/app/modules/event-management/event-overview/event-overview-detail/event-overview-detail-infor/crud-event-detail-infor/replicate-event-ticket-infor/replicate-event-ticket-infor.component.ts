import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { EPositionTextCell, ETypeDataTable, ETypeFormatDate, YES_NO } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IDropdown, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import {
  ReplicateTicketInfor,
  ReplicateTicketInforItem,
} from '@shared/interface/event-management/event-overview/EventOverviewDetailInfor.model';
import { Page } from '@shared/model/page';
import { EventOverviewService } from '@shared/services/event-overview.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'replicate-event-ticket-infor',
  templateUrl: './replicate-event-ticket-infor.component.html',
  styleUrls: ['./replicate-event-ticket-infor.component.scss'],
})
export class ReplicateEventTicketInforComponent extends CrudComponentBase {
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
  public replicateDTO: ReplicateTicketInfor = new ReplicateTicketInfor();
  public headerColumns: IHeaderColumn[] = [];
  public selectedData: ReplicateTicketInforItem[] = [];
  public listSelectTime: IDropdown[] = [];

  ngOnInit() {
    this.getListSelectTime(this.onChangeSelectTime.bind(this, 1));

    this.headerColumns = [
      {
        field: 'isSelected',
        header: '',
        width: '3rem',
        type: ETypeDataTable.SELECT_CHECK_BOX,
      },
      {
        field: 'id',
        header: '#ID',
        width: '3rem',
        type: ETypeDataTable.INDEX,
      },
      {
        field: 'name',
        header: 'Tên vé',
        width: 'auto',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'price',
        header: 'Giá vé',
        width: '8rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'total',
        header: 'Tổng số vé',
        width: '8rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'minimum',
        header: 'Mua tối thiểu',
        width: '10rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'maximum',
        header: 'Mua tối đa',
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
    ];
  }

  public onChangeSelectTime(event: any) {
    if (event) {
      if (this.replicateDTO.selectTime) {
        this.eventOverviewService.getEventDetailInfor(this.replicateDTO.selectTime).subscribe((res: any) => {
          this.replicateDTO.listTicket = [];
          if (this.handleResponseInterceptor(res, '')) {
            if (res.data.tickets && res.data.tickets.length) {
              this.replicateDTO.listTicket = res.data.tickets.map(
                (e: any) =>
                  ({
                    id: e.id,
                    isSelected: false,
                    name: e.name,
                    price: e.price,
                    total: e.quantity,
                    minimum: e.minBuy,
                    maximum: e.maxBuy,
                    isFree: e.isFree,
                    isShowApp: e.isShowApp === YES_NO.YES,
                  } as ReplicateTicketInforItem)
              );
            }
          }
        });
      }
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event?: any) {
    if (event) {
      if (this.replicateDTO.isValidData()) {
        const listTicketId = this.selectedData.map((e: ReplicateTicketInforItem) => e.id);
        this.eventOverviewService
          .replicateTicket({
            eventDetailId: this.eventOverviewService.eventDetailInforId,
            replicateTickets: listTicketId,
          })
          .subscribe(
            (response) => {
              if (this.handleResponseInterceptor(response, '')) {
                this.ref.close(response);
              }
            },
            (err) => {}
          );
      } else {
        this.messageDataValidator(this.replicateDTO.dataValidator);
      }
    }
  }

  private getListSelectTime(callBack: Function) {
    const newPage: Page = new Page();
    newPage.pageNumber = 0;
    newPage.pageSize = -1;
    this.eventOverviewService.findAllInfor(newPage).subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        if (res.data.items && res.data.items.length) {
          this.listSelectTime = res.data.items.map(
            (e: any) =>
              ({
                value: e.id,
                label: `${this.getDateTime(e.startDate)} - ${this.getDateTime(e.endDate)}`,
              } as IDropdown)
          );
          this.listSelectTime = this.listSelectTime.filter((e: IDropdown) => e.label !== ' - ');
          this.listSelectTime.length && (this.replicateDTO.selectTime = Number(this.listSelectTime[0].value));
          callBack();
        }
      }
    });
  }

  private getDateTime(data: string | undefined) {
    return data ? formatDate(data, ETypeFormatDate.DATE_TIME) : '';
  }
}
