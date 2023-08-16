import { ChangeDetectorRef, Component, Injector, ViewChild } from '@angular/core';
import { ETypeDataTable, FormNotificationConst, SaleTicketOrder } from '@shared/AppConsts';
import { FormNotificationComponent } from '@shared/components/form-notification/form-notification.component';
import { FormSearchDataComponent } from '@shared/components/form-search-data/form-search-data.component';
import { CrudComponentBase } from '@shared/crud-component-base';
import {
  IActionButtonTable,
  IDropdown,
  IHeaderColumn,
  INotiDataModal,
  ITabView,
} from '@shared/interface/InterfaceConst.interface';
import {
  CustomerSearchModel,
  IntroduceSearchModel,
} from '@shared/interface/sale-ticket-management/sale-ticket-order/CreateSaleTicketOrder.model';
import { RouterService } from '@shared/services/router.service';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';

@Component({
  selector: 'create-sale-ticket-order-customer',
  templateUrl: './create-sale-ticket-order-customer.component.html',
  styleUrls: ['./create-sale-ticket-order-customer.component.scss'],
})
export class CreateSaleTicketOrderCustomerComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private changeDetectorRef: ChangeDetectorRef,
    private dialogService: DialogService,
    public saleTicketOrderService: SaleTicketOrderService,
    private routerService: RouterService
  ) {
    super(injector, messageService);
  }
  public listTabPanelCustomer: ITabView[] = [];
  public headerColumnsCustomer: IHeaderColumn[][] = [];
  public listButtonTableCustomer: IActionButtonTable[] = [];
  public apiGetDataCustomer: Function[];
  public apiGetResponseCustomer: string[];
  public apiGetMapDataCustomer: Function[];
  public tabCustomer: number = this.CUSTOMER;

  public listTabPanelIntroduce: ITabView[] = [];
  public headerColumnsIntroduce: IHeaderColumn[][] = [];
  public listButtonTableIntroduce: IActionButtonTable[] = [];
  public apiGetDataIntroduce: Function[];
  public apiGetResponseIntroduce: string[];
  public apiGetMapDataIntroduce: Function[];
  public tabIntroduce: number = 0;

  @ViewChild('formSearchDataCustomer', { static: false })
  formSearchDataCustomer: FormSearchDataComponent;
  @ViewChild('formSearchDataIntroduce', { static: false })
  formSearchDataIntroduce: FormSearchDataComponent;

  public get CUSTOMER() {
    return SaleTicketOrder.CUSTOMER;
  }

  public get BUSINESS() {
    return SaleTicketOrder.BUSINESS;
  }

  public listAddress: IDropdown[] = [];

  ngOnInit() {
    this.listTabPanelCustomer = [
      {
        title: 'Khách hàng cá nhân',
      },
      {
        title: 'Khách hàng doanh nghiệp',
        isDisabled: true,
      },
    ] as ITabView[];

    this.headerColumnsCustomer = [
      [
        { field: 'name', header: 'Tên', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'phone', header: 'Số điện thoại', type: ETypeDataTable.TEXT, width: '10rem' },
        { field: 'idNo', header: 'Số giấy tờ', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'email', header: 'Email', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'address', header: 'Địa chỉ', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'action', header: '', type: ETypeDataTable.ACTION_BUTTON, width: '10rem' },
      ],
      [
        { field: 'name', header: 'Tên', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'abbreviation', header: 'Tên viết tắt', type: ETypeDataTable.TEXT, width: '10rem' },
        { field: 'taxCode', header: 'Mã số thuế', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'email', header: 'Email', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'address', header: 'Địa chỉ', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'action', header: '', type: ETypeDataTable.ACTION_BUTTON, width: '10rem' },
      ],
    ] as IHeaderColumn[][];

    this.listButtonTableCustomer = [
      {
        label: 'Chọn',
        icon: 'pi pi-check',
        showFunction: this.showBtnSelectCustomer,
        callBack: this.selectCustomer,
        styleClassButton: 'p-button',
      },
      {
        label: 'Xóa',
        icon: 'pi pi-times',
        showFunction: this.showBtnRemoveCustomer,
        callBack: this.removeCustomer,
        styleClassButton: 'p-button p-button-danger',
      },
    ];

    this.apiGetDataCustomer = [
      this.saleTicketOrderService.getDataCustomerSearch.bind(this.saleTicketOrderService),
      this.saleTicketOrderService.getDataBusinessSearch.bind(this.saleTicketOrderService),
    ];

    this.apiGetResponseCustomer = ['data', 'data'];

    this.apiGetMapDataCustomer = [
      (data: any) => {
        return data.items.map(
          (e: any) =>
            ({
              id: e.investorId,
              idenId: e.defaultIdentification.id,
              name: e.name || e.defaultIdentification.fullname || '',
              phone: e.phone || '',
              idNo: e.cifCode || '',
              email: e.email || '',
              type: this.CUSTOMER,
            } as CustomerSearchModel)
        );
      },
      (data: any) => {
        return data.items.map(
          (e: any) =>
            ({
              id: e.businessCustomerId,
              name: e.name || '',
              abbreviation: e.shortName || '',
              taxCode: e.taxCode || '',
              email: e.email || '',
              type: this.BUSINESS,
            } as CustomerSearchModel)
        );
      },
    ];

    this.listTabPanelIntroduce = [
      {
        title: 'Mã giới thiệu',
      },
    ] as ITabView[];

    this.headerColumnsIntroduce = [
      [
        { field: 'name', header: 'Tên', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'phone', header: 'Số điện thoại', type: ETypeDataTable.TEXT, width: '10rem' },
        { field: 'email', header: 'Email', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'transaction', header: 'Phòng giao dịch', type: ETypeDataTable.TEXT, width: 'auto' },
        { field: 'action', header: '', type: ETypeDataTable.ACTION_BUTTON, width: '10rem' },
      ],
    ] as IHeaderColumn[][];

    this.listButtonTableIntroduce = [
      {
        label: 'Chọn',
        icon: 'pi pi-check',
        showFunction: this.showBtnSelectIntroduce,
        callBack: this.selectIntroduce,
        styleClassButton: 'p-button',
      },
      {
        label: 'Xóa',
        icon: 'pi pi-times',
        showFunction: this.showBtnRemoveIntroduce,
        callBack: this.removeIntroduce,
        styleClassButton: 'p-button p-button-danger',
      },
    ];

    this.apiGetDataIntroduce = [this.saleTicketOrderService.getDataIntroduceSearch.bind(this.saleTicketOrderService)];

    this.apiGetResponseIntroduce = ['data'];

    this.apiGetMapDataIntroduce = [
      (data: any) => {
        return [
          {
            id: data.saleId,
            code: data.referralCode,
            name: data.fullname || '',
            phone: data.investor.phone || '',
            email: data.investor.email || '',
            transaction: data.departmentName || '',
          } as IntroduceSearchModel,
        ];
      },
    ];
  }

  ngAfterViewInit() {
    //if back button
    if (this.saleTicketOrderService.selectedOrder.customer.id) {
      this.formSearchDataCustomer.keyword = this.saleTicketOrderService.selectedOrder.customer.phone;
      this.formSearchDataCustomer.changeKeyword();
    }
    if (this.saleTicketOrderService.selectedOrder.introduce.id) {
      this.formSearchDataIntroduce.keyword = this.saleTicketOrderService.selectedOrder.introduce.phone;
      this.formSearchDataIntroduce.changeKeyword();
    }

    if (this.saleTicketOrderService.dtoCustomer.id) this.saleTicketOrderService.getListAddressOfCustomer();

    this.saleTicketOrderService._listAddressOfCustomer$.subscribe((res: IDropdown[] | undefined) => {
      this.listAddress = res || [];
    });

    this.changeDetectorRef.detectChanges();
    this.changeDetectorRef.markForCheck();
  }

  public showBtnSelectCustomer = (data: CustomerSearchModel, action: IActionButtonTable, index: number) => {
    return !this.saleTicketOrderService.dtoCustomer.id;
  };

  public showBtnRemoveCustomer = (data: CustomerSearchModel, action: IActionButtonTable, index: number) => {
    return !!this.saleTicketOrderService.dtoCustomer.id;
  };

  public selectCustomer = (row: CustomerSearchModel, index: number) => {
    if (row) {
      this.saleTicketOrderService.dtoCustomer = Object.assign(this.saleTicketOrderService.dtoCustomer, row);
      this.listAddress = [];
      this.saleTicketOrderService.dtoCustomer.addressId = undefined;
      this.saleTicketOrderService.dtoCustomer.addressName = '';
      this.saleTicketOrderService.getListAddressOfCustomer();
    }
  };

  public removeCustomer = (row: IntroduceSearchModel, index: number) => {
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
          title: 'Bạn có chắc chắn muốn bỏ chọn khách hàng?',
          icon: FormNotificationConst.IMAGE_CLOSE,
        } as INotiDataModal,
      });
      ref.onClose.subscribe((dataCallBack) => {
        if (dataCallBack?.accept) {
          this.saleTicketOrderService.dtoCustomer = new CustomerSearchModel();
        }
      });
    }
  };

  public showBtnSelectIntroduce = (data: IntroduceSearchModel, action: IActionButtonTable, index: number) => {
    return !this.saleTicketOrderService.dtoIntroduce.id;
  };

  public showBtnRemoveIntroduce = (data: IntroduceSearchModel, action: IActionButtonTable, index: number) => {
    return !!this.saleTicketOrderService.dtoIntroduce.id;
  };

  public selectIntroduce = (row: IntroduceSearchModel, index: number) => {
    if (row) {
      this.saleTicketOrderService.dtoIntroduce = Object.assign(this.saleTicketOrderService.dtoIntroduce, row);
    }
  };

  public removeIntroduce = (row: IntroduceSearchModel, index: number) => {
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
          this.saleTicketOrderService.dtoIntroduce = new IntroduceSearchModel();
        }
      });
    }
  };

  public onChangeTabView(event: any) {
    if (event) {
      this.saleTicketOrderService.dtoCustomer = new CustomerSearchModel();
      this.saleTicketOrderService.dtoCustomer.type = event.index;
      this.tabCustomer = event.index;
    }
  }

  public continue(event: any) {
    if (event) {
      if (this.saleTicketOrderService.dtoCustomer.id) {
        this.saleTicketOrderService.selectedOrder.customer = {
          ...this.saleTicketOrderService.dtoCustomer,
        } as CustomerSearchModel;
        this.saleTicketOrderService.selectedOrder.introduce = {
          ...this.saleTicketOrderService.dtoIntroduce,
        } as IntroduceSearchModel;
        this.routerService.routerNavigate(['/sale-ticket-management/sale-ticket-order/create/event']);
      } else {
        this.messageError('Vui lòng chọn Khách hàng!');
      }
    }
  }

  public onChangeAddress(event: any) {
    if (event) {
      this.saleTicketOrderService.dtoCustomer.addressName =
        this.listAddress.find((e: IDropdown) => e.value === this.saleTicketOrderService.dtoCustomer.addressId)?.label ||
        '';
    }
  }
}
