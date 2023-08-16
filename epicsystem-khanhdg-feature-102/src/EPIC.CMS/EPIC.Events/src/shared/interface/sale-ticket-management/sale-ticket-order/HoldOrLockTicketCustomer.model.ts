import { ETypeFormatDate } from '@shared/AppConsts';
import { formatDate } from '@shared/function-common';
import { IDataValidator, IDropdown } from '@shared/interface/InterfaceConst.interface';

export class HoldOrLockTicketCustomerModel {
  public id: number | undefined = undefined;
  public labelReason: string = '';
  public reason: number | undefined = undefined;
  public listReason: IDropdown[] = [];
  public isShowTime: boolean = false;
  public content: string = '';
  public time: string = '';
  public createUser: string = '';
  public createDate: string = formatDate(new Date(), ETypeFormatDate.DATE_TIME);
  public service: Function;
  public mapToObject: Function;
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidReason() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'expirereasonDate');
    if (this.reason) {
      return true;
    }
    this._dataValidator.unshift({
      name: 'reason',
      message: 'Vui lòng chọn thông tin Lý do',
    });
    return false;
  }

  public isValidData() {
    return this.isValidReason();
  }
}
