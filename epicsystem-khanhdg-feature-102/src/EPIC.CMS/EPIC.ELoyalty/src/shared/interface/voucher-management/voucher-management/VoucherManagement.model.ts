import { COMPARE_TYPE, VoucherManagement } from '@shared/AppConsts';
import { compareDate, formatCalendarItem, formatUrlImage, getUrlImage } from '@shared/function-common';
import { IDataValidator } from '@shared/interface/InterfaceConst.interface';

export class VoucherManagementModel {
  public voucherId: number;
  public code: string;
  public name: string;
  public kind: string;
  public type: string;
  public value: number;
  public point: number;
  public quantity: number;
  public apply: number;
  public isShowApp: boolean;
  public isHighlight: boolean;
  public createUser: string;
  public status: number;
}

export class CreateOrEditVoucher {
  public voucherId?: number = undefined;
  public avatar: string | undefined = undefined;
  public banner: string | undefined = undefined;
  public kind: string | undefined = undefined;
  public type: string | undefined = undefined;
  public code: string = '';
  public name: string = '';
  public displayName: string = '';
  public link: string = '';
  public valueUnit: string | undefined = undefined;
  public value: number = 0;
  public point: number = 0;
  public applyQuantity: number = 0;
  public changeQuantity: number = undefined;
  public expireDate: any = '';
  public batchEntryDate: any = '';
  public applyDate: any = '';
  public endDate: any = '';
  public contentType?: string = '';
  public content?: string = '';
  public isShowApp?: string = undefined;
  public isHighlight?: string = undefined;
  public isUsePrize?: string = undefined;
  private _dataValidator: IDataValidator[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidAvatar() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'avatar');
    if (this.avatar && this.avatar.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'avatar',
      message: 'Vui lòng chọn Ảnh voucher',
    });
    return false;
  }

  public isValidBanner() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'banner');
    if (this.banner && this.banner.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'banner',
      message: 'Vui lòng chọn Banner voucher',
    });
    return false;
  }

  public isValidCode() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'code');
    if (this.code && this.code.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'code',
      message: 'Vui lòng nhập thông tin Mã lô voucher',
    });
    return false;
  }

  public isValidName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'name');
    if (this.name && this.name.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'name',
      message: 'Vui lòng nhập thông tin Tên voucher',
    });
    return false;
  }

  public isValidDisplayName() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'displayName');
    if (this.displayName && this.displayName.length) {
      return true;
    }
    this._dataValidator.push({
      name: 'displayName',
      message: 'Vui lòng nhập thông tin Tên hiển thị voucher',
    });
    return false;
  }

  public isValidValue() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'value');
    if (!!Number(this.value)) {
      if (this.valueUnit === VoucherManagement.PERCENT && Number(this.value) > 100) {
        this._dataValidator.push({
          name: 'value',
          message: 'Giá trị voucher không hợp lệ',
        });
        return false;
      }
      return true;
    }
    this._dataValidator.push({
      name: 'value',
      message: 'Vui lòng nhập thông tin Giá trị voucher',
    });
    return false;
  }

  public isValidPoint() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'point');
    if (!!Number(this.point)) {
      return true;
    }
    this._dataValidator.push({
      name: 'point',
      message: 'Vui lòng nhập thông tin Điểm quy đổi',
    });
    return false;
  }

  public isValidApplyQuantity() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'applyQuantity');
    if (!!Number(this.applyQuantity)) {
      return true;
    }
    this._dataValidator.push({
      name: 'applyQuantity',
      message: 'Vui lòng nhập thông tin Số lượng phát hành',
    });
    return false;
  }

  public isValidExpireDate() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'expireDate');
    if (this.expireDate) {
      return true;
    }
    this._dataValidator.push({
      name: 'expireDate',
      message: 'Vui lòng chọn thông tin Ngày hết hạn',
    });
    return false;
  }

  public isValidBatchEntryDate () {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'batchEntryDate');
    if (this.batchEntryDate) {
      return true;
    }
    this._dataValidator.push({
      name: 'batchEntryDate',
      message: 'Vui lòng chọn thông tin Ngày nhập lô voucher',
    });
    return false;
  }

  public isValidApplyDate() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'applyDate');
    if (!this.applyDate) {
      this._dataValidator.push({
        name: 'applyDate',
        message: 'Vui lòng chọn thông tin Ngày áp dụng',
      });
      return false;
    } else if (compareDate(this.applyDate, this.expireDate, COMPARE_TYPE.GREATER)) {
      this._dataValidator.push({
        name: 'applyDate',
        message: 'Ngày áp dụng không thể lớn hơn Ngày hết hạn',
      });
      return false;
    }
    return true;
  }

  public isValidEndDate() {
    this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'endDate');
    if (this.endDate && compareDate(this.endDate, this.expireDate, COMPARE_TYPE.GREATER)) {
      this._dataValidator.push({
        name: 'endDate',
        message: 'Ngày kết thúc không thể lớn hơn Ngày hết hạn',
      });
      return false;
    }
    return true;
  }

  // public isValidLink() {
  //   this._dataValidator = this._dataValidator.filter((e: any) => e.name !== 'link');
  //   if ((this.link && this.link.length && this.type === 'DT') || this.type === 'C') {
  //     return true;
  //   }
  //   this._dataValidator.push({
  //     name: 'link',
  //     message: 'Vui lòng nhập thông tin Link voucher',
  //   });
  //   return false;
  // }

  public isValidData() {
    return (
      this.isValidAvatar() &&
      this.isValidBanner() &&
      this.isValidCode() &&
      this.isValidName() &&
      this.isValidDisplayName() &&
      this.isValidValue() &&
      this.isValidPoint() &&
      this.isValidApplyQuantity() &&
      this.isValidExpireDate() &&
      this.isValidBatchEntryDate() &&
      this.isValidApplyDate() &&
      this.isValidEndDate()
    );
  }

  public toObjectSendToAPI() {
    let result: any = {
      voucherId: this.voucherId,
      code: this.code,
      name: this.name,
      displayName: this.displayName,
      voucherType: this.kind,
      linkVoucher: this.link,
      useType: this.type,
      unit: this.valueUnit,
      value: this.value,
      startDate: this.applyDate ? formatCalendarItem(this.applyDate) : undefined,
      endDate: this.endDate ? formatCalendarItem(this.endDate) : undefined,
      batchEntryDate: this.batchEntryDate ? formatCalendarItem(this.batchEntryDate) : undefined,
      expiredDate: this.expireDate ? formatCalendarItem(this.expireDate) : undefined,
      point: this.point,
      avatar: this.avatar && this.avatar.length ? formatUrlImage(this.avatar) : undefined,
      bannerImageUrl: this.banner && this.banner.length ? formatUrlImage(this.banner) : undefined,
      descriptionContentType: this.contentType,
      descriptionContent: this.content,
      publishNum: this.applyQuantity,
      exchangeRoundNum: this.changeQuantity > 0 ? this.changeQuantity : null,
      isShowApp: this.isShowApp,
      isHot: this.isHighlight,
      isUseInLuckyDraw: this.isUsePrize,
    };
    return result;
  }

  public mapData(dto: any) {
    if (dto) {
      this.voucherId = dto.voucherId;
      this.avatar = dto.avatar && dto.avatar.length ? getUrlImage(dto.avatar) : undefined;
      this.banner = dto.bannerImageUrl && dto.bannerImageUrl.length ? getUrlImage(dto.bannerImageUrl) : undefined;
      this.kind = dto.voucherType;
      this.type = dto.useType;
      this.code = dto.code;
      this.name = dto.name;
      this.displayName = dto.displayName;
      this.link = dto.linkVoucher;
      this.valueUnit = dto.unit;
      this.value = dto.value;
      this.point = dto.point;
      this.applyQuantity = dto.publishNum;
      this.changeQuantity = dto.exchangeRoundNum;
      this.expireDate = dto.expiredDate ? new Date(dto.expiredDate) : '';
      this.applyDate = dto.startDate ? new Date(dto.startDate) : '';
      this.endDate = dto.endDate ? new Date(dto.endDate) : '';
      this.batchEntryDate = dto.batchEntryDate ? new Date(dto.batchEntryDate) : '';
      this.contentType = dto.descriptionContentType;
      this.content = dto.descriptionContent;
      this.isShowApp = dto.isShowApp;
      this.isHighlight = dto.isHot;
      this.isUsePrize = dto.isUseInLuckyDraw;
    }
  }
}

export class CustomerSearchModel {
  public id: number | undefined = undefined;
  public name: string = '';
  public numberPhone: string = '';
  public idNo?: string = '';
  public email: string = '';
  public abbreviation?: string = '';
  public taxCode?: string = '';
  public isSelected: boolean = false;
  public address?: string = '';
  public membershipClass?: string = '';
  public totalPoint?: number = undefined;
  public currentPoint?: number = undefined;
  public currentPointDisplay?: number = undefined;
}
