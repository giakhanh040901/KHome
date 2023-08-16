import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import { EPositionFrozenCell, EPositionTextCell, ETypeDataTable, ETypeFormatDate, SaleTicketOrder } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IActionButtonTable, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { SaleTicketOrderDetailListModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailList.model';
import { Page } from '@shared/model/page';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { SignalrService } from '@shared/services/signalr.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'sale-ticket-order-detail-list',
  templateUrl: './sale-ticket-order-detail-list.component.html',
  styleUrls: ['./sale-ticket-order-detail-list.component.scss'],
})
export class SaleTicketOrderDetailListComponent extends CrudComponentBase {
  public headerColumns: IHeaderColumn[] = [];
  public dataSource: SaleTicketOrderDetailListModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listButtonTable: IActionButtonTable[] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private saleTicketOrderService: SaleTicketOrderService,
    private _signalrService: SignalrService,
  ) {
    super(injector, messageService);
  }

  ngOnInit() {
    this.listButtonTable = [
      {
        label: '',
        icon: 'pi pi-download',
        showFunction: this.showBtnDownload,
        callBack: this.download,
        styleClassButton: 'p-button',
      },
      {
        label: '',
        icon: 'pi pi-eye',
        showFunction: this.showBtnView,
        callBack: this.view,
        styleClassButton: 'p-button',
      },
    ];
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
        field: 'name',
        header: 'Loại vé',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'code',
        header: 'Mã vé',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'checkin',
        header: 'Checkin',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'checkout',
        header: 'Checkout',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'isUsed',
        header: 'Sử dụng',
        width: '8rem',
        type: ETypeDataTable.CHECK_BOX,
      },
      {
        field: 'actionDownload',
        header: 'Tải về',
        type: ETypeDataTable.ACTION_BUTTON,
        width: '8rem',
      },
      {
        field: 'actionView',
        header: 'Xem vé',
        type: ETypeDataTable.ACTION_BUTTON,
        width: '8rem',
      },
    ];
    this.setPage();
    this.startSignalR();
  }

  startSignalR() {
    this._signalrService.startConnection()
      .then(() => {
        const updatedDataSource = this.dataSource.map(item => ({ ...item }));
        this._signalrService.listen('UpdateOrderTickeDetail', (data) => {
          const index = this.dataSource.findIndex(e => e.id === data?.id);
          if (index !== -1) {
            updatedDataSource[index].checkin = data.checkIn;
            updatedDataSource[index].checkout = data.checkOut;
            updatedDataSource[index].isUsed = data.status === SaleTicketOrder.orderTicketStatus.DA_THAM_GIA;
            this.dataSource = updatedDataSource;
          }
        });
       
      })
      .catch((error) => {
        console.log("Error while connecting to SignalR:", error);
      });
  }

  public showBtnDownload = (data: any, action: IActionButtonTable, index: number, col: IHeaderColumn) => {
    return col.field === 'actionDownload';
  };

  public showBtnView = (data: any, action: IActionButtonTable, index: number, col: IHeaderColumn) => {
    return col.field === 'actionView';
  };

  public download = (row: SaleTicketOrderDetailListModel, index: number) => {
    if (row) {
      if (row.ticketUrl && row.ticketUrl.length) {
        const url = this.saleTicketOrderService.getBaseApiUrl() + '/' + row.ticketUrl;
        window.open(url, '_blank');
      } else {
        this.messageWarn('Vé không tổn tại!');
      }
    }
  };

  public view = (row: SaleTicketOrderDetailListModel, index: number) => {
    if (row) {
      if (row.ticketUrl && row.ticketUrl.length) {
        const url = `/${row.ticketUrl}&download=true`;
        this.saleTicketOrderService.viewFilePDF(url).subscribe();
      } else {
        this.messageWarn('Vé không tổn tại!');
      }
    }
  };
  
  public changePage(event: any) {
    if (event) {
      this.setPage();
    }
  }

  public setPage() {
    this.isLoading = true;

    this.saleTicketOrderService.getDataSaleOrderDetailList(this.page).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  name: item.name,
                  code: item.ticketCode,
                  checkin: formatDate(item.checkIn, ETypeFormatDate.DATE_TIME),
                  checkout: formatDate(item.checkOut, ETypeFormatDate.DATE_TIME),
                  ticketUrl: item.ticketFilledUrl,
                  isUsed: item.status === SaleTicketOrder.orderTicketStatus.DA_THAM_GIA,
                } as SaleTicketOrderDetailListModel)
            );
          }
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }
}
