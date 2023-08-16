
export class TradingProvider {
  tradingProviderId: number;
  businessCustomerId: number;
  status: number;
  name: string;
  code: string;
  phone: string | null;
  email: string | null;
  shortName: string | null;
  repName: string | null;
  taxCode: string | null;
  partnerId: number;
  aliasName: string | null;
  businessCustomer: BusinessCustomer;
}

export class BusinessCustomer {
  businessCustomerId: number;
  code: string;
  name: string;
  shortName: string;
  address: string;
  tradingAddress: string;
  nation: string;
  phone: string;
  mobile: string | null;
  email: string;
  taxCode: string;
  licenseDate: string;
  licenseIssuer: string;
  capital: number;
  repName: string;
  repPosition: string;
  decisionNo: string | null;
  decisionDate: string | null;
  numberModified: number;
  status: number;
  cifCode: string;
  dateModified: string;
  isCheck: string;
  bankAccName: string | null;
  bankAccNo: string | null;
  bankName: string | null;
  bankId: number | null;
  bankBranchName: string | null;
  website: string | null;
  fanpage: string | null;
  businessRegistrationImg: string | null;
  tradingProviderId: number | null;
  server: string | null;
  key: string | null;
  secret: string | null;
  avatarImageUrl: string | null;
  stampImageUrl: string | null;
  referralCodeSelf: string;
  allowDuplicate: string;
  repIdNo: string | null;
  repIdDate: string | null;
  repIdIssuer: string | null;
  repSex: string | null;
  repAddress: string | null;
  repBirthDate: string | null;
  isAccountLogin: boolean;
  businessCustomerBank: any | null;
  businessCustomerBanks: BusinessCustomerBank[];
}

export class BusinessCustomerBank {
  businessCustomerBankAccId: number;
  businessCustomerId: number;
  bankAccName: string;
  bankAccNo: string;
  bankName: string;
  bankBranchName: string | null;
  bankCode: string;
  logo: string;
  bankId: number;
  fullBankName: string;
  status: number;
  isDefault: string;
}

export class TradingProviderInfo {
  businessCustomerId: number | null;
  aliasName: string | null;
  tradingProviderId: number | null; 
  name: string | null;

  constructor(data: Partial<TradingProvider> = {}) {
    this.businessCustomerId = data.businessCustomerId || null;
    this.aliasName = data.aliasName || null;
    this.tradingProviderId = data.tradingProviderId || null;
    this.name = data.name || null;
  }
}
