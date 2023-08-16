import { ChangeDetectorRef, Component, Injector } from '@angular/core';
import { CrudComponentBase } from '@shared/crud-component-base';
import { formatCalendarItem } from '@shared/function-common';
import { IDropdown } from '@shared/interface/InterfaceConst.interface';
import { EmulatorMsbModel } from '@shared/interface/sale-ticket-management/sale-ticket-order/EmulatorMsb.model';
import { SaleTicketOrderService } from '@shared/services/sale-ticket-order.service';
import { MessageService } from 'primeng/api';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';

export const TEST_GIA_LAP_MSB = 'test gia lap MSB';

@Component({
  selector: 'emulator-msb-dialog',
  templateUrl: './emulator-msb-dialog.component.html',
  styleUrls: ['./emulator-msb-dialog.component.scss'],
})
export class EmulatorMsbDialogComponent extends CrudComponentBase {
  constructor(
    injector: Injector,
    messageService: MessageService,
    public ref: DynamicDialogRef,
    private saleTicketOrderService: SaleTicketOrderService,
    private changeDetectorRef: ChangeDetectorRef,
    private config: DynamicDialogConfig
  ) {
    super(injector, messageService);
  }

  public dto: EmulatorMsbModel = new EmulatorMsbModel();
  public bankAccountId: number;
  public tranDate: any = new Date();
  public tranAmount: number;
  public bankData: any;
  public listBankAccount: IDropdown[] = [];

  ngOnInit() {
    if (this.config.data) {
      const { dataSource } = this.config.data;
      this.tranAmount = dataSource.tranAmount;
    }
  }

  ngAfterViewInit() {
    this.saleTicketOrderService._listBankAccount$.subscribe((res: IDropdown[]) => {
      if (res) {
        this.listBankAccount = res;
        if (this.listBankAccount && this.listBankAccount.length) {
          this.bankAccountId = Number(this.listBankAccount[0].value);
          this.getBankData();
        }
      }
    });

    this.changeDetectorRef.markForCheck();
    this.changeDetectorRef.detectChanges();
  }

  public onChangeBankAccountId(event: any) {
    if (event) {
      this.getBankData();
    }
  }

  private getBankData() {
    this.saleTicketOrderService.findByTradingBankId(this.bankAccountId).subscribe((resBank) => {
      if (this.handleResponseInterceptor(resBank)) {
        this.bankData = resBank?.data;
      }
    });
  }

  private getDTO() {
    const orderDetailId = this.saleTicketOrderService.selectedOrderId;
    const prefixMsb = this.bankData.prefixMsb;
    this.dto.tranSeq = new Date().getTime().toString();
    this.dto.vaCode = prefixMsb.toString().slice(0, 6);
    this.dto.vaNumber = prefixMsb + `EV` + orderDetailId;
    this.dto.fromAccountName = TEST_GIA_LAP_MSB;
    this.dto.fromAccountNumber = TEST_GIA_LAP_MSB;
    this.dto.toAccountName = TEST_GIA_LAP_MSB;
    this.dto.toAccountNumber = TEST_GIA_LAP_MSB;
    this.dto.tranRemark = TEST_GIA_LAP_MSB;
    this.dto.tranAmount = this.tranAmount.toString();
    this.dto.tranDate = formatCalendarItem(this.tranDate);
    this.dto.signature = TEST_GIA_LAP_MSB;
  }

  public noti(event: any) {
    if (event) {
      this.getDTO();
      this.saleTicketOrderService.notiMSB(this.dto).subscribe(
        (response) => {
          if (this.handleResponseInterceptor(response, 'Gửi thông báo MSB thành công')) {
          }
        },
        (err) => {
          this.messageError('Gửi thông báo MSB thất bại!');
        }
      );
    }
  }

  public close(event: any) {
    this.ref.close();
  }

  public save(event: any) {
    if (event) {
    }
  }
}
