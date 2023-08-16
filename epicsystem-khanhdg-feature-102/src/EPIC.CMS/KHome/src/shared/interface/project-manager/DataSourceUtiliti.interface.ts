export interface IDataSourceUtiliti {
    id: number;
    tienich: string;
    chon: boolean;
    danhsachcon?: IDataSourceUtiliti[],
}

export interface IDataSourceUtilitiImage {
    id: number;
    tenhinhanh: string;
    duongdan: string;
    status: number;
}