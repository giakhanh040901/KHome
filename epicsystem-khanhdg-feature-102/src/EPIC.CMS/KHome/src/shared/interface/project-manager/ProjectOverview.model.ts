export class ProjectOverviewModel {
  id: number;
  code: string;
  name: string;
  productType: string;
  ownerName: string;
  totalQuantity: number;
  distributionQuantity: number;
  soldQuantity: number;
  remainingQuantity: number;
  createdDate?: string;
  createdBy?: string;
  status: number;
}

export class CreateOrEditOverviewProject {
  id?: number = undefined;
  ownerId: number = undefined;
  code: string = "";
  name: string = "";
  contractorName: string = "";
  contractorLink?: string = "";
  contractorDescription?: string = "";
  operatingUnit?: string = "";
  operatingUnitLink?: string = "";
  operatingUnitDescription?: string = "";
  projectType?: number = undefined;
  guaranteeBank?: number = undefined;
  guaranteeBanks?: number[] = undefined;
  facebookLink?: string = "";
  website?: string = "";
  phone?: string = "";
  productTypes?: number[] = undefined;
  distributionTypes?: number[] = undefined;
  projectStatus?: number = undefined;
  landArea?: string = "";
  constructionArea?: string = "";
  buildingDensity?: string = "";
  landPlotNo?: string = "";
  mapSheetNo?: string = "";
  startDate?: any = "";
  expectedHandoverTime?: string = "";
  totalInvestment?: number = undefined;
  expectedSellingPrice?: number = undefined;
  minSellingPrice?: number = undefined;
  maxSellingPrice?: number = undefined;
  numberOfUnit?: number = undefined;
  provinceCode?: string = "";
  address?: string = "";
  latitude?: string = "";
  longitude?: string = "";
  status?: number = undefined;
  projectExtends?: any = [{index: 0}];
  private _dataValidator: any[] = [];

  public get dataValidator() {
    return this._dataValidator;
  }

  public isValidOwner() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "ownerId"
    );
    if (!!this.ownerId) {
      return true;
    }
    this._dataValidator.push({
      name: "ownerId",
      message: "Vui lòng nhập thông tin Chủ đầu tư",
    });
    return false;
  }

  public isValidCode() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "code"
    );
    if (this.code && this.code.length) {
      return true;
    }
    this._dataValidator.push({
      name: "code",
      message: "Vui lòng nhập thông tin Mã dự án",
    });
    return false;
  }

  public isValidName() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "name"
    );
    if (this.name && this.name.length) {
      return true;
    }
    this._dataValidator.push({
      name: "name",
      message: "Vui lòng nhập thông tin Tên dự án",
    });
    return false;
  }

  public isValidContractorName() {
    this._dataValidator = this._dataValidator.filter(
      (e: any) => e.name !== "contractorName"
    );
    if (this.contractorName && this.contractorName.length) {
      return true;
    }
    this._dataValidator.push({
      name: "contractorName",
      message: "Vui lòng nhập thông tin Tổng thầu xây dựng",
    });
    return false;
  }

  private isValidLink(value: string) {
    var expression =
      /[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)?/gi;
    var regex = new RegExp(expression);
    return value.match(regex);
  }

  // public isValidContractorLink() {
  //   if (this.contractorLink && this.contractorLink.length) {
  //     return this.isValidLink(this.contractorLink);
  //   }
  //   return true;
  // }

  // public isValidOperatingUnitLink() {
  //   if (this.operatingUnitLink && this.operatingUnitLink.length) {
  //     return this.isValidLink(this.operatingUnitLink);
  //   }
  //   return true;
  // }

  // public isValidWebsite() {
  //   if (this.website && this.website.length) {
  //     return this.isValidLink(this.website);
  //   }
  //   return true;
  // }

  public isValidData() {
    return (
      this.isValidOwner() && this.isValidCode() && this.isValidName() && this.isValidContractorName()
      // this.isValidContractorLink() &&
      // this.isValidOperatingUnitLink() &&
      // this.isValidWebsite()
    );
  }

  public toObjectSendToAPI() {
		const result = {
      id: this.id,
      ownerId: this.ownerId,
      code: this.code,
      name: this.name,
      contractorName: this.contractorName,
      contractorLink: this.contractorLink,
      contractorDescription: this.contractorDescription,
      operatingUnit: this.operatingUnit,
      operatingUnitLink: this.operatingUnitLink,
      operatingUnitDescription: this.operatingUnitDescription,
      projectType: this.projectType,
      guaranteeBanks: this.guaranteeBank ? [this.guaranteeBank] : undefined,
      website: this.website,
      facebookLink: this.facebookLink,
      phone: this.phone,
      productTypes: this.productTypes,
      distributionTypes: this.distributionTypes,
      projectStatus: this.projectStatus,
      landArea: this.landArea,
      constructionArea: this.constructionArea,
      buildingDensity: this.buildingDensity,
      landPlotNo: this.landPlotNo,
      mapSheetNo: this.mapSheetNo,
      startDate: this.startDate,
      expectedHandoverTime: this.expectedHandoverTime,
      totalInvestment: this.totalInvestment,
      expectedSellingPrice: this.expectedSellingPrice,
      minSellingPrice: this.minSellingPrice,
      maxSellingPrice: this.maxSellingPrice,
      numberOfUnit: this.numberOfUnit,
      provinceCode: this.provinceCode,
      address: this.address,
      latitude: this.latitude,
      longitude: this.longitude,
      projectExtends: this.projectExtends
    };
		Object.keys(result).forEach((key: string) => {
			result[key] = !!result[key] ? result[key] : undefined;
		})
    return result;
  }

  public mapDTOToModel(dto: any) {
    Object.keys(this).forEach((key: string) => {
      if (dto[key] || dto[key] === 0) {
        this[key] = dto[key];
      }
    })
    if (this.guaranteeBanks && this.guaranteeBanks.length) this.guaranteeBank = this.guaranteeBanks[0];

  }
}
