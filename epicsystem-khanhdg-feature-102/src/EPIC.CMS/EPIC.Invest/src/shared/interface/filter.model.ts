import { ApproveConst, DistributionConst, InterestPaymentConst, YesNoConst } from "@shared/AppConsts";
import { Sort } from "./p-table.model";

export class BasicFilter {
    keyword?: string;
    status?: number | string;
    sortFields?: Sort[] = [];
}

export class PolicyTemplateFilter extends BasicFilter {
    type?: string;
}

export class MediaFilter extends PolicyTemplateFilter {
    productId?: number;
}

export class SampleContractFilter extends BasicFilter {
    contractSource?: number;
    contractType?: number;
    status?: string;
}

export class DistributionContractFilter extends BasicFilter {
    policyId?: number;
    contractSource?: number;
}

export class ApproveFilter extends BasicFilter {
    dataType?: number;
    actionType?: string;
    requestDate?: Date;
    approveDate?: Date;
}

export class BasicOrderFilter {
    keyword?: string;
    searchField?: string;
    status?: number;
    tradingProviderIds?: number[] = [];
} 


export class QueryCollectionBankFilter extends BasicFilter {
    createdDate?: Date;
    contractCode?: string;
}

export class QueryPaymentBankFilter extends BasicFilter {
    approveDate?: Date;
    contractCode?: string;
}

export class OrderBaseFilter {
    keyword?: string;
    searchField?: string;
    status?: number;
    tradingProviderIds?: number[];
    sortFields: Sort[];
    paramIgnore: string[] = ['searchField', 'paramIgnore', 'sortFields', 'tradingProviderIds'];
}

export class OrderDeliveryFilter extends OrderBaseFilter {
    contractCode?:string;
    deliveryStatus?: number;
    source?: number;
    date: Date; // inputDate Ngày nhập
    typeDate: string;
    paramIgnore: string[] = [...this.paramIgnore, ...['typeDate']];
}

export class OrderFilter extends OrderBaseFilter {
    source?: number;
    orderer?: number; 
    distributionId?: number;
    policy?: number[];
    policyDetailId?: number;
	tradingDate?: Date;
    paramIgnore: string[] = [...this.paramIgnore, ...['policy']];
}

export class OrderBlockageFilter {
    keyword?: string;
    status?: number;
    sortFields: Sort[] = [];
    tradingProviderIds?: number[] = [];
}

export class OrderInterestFilter extends OrderBaseFilter {
    status?: number = InterestPaymentConst.STATUS_DUEDATE; // Loại chi trả
    methodInterest?: number = DistributionConst.CO_CHI_TIEN; // Loại chi trả
    ngayChiTra?: Date; // Ngày chi trả   
    isExactDate?: string = YesNoConst.NO; // Lọc theo ngày chính xác   
    isLastPeriod?: string = YesNoConst.NO; // Lọc theo ngày chính xác   
}

export class OrderWithdrawFilter extends OrderBaseFilter {
    status?: number = ApproveConst.STATUS_REQUEST;
    requestDate?: Date;
    approveDate?: Date;
    methodInterest?: number = DistributionConst.CO_CHI_TIEN;
}

export class OrderExpireFilter extends OrderBaseFilter {
    status?: number = InterestPaymentConst.STATUS_DUEDATE;
    ngayChiTra: Date;
    settlementMethod: number = InterestPaymentConst.EXPIRE_DONE;
    isLastPeriod: string = YesNoConst.YES;
    isExactDate: string = YesNoConst.NO;
    projectId: number;
    methodInterest: number = DistributionConst.CO_CHI_TIEN;
}

export class OrderHistoryFilter extends OrderBaseFilter {
    distributionId: number;
    policyId: number;
    policyDetailId: number;
    source: number;
    orderer: number;
    investHistoryStatus: number;
}

export class OrderRenewalFilter extends OrderHistoryFilter {
    settlementMethod: number;
}