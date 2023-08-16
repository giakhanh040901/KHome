import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { EConfigDataModal, ETypeFormatDate, SaleTicketOrder } from '@shared/AppConsts';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatDate } from '@shared/function-common';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { CrudSaleTicketTransaction } from '@shared/interface/sale-ticket-management/sale-ticket-order/SaleTicketOrderDetailTransaction.model';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

@Component({
  selector: 'crud-sale-ticket-transaction',
  templateUrl: './crud-sale-ticket-transaction.component.html',
  styleUrls: ['./crud-sale-ticket-transaction.component.scss'],
})
export class CrudSaleTicketTransactionComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    private ref: DynamicDialogRef,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig,
    private dialogService: DialogService,
    private saleTicketOrderService: SaleTicketOrderService
  ) {
    super(injector, messageService);
  }
  public crudDTO: CrudSaleTicketTransaction = new CrudSaleTicketTransaction();
  public type: string = EConfigDataModal.CREATE;
  public listAccount: IDropdown[] = [];

  public get isDisable() {
    return this.type === EConfigDataModal.VIEW;
  }

  public get listRevenueExpenditure() {
    return SaleTicketOrder.listRevenueExpenditure;
  }

  public get listTradingType() {
    return SaleTicketOrder.listTradingTypeTransaction;
  }

  public get listTransactionType() {
    return SaleTicketOrder.listTransactionType;
  }

  public get CHUYEN_KHOAN() {
    return SaleTicketOrder.CHUYEN_KHOAN;
  }

  ngOnInit() {
    if (this.config.data) {
      this.type = this.config.data.type;
    }
    if (this.type === EConfigDataModal.CREATE) {
      this.crudDTO.orderId = this.saleTicketOrderService.selectedOrderId;
      this.crudDTO.tradingDate = new Date();
      this.crudDTO.tradingKind = SaleTicketOrder.THU;
      this.crudDTO.tradingType = SaleTicketOrder.THANH_TOAN_MUA_VE;
      this.crudDTO.transactionType = SaleTicketOrder.CHUYEN_KHOAN;
      this.crudDTO.value = this.config.data.dataSource.value;
      this.crudDTO.description = this.config.data.dataSource.description;
      this.crudDTO.createUser = this.saleTicketOrderService.currentUser;
      this.crudDTO.createDate = formatDate(new Date(), ETypeFormatDate.DATE_TIME);
    } else {
      this.crudDTO.mapData(this.config.data.dataSource);
    }
  }

  ngAfterViewInit() {
    this.saleTicketOrderService._listBankAccount$.subscribe((res: IDropdown[]) => {
      if (res) {
        this.listAccount = res;
      }
    });

    this.changeDetectorRef.markForCheck();
    this.changeDetectorRef.detectChanges();
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event?: any) {
    if (event) {
      if (this.crudDTO.isValidData()) {
        this.saleTicketOrderService.crudSaleTicketOrderTransaction(this.crudDTO.toObjectSendToAPI(), this.type === EConfigDataModal.EDIT).subscribe(
          (response) => {
            if (this.handleResponseInterceptor(response, '')) {
              this.ref.close(response);
            }
          },
          (err) => {}
        );
      } else {
        this.messageDataValidator(this.crudDTO.dataValidator);
      }
    }
  }
}
