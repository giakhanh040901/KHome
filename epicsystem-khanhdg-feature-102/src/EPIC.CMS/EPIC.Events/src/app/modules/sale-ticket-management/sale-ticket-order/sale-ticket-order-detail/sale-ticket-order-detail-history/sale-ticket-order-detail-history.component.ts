import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  SaleTicketOrder,
} from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IActionButtonTable, IConstant, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { SaleTicketOrderDetailHistoryModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailHistory.model';
import { Page } from '@shared/model/page';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'sale-ticket-order-detail-history',
  templateUrl: './sale-ticket-order-detail-history.component.html',
  styleUrls: ['./sale-ticket-order-detail-history.component.scss'],
})
export class SaleTicketOrderDetailHistoryComponent extends CrudComponentBase {
  public headerColumns: IHeaderColumn[] = [];
  public dataSource: SaleTicketOrderDetailHistoryModel[] = [];
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
    private saleTicketOrderService: SaleTicketOrderService
  ) {
    super(injector, messageService);
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
        field: 'action',
        header: 'Hành động',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'changeData',
        header: 'Dữ liệu thay đổi',
        width: '15rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'oldData',
        header: 'Dữ liệu cũ',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'newData',
        header: 'Dữ liệu mới',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'description',
        header: 'Mô tả ',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'actionDate',
        header: 'Ngày thực hiện',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'actionUser',
        header: 'Người thực hiện',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
    ];
    this.setPage();
  }

  public changePage(event: any) {
    if (event) {
      this.setPage();
    }
  }

  public setPage() {
    this.isLoading = true;

    this.saleTicketOrderService.getSaleTicketOrderDetailHistory(this.page).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  action: SaleTicketOrder.listActionHistory.find((e: IConstant) => e.id === item.action)?.value || '',
                  changeData: item.fieldName,
                  oldData: item.oldValue,
                  newData: item.newValue,
                  description: item.summary,
                  actionDate: item.createdDate ? formatDate(item.createdDate, ETypeFormatDate.DATE_TIME) : '',
                  actionUser: item.createdBy,
                } as SaleTicketOrderDetailHistoryModel)
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
