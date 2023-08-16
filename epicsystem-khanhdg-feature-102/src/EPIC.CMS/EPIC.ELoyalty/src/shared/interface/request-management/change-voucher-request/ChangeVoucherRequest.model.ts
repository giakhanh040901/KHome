import { ChangeVoucherRequest } from '@shared/AppConsts';
import { formatCalendarItem } from '@shared/function-common';
import { IDataValidator, IDropdown } from '@shared/interface/InterfaceConst.interface';

export class ChangeVoucherRequestModel {
  public id: number;
  public customer: string;
  public numberPhone: string;
  public currentPoint: number;
  public changePoint: number;
  public requestDate: string;
  public receiveDate: string;
  public handOverDate: string;
  public finishDate: string;
  public status: number;
}

export class CrudChangeVoucherRequest {
  public id: number;
  public individualId?: number = undefined;
  public businessId?: number = undefined;
  public requestType: number | undefined = undefined;
  public applyType: number | undefined = undefined;
  public requestDate: any = '';
  public isMinusPoint?: boolean = undefined;
  public changeVoucherRequestItem: ChangeVoucherRequestItem[] = [];
  public description: string | undefined = undefined;
  public detailProcess?: DetailProcessChangePoint[];
  public status?: number = undefined;
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidIndividualId() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'individualId');
    if (this.individualId) {
      return true;
    }
    this._dataValidator.push({
      name: 'individualId',
      message: 'Vui lòng chọn thông tin Khách hàng',
    });
    return false;
  }

  public isValidCurrentPoint(currentPoint: number | undefined) {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'currentPoint');
    if (currentPoint < 0) {
      this._dataValidator.push({
        name: 'currentPoint',
        message: 'Điểm hiện tại của khách hàng không thể nhỏ hơn 0',
      });
      return false;
    }
    return true;
  }

  public isValidRequestDate() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'requestDate');
    if (this.requestDate) {
      return true;
    }
    this._dataValidator.push({
      name: 'requestDate',
      message: 'Vui lòng chọn thông tin Ngày yêu cầu',
    });
    return false;
  }

  public isValidChangeVoucherRequestItem(listVoucher: IDropdown[]) {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== 'changeVoucherRequestItem' && e.name !== 'voucherId' && e.name !== 'changeQuantity'
    );
    if (this.changeVoucherRequestItem && this.changeVoucherRequestItem.length) {
      let resultItem: boolean = true;
      let index: number = 0;
      while (index < this.changeVoucherRequestItem.length && !!resultItem) {
        const item = this.changeVoucherRequestItem[index];
        resultItem = item.isValidData(index, listVoucher);
        this._dataValidator.push(...item.dataValidator);
        index++;
      }
      return resultItem;
    }
    this._dataValidator.push({
      name: 'changeVoucherRequestItem',
      message: 'Vui lòng chọn Giá trị voucher',
    });
    return false;
  }

  public isValidData(listVoucher: IDropdown[], currentPoint: number | undefined) {
    return (
      this.isValidIndividualId() &&
      this.isValidRequestDate() &&
      this.isValidChangeVoucherRequestItem(listVoucher) &&
      this.isValidCurrentPoint(currentPoint)
    );
  }

  public toObjectSendToAPI() {
    return {
      id: this.id,
      investorId: this.individualId,
      requestType: this.requestType,
      allocationType: this.applyType,
      requestDate: this.requestDate ? formatCalendarItem(this.requestDate) : undefined,
      isMinusPoint:
        this.applyType === ChangeVoucherRequest.TANG_KHACH_HANG ? (this.isMinusPoint ? 'Y' : 'N') : undefined,
      description: this.description,
      details: this.changeVoucherRequestItem.map((item: ChangeVoucherRequestItem) => ({
        id: item.id,
        voucherId: item.voucherId,
        quantity: item.changeQuantity,
      })),
    };
  }

  public mapData(dto: any) {
    if (dto) {
      this.id = dto.id;
      this.individualId = dto.investorId;
      this.requestType = dto.requestType;
      this.applyType = dto.allocationType;
      this.requestDate = dto.requestDate ? new Date(dto.requestDate) : '';
      this.isMinusPoint = dto.isMinusPoint ? dto.isMinusPoint === 'Y' : undefined;
      this.description = dto.description;
      this.changeVoucherRequestItem =
        dto.details && dto.details.length
          ? dto.details.map((e: any) => {
              let item = new ChangeVoucherRequestItem();
              Object.assign(item, {
                id: e.id,
                voucherId: e.voucherId,
                changePoint: e.point,
                changeQuantity: e.quantity,
                totalPoint: e.totalConversionPoint,
                showBtnRemove: true,
              });
              return item;
            })
          : undefined;
      this.detailProcess =
        dto.statusLogs && dto.statusLogs.length
          ? dto.statusLogs.map(
              (e: any) =>
                ({
                  id: e.id,
                  date: e.createdDate,
                  status: e.status,
                  actionUser: e.createdBy,
                  source: e.source,
                  note: e.note,
                } as DetailProcessChangePoint)
            )
          : undefined;
      this.status = dto.status;
    }
  }
}

export class ChangeVoucherRequestItem {
  public id: number | undefined = undefined;
  public voucherId: number | undefined = undefined;
  public changePoint: number | undefined = undefined;
  public changeQuantity: number | undefined = undefined;
  public totalPoint: number | undefined = undefined;
  public showBtnRemove: boolean | undefined = undefined;
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidVoucherId(index: number) {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'voucherId');
    if (this.voucherId || this.voucherId === 0) {
      return true;
    }
    this._dataValidator.push({
      name: 'voucherId',
      message: 'Vui lòng chọn thông tin Voucher thứ ' + (index + 1),
    });
    return false;
  }

  public isValidChangeQuantity(index: number, listVoucher: IDropdown[]) {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'changeQuantity');
    if (this.changeQuantity) {
      const changeQuantity = listVoucher.find((e: IDropdown) => e.value === this.voucherId)?.rawData?.changeQuantity;
      if (changeQuantity) {
        if (this.changeQuantity > changeQuantity) {
          this._dataValidator.push({
            name: 'changeQuantity',
            message: 'Số lượng yêu cầu phải nhỏ hơn Số lượng đổi tối đa của voucher thứ ' + (index + 1),
          });
          return false;
        }
      }
      return true;
    }
    this._dataValidator.push({
      name: 'changeQuantity',
      message: 'Vui lòng nhập thông tin Số lượng yêu cầu thứ ' + (index + 1),
    });
    return false;
  }

  public isValidData(index: number, listVoucher: IDropdown[]) {
    return this.isValidVoucherId(index) && this.isValidChangeQuantity(index, listVoucher);
  }
}

export class DetailProcessChangePoint {
  public id: number;
  public date: string;
  public status: number;
  public actionUser: string;
  public source: number;
  public note: string;
}
