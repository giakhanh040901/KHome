import { Component, Injector } from '@angular/core';
import {
  EPositionFrozenCell,
  EPositionTextCell,
  ETypeDataTable,
  ETypeFormatDate,
  IndividualCustomer,
  SearchConst
} from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IActionTable, IConfigDataModal, IDropdown, IHeaderColumn } from '@shared/interface/InterfaceConst.interface';
import { IndividualCustomerDetailOfferModel } from '@shared/interface/customer-management/individual-customer/IndividualCustomer.model';
import { Page } from '@shared/model/page';
import { IndividualCustomerService } from '@shared/services/individual-customer-service';
import { VoucherManagementService } from '@shared/services/voucher-management-service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';
import { FormSetDisplayColumnComponent } from 'src/app/form-general/form-set-display-column/form-set-display-column/form-set-display-column.component';
import { IndividualVoucherDetailDialogComponent } from './individual-voucher-detail-dialog/individual-voucher-detail-dialog.component';

@Component({
  selector: 'individual-customer-detail-offer',
  templateUrl: './individual-customer-detail-offer.component.html',
  styleUrls: ['./individual-customer-detail-offer.component.scss'],
})
export class IndividualCustomerDetailOfferComponent extends CrudComponentBase {
  public dataSource: IndividualCustomerDetailOfferModel[] = [];
  public page: Page = new Page();
  public selectedColumns: IHeaderColumn[] = [];
  public headerColumns: IHeaderColumn[] = [];
  public filter: {
    keyword: string;
    voucherType: number | undefined;
    status: number | undefined;
  } = {
    keyword: '',
    voucherType: undefined,
    status: undefined,
  };
  public listAction: IActionTable[][] = [];

  constructor(
    injector: Injector,
    messageService: MessageService,
    private dialogService: DialogService,
    private individualCustomerService: IndividualCustomerService,
    private voucherManagementService: VoucherManagementService
  ) {
    super(injector, messageService);
  }

  public get listVoucherType() {
    return IndividualCustomer.listVoucherType;
  }

  public get listStatus() {
    return IndividualCustomer.listStatusVoucher;
  }

  public getStatusSeverity(code: any) {
    return IndividualCustomer.getStatusVoucher(code, 'severity');
  }

  public getStatusName(code: any) {
    return IndividualCustomer.getStatusVoucher(code, 'label');
  }

  ngOnInit() {
    this.headerColumns = [
      {
        field: 'voucherId',
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
        header: 'Tên voucher',
        width: '30rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'type',
        header: 'Loại hình',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'value',
        header: 'Giá trị',
        width: '12rem',
        isPin: true,
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'applyDate',
        header: 'Ngày áp dụng',
        width: '15rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'expiredDate',
        header: 'Ngày hết hạn',
        width: '15rem',
        type: ETypeDataTable.TEXT,
      },
      {
        field: 'createDate',
        header: 'Ngày cấp phát',
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
      {
        field: '',
        header: '',
        width: '3rem',
        type: ETypeDataTable.ACTION,
        posTextCell: EPositionTextCell.CENTER,
        isFrozen: true,
        posFrozen: EPositionFrozenCell.RIGHT,
      },
    ].map((item: IHeaderColumn, index: number) => {
      item.position = index + 1;
      return item;
    });
    this.selectedColumns = this.getLocalStorage(IndividualCustomer.keyStorageDetailOffer) ?? this.headerColumns;
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
  }

  public setColumn(event: any) {
    if (event) {
      const ref = this.dialogService.open(
        FormSetDisplayColumnComponent,
        this.getConfigDialogServiceDisplayTableColumn(this.headerColumns, this.selectedColumns)
      );
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.selectedColumns = dataCallBack.data.sort(function (a, b) {
            return a.position - b.position;
          });
          this.setLocalStorage(this.selectedColumns, IndividualCustomer.keyStorageDetailOffer);
        }
      });
    }
  }

  public funcStyleClassStatus = (status: any) => {
    return this.getStatusSeverity(status);
  };

  public funcLabelStatus = (status: any) => {
    return this.getStatusName(status);
  };

  public setPage(pageInfo?: any) {
    this.isLoading = true;
    this.page.pageNumber = pageInfo?.page ?? this.offset;
    if (pageInfo?.rows) this.page.pageSize = pageInfo?.rows;
    this.individualCustomerService.getListVoucherByCustomerId(this.page, this.filter).subscribe(
      (res) => {
        this.isLoading = false;
        if (this.handleResponseInterceptor(res, '')) {
          this.page.totalItems = res.data.totalItems;
          if (res.data?.items) {
            this.dataSource = res.data.items.map(
              (item: any) =>
                ({
                  id: item.conversionPointDetailId,
                  voucherId: item.voucherId,
                  name: item.name,
                  type: IndividualCustomer.listVoucherType.find((e: IDropdown) => e.value === item.voucherType)?.label,
                  value: this.formatCurrency(item.value),
                  applyDate: item.startDate ? formatDate(item.startDate, ETypeFormatDate.DATE) : '',
                  expiredDate: item.expiredDate ? formatDate(item.expiredDate, ETypeFormatDate.DATE) : '',
                  createDate: item.conversionPointFinishedDate ? formatDate(item.conversionPointFinishedDate, ETypeFormatDate.DATE_TIME) : '',
                  createUser: item.createdBy,
                  status: ((this.filter.status === IndividualCustomer.HET_HAN_VOUCHER || item.isExpired) && this.filter.status !== IndividualCustomer.HUY_YEU_CAU) 
                          ? IndividualCustomer.HET_HAN_VOUCHER
                          : item.conversionPointStatus,
                } as IndividualCustomerDetailOfferModel)
            );
            this.genListAction();
          }
        }
      },
      (err) => {
        this.isLoading = false;
      }
    );
  }

  public genListAction() {
    this.listAction = this.dataSource.map((data: IndividualCustomerDetailOfferModel, index: number) => {
      const actions: IActionTable[] = [];

      actions.push({
        data: data,
        label: 'Thông tin chi tiết',
        icon: 'pi pi-info-circle',
        command: ($event) => {
          this.detail($event.item.data);
        },
      });

      return actions;
    });
  }

  public detail(data: IndividualCustomerDetailOfferModel) {
    if (data.voucherId) {
      this.voucherManagementService.getVoucherCustomerById(data.id).subscribe((res: any) => {
        if (res) {
          this.dialogService.open(IndividualVoucherDetailDialogComponent, {
            header: 'Xem chi tiết',
            width: '1000px',
            baseZIndex: 10000,
            data: {
              dataSource: res.data,
            } as IConfigDataModal,
          });
        }
      });
    }
  }

  public changeFilter(event: any) {
    if (event) {
      this.setPage({ page: this.offset });
    }
  }
}
