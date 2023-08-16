import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { Router } from '@angular/router';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  PermissionLoyaltyConst,
  SearchConst,
  VoucherManagement,
} from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem, formatCurrency, formatDate } from '@shared/function-common';
import { IActionTable, IDropdown, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { ApplyVoucherHistoryModel } from '@shared/interface/voucher-management/apply-voucher-history/ApplyVoucherHistory.model';
import { Page } from '@shared/model/page';
import { VoucherManagementService } from '@shared/services/voucher-management-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { BreadcrumbService } from 'src/app/layout/breadcrumb/breadcrumb.service';

@Component({
  selector: 'apply-voucher-history',
  templateUrl: './apply-voucher-history.component.html',
  styleUrls: ['./apply-voucher-history.component.scss'],
})
export class ApplyVoucherHistoryComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private breadcrumbService: BreadcrumbService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private voucherManagementService: VoucherManagementService
  ) {
    super(injector, messageService);
    this.breadcrumbService.setItems([
      { label: 'Trang chủ', routerLink: ['/home'] },
      { label: 'Quản lý ưu đãi', routerLink: ['/home'] },
      { label: 'Lịch sử cấp phát' },
    ]);
  }

  public dataSource: ApplyVoucherHistoryModel[] = [];
  public isLoading: boolean;
  public page: Page;
  public listAction: IActionTable[][] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    kind: number | undefined;
    type: string | undefined;
    applyDate: any | undefined;
  } = {
    keyword: '',
    kind: undefined,
    type: undefined,
    applyDate: '',
  };
  public baseUrl: string = '';

  public get listKindOfVoucher() {
    return VoucherManagement.listKindOfVoucher;
  }

  public get listTypeOfVoucher() {
    return VoucherManagement.listTypeOfVoucher;
  }

  ngOnInit(): void {
    this.baseUrl = this.AppConsts.remoteServiceBaseUrl ?? this.baseUrl;
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
        field: 'code',
        header: 'Mã lô',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'name',
        header: 'Tên voucher',
        width: 'auto',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'kind',
        header: 'Loại hình',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại voucher',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'value',
        header: 'Giá trị',
        width: '10rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'customer',
        header: 'Khách hàng',
        width: '15rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'applyDate',
        header: 'Ngày cấp phát',
        width: '15rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'expiredDate',
        header: 'Ngày hết hạn',
        width: '15rem',
        type: ETypeDataTable.TEXT,
      },
    ].map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.setPage({ page: this.offset });
  }

  ngAfterViewInit() {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.filter.keyword === '') {
        this.setPage({ page: this.offset });
      } else {
        this.setPage();
      }
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public changeFilter(event) {
    this.setPage({ page: this.offset });
  }

  public setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.page.keyword = this.filter.keyword;
    let filter = { ...this.filter };
    filter.applyDate && (filter.applyDate = formatCalendarItem(filter.applyDate));

    this.voucherManagementService.voucherHistory(this.page, filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.voucherId,
                  code: item.code,
                  name: item.name,
                  kind: this.listKindOfVoucher.find((e: IDropdown) => e.value === item.voucherType)?.label || '',
                  type: this.listTypeOfVoucher.find((e: IDropdown) => e.value === item.useType)?.label || '',
                  value: item.value
                    ? ((value: number, valueUnit: string) => {
                        return `${formatCurrency(value)} ${
                          VoucherManagement.listValueUnit.find((e: IDropdown) => e.value === valueUnit)?.label || ''
                        }`;
                      })(item.value, item.unit)
                    : 0,
                  customer: item.customer,
                  applyDate: item.conversionPointFinishedDate
                    ? formatDate(item.conversionPointFinishedDate, ETypeFormatDate.DATE)
                    : '',
                  expiredDate: item.expiredDate ? formatDate(item.expiredDate, ETypeFormatDate.DATE) : '',
                } as ApplyVoucherHistoryModel)
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
