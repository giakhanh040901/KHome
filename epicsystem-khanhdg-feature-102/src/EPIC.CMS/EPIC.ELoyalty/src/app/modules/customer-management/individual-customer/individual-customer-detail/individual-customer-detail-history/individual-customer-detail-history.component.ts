import { Component, Injector } from '@angular/core';
import { EPositionFrozenCell, EPositionTextCell, ETypeDataTable } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem } from '@shared/function-common';
import { IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { IndividualCustomerDetailHistory } from '@shared/interface/customer-management/individual-customer/IndividualCustomer.model';
import { Page } from '@shared/model/page';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'individual-customer-detail-history',
  templateUrl: './individual-customer-detail-history.component.html',
  styleUrls: ['./individual-customer-detail-history.component.scss'],
})
export class IndividualCustomerDetailHistoryComponent extends CrudComponentBase {
  public dataSource: IndividualCustomerDetailHistory[] = [];
  public page: Page = new Page();
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    program: number | undefined;
    status: number | undefined;
    date: any | undefined;
  } = {
    program: undefined,
    status: undefined,
    date: '',
  };

  constructor(
    injector: Injector, 
    messageService: MessageService, 
  ) {
    super(injector, messageService);
  }

  public get listProgram() {
    return [];
  }

  public get listStatus() {
    return [];
  }

  public getStatusSeverity(code: any) {
    // return VoucherManagement.getStatus(code, 'severity');
  }

  public getStatusName(code: any) {
    // return VoucherManagement.getStatus(code, 'label');
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
        field: 'customer',
        header: 'Tên Khách hàng',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'program',
        header: 'Tên chương trình',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'prize',
        header: 'Quà tặng tham gia',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'date',
        header: 'Thời gian tham gia',
        width: '15rem',
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
    ].map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.setPage({ page: this.offset });
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  public setPage(pageInfo?: any) {
    // this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    let filter = { ...this.filter };
    filter.date && (filter.date = formatCalendarItem(filter.date));

    // this.individualCustomerService.getListVoucherByCustomerId(this.page, filter).subscribe(
    //   (res) => {
    //     this.isLoading = false;
    //     if (this.handleResponseInterceptor(res, '')) {
    //       this.page.totalItems = res.data.totalItems;
    //       if (res.data?.items) {
    //         this.dataSource = res.data.items.map(
    //           (item: any) =>
    //             ({
    //             } as IndividualCustomerDetailHistory)
    //         );
    //       }
    //     }
    //   },
    //   (err) => {
    //     this.isLoading = false;
    //   }
    // );

    this.dataSource = [
      {
        id: 1111,
        customer: '1111',
        program: '1111',
        prize: '1111',
        date: '1111',
        status: 1111,
      },
      {
        id: 2222,
        customer: '2222',
        program: '2222',
        prize: '2222',
        date: '2222',
        status: 2222,
      },
      {
        id: 3333,
        customer: '3333',
        program: '3333',
        prize: '3333',
        date: '3333',
        status: 3333,
      },
      {
        id: 4444,
        customer: '4444',
        program: '4444',
        prize: '4444',
        date: '4444',
        status: 4444,
      },
    ] as IndividualCustomerDetailHistory[];
  }

  public changeFilter(event: any) {
    if (event) {
      this.setPage({ page: this.offset });
    }
  }
}
