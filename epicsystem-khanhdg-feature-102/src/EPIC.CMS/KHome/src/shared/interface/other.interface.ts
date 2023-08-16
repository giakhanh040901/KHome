import { EAcceptFile, IconConfirm } from "@shared/consts/base.const";

export interface IEConfirm {
    message: string,
    icon?: IconConfirm,
}

export interface IParamHandleDTO {
    idName?: string;
    isCheckNull?: boolean; 
}

export interface IType {
    name?: string;
    code?: string | number;
}

export interface ITag extends IType {
    severity: string,
}

export interface IErrorCode {
    [key: number] : string,
}

export interface IDialogUploadFileConfig {
    folderUpload?: string,
    header?: string, 
    width?:string, 
    uploadServer?: boolean, 
    multiple?: boolean, 
    accept?: EAcceptFile,
    quantity?: number, // Số phần tử upload lấy từ phần tử đầu tiên
    previewBeforeUpload?: boolean,
    callback?: Function,
}