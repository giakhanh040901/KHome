import { ETypeFormatDate, SaleTicketOrder } from '@shared/AppConsts';
import { formatCalendarItem, formatDate } from '@shared/function-common';
import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class SaleTicketOrderDetailTransactionModel {
  public id: number;
  public code: number;
  public revenueExpenditure: number;
  public type: number;
  public tradingDate: string;
  public value: string;
  public description: string;
  public status: number;
}

export class CrudSaleTicketTransaction {
  public id?: number = undefined;
  public orderId: number;
  public tradingCode: string = '';
  public tradingDate: any = '';
  public tradingKind: number | undefined = undefined;
  public tradingType: number | undefined = undefined;
  public transactionType: number | undefined = undefined;
  public value: number = 0;
  public account: number | undefined = undefined;
  public description: string = '';
  public createUser: string = '';
  public createDate: any = '';
  public approveUser: string = '';
  public approveDate: any = '';
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  private isValidTradingDate() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'tradingDate');
    if (this.tradingDate) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'tradingDate',
      message: 'Vui lòng chọn Ngày giao dịch',
    });
    return false;
  }

  private isValidTransactionType() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'transactionType');
    if (this.transactionType) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'transactionType',
      message: 'Vui lòng chọn Loại thanh toán',
    });
    return false;
  }

  private isValidValue() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'value');
    if (this.value) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'value',
      message: 'Vui lòng nhập Số tiền',
    });
    return false;
  }

  private isValidAccount() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'account');
    if (this.transactionType === SaleTicketOrder.CHUYEN_KHOAN && !this.account) {
      this._dataValidator.unshift({
        name: 'account',
        message: 'Vui lòng chọn Tài khoản nhận tiền',
      });
      return false;
    }
    return true;
  }

  public isValidData() {
    return this.isValidTradingDate() && this.isValidTransactionType() && this.isValidValue() && this.isValidAccount();
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      orderId: this.orderId,
      tranDate: formatCalendarItem(this.tradingDate),
      tranClassify: this.tradingType,
      paymentType: this.transactionType,
      paymentAmount: Number(this.value),
      tradingBankAccountId: this.account,
      description: this.description,
    };
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.orderId = dto.orderId;
      this.tradingCode = dto.orderNo;
      this.tradingDate = dto.tranDate ? new Date(dto.tranDate) : '';
      this.tradingKind = SaleTicketOrder.THU;
      this.tradingType = dto.tranClassify;
      this.transactionType = dto.paymentType;
      this.value = dto.paymentAmount;
      this.account = dto.tradingBankAccountId;
      this.description = dto.description;
      this.createUser = dto.createdBy;
      this.createDate = dto.createdDate ? formatDate(dto.createdDate, ETypeFormatDate.DATE_TIME) : '';
      this.approveUser = dto.approveBy;
      this.approveDate = dto.approveDate ? formatDate(dto.approveDate, ETypeFormatDate.DATE_TIME) : '';
    }
  }
}
