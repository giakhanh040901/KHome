export class AppConsts {
    static remoteServiceBaseUrlLocal: string;
    static remoteServiceBaseUrl: string;
    static appBaseUrl: string;
    static appBaseHref: string; // returns angular's base-href parameter value if used during the publish

    static grantType: string = 'password';
    static clientId: string;
    static clientSecret: string;

    static localeMappings: any = [];

    static readonly userManagement = {
        defaultAdminUserName: 'admin'
    };

    static readonly authorization = {
        encryptedAuthTokenName: 'enc_auth_token'
    };
}

export class AppPermissionNames {
    // user
    public readonly Pages_Users = "Pages.Users";
    public readonly Actions_Users_Create = "Actions.Users.Create";
    public readonly Actions_Users_Update = "Actions.Users.Update";
    public readonly Actions_Users_Delete = "Actions.Users.Delete";
    public readonly Actions_Users_Activation = "Actions.Users.Activation";

    // role
    public readonly Pages_Roles = "Pages.Roles";
    public readonly Actions_Roles_Create = "Actions.Roles.Create";
    public readonly Actions_Roles_Update = "Actions.Roles.Update";
    public readonly Actions_Roles_Delete = "Actions.Roles.Delete";
}

export class DataTableConst {
    public static message = {
        emptyMessage: 'Không có dữ liệu',
        totalMessage: 'bản ghi',
        selectedMessage: 'chọn'
    }
}

export class ProductBondDetailConst {

    public static getUnitDates(code){
        for (let item of this.unitDates) {
            if(item.code == code) return item.name;
        }
        return '';
    }

    public static getAllowOnlineTradings(code){
        for (let item of this.allowOnlineTradings) {
            if(item.code == code) return item.name;
        }
        return '';
    }

    public static unitDates = [
        {
            name: 'Năm',
            code: 'Y',
        },
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Tuần',
            code: 'W'
        },
        {
            name: 'Ngày',
            code: 'D'
        }
    ]

    public static allowOnlineTradings = [
        {
            name: 'Cho phép',
            code: 'Y'
        },
        {
            name: 'Không cho phép',
            code: 'N'
        }
    ]
}

export class StatusResponseConst {
    public static list = [
        {
            value : false,
            status : 0,
        },
        {
            value : true,
            status : 1,
        },
    ]

    public static RESPONSE_TRUE = 1;
    public static RESPONSE_FALSE = 0;

}

export class ContractTemplateConst{
    public static contractTempType = [
        {
            name:'Nhập nội dung hợp đồng',
            code: '1'
        },
        {
            name: 'Upload lên file repx',
            code: '2'
        }

    ]

    public static getNameType(code) {
        let type = this.contractTempType.find(type => type.code == code);
        if(type) return type.name;
        return '';
    }
}

export class ProductBondInfoConst {
    public static periodUnits = [
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Năm',
            code: 'Y'
        }
    ];
    public static couponTypes = [
        {
            name: 'Coupon',
            code: '1'
        },
        {
            name: 'Zero Coupon',
            code: '0'
        }
    ];
    public static interestRateTypes = [
        {
            name: 'Định kỳ',
            code: '1'
        },
        {
            name: 'Cuối kỳ',
            code: '2'
        }
    ];
    public static interestCouponTypes = [
        {
            name: 'Thả nổi',
            code: '1'
        },
        {
            name: 'Định kỳ',
            code: '2'
        }
    ];
    public static boolean = [
        {
            name: 'Có',
            code: 'Y'
        },
        {
            name: 'Không',
            code: 'N'
        },
    ];

    public static getPeriodUnits(code){
        for (let item of this.periodUnits) {
            if(item.code == code) return item.name;
        }
        return '';
    }

    public static getCouponTypes(code){
        for (let item of this.couponTypes) {
            if(item.code == code) return item.name;
        }
        return '';
    }

    public static getInterestRateTypes(code){
        for (let item of this.interestRateTypes) {
            if(item.code == code) return item.name;
        }
        return '';
    }

    public static getInterestCouponTypes(code){
        for (let item of this.interestCouponTypes) {
            if(item.code == code) return item.name;
        }
        return '';
    }

    public static getBoolean(code){
        for (let item of this.boolean) {
            if(item.code == code) return item.name;
        }
        return '';
    }
}

export class IssuerConst {
    //
    public static status = [
        {
            name: 'Kích hoạt',
            code: 'A',
        },
        {
            name: 'Không kích hoạt',
            code: 'D',
        },
        {
            name: 'Đóng',
            code: 'C',
        },
    ];
    public static STATUS_ACTIVE = 'A';
    public static STATUS_DISABLE = 'D';
    public static STATUS_CLOSE = 'C';

    public static getStatusName(code) {
        let status = this.status.find(status => status.code == code);
        if(status) return status.name;
        return '';
    }

    //
    public static types = [
        {
            name: 'Tổ chức',
            code: 1,
        },
        {
            name: 'Cá nhân',
            code: 0,
        },

    ];

    public static getNameType(code) {
        let type = this.types.find(type => type.code == code);
        if(type) return type.name;
        return '';
    }

    //
}

export class StatusDeleteConst {
    public static list = [
        {
            name: 'Đã xóa',
            code: 'Y',
        },
        {
            name: 'Chưa xóa',
            code: 'N',
        },
    ]

    public static DELETE_TRUE = 'Y';
    public static DELETE_FALSE = 'N';
}



export class YesNoConst {
    public static list = [
        {
            name: 'Có',
            code: 'Y',
        },
        {
            name: 'Không',
            code: 'N',
        },
    ]

    public static getName(code){
        for (let item of this.list) {
            if(item.code == code) return item.name;
        }
        return '-';
    }

    public static STATUS_YES = 'Y';
    public static STATUS_NO = 'N';
}

export class ProductBondInterestConst {
    public static periodUnits = [
        {
            name: 'Năm',
            code: 'Y',
        },
        {
            name: 'Tháng',
            code: 'M'
        },
        {
            name: 'Tuần',
            code: 'W'
        },
        {
            name: 'Ngày',
            code: 'D'
        }
    ];

    public static getPeriodUnits(code){
        for (let item of this.periodUnits) {
            if(item.code == code) return item.name;
        }
        return '';
    }
}
export class ProductPolicyConst {
    public static types = [
        {
            name: 'Pnote',
            code: '1',
        },
        {
            name: 'Pro',
            code: '2',
        },
        {
            name: 'ProA',
            code: '3',
        },
    ];

    public static getNameType(code) {
        for (let item of this.types) {
            if(item.code == code) return item.name;
        }
        return '-';
    }

    public static customerTypes = [
        {
            name: 'Chuyên nghiệp',
            code: 'P',
        },
        {
            name: 'Không chuyên nghiệp',
            code: 'N'
        },
    ];

    public static getCustomerType(code) {
        for (let item of this.customerTypes) {
            if(item.code == code) return item.name;
        }
        return '-';
    }

    public static statusList = [
        {
            name: 'Chờ duyệt',
            code: 'P',
        },
        {
            name: 'Đã duyệt',
            code: 'A'
        },
        {
            name: 'Từ chối duyệt',
            code: 'R'
        },
    ];

    public static getNameStatus(code){
        for (let item of this.statusList) {
            if(item.code == code) return item.name;
        }
        return '-';
    }

    public static markets = [
        {
            name: 'Sơ cấp',
            code: '1',
        },
        {
            name: 'Thứ cấp',
            code: '2',
        },
    ];

    public static getNameMarket(code){
        for (let item of this.markets) {
            if(item.code == code) return item.name;
        }
        return '-';
    }

    public static PRIMARY_MARKET = '1';
    public static SECONDARY_MARKET = '2';
}




