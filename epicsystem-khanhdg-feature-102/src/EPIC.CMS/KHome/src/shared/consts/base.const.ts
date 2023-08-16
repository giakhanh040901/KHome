export enum StatusResonse {
    SUCCESS = 1,
    ERROR = 0,
}

export enum YesNo {
    YES = "Y",
    NO = "N",
}

export enum ActiveDeactive {
    ACTIVE = "A",
    DEACTIVE = "D",
}

export enum UserType {
    T = 'T',    // TradingProvider
    RT = 'RT',  // Root Trading
    P = 'P',    // Partner
    RP = 'RP',  // Root Partner
    E = 'E',    // Epic
    RE = 'RE',  // Root Epic
}

export enum UnitTime {
    DAY = 'D',  // NGÀY 
    MONTH = 'M', // THÁNG
    YEAR = 'Y', // NĂM
    QUARTER = 'Q', // QUÝ
}

export enum Action {
    ADD = 1,
    UPDATE = 2,
    DELETE = 3,
}

export enum EFormatDate {
    DATE_TIME_SECOND = 'DD-MM-YYYY HH:mm:ss', 
    DATE_TIME = 'DD-MM-YYYY HH:mm',
    DATE = 'DD-MM-YYYY',
    DATE_DMY_Hms = 'DD-MM-YYYY HH:mm:ss', 
    DATE_DMY_Hm = 'DD-MM-YYYY HH:mm',
    DATE_DMY = 'DD-MM-YYYY',
}

export enum IconConfirm {
    APPROVE = 'assets/layout/images/icon-dialog-confirm/approve.svg',
    DELETE = 'assets/layout/images/icon-dialog-confirm/delete.svg',
    WARNING = 'assets/layout/images/icon-dialog-confirm/warning.svg',
    QUESTION = 'assets/layout/images/icon-dialog-confirm/question.svg',
}

export enum ContentTypeEView {
    MARKDOWN = 'MARKDOWN',
    HTML = 'HTML',
    IMAGE = "IMAGE",
    FILE = "FILE",
}

export enum EAcceptFile {
    ALL = '',
    IMAGE = 'image',
    VIDEO = 'video',
    MEDIA = 'media',
}