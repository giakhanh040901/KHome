export interface IContractCodeStructure {
    id?: number;
    status?: string;
    name?: string;
    configContractCodeDetails?: ConfigContractCodeDetail[];
    contractCode?: string;
    contractCodeStructure?: string;
    description?: string;
    createdBy?: string;
    createdDate?: string;
    tradingProviderId?: number;
}
    
export interface ConfigContractCodeDetail {
    sortOrder: number;
    id?: number;
    key?: string;
    value?: string;
    configContractCodeId?: number;
}
      