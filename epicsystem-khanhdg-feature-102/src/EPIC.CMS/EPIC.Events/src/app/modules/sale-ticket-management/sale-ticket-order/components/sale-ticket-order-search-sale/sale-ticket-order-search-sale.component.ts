import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { ETypeDataTable, FormNotificationConst, SearchConst } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import { IActionButtonTable, IHeaderColumn, INotiDataModal } from '@shared/interface/InterfaceConst.interface';
import { IntroduceSearchModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'sale-ticket-order-search-sale',
  templateUrl: './sale-ticket-order-search-sale.component.html',
  styleUrls: ['./sale-ticket-order-search-sale.component.scss'],
})
export class SaleTicketOrderSearchSaleComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private dialogService: DialogService,
    public saleTicketOrderService: SaleTicketOrderService,
    private routerService: RouterService,
    private config: DynamicDialogConfig,
    private ref: DynamicDialogRef
  ) {
    super(injector, messageService);
  }

  public headerColumns: IHeaderColumn[] = [];
  public dataSource: IntroduceSearchModel[] = [];
  public listButtonTable: IActionButtonTable[] = [];
  public selectedData: IntroduceSearchModel = new IntroduceSearchModel();

  ngOnInit() {
    this.headerColumns = [
      { field: 'name', header: 'Tên', type: ETypeDataTable.TEXT, width: 'auto' },
      { field: 'phone', header: 'Số điện thoại', type: ETypeDataTable.TEXT, width: '10rem' },
      { field: 'email', header: 'Email', type: ETypeDataTable.TEXT, width: 'auto' },
      { field: 'transaction', header: 'Phòng giao dịch', type: ETypeDataTable.TEXT, width: 'auto' },
      { field: 'action', header: '', type: ETypeDataTable.ACTION_BUTTON, width: '10rem' },
    ] as IHeaderColumn[];
    this.listButtonTable = [
      {
        label: 'Chọn',
        icon: 'pi pi-check',
        showFunction: this.showBtnSelect,
        callBack: this.select,
        styleClassButton: 'p-button',
      },
      {
        label: 'Xóa',
        icon: 'pi pi-times',
        showFunction: this.showBtnRemove,
        callBack: this.remove,
        styleClassButton: 'p-button p-button-danger',
      },
    ];
    if (this.config.data.dataSource) {
      this.keyword = this.config.data.dataSource.code;
      this.selectedData = Object.assign(this.selectedData, this.config.data.dataSource);
      this.getData();
    }
  }

  ngAfterViewInit() {
    this.subject.keyword.pipe(debounceTime(SearchConst.DEBOUNCE_TIME)).subscribe(() => {
      if (this.keyword.trim()) {
        this.getData();
      } else {
        this.dataSource = [];
      }
    });
    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public showBtnSelect = (data: IntroduceSearchModel, action: IActionButtonTable, index: number) => {
    return !this.selectedData.code;
  };

  public showBtnRemove = (data: IntroduceSearchModel, action: IActionButtonTable, index: number) => {
    return !!this.selectedData.code;
  };

  public select = (row: IntroduceSearchModel, index: number) => {
    if (row) {
      this.selectedData = Object.assign(this.selectedData, row);
    }
  };

  public remove = (row: IntroduceSearchModel, index: number) => {
    if (row) {
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
          title: 'Bạn có chắc chắn muốn bỏ chọn mã giới thiệu?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.selectedData = new IntroduceSearchModel();
        }
      });
    }
  };

  public onClickSearch(event: any) {
    if (event) {
      this.getData();
    }
  }

  private getData() {
    this.saleTicketOrderService.getDataIntroduceSearch(this.keyword).subscribe((res: any) => {
      if (this.handleResponseInterceptor(res, '') && res.data) {
        const { data } = res;
        this.dataSource = [
          {
            id: data.saleId,
            code: data.referralCode,
            name: data.fullname || '',
            phone: data.investor.phone || '',
            email: data.investor.email || '',
            transaction: data.departmentName || '',
            contractTransaction: '',
          } as IntroduceSearchModel,
        ];
        this.selectedData = this.dataSource[0];
      }
    });
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event: any) {
    if (event) {
      this.ref.close(this.selectedData);
    }
  }
}
