export class IndividualCustomerModel {
  public id: number;
  public code: string;
  public name: string;
  public phoneNumber: string;
  public email: string;
  public gender: string;
  public totalPoint: number;
  public currentPoint: number;
  public class: string;
  public voucherNumber: number;
  public status: boolean;
}

export class IndividualCustomerDetailOverviewModel {
  public id: number | undefined = undefined;
  public code?: string = '';
  public name: string = '';
  public birthday: any = '';
  public gender: number | undefined = undefined;
  public referralCode: string = '';
  public numberPhone?: string = '';
  public email?: string = '';
  public cardType?: string = '';
  public cardNumber?: string = '';
  public address?: string = '';
  public joinDate?: any = '';

  public mapDTO(dto: any) {
    this.code = dto.cifCode;
    this.name = dto.defaultIdentification.fullname;
    this.birthday = dto.defaultIdentification.dateOfBirth ? new Date(dto.defaultIdentification.dateOfBirth) : '';
    this.gender = dto.defaultIdentification.sex;
    this.referralCode = dto.referralCodeSelf;
    this.numberPhone = dto.phone;
    this.email = dto.email;
    this.cardType = dto.defaultIdentification.idType;
    this.cardNumber = dto.defaultIdentification.idNo;
    this.address = dto.contactAddress;
    this.joinDate = dto.createdDate ? new Date(dto.createdDate) : '';
  }
}

export class IndividualCustomerDetailAccumulateModel {
  public totalPoint?: string = '';
  public usePoint?: string = '';
  public remainPoint?: string = '';
  public membershipClass?: string = '';

  public mapDTO(dto: any) {
    this.totalPoint = dto.loyTotalPoint;
    this.usePoint = dto.loyConsumePoint;
    this.remainPoint = dto.loyCurrentPoint;
    this.membershipClass = dto.rankName;
  }
}

export class IndividualCustomerDetailOfferModel {
  public id: number;
  public voucherId: number;
  public name: string;
  public type: string;
  public value: string;
  public applyDate: string;
  public expiredDate: string;
  public createDate: string;
  public status: number;
}

export class HistoryOfPoints {
  public id: number;
  public date: string;
  public point: number;
  public type: string;
  public reason: string;
  public settingUser: string;
}

export class IndividualCustomerVoucherDetail {
  public name: string;
  public phone: string;
  public currentPoint: number;
  public changePoint: number;
  public voucher: string;
  public progressDetail: IndividualCustomerVoucherProgressDetail[];

  public mapData(dto: any) {
    if (dto) {
      this.name = dto.fullname;
      this.phone = dto.phone;
      this.currentPoint = dto.loyCurrentPoint;
      this.changePoint = dto.voucherPoint;
      this.voucher = dto.voucherName;
      this.progressDetail =
        dto.conversionPointStatusLogs && dto.conversionPointStatusLogs.length
          ? dto.conversionPointStatusLogs.map(
              (e: any) =>
                ({
                  id: e.id,
                  date: e.createdDate,
                  status: e.status,
                  user: e.createdBy,
                  source: e.source,
                  description: e.note,
                } as IndividualCustomerVoucherProgressDetail)
            )
          : [];
    }
  }
}

export class IndividualCustomerVoucherProgressDetail {
  public id: number;
  public date: string;
  public status: number;
  public user: string;
  public source: number;
  public description: string;
}

export class IndividualCustomerDetailHistory {
  public id: number;
  public customer: string;
  public program: string;
  public prize: string;
  public date: string;
  public status: number;
}
