export class PolicyDetail {
    id: number;
    tradingProviderId: number;
    policyTempId: number;
    sortOrder: number;
    name: string;
    shortName: string;
    profit: number;
    interestDays: number;
    periodQuantity: number;
    periodType: string;
    interestType: number;
    interestPeriodQuantity: number;
    interestPeriodType: string;
    repeatFixedDate: any;
    status: string;
    fakeId?: any;
  }
  
  export class Policy {
    id?: number;
    tradingProviderId?: number;
    code?: string;
    name?: string;
    minMoney?: number;
    minInvestDay?: number;
    maxMoney?: number;
    incomeTax?: number;
    investorType?: string;
    classify?: number;
    calculateType?: number;
    garnerType?: number;
    interestType?: number;
    interestPeriodQuantity?: any;
    interestPeriodType?: any;
    repeatFixedDate?: any;
    minWithdraw?: number;
    maxWithdraw?: number;
    withdrawFee?: number;
    withdrawFeeType?: number;
    orderOfWithdrawal?: number;
    isTransferAssets?: string;
    transferAssetsFee?: number;
    description?: string;
    sortOrder?: number;
    status?: string;
    policyDetails?: PolicyDetail[];
    contractTemplateTemps?: any;
  }

  export class RepeatFixedDate {
    name: string;
    code: number;
  }
  