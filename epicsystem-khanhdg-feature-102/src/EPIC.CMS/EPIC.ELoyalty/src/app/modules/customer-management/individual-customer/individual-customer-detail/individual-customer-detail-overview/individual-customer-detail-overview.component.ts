import { Component, Injector } from '@angular/core';
import { AccumulatePointManegement, ETypeDataTable, ETypeFormatDate, IndividualCustomer } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IDropdown, IHeaderColumn, IImage, IValueFormatter } from '@shared/interface/InterfaceConst.interface';
import {
  HistoryOfPoints,
  IndividualCustomerDetailAccumulateModel,
  IndividualCustomerDetailOverviewModel,
} from '@shared/interface/customer-management/individual-customer/IndividualCustomer.model';
import { AccumulatePointManagementService } from '@shared/services/accumulate-point-management-service';
import { IndividualCustomerService } from '@shared/services/individual-customer-service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'individual-customer-detail-overview',
  templateUrl: './individual-customer-detail-overview.component.html',
  styleUrls: ['./individual-customer-detail-overview.component.scss'],
})
export class IndividualCustomerDetailOverviewComponent extends CrudComponentBase {
  public faceImage: IImage = {
    src: 'assets/layout/images/avatar.png',
    width: '100%',
    height: '100%',
  };

  public overview: IndividualCustomerDetailOverviewModel = new IndividualCustomerDetailOverviewModel();
  public accumulate: IndividualCustomerDetailAccumulateModel = new IndividualCustomerDetailAccumulateModel();
  public filter: {
    type: number | undefined;
    reason: number | undefined;
  } = {
    type: undefined,
    reason: undefined,
  };
  public listReason: IDropdown[] = [];
  public listReasonOfType: IDropdown[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public dataSource: HistoryOfPoints[] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private individualCustomerService: IndividualCustomerService,
    private accumulatePointManagementService: AccumulatePointManagementService
  ) {
    super(injector, messageService);
  }

  public get listGender() {
    return IndividualCustomer.listGender;
  }
  public get listCardType() {
    return IndividualCustomer.listCardType;
  }

  public get listType() {
    return AccumulatePointManegement.listType;
  }

  ngOnInit() {
    this.getListReasonOfTpe();
    this.headerColumns = [
      {
        field: 'id',
        header: 'ID',
        type: ETypeDataTable.TEXT,
        width: '5rem',
      },
      {
        field: 'date',
        header: 'Ngày',
        type: ETypeDataTable.TEXT,
        width: '10rem',
        valueFormatter: (param: IValueFormatter) => (param.data ? formatDate(param.data, ETypeFormatDate.DATE) : ''),
      },
      {
        field: 'point',
        header: 'Số điểm',
        type: ETypeDataTable.TEXT,
        width: '10rem',
      },
      {
        field: 'type',
        header: 'Loại',
        type: ETypeDataTable.TEXT,
        width: '10rem',
        valueFormatter: (param: IValueFormatter) =>
          this.listType.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'reason',
        header: 'Lý do',
        type: ETypeDataTable.TEXT,
        width: 'auto',
        valueFormatter: (param: IValueFormatter) =>
          this.listReasonOfType.find((e: IDropdown) => e.value === param.data)?.label || '',
      },
      {
        field: 'settingUser',
        header: 'Người cài đặt',
        type: ETypeDataTable.TEXT,
        width: '10rem',
      },
    ] as IHeaderColumn[];
    this.setPage({ page: this.offset });
  }

  ngAfterViewInit() {
    this.individualCustomerService.getCustomerDetail().subscribe((res: any) => {
      if (res.data) {
        this.overview.mapDTO(res.data);
        this.accumulate.mapDTO(res.data);
      }
    });
  }

  public resetFilter(event: any) {
    if (event) {
      this.filter.type = undefined;
      this.filter.reason = undefined;
      this.listReason = [];
      this.setPage({ page: this.offset });
    }
  }

  public onChangeType(event: any) {
    if (event) {
      this.listReason = this.listReasonOfType.filter(
        (e: IDropdown) => e.rawData.type === this.filter.type || !e.rawData.type
      );
      this.filter.reason = undefined;
      this.setPage({ page: this.offset });
    }
  }

  public onChangeReason(event: any) {
    if (event) {
      this.setPage({ page: this.offset });
    }
  }

  public setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;

    this.individualCustomerService.getHistoryOfInvestor(this.page, this.filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.id,
                  date: item.createdDate,
                  point: item.point,
                  type: item.pointType,
                  reason: item.reason,
                  settingUser: item.createdBy,
                } as HistoryOfPoints)
            );
          }
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }

  private getListReasonOfTpe() {
    this.accumulatePointManagementService.getAllReasons().subscribe((res) => {
      if (this.handleResponseInterceptor(res, '')) {
        if (res.data && res.data.length) {
          this.listReasonOfType = res.data.map(
            (e: any) =>
              ({
                value: e.value,
                label: e.label,
                rawData: {
                  type: e.type,
                },
              } as IDropdown)
          );
        }
      }
    });
  }
}
